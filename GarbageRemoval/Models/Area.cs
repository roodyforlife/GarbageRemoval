using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.Models
{
    public class Area
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public Brigade Brigade { get; set; }
        public int BrigadeId { get; set; }
        public List<House> Houses { get; set; }
    }
}
