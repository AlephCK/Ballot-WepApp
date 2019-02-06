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
    public class CategoriaController : Controller
    {
        private BallotDBEntities db = new BallotDBEntities();

        // GET: Categoria
        public ActionResult Index()
        {
            var categorias = db.Categorias.Include(c => c.Autor).Include(c => c.Estado);
            return View(categorias.ToList());
        }

        // GET: Categoria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: Categoria/Create
        public ActionResult Create()
        {
            ViewBag.ID_Autor = new SelectList(db.Autors, "ID_Autor", "ID_Autor");
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo");
            return View();
        }

        // POST: Categoria/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Categoria,Nombre,ID_Autor,Fecha_Creacion,ID_Estado")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Autor = new SelectList(db.Autors, "ID_Autor", "ID_Autor", categoria.ID_Autor);
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo", categoria.ID_Estado);
            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Autor = new SelectList(db.Autors, "ID_Autor", "ID_Autor", categoria.ID_Autor);
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo", categoria.ID_Estado);
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Categoria,Nombre,ID_Autor,Fecha_Creacion,ID_Estado")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Autor = new SelectList(db.Autors, "ID_Autor", "ID_Autor", categoria.ID_Autor);
            ViewBag.ID_Estado = new SelectList(db.Estadoes, "ID_Estado", "Titulo", categoria.ID_Estado);
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categoria categoria = db.Categorias.Find(id);
            db.Categorias.Remove(categoria);
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
