using PIIIProject.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        private Quiz _quiz;

        public QuizWindow(Quiz quiz)
        {
            InitializeComponent();

            // Set the DataContext
            this.DataContext = quiz;
            
            // Save the reference
            _quiz = quiz;

            // Populate the Questions list view
            lsvQuestions.ItemsSource = quiz.Questions;

            // Call StartQuiz() to ensure that the Results are reset
            _quiz.StartQuiz();
        }

        private void lsvQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the select question index
            int selectedQuestion = lsvQuestions.SelectedIndex;

            // When the question selection changes, the options must follow:
            lsvOptions.ItemsSource = _quiz.Questions[selectedQuestion].Options;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // Shift by -1 to go up.
            ShiftQuestionSelection(1);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            // Shift by 1 to go down.
            ShiftQuestionSelection(-1);
        }

        private void ShiftQuestionSelection(int amount)
        {
            // Grab the selected index
            int selected = lsvQuestions.SelectedIndex;
            
            // Add the amount to selected
            selected += amount;

            // Clamp the selected index within the bounds of the number of questions.
            lsvQuestions.SelectedIndex = Math.Clamp(selected, 0, _quiz.Questions.Length);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int selectedQuestion = lsvQuestions.SelectedIndex;
            int selectedOption = lsvOptions.SelectedIndex;

            // If they haven't selected a question or option yet, just silently return
            // There is no need to have a annoying error message for something trivial.
            if (selectedQuestion == -1 || selectedOption == -1)
                return;

            // Don't allow the user to answer the same question again.
            if (_quiz.Questions[selectedQuestion].Status != QuestionStatus.Unanswered)
                return;

            // Input the user's choice.
            _quiz.InputResult(selectedQuestion, selectedOption);

            // Move to the next question if possible.
            ShiftQuestionSelection(1);

            // Refresh the lsvQuestions to update the status column
            lsvQuestions.Items.Refresh();
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user if they want to finish before answer all questions
            if(!_quiz.IsComplete())
            {
                // If the user wants to continue their quiz, don't finish.
                if (MessageBox.Show("Are you sure want to finish early?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }

            // Show the results in the ResultWindow
            ResultWindow resultWindow = new ResultWindow(_quiz.GetResult());

            // ShowDialog so if the user does the quiz again, they can't see the correct answers from this application.
            resultWindow.ShowDialog();

            // Close the current quiz window (will happen after ShowDialog() is finished executing)
            this.Close();
        }
    }
}
