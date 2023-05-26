using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckpointTerminal.Singletons;

namespace CheckpointTerminal.Models
{
    public class EmployeeWithRoleName : Employee
    {
        public string RoleTitle { get; set; }
        public bool IsCurrentEmployee { get; set; }
    }
}
