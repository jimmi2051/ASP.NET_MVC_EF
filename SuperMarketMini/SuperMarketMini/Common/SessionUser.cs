using System;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketMini.Common
{
    [Serializable]
    public class SessionUser
    {       
        [StringLength(50), DataType("varchar")]
        [Required(ErrorMessage = "Username is required")]
        public String Username { get; set; }
        public String DisplayName { get; set; }
        public String Img { get; set; }
    }
}