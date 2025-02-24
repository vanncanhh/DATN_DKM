using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class ProjectDkc
    {
        public ProjectDkc()
        {
            DeviceOfProjects = new HashSet<DeviceOfProject>();
            UsageDevices = new HashSet<UsageDevice>();
        }

        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectSymbol { get; set; }
        public int? ManagerProject { get; set; }
        public string? Address { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public int? TypeProject { get; set; }

        public virtual User? ManagerProjectNavigation { get; set; }
        public virtual ICollection<DeviceOfProject> DeviceOfProjects { get; set; }
        public virtual ICollection<UsageDevice> UsageDevices { get; set; }
    }
}
