using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WBAZTECA1.Models;

namespace WBAZTECA1.Controllers
{
    public class HomeController : Controller
    {
        AZTECA5 data = new AZTECA5();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Validar(Logeo l)
        {
            try
            {                
                
                Logeo log = data.Logeo.Where(x => x.Usuario == l.Usuario && x.Contraseña == l.Contraseña).FirstOrDefault();
                if (log == null)
                    throw new Exception("Usuario y/o contraseña Incorrecto");
                
                List<Folio> list = data.Folio.Where(x => x.idnota == log.id).ToList();
                
                Session["l"] = log;
                return View("Inicio",list);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Index");
            }
        }

        public ActionResult Inicio( )
        {
            Logeo log = new Logeo();
            if (Session["l"] == null)
            {
                return RedirectToAction("Cerrar");
            }
            else
            {
                log = (Logeo)Session["l"];
            }
            List<Folio> list = data.Folio.Where(x => x.idnota == log.id).ToList();

            return View(list);
        }

        public ActionResult Registro(Logeo l)
        {
            data.Logeo.Add(l);
            data.SaveChanges();
            data.Dispose();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            Logeo log = new Logeo();
            if (Session["l"] == null)
            {
                return RedirectToAction("Cerrar");
            }
            else
            {
                log = (Logeo)Session["l"];
            }
            Folio f = new Folio();
            f.idnota = log.id;

            List<Detalles> d = data.Detalles.ToList();
            ViewBag.idF = new SelectList(d, "id", "Componente");
            

            return View(f);
        }

        public ActionResult Create(Folio f)
        {
            try
            {
                data.Folio.Add(f);
                data.SaveChanges();
                data.Dispose();
                return RedirectToAction("Inicio");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            Logeo log = new Logeo();
            if (Session["l"] == null)
            {
                return RedirectToAction("Cerrar");
            }
            else
            {
                log = (Logeo)Session["l"];
            }
            //Todo t = new Todo();
            Folio f = new Folio();
            f = data.Folio.Include("Detalles").Where(x => x.id == id).FirstOrDefault();
            //t.IdLogeo = log.id;
            //f.idnota = log.id;

            return View(f);
        }

        public ActionResult Cerrar()
        {
            Session["l"] = null;
            return View("Index");
        }

    }
}