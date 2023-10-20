namespace Pi4_MVC_Frituur_V3.Models
{
    public class OwnerRegister
    {
        public int OwnerId { get; set; }
        public int RegisterId { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual Register Register { get; set; }
    }
}
