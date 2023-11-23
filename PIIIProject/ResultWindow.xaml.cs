using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private string _result;

        public ResultWindow(string result)
        {
            InitializeComponent();

            // Save the result for the btnSave
            _result = result;

            // Set the text to the result.
            txbResult.Text = result;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Popup a dialog to save the contents of the quiz result.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files | *.txt";

            // If they chose a valid location.
            if(saveFileDialog.ShowDialog() == true)
            {
                // Write the data.
                File.WriteAllText(saveFileDialog.FileName, _result);
            }
        }
    }
}
