namespace Pautas.Models.Pautas
{
    public class ImageModel : GenericResponse
    {
        public int ID { get; set; }        
        public int PAUTA_ID { get; set; }
        public int IMAGES_ID { get; set; }
        public string URL { get; set; }
        public string IMAGES_NAME { get; set; }

        public List<ImageModel> ListaPautaImages { get; set; }

    }
}
