using System.Text.Json.Serialization;

namespace Pautas.Models.Login
{
    public class User : GenericResponse
    {
        public int? idUser { get; set; }
        //public string name { get; set; }
        //public string userLogin { get; set; }
        //public string password { get; set; }

        //public int? idRol { get; set; }
        //public string rolDesc { get; set; }
        //public int? Id { get; set; }
        //public int? IdUser { get; set; }

        public string Name { get; set; }
        //public int? IdRol { get; set; }
        //public string Token { get; set; }
        //public int? IdStatus { get; set; }
        //public string Correo { get; set; }
        //public string Apellido { get; set; }
       public string Clave { get; set; }
    }
}
