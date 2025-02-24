using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class DestructiveType
    {
        public DestructiveType()
        {
            DestructivelDevices = new HashSet<DestructivelDevice>();
        }

        public int Id { get; set; }
        public string? TypeName { get; set; }
        public string? Notes { get; set; }

        public virtual ICollection<DestructivelDevice> DestructivelDevices { get; set; }
    }
}
