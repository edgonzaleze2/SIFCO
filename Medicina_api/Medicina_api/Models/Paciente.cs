namespace Medicina_api.Models
{
    public class Paciente
    {
        public int? codigo { get; set; } = null;
        public string nombrecompleto { get; set; } 

        public string? fechanacimiento { get; set; } = null;

        public string? sexo { get; set; } = null;

        public string? dpi { get; set; } = null;

        public string? fechaingreso { get; set; } = null;

    }
}
