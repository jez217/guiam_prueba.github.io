using System.Text.Json.Serialization;

namespace Pautas.Models.Admin
{
    public class Level : GenericResponse
    {
        public int Id_level { get; set; }
        public string Name { get; set; }


    }
}
