using System;
using System.Windows;

namespace BoofToof
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // This method is called when the application starts.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Optional: Set up global exception handling.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Optional: Initialize any global services, logging, or a DI container here.
            // For example, if you use a dependency injection container, you might initialize it here.
            // For this example, we simply create and show the main window.
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        // Handles exceptions thrown on non-UI threads.
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception occurred: " + e.ExceptionObject.ToString(),
                            "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            // Consider logging the exception to a file or telemetry system here.
        }

        // Handles exceptions thrown on the UI thread.
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An application error occurred: " + e.Exception.Message,
                            "UI Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            // Optionally, prevent the application from crashing:
            e.Handled = true;
        }
    }
}
