namespace Pi4_MVC_Frituur_V3.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string? Ownerusername { get; set; }
        public string? Ownerpassword { get; set; }
        public int SnackbarId { get; set; }
        public virtual Snackbar? Snackbar { get; set; }
        public virtual ICollection<Register>? Registers { get; set; }

    }
}
