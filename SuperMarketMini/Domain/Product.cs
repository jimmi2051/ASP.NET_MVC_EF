using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SuperMarketMini.Domain
{
    public class Product
    {
        [Key]
        [StringLength(10), DataType("varchar")]
        public String ProductID { get; set; }

        public String Name { get; set; }

        public String Detail { get; set; }

        public String Images { get; set; }

        public int Quality { get; set; }

        public float PriceBuy { get; set; }

        public float PriceSell { get; set; }

        [DataType("bit")]
        public int Product_Hot { get; set; }

        [DataType("bit")]
        public int Special { get; set; }

        public float Discount { get; set; }

        public String UnitBrief { get; set; }

        [DataType("bit")]
        public int Status { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        [ForeignKey("Category")]
        [StringLength(10), DataType("varchar")]
        public String CategoryID { get; set; }

        [ForeignKey("Supplier")]
        [StringLength(10), DataType("varchar")]
        public String SupplierID { get; set; }

        public virtual Category Category { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual ICollection<Receipt_Note_Detail> Receipt_Note_Details { get; set; }
    }
}