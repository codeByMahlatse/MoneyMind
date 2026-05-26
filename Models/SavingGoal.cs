using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FinalApp.Models
{
    using SQLite;

    public class SavingGoal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal TargetAmount { get; set; }

        [Ignore]
        public decimal Progress => TargetAmount == 0 ? 0 : CurrentAmount / TargetAmount;

        [Ignore]
        public string DisplayAmount => $"${CurrentAmount:N2}";

        [Ignore]
        public string DisplayTarget => $"${TargetAmount:N2}";
    }

}
