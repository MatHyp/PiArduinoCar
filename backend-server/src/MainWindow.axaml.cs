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

            // Mouse click events
            WKey.PointerPressed += (s, e) => { WKey.Background = Brushes.LimeGreen; Console.WriteLine("W pressed"); };
            WKey.PointerReleased += (s, e) => { WKey.Background = Brushes.Gray; Console.WriteLine("W released"); };

            AKey.PointerPressed += (s, e) => { AKey.Background = Brushes.LimeGreen; Console.WriteLine("A pressed"); };
            AKey.PointerReleased += (s, e) => { AKey.Background = Brushes.Gray; Console.WriteLine("A released"); };

            SKey.PointerPressed += (s, e) => { SKey.Background = Brushes.LimeGreen; Console.WriteLine("S pressed"); };
            SKey.PointerReleased += (s, e) => { SKey.Background = Brushes.Gray; Console.WriteLine("S released"); };

            DKey.PointerPressed += (s, e) => { DKey.Background = Brushes.LimeGreen; Console.WriteLine("D pressed"); };
            DKey.PointerReleased += (s, e) => { DKey.Background = Brushes.Gray; Console.WriteLine("D released"); };

            // Keyboard events
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W: WKey.Background = Brushes.LimeGreen; Console.WriteLine("W pressed via keyboard"); break;
                case Key.A: AKey.Background = Brushes.LimeGreen; Console.WriteLine("A pressed via keyboard"); break;
                case Key.S: SKey.Background = Brushes.LimeGreen; Console.WriteLine("S pressed via keyboard"); break;
                case Key.D: DKey.Background = Brushes.LimeGreen; Console.WriteLine("D pressed via keyboard"); break;
            }
        }

        private void OnKeyUp(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W: WKey.Background = Brushes.Gray; Console.WriteLine("W released via keyboard"); break;
                case Key.A: AKey.Background = Brushes.Gray; Console.WriteLine("A released via keyboard"); break;
                case Key.S: SKey.Background = Brushes.Gray; Console.WriteLine("S released via keyboard"); break;
                case Key.D: DKey.Background = Brushes.Gray; Console.WriteLine("D released via keyboard"); break;
            }
        }
    }
}
