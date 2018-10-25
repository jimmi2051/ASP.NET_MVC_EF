using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketMini.Domain
{
    public class TypeUser
    {
        [Key]
        [StringLength(10), DataType("varchar")]
        [Required]
        public String TypeID { get; set; }

        [StringLength(255), Required]
        public String DisplayName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}