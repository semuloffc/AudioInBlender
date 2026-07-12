import bpy
import socket
import threading
import queue
import time

data_queue = queue.Queue()
controller_name = "AudioController"
last_update_time = 0
update_interval = 0.03  # 30 раз в секунду

def udp_listener():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.bind(('0.0.0.0', 9000))
    sock.settimeout(0.5)
    print("UDP server started")
    while True:
        try:
            data, addr = sock.recvfrom(1024)
            msg = data.decode('utf-8').replace(',', '.')
            value = float(msg)
            data_queue.put(value)
        except socket.timeout:
            continue
        except:
            break
    sock.close()

def update_controller(scene):
    global last_update_time
    if data_queue.empty():
        return
    now = time.time()
    if now - last_update_time < update_interval:
        return
    while not data_queue.empty():
        val = data_queue.get()
    controller = bpy.data.objects.get(controller_name)
    if controller:
        controller.location.x = val
        last_update_time = now

thread = threading.Thread(target=udp_listener, daemon=True)
thread.start()

if update_controller not in bpy.app.handlers.frame_change_pre:
    bpy.app.handlers.frame_change_pre.append(update_controller)

print("UDP listener + frame updater started. Location.x updates at 30 Hz max.")