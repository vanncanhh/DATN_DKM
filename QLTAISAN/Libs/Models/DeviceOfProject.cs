namespace Libs.Models
{
    public partial class DeviceOfProject
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? DateOfDelivery { get; set; }
        public DateTime? DateOfReturn { get; set; }
        public int? Status { get; set; }
        public string? Notes { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Device? Device { get; set; }
        public virtual ProjectDkc? Project { get; set; }
    }
}
