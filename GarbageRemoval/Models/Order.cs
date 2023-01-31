using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Brigade Brigade { get; set; }
        public int BrigadeId { get; set; }
        public House House { get; set; }
        public int HouseId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsServed { get; set; }
        public int CostPerKilogram { get; set; } // Price per kilogram of garbage
        public int GarbadgeWeight { get; set; }
    }
}
