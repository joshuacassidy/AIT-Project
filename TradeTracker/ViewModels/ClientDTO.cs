using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeTracker.ViewModels
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<ProductDTO> Products { get; set; }
    }
}