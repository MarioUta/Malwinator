import cv2
import socketio
import base64
import sys
import requests

# Initialize Socket.IO client
http_session = requests.Session()
http_session.verify = False
sio = socketio.Client(http_session=http_session)

@sio.event
def connect():
    print('Connected to server')

@sio.event
def disconnect():
    print('Disconnected from server')


def send_video_to_server():
    if len(sys.argv) != 2:
        exit()
    # Connect to the Flask server
    cap = cv2.VideoCapture(0)
    while True:
        try:
            sio.connect(sys.argv[1])
        except:
            continue
        connections = 0

        while True:
            # Capture frame from camera
            ret, frame = cap.read()

            # Encode frame as JPEG
            _, encoded_frame = cv2.imencode('.jpg', frame)
            jpeg_data = base64.b64encode(encoded_frame.tobytes()).decode('utf-8')

            # Send JPEG data to the server
            try:
                sio.emit('stream', jpeg_data)
            except:
                break

            connections += 1
            # Break the loop if 'q' is pressed
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break
        sio.disconnect()

    # Release the camera and disconnect from the server
    cap.release()

if __name__ == "__main__":
    send_video_to_server()