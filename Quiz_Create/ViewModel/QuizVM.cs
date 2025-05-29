using Quiz_Create.Model;
using Quiz_Create.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;

namespace Quiz_Create.ViewModel
{
    using Model;
    using BaseClass;
    using Model.Export;
    using System.Globalization;
    using System.Windows.Data;

    internal class QuizVM : ViewModelBase
    {
        private Quiz _database = new Model.Quiz();
        private readonly IQuizSaveService _saveService = new QuizSaveServiceJSON();
        private readonly IQuizLoadService _loadService = new QuizLoadServiceJSON();
        public ObservableCollection<Question> Questions
        {
            get => _database.Questions;
            set => _database.Questions = value;
        }

        #region Binded Strings
        public string tx_quizNameString { get; set; }
        public string tx_questionString { get; set; }

        public string tx_answerString1 { get; set; }
        public string tx_answerString2 { get; set; }
        public string tx_answerString3 { get; set; }
        public string tx_answerString4 { get; set; }
        #endregion

        #region SaveQuestion Command
        private ICommand saveQuestion;
        public ICommand SaveQuestion => (saveQuestion ?? (saveQuestion = new RelayCommand(
            p =>
            {
                var quizAnswers = new List<Answer>
                {
                    new Answer(tx_answerString1, IsAnswerCorrect1),
                    new Answer(tx_answerString2, IsAnswerCorrect2),
                    new Answer(tx_answerString3, IsAnswerCorrect3),
                    new Answer(tx_answerString4, IsAnswerCorrect4)
                };

                if (SelectedQuestion != null)
                {
                    SelectedQuestion.questionString = tx_questionString;
                    SelectedQuestion.Answers = quizAnswers;
                }

                else
                {
                    int id = Questions.Count + 1;
                    var question = new Question(id, tx_questionString, quizAnswers);
                    Questions.Add(question);
                }

                ClearForm.Execute(null);
                OnPropertyChanged(nameof(Questions));
            }
            ,
            p => !string.IsNullOrWhiteSpace(tx_questionString) && !string.IsNullOrWhiteSpace(tx_answerString1)
                    && !string.IsNullOrWhiteSpace(tx_answerString2) && !string.IsNullOrWhiteSpace(tx_answerString3)
                    && !string.IsNullOrWhiteSpace(tx_answerString4) && 
                    !(IsAnswerCorrect1 == false && IsAnswerCorrect2 == false && IsAnswerCorrect3 == false && IsAnswerCorrect4 == false)
            )));
        #endregion

        #region SelectedQuestion Command
        // To jest zrobione, by sprawdzać, czy jakieś pytanie jest zaznaczone
        // Potrzebne do edytowania i usuwania pytań ✨✨
        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));

                if (value != null)
                {
                    tx_questionString = value.questionString;
                    tx_answerString1 = value.Answers[0].answerString;
                    IsAnswerCorrect1 = value.Answers[0].isCorrect;
                    tx_answerString2 = value.Answers[1].answerString;
                    IsAnswerCorrect2 = value.Answers[1].isCorrect;
                    tx_answerString3 = value.Answers[2].answerString;
                    IsAnswerCorrect3 = value.Answers[2].isCorrect;
                    tx_answerString4 = value.Answers[3].answerString;
                    IsAnswerCorrect4 = value.Answers[3].isCorrect;
                    OnPropertyChanged(nameof(tx_questionString), nameof(tx_answerString1),
                        nameof(tx_answerString2), nameof(tx_answerString3), nameof(tx_answerString4));
                }
            }
        }
        #endregion

        #region DeleteQuestion Command
        private ICommand deleteQuestion;
        public ICommand DeleteQuestion => deleteQuestion ??= new RelayCommand(
            p =>
            {
                if (SelectedQuestion != null)
                {
                    Questions.Remove(SelectedQuestion);

                    for (int i = 0; i < Questions.Count; i++)
                        Questions[i].ID = i + 1;

                    _database.Questions = Questions;

                    SelectedQuestion = null;
                    ClearForm.Execute(null);

                    OnPropertyChanged(nameof(Questions));
                }
            },
            p => SelectedQuestion != null
        );
        #endregion

        #region Answer Checks
        #region Answer1 Check
        private bool isAnswerCorrect1;
        public bool IsAnswerCorrect1
        {
            get => isAnswerCorrect1;
            set
            {
                if (isAnswerCorrect1 != value)
                {
                    isAnswerCorrect1 = value;
                    OnPropertyChanged(nameof(IsAnswerCorrect1));
                }
            }
        }
        #endregion

        #region Answer2 Check
        private bool isAnswerCorrect2;
        public bool IsAnswerCorrect2
        {
            get => isAnswerCorrect2;
            set
            {
                if (isAnswerCorrect2 != value)
                {
                    isAnswerCorrect2 = value;
                    OnPropertyChanged(nameof(IsAnswerCorrect2));
                }
            }
        }
        #endregion

        #region Answer3 Check
        private bool isAnswerCorrect3;
        public bool IsAnswerCorrect3
        {
            get => isAnswerCorrect3;
            set
            {
                if (isAnswerCorrect3 != value)
                {
                    isAnswerCorrect3 = value;
                    OnPropertyChanged(nameof(IsAnswerCorrect3));
                }
            }
        }
        #endregion

        #region Answer4 Check
        private bool isAnswerCorrect4;
        public bool IsAnswerCorrect4
        {
            get => isAnswerCorrect4;
            set
            {
                if (isAnswerCorrect4 != value)
                {
                    isAnswerCorrect4 = value;
                    OnPropertyChanged(nameof(IsAnswerCorrect4));
                }
            }
        }
        #endregion
        #endregion

        #region ClearForm
        private ICommand clearForm;
        public ICommand ClearForm => clearForm ??= new RelayCommand(
            p =>
            {
                SelectedQuestion = null;
                #region Clears
                tx_questionString = tx_answerString1 = tx_answerString2 = tx_answerString3 = tx_answerString4 = string.Empty;
                IsAnswerCorrect1 = IsAnswerCorrect2 = IsAnswerCorrect3 = IsAnswerCorrect4 = false;
                #endregion

                OnPropertyChanged(nameof(SelectedQuestion), nameof(tx_questionString),
                    nameof(tx_answerString1), nameof(tx_answerString2),
                    nameof(tx_answerString3), nameof(tx_answerString4));
            },
            p => true
        );
        #endregion

        #region ClearDatabase Command
        private ICommand clearDatabase;
        public ICommand ClearDatabase => (clearDatabase ??= new RelayCommand(
            p =>
            {
                _database.ClearQuestions();
                OnPropertyChanged(nameof(Questions));
            }
            ,
            p => Questions.Count > 0
            ));
        #endregion

        #region JSON SaveToFile
        private ICommand savetoFile;
        public ICommand SavetoFile => (savetoFile ??= new RelayCommand(
            p =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Filter = "Pliki JSON (*.json)|*.json",
                    Title = "Zapisz QUIZ",
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    _database.quizName = tx_quizNameString;
                    string filePath = saveFileDialog.FileName;
                    _saveService.SaveQuiz(_database, filePath);

                    Questions.Clear();
                    tx_quizNameString = string.Empty;
                    ClearForm.Execute(null);
                    OnPropertyChanged(nameof(Questions), nameof(tx_quizNameString));
                }
            }
            ,
            p => !string.IsNullOrEmpty(tx_quizNameString) && Questions.Count > 0
            ));
        #endregion

        #region JSON LoadFromFile
        private ICommand loadFromFile;
        public ICommand LoadFromFile => (loadFromFile ??= new RelayCommand(
            p =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Pliki JSON (*.json)|*.json",
                    Title = "Wczytaj QUIZ"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    var loadedQuiz = _loadService.LoadQuiz(filePath);

                    _database.quizName = tx_quizNameString = loadedQuiz.quizName;

                    _database.Questions.Clear();
                    foreach (var q in loadedQuiz.Questions)
                        _database.Questions.Add(q);

                    Questions.Clear();
                    foreach (var q in loadedQuiz.Questions)
                        Questions.Add(q);

                    OnPropertyChanged(nameof(Questions), nameof(tx_quizNameString));
                }
            }
            ,
            p => true
            ));
        #endregion
    }
}
