namespace Libs.Models
{
    public partial class UsageDevice
    {
        public int Id { get; set; }
        public DateTime? DateUse { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public string? Notes { get; set; }
        public int? DeviceId { get; set; }

        public virtual Device? Device { get; set; }
        public virtual ProjectDkc? Project { get; set; }
        public virtual User? User { get; set; }
    }
}
