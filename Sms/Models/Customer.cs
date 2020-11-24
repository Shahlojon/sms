using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sms.Models
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Alphanumeric { get; set; }
    }
}
