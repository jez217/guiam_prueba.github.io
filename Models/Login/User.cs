using Microsoft.AspNetCore.Mvc.Rendering;
using Pautas.Models.Admin;
using Pautas.Models.Pautas;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Pautas.Models.Login
{
    public class User : GenericResponse
    {
        public int? id { get; set; }
        public int? idUser { get; set; }
        public int? IdRol { get; set; }
        public int? IdStatus { get; set; }
        public int? IdLevel { get; set; }
        public int? IdCurso { get; set; }
        public string? Name { get; set; }
        public string? LevelName { get; set; }
        public string? Clave { get; set; }
        public string? Correo { get; set; }
        public string? Apellido { get; set; }
        public string Token { get; set; }
        public string Descripcion { get; set; }
        public int? Porcentaje { get; set; }
        public int? Pagar { get; set; }
        public List<SelectListItem> LEVELDROPDOWNS { get; set; }
        public List<SelectListItem> CURSODROPDOWNS { get; set; }
        public List<SelectListItem> ROLDROPDOWNS { get; set; }
        public List<User> ListaUserDetail { get; set; }
        public List<User> Levels { get; set; }
        public ClaimsIdentity? Role { get; internal set; }
    }
}
