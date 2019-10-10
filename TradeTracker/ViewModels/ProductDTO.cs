using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeTracker.ViewModels
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

        public int StatusId { get; set; }
        public StatusDTO Status { get; set; }

        public ICollection<ClientDTO> Clients { get; set; }
    }
}