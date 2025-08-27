using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;

namespace BackendServer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Start backend servers in background threads
            Task.Run(() =>
            {
                
            });

            // Start Avalonia GUI
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
