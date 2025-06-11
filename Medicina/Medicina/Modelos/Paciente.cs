using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medicina.Models
{
    public class Paciente
    {
        public int? id {get; set;} = null;
        public string nombrecompleto { get; set; }
        public string fecha_nacimiento { get; set; }
        public string genero { get; set; }
        public string dpi { get; set; }
        
    }

    public class ResultModel {
        public bool Estado;

        public string Mensaje;

        public Object objeto;

    }

}