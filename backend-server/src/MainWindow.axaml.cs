using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using UDP;
namespace BackendServer
{
    public partial class MainWindow : Window
    {
        private UDPSocket udp = new UDPSocket();
        public MainWindow()
        {
            InitializeComponent();
            // udp.Client("127.0.0.1", 12000);


            // Keyboard events
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            // switch (e.Key)
            // {
            //     case Key.W:

            //         WKey.Background = Brushes.LimeGreen;
            //         udp.Send("W");
            //         break;

            //     case Key.A:
            //         AKey.Background = Brushes.LimeGreen;
            //         udp.Send("A");

            //         break;
            //     case Key.S:
            //         SKey.Background = Brushes.LimeGreen;
            //         udp.Send("S");
            //         break;
            //     case
            //         Key.D:
            //         DKey.Background = Brushes.LimeGreen;
            //         udp.Send("D");
            //         break;
            // }
        }

        private void OnKeyUp(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    WKey.Background = Brushes.Gray;
                    Console.WriteLine("W released via keyboard"); break;
                case Key.A:
                    AKey.Background = Brushes.Gray;
                    Console.WriteLine("A released via keyboard"); break;
                case Key.S:
                    SKey.Background = Brushes.Gray;
                    Console.WriteLine("S released via keyboard"); break;
                case Key.D:
                    DKey.Background = Brushes.Gray;
                    Console.WriteLine("D released via keyboard"); break;
            }
        }
    }
}
