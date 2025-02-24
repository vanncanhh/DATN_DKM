using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class UserGroup
    {
        public UserGroup()
        {
            Credentials = new HashSet<Credential>();
            UserLogins = new HashSet<UserLogin>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
