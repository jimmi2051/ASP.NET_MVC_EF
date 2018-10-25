using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketMini.Domain
{
    public class Message
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Sendby { get; set; }

        public string Displayname { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        [DataType("bit")]
        public int status { get; set; }

    }
}
