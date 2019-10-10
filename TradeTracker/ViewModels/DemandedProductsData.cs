using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeTracker.ViewModels
{
    public class DemandedProductsData
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public bool Requested { get; set; }
    }
}