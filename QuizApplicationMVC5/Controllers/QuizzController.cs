using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using QuizApplicationMVC5.EDMX;
using QuizApplicationMVC5.viewModels;
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
                       }).ToList()

                   }).AsQueryable();


            }

            return View(questions);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,NormalUser")]
        public ActionResult QuizTest(List<QuizAnswersVM> resultQuiz)
        {
            List<QuizAnswersVM> finalResultQuiz = new List<viewModels.QuizAnswersVM>();

            UserAnswer ua = new UserAnswer();
            foreach (QuizAnswersVM answser in resultQuiz)
            {
                QuizAnswersVM result = dbContext.Answers.Where(a => a.QuestionID == answser.QuestionID).Select(a => new QuizAnswersVM
                {
                    QuestionID = a.QuestionID.Value,
                    AnswerQ = a.AnswerText,
                    isCorrect = (answser.AnswerQ.ToLower().Equals(a.AnswerText.ToLower()))

                }).FirstOrDefault();

                finalResultQuiz.Add(result);

                ua.QuestionID = result.QuestionID;
                ua.UserAnswerText = result.AnswerQ;
                dbContext.UserAnswers.Add(ua);
                dbContext.SaveChanges();

            }

            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }




    }
}