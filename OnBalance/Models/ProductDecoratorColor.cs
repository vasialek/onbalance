namespace OnBalance.Models
{
    public class ProductDecoratorColor
    {
        /// <summary>
        /// To help finding product
        /// </summary>
        public int ProductId { get; set; }
        public string SizeName { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }
        public string Remarks { get; set; }

        public string ToJs()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}