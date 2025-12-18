using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace BackendServer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    WKey.Background = Brushes.LimeGreen;
                    break;
                case Key.A:
                    AKey.Background = Brushes.LimeGreen;
                    break;
                case Key.S:
                    SKey.Background = Brushes.LimeGreen;
                    break;
                case Key.D:
                    DKey.Background = Brushes.LimeGreen;
                    break;
            }
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
