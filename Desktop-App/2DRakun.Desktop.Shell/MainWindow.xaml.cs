using Microsoft.Owin.Hosting;
using Microsoft.Web.WebView2.Core;
using System;
using System.ComponentModel;
using System.Windows;
using _2DRakun;

namespace _2DRakun.Desktop.Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDisposable _webApp;

        public MainWindow()
        {
            InitializeComponent();
            
            // Ensure the database exists and has the correct schema
            DbHelper.InitializeDatabase();

            InitializeAsync();
        }

        async void InitializeAsync()
        {
            // Start the self-hosted server
            string url = "http://localhost:8080";
            _webApp = WebApp.Start<_2DRakun.Startup>(url);

            // Initialize WebView2 and navigate to the local server
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.Navigate(url);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _webApp?.Dispose();
            base.OnClosing(e);
        }
    }
}
