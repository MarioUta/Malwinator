import requests
import cv2


def send_video_to_server():
    cap = cv2.VideoCapture(0)
    url = 'http://192.168.0.146:3000/upload'
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break
        # Encode the frame (you might need to use a proper encoder)
        _, encoded_frame = cv2.imencode('.jpg', frame)
        files = {'file': encoded_frame.tobytes()}
        response = requests.post(url, files=files, verify=False)
        if response.status_code != 200:
            print("Failed to upload frame")
            break
    cap.release()

if __name__ == "__main__":
    try:
        send_video_to_server()
    except:
        pass