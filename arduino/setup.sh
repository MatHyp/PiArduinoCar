#!/bin/bash

# echo "----- [🛠] Checking for system updates..."
# sudo apt update -y

# echo "----- [📦] Ensuring required system packages are installed..."
# sudo apt install -y python3 python3-venv python3-pip git

# Check if virtual environment already exists
if [ ! -d ".venv" ]; then
    echo "----- [🐍] Creating Python virtual environment..."
    python3 -m venv .venv
else
    echo "✅ Python virtual environment already exists."
fi

# Activate venv
source .venv/bin/activate

# Check if PlatformIO is already installed in venv
if ! .venv/bin/pio --version &>/dev/null; then
    echo "----- [📦] Installing PlatformIO..."
    pip install --upgrade pip
    pip install platformio
else
    echo "✅ PlatformIO is already installed."
fi

# Run PlatformIO upload
echo "----- [📤] Uploading code to Arduino..."
.venv/bin/pio run --target upload --upload-port /dev/ttyACM0

echo "✅ Upload complete!"
