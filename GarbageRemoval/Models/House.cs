using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.Models
{
    public class House
    {
        public int HouseId { get; set; }
        public string Address { get; set; }
        public Area Area { get; set; }
        public int AreaId { get; set; }
        public List<Order> Orders { get; set; }
    }
}
