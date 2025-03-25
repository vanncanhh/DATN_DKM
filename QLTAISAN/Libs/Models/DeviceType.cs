namespace Libs.Models
{
    public partial class DeviceType
    {
        public DeviceType()
        {
            Devices = new HashSet<Device>();
            RequestDevices = new HashSet<RequestDevice>();
        }

        public int Id { get; set; }
        public string? TypeName { get; set; }
        public string? TypeSymbol { get; set; }
        public string? Notes { get; set; }
        public int? OrderCode { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<RequestDevice> RequestDevices { get; set; }
    }
}
