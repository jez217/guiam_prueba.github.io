using System.Text.Json.Serialization;

namespace Pautas.Models.Profesor
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public int FolderId { get; set; }
        public DateTime UploadedAt { get; set; }
    }

}
