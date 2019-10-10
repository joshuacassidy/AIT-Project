using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradeTracker.Models
{
    public class Client
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Client Name")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}