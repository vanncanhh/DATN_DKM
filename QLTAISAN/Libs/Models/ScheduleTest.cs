using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class ScheduleTest
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? DateOfTest { get; set; }
        public DateTime? NextDateOfTest { get; set; }
        public string? CategoryTest { get; set; }
        public int? UserTest { get; set; }
        public string? Notes { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Device? Device { get; set; }
        public virtual User? UserTestNavigation { get; set; }
    }
}
