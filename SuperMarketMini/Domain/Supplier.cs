using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketMini.Domain
{
    public class Supplier
    {
        [Key]
        [StringLength(10), DataType("varchar")]
        public String SupplierID { get; set; }

        [StringLength(255), Required(ErrorMessage = "This is required")]
        public String Name { get; set; }

        public String Address { get; set; }

        [DataType("varchar"),StringLength(15)]
        public String Phone { get; set; }

        [DataType("varchar"), StringLength(100)]
        public String Email { get; set; }

        [DataType("bit")]
        public int Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}