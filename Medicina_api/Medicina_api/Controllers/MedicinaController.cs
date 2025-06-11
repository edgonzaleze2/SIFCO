using Medicina_api.DB;
using Medicina_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medicina_api.Controllers
{
    [Route("api/Medicina")]
    [ApiController]
    public class MedicinaController : ControllerBase
    {
        Consultas consultas = new Consultas();
        [HttpPost]
        [Route("addPaciente")]
        public IActionResult RegistrarPaciente(Paciente paciente)
        {

            var RegistraPaciente = consultas.InsertarPaciente(paciente);
            return Ok(RegistraPaciente);

        }

        [HttpGet]
        [Route("getPaciente")]
        public IActionResult ObtenerPaciente(string dpi)
        {

            var obtenerPaciente = consultas.ObtenerPaciente(dpi);
            return Ok(obtenerPaciente);

        }


        [HttpGet]
        [Route("getPacientes_orden")]

        public IActionResult ObtenerPacientes_orden(string orden)
        {
            var obtenerPacientes_orden = consultas.ObtenerPaciente_orden(orden);
          return Ok(obtenerPacientes_orden);
        }
    }
}
