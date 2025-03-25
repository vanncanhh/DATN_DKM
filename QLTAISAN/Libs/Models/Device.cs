namespace Libs.Models
{
    public partial class Device
    {
        public Device()
        {
            DestructivelDevices = new HashSet<DestructivelDevice>();
            DeviceOfProjects = new HashSet<DeviceOfProject>();
            RepairDetails = new HashSet<RepairDetail>();
            ScheduleTests = new HashSet<ScheduleTest>();
            UsageDevices = new HashSet<UsageDevice>();
        }

        public int Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public int? TypeOfDevice { get; set; }
        public int? ParentId { get; set; }
        public string? Configuration { get; set; }
        public double? Price { get; set; }
        public string? PurchaseContract { get; set; }
        public DateTime? DateOfPurchase { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? Guarantee { get; set; }
        public int? UserId { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public int? StatusRepair { get; set; }
        public string? NewCode { get; set; }

        public virtual Supplier? Supplier { get; set; }
        public virtual DeviceType? TypeOfDeviceNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<DestructivelDevice> DestructivelDevices { get; set; }
        public virtual ICollection<DeviceOfProject> DeviceOfProjects { get; set; }
        public virtual ICollection<RepairDetail> RepairDetails { get; set; }
        public virtual ICollection<ScheduleTest> ScheduleTests { get; set; }
        public virtual ICollection<UsageDevice> UsageDevices { get; set; }
    }
}
