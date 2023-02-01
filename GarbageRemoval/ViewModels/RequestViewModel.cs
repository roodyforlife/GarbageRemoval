using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.ViewModels
{
    public class RequestViewModel
    {
        public List<string[]> Result { get; set; } = new List<string[]>();
        public string[] Displays { get; set; }
    }
}
