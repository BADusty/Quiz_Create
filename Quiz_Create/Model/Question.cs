using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Create.Model
{
    class Question : INotifyPropertyChanged
    {
        public int ID { get; set; }

        private string _questionString;
        public string questionString
        {
            get => _questionString;
            set
            {
                _questionString = value;
                OnPropertyChanged(nameof(questionString));
            }
        }

        private List<Answer> _answers;
        public List<Answer> Answers
        {
            get => _answers;
            set
            {
                _answers = value;
                OnPropertyChanged(nameof(Answers));
            }
        }

        public Question() { }

        public Question(int ID, string questionString, List<Answer> answers)
        {
            this.ID = ID;
            this.questionString = questionString;
            Answers = [.. answers]; //To podpowiedziało mi Visual Studio. Nawet nie wiedziałem, że tak się da
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
