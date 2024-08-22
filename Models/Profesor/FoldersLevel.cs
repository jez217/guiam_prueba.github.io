using System.Text.Json.Serialization;

namespace Pautas.Models.Profesor
{
    public class FoldersLevel
    {
        public int Id { get; set; }        
        public int Id_Folders_level { get; set; }
        public string? Name { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Id_subfolders_curso { get; set; }

        public List<FoldersCurso> SubFolders { get; set; } = new List<FoldersCurso>();

        public List<File> Files { get; set; } = new List<File>();

        public List<ImageModel> Listamages { get; set; }

    }

}
