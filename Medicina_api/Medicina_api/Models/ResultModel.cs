namespace Medicina_api.Models
{
    public class ResultModel
    {
        public bool Estado { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public object Objeto { get; set; } = string.Empty;
    }
}
