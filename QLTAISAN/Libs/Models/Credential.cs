using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class Credential
    {
        public string UserGroupId { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public int Id { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual UserGroup UserGroup { get; set; } = null!;
    }
}
