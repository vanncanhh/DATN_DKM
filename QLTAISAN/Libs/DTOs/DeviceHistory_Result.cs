namespace Libs.DTOs
{
    public class DeviceHistory_Result
    {
        public string? ProjectName { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public string? Configuration { get; set; }
        public Nullable<int> TypeOfDevice { get; set; }
        public Nullable<System.DateTime> DateOfDelivery { get; set; }
        public Nullable<System.DateTime> DateOfReturn { get; set; }
        public string? Notes { get; set; }
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public string? DeviceCode { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> StatusRepair { get; set; }
    }
}
