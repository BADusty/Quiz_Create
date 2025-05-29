using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Quiz_Create.Model.Export
{
    using Model;
    using static Quiz_Create.Model.Export.QuizExportModel;
    using System.Text.Json;
    using System.Collections.ObjectModel;

    interface IQuizSaveService
    {
        void SaveQuiz(Quiz database, string filePath);
    }

    interface IQuizLoadService
    {
        Quiz LoadQuiz(string Filepath);
    }

    #region JSON Save
    class QuizSaveServiceJSON : IQuizSaveService
    {
        public void SaveQuiz(Quiz database, string filePath)
        {
            var export = new QuizExport
            {
                QuizName = database.quizName,
                Questions = database.Questions.Select(q => new QuestionExport
                {
                    ID = q.ID,
                    Question = q.questionString,
                    Answers = q.Answers.Select(a => new AnswerExport
                    {
                        Answer = a.answerString,
                        Correct = a.isCorrect,
                        UserChosen = a.isUserChecked
                    }).ToList()
                }).ToList()
            };

            string json = JsonSerializer.Serialize(export, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            byte[] encrypted = IO.AES.EncryptString(json);

            File.WriteAllBytes(filePath, encrypted);
        }
    }
    #endregion

    #region JSON Load
    class QuizLoadServiceJSON : IQuizLoadService
    {
        public Quiz LoadQuiz(string filePath)
        {
            byte[] encryptedBytes = File.ReadAllBytes(filePath);
            string decryptedJson = IO.AES.DecryptBytes(encryptedBytes);
            var import = JsonSerializer.Deserialize<QuizExport>(decryptedJson);

            var quiz = new Quiz
            {
                quizName = import.QuizName,
                Questions = new ObservableCollection<Question>(
                    import.Questions.Select(q => new Question
                    {
                        ID = q.ID,
                        questionString = q.Question,
                        Answers = new List<Answer>(
                            q.Answers.Select(a => new Answer
                            {
                                answerString = a.Answer,
                                isCorrect = a.Correct,
                                isUserChecked = a.UserChosen
                            })
                        )
                    })
                )
            };
            return quiz;
        }
    }
    #endregion
}
