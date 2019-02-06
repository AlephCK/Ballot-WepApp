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
    public class PreguntaController : Controller
    {
        private BallotDBEntities db = new BallotDBEntities();

        // GET: Pregunta
        public ActionResult Index()
        {
            var preguntas = db.Preguntas.Include(p => p.Categoria).Include(p => p.Estado).Include(p => p.Nivel);
            return View(preguntas.ToList());
        }

        // GET: Pregunta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Preguntas.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }

        // GET: Pregunta/Create
        public ActionResult Create()
        {
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre");
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo");
            ViewBag.ID_Nivel = new SelectList(db.Nivels, "ID_Nivel", "Nombre");
            return View();
        }

        // POST: Pregunta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Pregunta,Titulo,ID_Categoria,ID_Nivel,ID_Autor,ID_Estado,Fecha_Creacion")] Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                db.Preguntas.Add(pregunta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre", pregunta.ID_Categoria);
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo", pregunta.ID_Estado);
            ViewBag.ID_Nivel = new SelectList(db.Nivels, "ID_Nivel", "Nombre", pregunta.ID_Nivel);
            return View(pregunta);
        }

        // GET: Pregunta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Preguntas.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre", pregunta.ID_Categoria);
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo", pregunta.ID_Estado);
            ViewBag.ID_Nivel = new SelectList(db.Nivels, "ID_Nivel", "Nombre", pregunta.ID_Nivel);
            return View(pregunta);
        }

        // POST: Pregunta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Pregunta,Titulo,ID_Categoria,ID_Nivel,ID_Autor,ID_Estado,Fecha_Creacion")] Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pregunta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Categoria = new SelectList(db.Categorias, "ID_Categoria", "Nombre", pregunta.ID_Categoria);
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo", pregunta.ID_Estado);
            ViewBag.ID_Nivel = new SelectList(db.Nivels, "ID_Nivel", "Nombre", pregunta.ID_Nivel);
            return View(pregunta);
        }

        // GET: Pregunta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta pregunta = db.Preguntas.Find(id);
            if (pregunta == null)
            {
                return HttpNotFound();
            }
            return View(pregunta);
        }

        // POST: Pregunta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pregunta pregunta = db.Preguntas.Find(id);
            db.Preguntas.Remove(pregunta);
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
