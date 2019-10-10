using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradeTracker.Models
{
    public class Category
    {

        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }


        public string UserId { get; set; }

    }
}