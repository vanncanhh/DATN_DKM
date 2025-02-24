using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class RepairType
    {
        public RepairType()
        {
            RepairDetails = new HashSet<RepairDetail>();
        }

        public int Id { get; set; }
        public string? TypeName { get; set; }
        public string? Notes { get; set; }

        public virtual ICollection<RepairDetail> RepairDetails { get; set; }
    }
}
