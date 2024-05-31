using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pautas.Models.Pautas
{
    public class PautaModel : GenericResponse
    {
        public int PAUTA_ID { get; set; }
        public int ID { get; set; }
        public int? IMAGES_ID { get; set; }
        public string? URL { get; set; }
        public string? IMAGES_NAME { get; set; }
        public int? REGISTER_COUNT { get; set; }
        public int? STATUS_ID { get; set; }
        public string STATUS { get; set; }
        public int? ANNO { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public DateTime DATE_NOW_EDIT { get; set; }
        public DateTime DATE_NOW_CREATE { get; set; }
        public string? CAMPANNA { get; set; }
        public decimal? SALE { get; set; }
        public decimal? UTILITY { get; set; }
        public decimal? MARGIN { get; set; }
        public string PRODUCT_NAME { get; set; }
        public int? PRODUCT_CODE { get; set; }
        public decimal? SCOPE_RRSS { get; set; }
        public decimal? COST_RESULT { get; set; }
        public decimal? INV_ADS { get; set; }
        public decimal? ROI { get; set; }
        public int? MONTH_ID { get; set; }
        public string NAME_MONTH { get; set; }
        public List<SelectListItem> MONTHDROPDOWNS { get; set; }
        public int[] STORE_CODE { get; set; }
        public List<SelectListItem> STOREDROPDOWNS { get; set; }
        public string? STORE_NAME { get; set; }
        public int? PC_ID { get; set; }
        public string PC_NAME { get; set; }
        public List<SelectListItem> PCDROPDOWNS { get; set; }
        public int? SALE_PIECE { get; set; }
        public string USER_CREATE { get; set; }
        public string USER_UPDATE { get; set; }
        public DateTime DATE_UPDATE { get; set; }
        public string? PROD_DESC { get; set; }
        public decimal? PRICE_RETAIL { get; set; }
        public int? QTY { get; set; }

        public List<ProductModel> ListaProducDetail { get; set; }
        public List<ImageModel> ListaPautaImages { get; set; }

    }
}
