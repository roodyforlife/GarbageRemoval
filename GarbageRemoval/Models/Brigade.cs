using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.Models
{
    public class Brigade
    {
        public int BrigadeId { get; set; }
        public string BrigadeName { get; set; }
        public DateTime CreateDate { get; set; }
        public string UniformColor { get; set; }
        public Administration Administration { get; set; }
        public int AdministrationId { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Area> Areas { get; set; }
        public List<Order> Orders { get; set; }
    }
}
