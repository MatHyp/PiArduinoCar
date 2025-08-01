#!/bin/bash

# Create and activate venv if not exists
if [ ! -d ".venv" ]; then
  echo "🔧 Creating virtual environment..."
  python3 -m venv .venv
fi

echo "✅ Activating virtual environment..."
source .venv/bin/activate

echo "----------------------------------------"
echo "📦 Installing dependencies"
echo "----------------------------------------"
pip install --upgrade pip
pip install -r requirements.txt

echo ""
echo "----------------------------------------"
echo "🚀 Running the application"
echo "----------------------------------------"
python3 main.py
