using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace Pautas.Models.Admin
{
    public class Curso : GenericResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }


    }
}
