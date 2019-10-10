using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradeTracker.Models;

namespace TradeTracker.ViewModels
{
    public class ClientsIndexData
    {
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}