using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Taken from CSConferenceApp:
            MessageBox.Show("Exception not handles:\n" + e.Exception.Message, "Error");

            // Let the app continue running.
            e.Handled = true;
        }
    }
}
