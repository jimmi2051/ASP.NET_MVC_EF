using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketMini.Domain
{
    public class User
    {
        [Key]
        [StringLength(50), DataType("varchar")]        
        [Required(ErrorMessage = "Username is required")]
        public String Username { get; set; }

        [StringLength(50), MinLength(6, ErrorMessage = "Password min length is 6"), DataType("varchar")]
        public String Password { get; set; }

        [StringLength(100),  DataType("varchar")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Ex: jimmmi2051@gmail.com")]
        public String Email { get; set; }

        public String Images { get; set; }

        [StringLength(15), DataType("varchar")]
        public String Phone { get; set; }

        public DateTime? Birthday { get; set; }

        public String Address1 { get; set; }

        public String Address2 { get; set; }

        [StringLength(255)]
        public String DisplayName { get; set; }

        public int? Point { get; set; }

        public int? Trust { get; set; }

        [StringLength(10)]
        public String Sex { get; set; }

        public DateTime Created { get; set; }

        [DataType("bit")]
        public int Status { get; set; }


        [ForeignKey("TypeUser")]
        [StringLength(10), DataType("varchar")]
        [Required]
        public String TypeID { get; set; }

        public virtual TypeUser TypeUser { get; set; }

    }
}