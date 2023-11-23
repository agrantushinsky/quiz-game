using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace PIIIProject.Models
{
    public class Quiz : ISavable, IExportable
    {
        /* Backing Fields */
        private string _title;
        private string _description;
        private Question[] _questions;

        /* Constructors */
        public Quiz() { }
        public Quiz(string title, string description, Question[] questions)
        {
            Title = title;
            Description = description;
            Questions = questions;
        }

        /* Methods */
        public string Save()
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Append the Title and Descriptoin on their own line.
            stringBuilder.AppendLine(Title);
            stringBuilder.AppendLine(Description);

            // Append each question on their own line
            foreach(Question question in Questions)
                stringBuilder.AppendLine(question.Save());

            // Return the result.
            return stringBuilder.ToString();
        }

        public void Load(string data)
        {
            // Remove carriage returns (notepad was saving with \r\n line endings)
            data = data.Replace("\r", "");

            const int TITLE_INDEX = 0;
            const int DESCRIPTION_INDEX = 1;
            const int QUESTIONS_OFFSET = 2;

            // Since we are splitting by \n, it is a good idea to remove empty entries
            // because there may be an empty line at the end of the file.
            string[] lines = data.Split(GetSeparator(), StringSplitOptions.RemoveEmptyEntries);

            Title = lines[TITLE_INDEX];
            Description = lines[DESCRIPTION_INDEX];

            // Create new Question array accounting for the offset.
            Questions = new Question[lines.Length - QUESTIONS_OFFSET];

            for(int i = QUESTIONS_OFFSET; i < lines.Length; i++)
            {
                // Create question object
                Questions[i - QUESTIONS_OFFSET] = new Question();

                // Load the data
                Questions[i - QUESTIONS_OFFSET].Load(lines[i]);
            }
        }

        public char GetSeparator()
        {
            return '\n';
        }

        public static string GetExtension()
        {
            return ".quiz";
        }

        public void StartQuiz()
        {
            // If the user does the same quiz twice, Status will contain the wrong results.
            foreach (Question question in Questions)
                question.Status = QuestionStatus.Unanswered;
        }

        public bool IsComplete()
        {
            // If it finds it an Unanswered question, return false.
            foreach (Question question in Questions)
                if (question.Status == QuestionStatus.Unanswered)
                    return false;

            return true;
        }

        public void InputResult(int questionIndex, int optionIndex)
        {
            // Error checking the indices in done at the Window level.
            Question question = Questions[questionIndex];
            Option option = question.Options[optionIndex];

            // If the option was correct, set the status to correct, otherwise, set it to incorrect.
            question.Status = option.IsCorrect ? QuestionStatus.Correct : QuestionStatus.Incorrect;

            // Set the PickedIndex
            question.PickedIndex = optionIndex;
        }

        public int GetScore()
        {
            // Start with full score, then remove the wrong answers from it.
            int score = GetTotalQuestions();
            foreach (Question question in Questions)
            {
                // If Status is < Correct, it must be Unanswered or Incorrect, therefore wrong.
                // Decrement the score.
                if (question.Status < QuestionStatus.Correct)
                    score--;
            }

            return score;
        }

        public int GetTotalQuestions()
        {
            return Questions.Length;
        }

        public string GetResult()
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Append title
            stringBuilder.AppendLine($"Title: {Title}");

            // Append time stamp
            stringBuilder.AppendLine($"Completed: {DateTime.Now}\n");

            // Append the results
            stringBuilder.AppendLine($"Results:");

            for(int i = 0; i < Questions.Length; i++)
            {
                Question question = Questions[i];

                // Append question text and the users answer
                stringBuilder.AppendLine($"Question #{i+1}: {question.QuestionText}\n" +
                    $"Your answer: {question.PickedAnswer}");

                // If they were correct, let them know.
                if(question.Status == QuestionStatus.Correct)
                {
                    stringBuilder.AppendLine("Correct!");
                }
                else
                {
                    // If incorrect, tell them the correct answer.
                    stringBuilder.AppendLine("Wrong.");
                    stringBuilder.AppendLine($"Correct Answer: {question.CorrectAnswer}");
                }
                stringBuilder.AppendLine();
            }

            // Calculate percentage from their score 
            string percentage = ((double)GetScore() / GetTotalQuestions() * 100).ToString("0.##");

            // Append their score with the percentage.
            stringBuilder.AppendLine($"Scored: {GetScore()}/{GetTotalQuestions()} ({percentage}%)");

            return stringBuilder.ToString();
        }

        public void Export(string fileName)
        {
            // Write the string from Save()
            File.WriteAllText(fileName, Save());
        }

        public void Import(string fileName)
        {
            // Read the data
            string data = File.ReadAllText(fileName);

            // Load the data into the current instance.
            Load(data);
        }

        /* Properties */
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Title cannot be null/empty.", "Title");

                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Description cannot be null/empty.", "Description");

                _description = value;
            }
        }

        public Question[] Questions
        {
            get => _questions;
            private set => _questions = value;
        }
    }
}
