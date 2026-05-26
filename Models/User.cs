using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinalApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        // User.cs
            public int Id { get; set; }  // Important for database primary key
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        

        public string EmploymentStatus { get; set; }
            public string IncomeLevel { get; set; }
            public string FinancialGoal { get; set; }
        
    }
    
}
