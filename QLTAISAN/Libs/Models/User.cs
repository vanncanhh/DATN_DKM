namespace Libs.Models
{
    public partial class User
    {
        public User()
        {
            DestructivelDevices = new HashSet<DestructivelDevice>();
            Devices = new HashSet<Device>();
            ProjectDkcs = new HashSet<ProjectDkc>();
            RepairDetails = new HashSet<RepairDetail>();
            RequestDevices = new HashSet<RequestDevice>();
            ScheduleTests = new HashSet<ScheduleTest>();
            UsageDevices = new HashSet<UsageDevice>();
        }

        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public int? RoleId { get; set; }
        public int? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public string? GroupId { get; set; }

        public virtual ICollection<DestructivelDevice> DestructivelDevices { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<ProjectDkc> ProjectDkcs { get; set; }
        public virtual ICollection<RepairDetail> RepairDetails { get; set; }
        public virtual ICollection<RequestDevice> RequestDevices { get; set; }
        public virtual ICollection<ScheduleTest> ScheduleTests { get; set; }
        public virtual ICollection<UsageDevice> UsageDevices { get; set; }
    }
}
