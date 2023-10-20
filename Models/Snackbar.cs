namespace Pi4_MVC_Frituur_V3.Models
{
    public class Snackbar
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }

        public virtual ICollection<Owner> Owners { get; set; }
    }
}
