using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace Pautas.Models.Admin
{
    public class Rol : GenericResponse
    {
        public int IdRol { get; set; }
        public string? Descripcion { get; set; }


    }
}
