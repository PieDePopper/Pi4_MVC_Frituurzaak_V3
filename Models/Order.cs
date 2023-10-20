namespace Pi4_MVC_Frituur_V3.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? OrderregelId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Orderregel> Orderregels { get; set; }

    }
}
