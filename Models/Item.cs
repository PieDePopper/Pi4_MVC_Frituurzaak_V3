using Microsoft.EntityFrameworkCore.Migrations;
//using Pi4_MVC_Frituur_V3.Controllers;
//using Pi4_MVC_Frituur_V3.Data;
//using Pi4_MVC_Frituur_V3.Data.Migrations;
using System.Security.Policy;

namespace Pi4_MVC_Frituur_V3.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        //public string Categorie { get; set; }
        public int? OrderregelId { get; set; }
        
        public virtual ICollection<Orderregel>? Orderregels { get; set; }







    }

    
    //public static Item Read(int id)
    //{
    //    ItemsController itemsController = dbv1.Instance;
    //}

}
