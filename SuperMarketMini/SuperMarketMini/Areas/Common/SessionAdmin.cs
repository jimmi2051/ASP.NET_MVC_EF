using System;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketMini.Areas.Admin.Common
{
    [Serializable]
    public class SessionAdmin
    {       
        [StringLength(50), DataType("varchar")]
        [Required(ErrorMessage = "Username is required")]
        public String Username { get; set; }
        public String DisplayName { get; set; }
        public String Img { get; set; }
    }
}