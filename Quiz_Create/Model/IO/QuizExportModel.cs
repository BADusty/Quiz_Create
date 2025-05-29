using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Create.Model.Export
{
    public class QuizExportModel
    {
        public class QuizExport
        {
            public string QuizName { get; set; }
            public List<QuestionExport> Questions { get; set; }
        }

        public class QuestionExport
        {
            public int ID { get; set; }
            public string Question { get; set; }
            public List<AnswerExport> Answers { get; set; }
        }

        public class AnswerExport
        {
            public string Answer { get; set; }
            public bool Correct { get; set; }
            public bool UserChosen { get; set; }
        }
    }
}
