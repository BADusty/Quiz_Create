using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Create.Model
{
    class Quiz
    {
        public string? quizName { get; set; }
        public ObservableCollection<Question> Questions { get; set; }

        public Quiz() 
        {
            Questions = new ObservableCollection<Question>();
        }

        public void ClearQuestions()
        {
            Questions.Clear();
        }
    }
}
