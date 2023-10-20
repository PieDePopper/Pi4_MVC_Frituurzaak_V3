namespace Pi4_MVC_Frituur_V3.Models
{
    public class Orderregel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        //public string? ItemName { get; set; }
        //public decimal? ItemPrice { get; set; }
        public int Quantity { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Item? Item { get; set; }
    }
}
