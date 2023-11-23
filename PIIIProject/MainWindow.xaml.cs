using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
// Required to remove ambiguity error.
using Path = System.IO.Path;
using PIIIProject.Models;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Make a placeholder quiz
            _placeholder = new Quiz("Select a quiz...", "Select a quiz...", null);

            // Set the placeholder to the DataContext
            stkSelectedQuiz.DataContext = _placeholder;

            // Create a list of blacklist quizzes for ones that 
            // failed to parse:
            _blackListedQuizzes = new List<int>();
        }
        private Quiz _placeholder;
        private string[] _quizFiles;
        private string _selected;
        private int _selectedIndex;

        private Quiz[] _quizzes;

        // I am sorry.
        private List<int> _blackListedQuizzes;

        private void lsvFilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // When the selection changes update the Quizzes
            // If it fails, we should return.
            UpdateQuizzes();

            // Update the selectd index
            UpdateSelectedIndex();
            
            // Set selected backing field to the file path for the quiz.
            if(_selectedIndex != -1)
                _selected = _quizFiles[_selectedIndex];
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Open a dialog to select a quiz.
            // We will also find all the quizzes found in the same directory.
            // Unfortunately, WPF doesn't not have a folder browse.

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Quiz Files | *.quiz";

            // Clear the blacklist
            _blackListedQuizzes.Clear();

            // If they selected a file.
            if(openFileDialog.ShowDialog() == true)
            {
                // Set the backing field with the file name
                _selected = openFileDialog.FileName;

                // Get the directory of the file
                string directory = Path.GetDirectoryName(_selected);

                // Get an array of file with the type .quiz
                _quizFiles = Directory.GetFiles(directory, $"*{Quiz.GetExtension()}");
            }

            // If quiz files were found:
            if(_quizFiles != null && _quizFiles.Length > 0 
                && _selected != null)
            {
                // Include only the file name in the listview
                string[] fileNames = new string[_quizFiles.Length];

                // Populate the fileNames array
                for(int i = 0; i < _quizFiles.Length; i++)
                    fileNames[i] = Path.GetFileName(_quizFiles[i]);

                // Set the FileList to the fileNames.
                lsvFilesList.ItemsSource = fileNames;
                lsvFilesList.Items.Refresh(); // refresh.

                // Set the selecteditem to the one from the dialog.
                lsvFilesList.SelectedItem = Path.GetFileName(_selected);
            }
        }

        private void UpdateQuizzes()
        {
            // This method updates the Quiz[] from quizFiles[]

            // If no files are found, return.
            if (_quizFiles.Length == 0)
                return;

            // Create the array of itself.
            _quizzes = new Quiz[_quizFiles.Length];

            for(int i = 0; i < _quizFiles.Length; i++)
            {
                // Do nothing on blacklisted quiz.
                if (IsQuizBlackListed(i))
                    continue;
                
                // Create an empty Quiz object.
                _quizzes[i] = new Quiz();

                // Import the data using IExportable method.
                // This exception is not being caught by the Application_DispatcherUnhandledException
                try
                {
                    _quizzes[i].Import(_quizFiles[i]);
                }
                catch(Exception e)
                {
                    ShowErrorMesssage($"Failed to parse:\n{_quizFiles[i]}\nReason: {e.Message}");

                    // Not an ideal fix, nevertheless, blacklist the broken quiz.
                    _blackListedQuizzes.Add(i);
                }
            }
        }

        private void SetSelectedQuizContext(Quiz quiz)
        {
            // DataContext so the labels can see the Quiz data.
            stkSelectedQuiz.DataContext = quiz;

            // Update the questions preview.
            lsvQuestions.ItemsSource = quiz.Questions;
        }

        private void UpdateSelectedIndex()
        {
            // Set the backing field
            _selectedIndex = lsvFilesList.SelectedIndex;

            if(IsQuizBlackListed(_selectedIndex))
            {
                ShowErrorMesssage("This quiz failed to parse, please select a different quiz.");

                // Set the preview back to the placeholder
                stkSelectedQuiz.DataContext = _placeholder;

                // Clear questions preview
                lsvQuestions.ItemsSource = null;

                // Set the selection to nothing.
                _selectedIndex = -1;
                lsvFilesList.SelectedIndex = -1;

                return;
            }

            // Update the window.
            // This if statements is necessary.
            if(_selectedIndex != -1)
                SetSelectedQuizContext(_quizzes[_selectedIndex]);
        }

        private void btnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            // If no quizzes have been or imported or the selected index is invalid,
            // give an error.
            if(_quizzes == null || _selectedIndex == -1)
            {
                ShowErrorMesssage("You must select a quiz.");
                return;
            }

            // Selected index will always be set as long as quizzes is not null.
            Quiz selectedQuiz = _quizzes[_selectedIndex];

            // Create the quizWindow as a dialog passing the selected quiz
            QuizWindow quizWindow = new QuizWindow(selectedQuiz);
            quizWindow.ShowDialog();
        }

        private void ShowErrorMesssage(string message)
        {
            // Simple error message box.
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool IsQuizBlackListed(int index)
        {
            // Return true if the index is found the list.
            foreach(int blacklist in _blackListedQuizzes)
                if(blacklist == index)
                    return true;

            return false;
        }
    }
}
