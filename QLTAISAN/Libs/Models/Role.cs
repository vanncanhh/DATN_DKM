using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class Role
    {
        public Role()
        {
            Credentials = new HashSet<Credential>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
