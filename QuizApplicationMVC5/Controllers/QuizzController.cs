using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using QuizApplicationMVC5.EDMX;
using QuizApplicationMVC5.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace QuizApplicationMVC5.Controllers
{
    public class QuizzController : Controller
    {

        private DBQuizEntities dbContext = new DBQuizEntities();
        private static UserVM userConnected { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUser(UserVM user)
        {
            userConnected = dbContext.Users.Where(u => u.UserName == user.UserName && u.Password == user.Password)
                                         .Select(u => new UserVM
                                         {
                                             UserID = u.UserID,
                                             UserName = u.UserName,
                                             Password = u.Password,
                                             TypeUser = u.TypeUser,
                                             FullName = u.FullName,
                                             ProfilImage = u.ProfilImage,

                                         }).FirstOrDefault();

            if (userConnected != null)
            {
                User_Role_Manager(userConnected);

                return RedirectToAction("SelectQuizz");
            }
            else
            {
                ViewBag.Msg = "Sorry : user and/or password were incorrect !!";
                return View();
            }

        }

        private void User_Role_Manager(UserVM userConnected)
        {
            //Asignación de Permisos
            var ident = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, userConnected.UserID.ToString()),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim(ClaimTypes.Name,userConnected.FullName),
                    }, DefaultAuthenticationTypes.ApplicationCookie);

            //Asignación del rol correspondiente
            switch (userConnected.TypeUser)
            {
                case "Admin":
                    ident.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    break;

                case "NormalUser":
                    ident.AddClaim(new Claim(ClaimTypes.Role, "NormalUser"));
                    break;
            }


            HttpContext.GetOwinContext().Authentication.SignIn(
            new AuthenticationProperties { IsPersistent = false }, ident);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Log_Off()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
            new AuthenticationProperties { IsPersistent = false });

            return RedirectToAction("GetUser");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,NormalUser")]
        public ActionResult SelectQuizz()
        {
            QuizVM quiz = new viewModels.QuizVM();
            quiz.ListOfQuizz = dbContext.Quizs.Select(q => new SelectListItem
            {
                Text = q.QuizName,
                Value = q.QuizID.ToString()

            }).ToList();

            //Carga de Datos de Usuario si se borró los datos del Session
            if (userConnected == null)
            {
                string id = User.Identity.GetUserId();

                userConnected = dbContext.Users.Where(u => u.UserID.ToString() == id)
                                        .Select(u => new UserVM
                                        {
                                            UserID = u.UserID,
                                            UserName = u.UserName,
                                            Password = u.Password,
                                            TypeUser = u.TypeUser,
                                            FullName = u.FullName,
                                            ProfilImage = u.ProfilImage,

                                        }).FirstOrDefault();
            }

            Session["UserConnected"] = userConnected;

            return View(quiz);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,NormalUser")]
        public ActionResult SelectQuizz(QuizVM quiz)
        {
            QuizVM quizSelected = dbContext.Quizs.Where(q => q.QuizID == quiz.QuizID).Select(q => new QuizVM
            {
                QuizID = q.QuizID,
                QuizName = q.QuizName,

            }).FirstOrDefault();

            if (quizSelected != null)
            {
                Session["SelectedQuiz"] = quizSelected;

                return RedirectToAction("QuizTest");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,NormalUser")]
        public ActionResult QuizTest()
        {
            QuizVM quizSelected = Session["SelectedQuiz"] as QuizVM;
            IQueryable<QuestionVM> questions = null;

            Random random = new Random();

            if (quizSelected != null)
            {
                questions = dbContext.Questions.Where(q => q.Quiz.QuizID == quizSelected.QuizID)
                   .Select(q => new QuestionVM
                   {
                       QuestionID = q.QuestionID,
                       QuestionText = q.QuestionText,
                       Choices = q.Choices.Select(c => new ChoiceVM
                       {
                           ChoiceID = c.ChoiceID,
                           ChoiceText = c.ChoiceText
                       }).ToList(),
                       Answers = q.Answers.Select(c => new AnswerVM
                       {
                           AnswerID = c.AnswerID,
                           AnswerText = c.AnswerText
                       }).ToList(),

                       listado_opciones = q.Choices.Select(x => x.ChoiceText).Union(q.Answers.Select(y => y.AnswerText)).ToList()

                   }).AsQueryable();

            }
            else
            {
                return RedirectToAction("SelectQuizz");
            }

            return View(questions);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,NormalUser")]
        public ActionResult QuizTest(List<QuizAnswersVM> resultQuiz)
        {
            List<QuizAnswersVM> finalResultQuiz = new List<QuizAnswersVM>();

            foreach (QuizAnswersVM answser in resultQuiz)
            {
                QuizAnswersVM result = dbContext.Answers.Where(a => a.QuestionID == answser.QuestionID).Select(a => new QuizAnswersVM
                {
                    QuestionID = a.QuestionID.Value,
                    AnswerQ = a.AnswerText,
                    isCorrect = (answser.AnswerQ.ToLower().Equals(a.AnswerText.ToLower()))

                }).FirstOrDefault();

                finalResultQuiz.Add(result);

                try
                {
                    bool chech_answer = dbContext.Answers.Select(x => x.AnswerText).ToList().Contains(answser.AnswerQ);

                    var update_resultados = resultQuiz.Where(x => x.AnswerQ == answser.AnswerQ).Select(y => new UserAnswer
                    {
                        QuestionID = y.QuestionID,
                        UserAnswerText = y.AnswerQ,
                        Is_Answer = chech_answer

                    }).FirstOrDefault();

                    dbContext.UserAnswers.Add(update_resultados);

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }

        private int[] getFormatoListado(string list_id)
        {
            var allowedChars = "01234567890,";
            list_id = new string(list_id.Where(c => allowedChars.Contains(c)).ToArray());
            
            return list_id.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
        }

        public JsonResult ObtenerDatos(string data)
        {
            int c_result = 0;
            int i_result = 0;

            if(data != "[]")
            {
                int[] listado_ids = getFormatoListado(data);

                foreach (int id in listado_ids)
                {
                    c_result += dbContext.UserAnswers.Where(x => x.Is_Answer == true && x.QuestionID == id).Count();
                    i_result += dbContext.UserAnswers.Where(x => x.Is_Answer == false && x.QuestionID == id).Count();
                }
            }

            return Json( new { c_result = c_result, i_result = i_result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,NormalUser")]
        public ActionResult Statistics()
        {
            var listado_estadisticas = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Gráfico Circular", Value = "pie"},
                new SelectListItem { Text = "Gráfico de Área Polar", Value = "polarArea"},
                new SelectListItem { Text = "Barras Verticales", Value = "bar"},
                new SelectListItem { Text = "Gráfico de Linea", Value = "line"},
                new SelectListItem { Text = "Radar", Value = "radar"},
            }, "Value", "Text");

            ViewBag.listado_estadisticas = listado_estadisticas;

            var listado_preguntas = dbContext.Questions.ToList().Select(u => new SelectListItem
            {
                Text = u.QuestionText,
                Value = u.QuestionID.ToString()
            });

            ViewBag.listado_preguntas = listado_preguntas;

            ViewBag.C_Choices = dbContext.UserAnswers.Where(x => x.Is_Answer == true).Count();
            ViewBag.I_Choices = dbContext.UserAnswers.Where(x => x.Is_Answer == false).Count();

            return View();
        }


    }
}