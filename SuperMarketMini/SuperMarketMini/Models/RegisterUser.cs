using System;
using System.ComponentModel.DataAnnotations;
namespace SuperMarketMini.Models
{
    public class RegisterUser
    {
        [Key]
        [StringLength(50), DataType("varchar")]
        [Required(ErrorMessage = "Username is required")]
        public String Username { get; set; }

        [StringLength(50), MinLength(6, ErrorMessage = "Password min length is 6"), Required(ErrorMessage = "Password is required"), DataType("varchar")]
        public String Password { get; set; }

        [StringLength(50)]
        [Compare("Password",ErrorMessage ="Please check confirm password again")]
        public String ConfirmPassword { get; set; }

        [StringLength(100), Required(ErrorMessage = "Email is required"), DataType("varchar")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Ex: jimmmi2051@gmail.com")]
        public String Email { get; set; }

        [StringLength(15), DataType("varchar")]
        public String Phone { get; set; }


        [StringLength(255)]
        public String DisplayName { get; set; }

        [StringLength(10)]
        public String Sex { get; set; }


    }
}