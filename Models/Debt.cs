using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinalApp.Models
{
    public class Debt
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Creditor { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        
    }

}
