namespace Pautas.Models.Pautas
{
    public class ProductModel : GenericResponse
    {
        public int PAUTA_ID { get; set; }
        public int? PRODUCT_CODE { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string PROD_DESC { get; set; }
        public decimal? PRICE_RETAIL { get; set; }
        public string PHOTO_PATH { get; set; }
        public decimal? SALE { get; set; }
        public decimal? UTILITY { get; set; }
        public decimal? MARGIN { get; set; }
        public string? PRODUCT_NAME { get; set; }
        public decimal? ROI { get; set; }
        public int? SALE_PIECE { get; set; }
        public int? QTY { get; set; }
        public int? TRANSNUM { get; set; }
        public decimal? TICKET_PROM { get; set; }

        public List<ProductModel> ListaProducDetail { get; set; }

    }
}
