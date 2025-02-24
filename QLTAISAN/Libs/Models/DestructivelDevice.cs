using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class DestructivelDevice
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? DateOfDestructive { get; set; }
        public int? TypeOfDestructive { get; set; }
        public string? AddressOfDestructive { get; set; }
        public int? UserId { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Device? Device { get; set; }
        public virtual DestructiveType? TypeOfDestructiveNavigation { get; set; }
        public virtual User? User { get; set; }
    }
}
