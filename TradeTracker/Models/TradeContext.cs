using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TradeTracker.Models
{
    public class TradeContext : DbContext
    {
        public TradeContext() : base("TradeContext")
        {

        }

        public System.Data.Entity.DbSet<TradeTracker.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<TradeTracker.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<TradeTracker.Models.Status> Status { get; set; }

        public System.Data.Entity.DbSet<TradeTracker.Models.Product> Products { get; set; }


    }
}