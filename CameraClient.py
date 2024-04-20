import cv2
import socket
import pickle
import struct

try:
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client_socket.connect(('192.168.0.169', 8888))  # Replace 'server_ip_address' with the actual server IP
    data = b""
    cap = cv2.VideoCapture(0)
    while True:
        ret, frame = cap.read()
        frame_data = pickle.dumps(frame)
        client_socket.sendall(struct.pack("Q", len(frame_data)))
        client_socket.sendall(frame_data)
        if cv2.waitKey(1) == 13:
            break

    cap.release()
    cv2.destroyAllWindows()
except:
    pass