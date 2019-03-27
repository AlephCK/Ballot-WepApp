using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuizApplicationMVC5.EDMX;

namespace QuizApplicationMVC5.Controllers
{
    public class AnswerController : Controller
    {
        private DBQuizEntities db = new DBQuizEntities();

        // GET: Answer
        [Authorize(Roles = "Admin")]
        //public ActionResult Index()
        //{
        //    var answers = db.Answers.Include(a => a.Question);
        //    return View(answers.ToList());
        //}

        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var answers = db.Answers.Include(a => a.Question);
            answers = from q in db.Answers
                      select q;
            if (!String.IsNullOrEmpty(searchString))
            {
                answers = answers.Where(q => q.AnswerText.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    answers = answers.OrderByDescending(q => q.AnswerText);
                    break;
                case "Question":
                    answers = answers.OrderByDescending(q => q.Question.QuestionText);
                    break;
                default:
                    answers = answers.OrderBy(q => q.AnswerText);
                    break;
            }
            return View(answers.ToList());
        }


        // GET: Answer/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: Answer/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText");
            return View();
        }

        // POST: Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "AnswerID,AnswerText,QuestionID")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                int check_no_answers = db.Answers.Where(y => y.QuestionID == answer.QuestionID).Count();

                if (check_no_answers >= 1)
                    ModelState.AddModelError("", "Error añadiendo la respuesta. ¡Una pregunta no puede tener más de una respuesta!");

                else
                {
                    db.Answers.Add(answer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText", answer.QuestionID);
            return View(answer);
        }

        static int? question_id;
        // GET: Answer/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }

            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText", answer.QuestionID);
            question_id = answer.QuestionID;

            return View(answer);
        }

        // POST: Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "AnswerID,AnswerText,QuestionID")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                answer.QuestionID = question_id;
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            ViewBag.QuestionID = new SelectList(db.Questions, "QuestionID", "QuestionText", answer.QuestionID);
            return View(answer);
        }

        // GET: Answer/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);
            db.Answers.Remove(answer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
