import bpy
import socket
import threading
import queue
import time

# ========== CONFIGURATION ==========
UDP_PORT = 9000                 # Port for incoming UDP data
OBJECT_NAME = "AudioController" # Target object in the scene
PROPERTY_PATH = "location"      # Property: "location", "rotation_euler", "scale"
PROPERTY_INDEX = 0              # 0 = X, 1 = Y, 2 = Z
INSERT_KEYFRAMES = True         # Insert keyframes (if False, just moves the object)
MIN_CHANGE = 0.001              # Minimum change to insert a keyframe (0 = always)
USE_FRAME_CHANGE_POST = False   # False = before frame change, True = after
# =================================

data_queue = queue.Queue()
last_value = None

def udp_listener():
    """Background thread: receives UDP packets and puts values into a queue."""
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.bind(('0.0.0.0', UDP_PORT))
    sock.settimeout(0.5)
    print(f"UDP server started on port {UDP_PORT}")
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
    """Called every frame (pre or post). Takes the latest UDP value,
    updates the object's property, and inserts a keyframe if enabled."""
    global last_value

    if data_queue.empty():
        return

    # Take only the most recent value (discard older ones)
    while not data_queue.empty():
        val = data_queue.get()

    obj = bpy.data.objects.get(OBJECT_NAME)
    if not obj:
        return

    # Skip if the change is too small (to reduce keyframe spam)
    if last_value is not None and abs(val - last_value) < MIN_CHANGE:
        return

    # Get the property attribute (location / rotation_euler / scale)
    attr = getattr(obj, PROPERTY_PATH, None)
    if attr is None:
        print(f"Property {PROPERTY_PATH} not found on object {OBJECT_NAME}")
        return

    # Set the value at the specified index (for vectors)
    if hasattr(attr, '__getitem__') and len(attr) > PROPERTY_INDEX:
        attr[PROPERTY_INDEX] = val
    else:
        # If the property is a single number (rare), assign directly
        setattr(obj, PROPERTY_PATH, val)

    # Insert a keyframe
    if INSERT_KEYFRAMES:
        obj.keyframe_insert(data_path=PROPERTY_PATH, index=PROPERTY_INDEX)

    last_value = val

# Launch the UDP listener thread
thread = threading.Thread(target=udp_listener, daemon=True)
thread.start()

# Choose the appropriate handler (pre or post)
handlers = bpy.app.handlers.frame_change_post if USE_FRAME_CHANGE_POST else bpy.app.handlers.frame_change_pre

# Remove any previous instance of the handler to avoid duplicates
if update_controller in handlers:
    handlers.remove(update_controller)
handlers.append(update_controller)

fps = bpy.context.scene.render.fps
print(f" Script started. FPS: {fps}, updating {OBJECT_NAME}.{PROPERTY_PATH}[{PROPERTY_INDEX}]")
print(f"   Keyframes: {'ON' if INSERT_KEYFRAMES else 'OFF'}, change threshold: {MIN_CHANGE}")