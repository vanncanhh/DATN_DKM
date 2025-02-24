using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Devices = new HashSet<Device>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Support { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
