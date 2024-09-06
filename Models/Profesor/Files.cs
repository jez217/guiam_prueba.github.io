using Pautas.Models.Pautas;
using System.Text.Json.Serialization;

namespace Pautas.Models.Profesor
{
    public class Files
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }

        public int FolderParentId { get; set; }
        public int Id_level_reference { get; set; }
        public int FolderLevelId { get; set; }
        public DateTime UploadedAt { get; set; }

        public List<Files> ListaFileImages { get; set; }


    }

}
