using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;

namespace PIIIProject.Models
{
    public class Option : ISavable
    {
        /* Backing Fields */
        private string _answer;
        private bool _correctAnswer;

        /* Constructors */
        public Option() { }
        public Option(string answer, bool correctAnswer)
        {
            Answer = answer;
            IsCorrect = correctAnswer;
        }

        /* Properties */
        public string Answer
        {
            get => _answer;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Answer cannot be null/empty.", "Answer");

                _answer = value;
            }
        }

        public bool IsCorrect
        {
            get => _correctAnswer;
            set => _correctAnswer = value;
        }

        /* Methods */
        public string Save()
        {
            return $"{Answer}{GetSeparator()}{_correctAnswer}";
        }

        public void Load(string data)
        {
            const int ANSWER_INDEX = 0;
            const int ISCORRECT_INDEX = 1;

            string[] splits = data.Split(GetSeparator()); 

            // Setup backing fields from the split
            Answer = splits[ANSWER_INDEX];
            IsCorrect = bool.Parse(splits[ISCORRECT_INDEX]);
        }

        public char GetSeparator()
        {
            return ':';
        }
    }
}
