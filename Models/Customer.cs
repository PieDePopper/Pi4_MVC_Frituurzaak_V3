namespace Pi4_MVC_Frituur_V3.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RegisterId { get; set; }
        public virtual Register Register { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}

