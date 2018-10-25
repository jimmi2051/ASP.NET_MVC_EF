using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SuperMarketMini.Domain
{
    public class Receipt_Note
    {    
        [Key]
        [StringLength(10)]
        public string Receipt_NoteID { get; set; }

        [ForeignKey("User")]
        [StringLength(50), DataType("varchar")]
        [Required(ErrorMessage = "Username is required")]
        public String Username { get; set; }

        public float Amount { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        [DataType("bit")]
        public int Status { get; set; }


        public virtual ICollection<Receipt_Note_Detail> Receipt_Note_Detail { get; set; }

        public virtual User User { get; set; }
    }
}