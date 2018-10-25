using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SuperMarketMini.Domain
{
    public class Order
    {
        [Key]
        public String OrderID { get; set; }

        [ForeignKey("User")]
        [StringLength(50), DataType("varchar")]
        [Required(ErrorMessage = "Username is required")]
        public String Username { get; set; }

        public float Amount { get; set; }

        [Required(ErrorMessage ="Address is required")]
        public String Address { get; set; }

        public String Description { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequireDate { get; set; }

        [DataType("bit")]
        public int Status { get; set; }

        public String Contact { get; set; }

        public virtual ICollection<Order_Detail> Order_Detail { get; set; }

        public virtual User User { get; set; }

    }
}