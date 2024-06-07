using System.Text.Json.Serialization;

namespace Pautas.Models.Admin
{
    public class Student : GenericResponse
    {
        public int Id_student { get; set; }
        public string Name { get; set; }
        public int Id_level { get; set; }


    }
}
