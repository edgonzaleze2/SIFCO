using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medicina.Models;

namespace Medicina.Controllers
{
    public class PacientesController : Controller
    {
        private DB.Consultas con = new DB.Consultas(); 
        // GET: Pacientes
        public ActionResult Index()
        {
            return View("~/Views/Pacientes/Index.cshtml");
        }

        [HttpPost]
        public ActionResult IngresarPaciente(Paciente paciente) {
            ResultModel result = new Models.ResultModel();
            result.Estado = false;
            try
            {
              result.Estado =   con.IngresarPaciente(paciente);

            }
            catch (Exception)
            {
                result.Estado = false; 
            }
           
            return Json(result); 
        }

        [HttpGet]
        public ActionResult ConsultarPaciente() {
            ResultModel result = new ResultModel();
            result.Estado = false;
            try
            {
                result.objeto = con.Consultarpaciente(); 
            }
            catch (Exception ex)
            {
                Utilidades.Logs.RegistrarLog("problemas al consulta" + ex.Message);
            }
            return Json(result.objeto);
        }

    }
}