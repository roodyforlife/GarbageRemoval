using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarbageRemoval.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int Salary { get; set; } // per month
        public Brigade Brigade { get; set; }
        public int BrigadeId { get; set; }
    }
}
