using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wpBallot.Models;

namespace wpBallot.Controllers
{
    public class Respuesta_PreguntaController : Controller
    {
        private BallotDBEntities db = new BallotDBEntities();

        // GET: Respuesta_Pregunta
        public ActionResult Index()
        {
            var respuesta_Pregunta = db.Respuesta_Pregunta.Include(r => r.Pregunta);
            return View(respuesta_Pregunta.ToList());
        }

        // GET: Respuesta_Pregunta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta_Pregunta respuesta_Pregunta = db.Respuesta_Pregunta.Find(id);
            if (respuesta_Pregunta == null)
            {
                return HttpNotFound();
            }
            return View(respuesta_Pregunta);
        }

        // GET: Respuesta_Pregunta/Create
        public ActionResult Create()
        {
            ViewBag.ID_Pregunta = new SelectList(db.Preguntas, "ID_Pregunta", "Titulo");
            return View();
        }

        // POST: Respuesta_Pregunta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_RespuestaPreg,ID_Pregunta,Texto_Respuesta,RespuestaCorrecta")] Respuesta_Pregunta respuesta_Pregunta)
        {
            if (ModelState.IsValid)
            {
                db.Respuesta_Pregunta.Add(respuesta_Pregunta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Pregunta = new SelectList(db.Preguntas, "ID_Pregunta", "Titulo", respuesta_Pregunta.ID_Pregunta);
            return View(respuesta_Pregunta);
        }

        // GET: Respuesta_Pregunta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta_Pregunta respuesta_Pregunta = db.Respuesta_Pregunta.Find(id);
            if (respuesta_Pregunta == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Pregunta = new SelectList(db.Preguntas, "ID_Pregunta", "Titulo", respuesta_Pregunta.ID_Pregunta);
            return View(respuesta_Pregunta);
        }

        // POST: Respuesta_Pregunta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_RespuestaPreg,ID_Pregunta,Texto_Respuesta,RespuestaCorrecta")] Respuesta_Pregunta respuesta_Pregunta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(respuesta_Pregunta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Pregunta = new SelectList(db.Preguntas, "ID_Pregunta", "Titulo", respuesta_Pregunta.ID_Pregunta);
            return View(respuesta_Pregunta);
        }

        // GET: Respuesta_Pregunta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta_Pregunta respuesta_Pregunta = db.Respuesta_Pregunta.Find(id);
            if (respuesta_Pregunta == null)
            {
                return HttpNotFound();
            }
            return View(respuesta_Pregunta);
        }

        // POST: Respuesta_Pregunta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Respuesta_Pregunta respuesta_Pregunta = db.Respuesta_Pregunta.Find(id);
            db.Respuesta_Pregunta.Remove(respuesta_Pregunta);
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
