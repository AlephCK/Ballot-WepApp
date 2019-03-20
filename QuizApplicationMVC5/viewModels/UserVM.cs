using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApplicationMVC5.viewModels
{
    public class UserVM
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TypeUser { get; set; }
        public string FullName { get; set; }
        public string ProfilImage { get; set; }
    }

    public class QuizVM
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
        public List<SelectListItem> ListOfQuizz { get; set; }

    }

    public class QuestionVM
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string Anwser { get; set; }
        public ICollection<ChoiceVM> Choices { get; set; }
        public ICollection<AnswerVM> Answers { get; set; }

        private List<string> _listado_opciones;
        public List<string> listado_opciones
        {
            get
            {
                return Shuffle(_listado_opciones);
            }
            set
            {
                _listado_opciones = value;
            }
        }

        private static List<T> Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }

    public class ChoiceVM
    {
        public int ChoiceID { get; set; }
        public string ChoiceText { get; set; }

    }

    public class AnswerVM
    {
        public int AnswerID { get; set; }
        public string AnswerText { get; set; }

    }

    public class QuizAnswersVM
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string AnswerQ { get; set; }
        public bool isCorrect { get; set; }


    }
}