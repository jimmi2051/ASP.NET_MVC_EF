
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SuperMarketMini.Domain
{
    public class Order_Detail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string ProductID { get; set; }

        public int Quality { get; set; }

        public float Price { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}