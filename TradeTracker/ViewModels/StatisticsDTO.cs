using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeTracker.ViewModels
{
    public class StatisticsDTO
    {
        public List<ClientDTO> Clients { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public List<StatusDTO> Status { get; set; }
    }
}