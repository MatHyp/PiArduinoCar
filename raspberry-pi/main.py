from src.SerialConnection import SerialConnection
from src.ScrcpyScreenCapture import ScrcpyScreenCapture


def main():
    capture_system = ScrcpyScreenCapture()
    capture_system.start()
   


if __name__ == "__main__":
    main()