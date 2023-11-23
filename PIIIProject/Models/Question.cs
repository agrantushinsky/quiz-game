using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace PIIIProject.Models
{
    // Enum to hold the possible Question states.
    public enum QuestionStatus
    {
        Unanswered,
        Incorrect,
        Correct
    }

    public class Question : ISavable
    {
        /* Backing Fields */
        private string _question;
        private Option[] _options;
        private QuestionStatus _status;
        private int _pickedIndex;

        /* Constructors */
        public Question() { }
        public Question(string question, Option[] options)
        {
            QuestionText = question;
            Options = options;
            Status = QuestionStatus.Unanswered;
        }

        /* Properties */
        public string QuestionText
        {
            get => _question;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("QuestionText cannot be null/empty.", "QuestionText");

                _question = value;
            }
        }

        public Option[] Options
        {
            get => _options;
            set
            {
                const int NUMBER_OF_OPTIONS = 4;
                // Don't allow more or less than 4 options.
                if (value.Length != NUMBER_OF_OPTIONS)
                    throw new ArgumentOutOfRangeException("Too many Options, there must be 4.", "Options");

                _options = value;
            }
        }

        public QuestionStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public int PickedIndex
        {
            get => _pickedIndex;
            set
            {
                // Allow allow PickedIndex to be within the range Options
                if (value < 0 || value > Options.Length - 1)
                    throw new IndexOutOfRangeException("PickedIndex must be in the range of Options' Length");

                _pickedIndex = value;
            }
        }

        public string PickedAnswer
        {
            get => Options[PickedIndex].Answer;
        }

        public string CorrectAnswer
        {
            get
            {
                // Once the correct answer is found, return it.
                foreach (Option option in Options)
                    if (option.IsCorrect)
                        return option.Answer;

                return string.Empty;
            }
        }

        /* Methods */
        public string Save()
        {
            StringBuilder stringBuilder = new StringBuilder();
            // Append question text
            stringBuilder.Append(QuestionText);

            // Add separator so we can then append the Options
            stringBuilder.Append(GetSeparator());

            // Loop through all options
            for(int i = 0; i < Options.Length; i++)
            {
                // Append option
                stringBuilder.Append(Options[i].Save());

                // Don't add the separator if it was the last option
                if(i < Options.Length - 1)
                    stringBuilder.Append(GetSeparator());
            }

            // Return the result.
            return stringBuilder.ToString();
        }

        public void Load(string data)
        {
            string[] splits = data.Split(GetSeparator());

            const int QUESTIONTEXT_INDEX = 0;

            const int OPTION_OFFSET = 1;

            // Load in the QuestionText
            QuestionText = splits[QUESTIONTEXT_INDEX];

            // -1 because we don't want to include the question
            Options = new Option[splits.Length - OPTION_OFFSET];

            // Start at index 1 to skip over the question text
            for (int i = OPTION_OFFSET; i < splits.Length; i++)
            {
                Options[i - OPTION_OFFSET] = new Option();
                Options[i - OPTION_OFFSET].Load(splits[i]);
            }
        }

        public char GetSeparator()
        {
            return ',';
        }
    }
}
