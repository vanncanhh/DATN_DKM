using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class DeviceDevice
    {
        public int Id { get; set; }
        public int? DeviceCodeParents { get; set; }
        public int? DeviceCodeChildren { get; set; }
        public int? TypeSymbolParents { get; set; }
        public int? TypeSymbolChildren { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? UserCreated { get; set; }
        public int? UserModified { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Notes { get; set; }
        public int? TypeComponant { get; set; }

        public virtual Device? DeviceCodeChildrenNavigation { get; set; }
        public virtual Device? DeviceCodeParentsNavigation { get; set; }
    }
}
