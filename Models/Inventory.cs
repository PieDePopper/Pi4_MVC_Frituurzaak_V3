namespace Pi4_MVC_Frituur_V3.Models
{
    // Stock of voorraad
    public class Inventory
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int? RegisterId { get; set; }
        public virtual Register? Register { get; set; }
    }
}
