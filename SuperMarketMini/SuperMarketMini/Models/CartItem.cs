using SuperMarketMini.Domain;
using System;

namespace SuperMarketMini.Models
{
    [Serializable]
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quality { get; set; }
    }
}