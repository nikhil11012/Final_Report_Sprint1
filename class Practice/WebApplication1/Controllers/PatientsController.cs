using System.Security.Cryptography.Pkcs;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PatientsController : Controller
    {
        private readonly CRUD _context;
        public PatientsController(CRUD context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            List<Patient> patients = _context.GetPatients();
            return View(patients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Patient p)
        {
            _context.AddPatient(p);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Patient p = _context.GetPatientById(id);
            return View(p);
        }
        [HttpPost]
        public IActionResult Edit(Patient p)
        {
            _context.UpdatePatient(p);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Patient p = _context.GetPatientById(id);
            return View(p);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(Patient p)
        {
            TempData["Message"] = _context.DeletePatient(p.Id);
            return RedirectToAction("Index");
        }
    }
    }