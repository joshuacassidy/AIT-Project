using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradeTracker.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [RegularExpression(@"^[0-9]+\.[0-9]{0,2}|[0-9]+$", ErrorMessage = "Price must have 2 decimal places or less")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        public virtual ICollection<Client> Clients { get; set; }


        public string UserId { get; set; }

    }
}