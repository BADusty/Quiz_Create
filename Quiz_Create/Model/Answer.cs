using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Create.Model
{
    class Answer
    {
        public string answerString { get; set; }
        public bool isCorrect { get; set; }
        public bool isUserChecked { get; set; }

        public Answer() { }

        public Answer(string answerString, bool isCorrect)
        {
            this.answerString = answerString;
            this.isCorrect = isCorrect;
            isUserChecked = false;
        }
    }
}
