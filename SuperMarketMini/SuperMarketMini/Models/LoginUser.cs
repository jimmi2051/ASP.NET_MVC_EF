using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketMini.Models
{
    public class LoginUser
    {
        [StringLength(50), DataType("varchar")]
        [Required(ErrorMessage ="Please enter your username")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public String Password { get; set; }

        public bool RememberMe { get; set; }
    }
}