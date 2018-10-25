using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketMini.Domain
{
    public class Category
    {
        [Key]
        [StringLength(10), DataType("varchar")]
        public String CategoryID { get; set; }

        [StringLength(255), Required(ErrorMessage = "This is required")]
        public String Name { get; set; }

        public String GroupName { get; set; }

        [DataType("bit")]
        public int Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}