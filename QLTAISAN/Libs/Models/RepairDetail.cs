namespace Libs.Models
{
    public partial class RepairDetail
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? DateOfRepair { get; set; }
        public DateTime? NextDateOfRepair { get; set; }
        public int? TimeOrder { get; set; }
        public int? TypeOfRepair { get; set; }
        public string? AddressOfRepair { get; set; }
        public int? UserId { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? Price { get; set; }
        public int? Status { get; set; }

        public virtual Device? Device { get; set; }
        public virtual RepairType? TypeOfRepairNavigation { get; set; }
        public virtual User? User { get; set; }
    }
}
