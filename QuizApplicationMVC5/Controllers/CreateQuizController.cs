﻿using System;
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
    public class CreateQuizController : Controller
    {
        private DBQuizEntities db = new DBQuizEntities();

        // GET: CreateQuiz
        [Authorize(Roles = "Admin")]

        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var quizz = from q in db.Quizs
                           select q;
            if (!String.IsNullOrEmpty(searchString))
            {
                quizz = quizz.Where(q => q.QuizName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    quizz = quizz.OrderByDescending(q => q.QuizName);
                    break;
                case "author":
                    quizz = quizz.OrderByDescending(q => q.Author);
                    break;
                case "category":
                    quizz = quizz.OrderByDescending(q => q.Category);
                    break;
                default:
                    quizz = quizz.OrderBy(q => q.QuizName);
                    break;
            }
            return View(quizz.ToList());
        }

        // GET: CreateQuiz/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: CreateQuiz/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateQuiz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "QuizID,QuizName,Category,Author")] Quiz quiz)
        {
            quiz.Author = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Quizs.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quiz);
        }

        // GET: CreateQuiz/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: CreateQuiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "QuizID,QuizName,Category,Author")] Quiz quiz)
        {
            quiz.Author = User.Identity.Name;

            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        // GET: CreateQuiz/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: CreateQuiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Quiz quiz = db.Quizs.Find(id);
            db.Quizs.Remove(quiz);
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
