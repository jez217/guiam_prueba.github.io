using System.Text.Json.Serialization;

namespace Pautas.Models.Profesor
{
    public class FoldersCurso
    {
        public int Id { get; set; }        
        public int? ParentFolderId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<FoldersCurso> SubFolders { get; set; } = new List<FoldersCurso>();

        public List<Files> Files { get; set; } = new List<Files>();

        public List<ImageModel> Listamages { get; set; }

    }

}
