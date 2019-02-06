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
    public class Respuesta_Pregunta_UsuarioController : Controller
    {
        private BallotDBEntities db = new BallotDBEntities();

        // GET: Respuesta_Pregunta_Usuario
        public ActionResult Index()
        {
            var respuesta_Pregunta_Usuario = db.Respuesta_Pregunta_Usuario.Include(r => r.Respuesta_Pregunta).Include(r => r.Usuario);
            return View(respuesta_Pregunta_Usuario.ToList());
        }

        // GET: Respuesta_Pregunta_Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta_Pregunta_Usuario respuesta_Pregunta_Usuario = db.Respuesta_Pregunta_Usuario.Find(id);
            if (respuesta_Pregunta_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(respuesta_Pregunta_Usuario);
        }

        // GET: Respuesta_Pregunta_Usuario/Create
        public ActionResult Create()
        {
            ViewBag.ID_RespuestaPreg = new SelectList(db.Respuesta_Pregunta, "ID_RespuestaPreg", "Texto_Respuesta");
            ViewBag.ID_Usuario = new SelectList(db.Usuarios, "ID_Usuario", "Nombre");
            return View();
        }

        // POST: Respuesta_Pregunta_Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_RespuestaUsuario,ID_RespuestaPreg,ID_Usuario,RespuestaSeleccionada,Fecha_Respuesta")] Respuesta_Pregunta_Usuario respuesta_Pregunta_Usuario)
        {
            if (ModelState.IsValid)
            {
                db.Respuesta_Pregunta_Usuario.Add(respuesta_Pregunta_Usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_RespuestaPreg = new SelectList(db.Respuesta_Pregunta, "ID_RespuestaPreg", "Texto_Respuesta", respuesta_Pregunta_Usuario.ID_RespuestaPreg);
            ViewBag.ID_Usuario = new SelectList(db.Usuarios, "ID_Usuario", "Nombre", respuesta_Pregunta_Usuario.ID_Usuario);
            return View(respuesta_Pregunta_Usuario);
        }

        // GET: Respuesta_Pregunta_Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta_Pregunta_Usuario respuesta_Pregunta_Usuario = db.Respuesta_Pregunta_Usuario.Find(id);
            if (respuesta_Pregunta_Usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_RespuestaPreg = new SelectList(db.Respuesta_Pregunta, "ID_RespuestaPreg", "Texto_Respuesta", respuesta_Pregunta_Usuario.ID_RespuestaPreg);
            ViewBag.ID_Usuario = new SelectList(db.Usuarios, "ID_Usuario", "Nombre", respuesta_Pregunta_Usuario.ID_Usuario);
            return View(respuesta_Pregunta_Usuario);
        }

        // POST: Respuesta_Pregunta_Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_RespuestaUsuario,ID_RespuestaPreg,ID_Usuario,RespuestaSeleccionada,Fecha_Respuesta")] Respuesta_Pregunta_Usuario respuesta_Pregunta_Usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(respuesta_Pregunta_Usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_RespuestaPreg = new SelectList(db.Respuesta_Pregunta, "ID_RespuestaPreg", "Texto_Respuesta", respuesta_Pregunta_Usuario.ID_RespuestaPreg);
            ViewBag.ID_Usuario = new SelectList(db.Usuarios, "ID_Usuario", "Nombre", respuesta_Pregunta_Usuario.ID_Usuario);
            return View(respuesta_Pregunta_Usuario);
        }

        // GET: Respuesta_Pregunta_Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta_Pregunta_Usuario respuesta_Pregunta_Usuario = db.Respuesta_Pregunta_Usuario.Find(id);
            if (respuesta_Pregunta_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(respuesta_Pregunta_Usuario);
        }

        // POST: Respuesta_Pregunta_Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Respuesta_Pregunta_Usuario respuesta_Pregunta_Usuario = db.Respuesta_Pregunta_Usuario.Find(id);
            db.Respuesta_Pregunta_Usuario.Remove(respuesta_Pregunta_Usuario);
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
