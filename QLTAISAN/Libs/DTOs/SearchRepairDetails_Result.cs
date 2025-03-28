namespace Libs.DTOs
{
    public class SearchRepairDetails_Result
    {
        public string? DeviceName { get; set; }
        public Nullable<System.DateTime> DateOfRepair { get; set; }
        public string? AddressOfRepair { get; set; }
        public Nullable<int> TimeOrder { get; set; }
        public string? TypeName { get; set; }
        public string? FullName { get; set; }
        public string? Notes { get; set; }
        public Nullable<System.DateTime> NextDateOfRepair { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public int Id { get; set; }
        public string? DeviceCode { get; set; }
        public int Status { get; set; }
        public Nullable<double> Price { get; set; }
        public string? PriceOne { get; set; }
    }
}
