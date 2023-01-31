using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.Models
{
    public class Administration
    {
        public int AdministrationId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public List<Brigade> Brigades { get; set; }
    }
}
