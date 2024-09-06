using System.Text.Json.Serialization;

namespace Pautas.Models.Profesor
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }
        public int? Id_Folders_level { get; set; }
        public int? Id_level_reference { get; set; }
        public int? Id_Folder_curso { get; set; }
        public int? Id_subfolders_curso { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        public List<File> Files { get; set; } = new List<File>();

        public List<ImageModel> Listamages { get; set; }

        public int? FolderLevel { get; set; }
    }

}
