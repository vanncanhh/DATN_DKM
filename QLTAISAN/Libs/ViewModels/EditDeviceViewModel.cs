namespace Libs.ViewModels
{
    public class EditDeviceViewModel
    {
        public int DeviceId { get; set; }
        public string? DeviceCode { get; set; }
        public string? NewCode { get; set; }
        public string? DeviceName { get; set; }
        public int? TypeOfDevice { get; set; }
        public int? ParentId { get; set; }
        public string? Configuration { get; set; }
        public double? Price { get; set; }
        public string? PurchaseContract { get; set; }
        public DateTime? DateOfPurchase { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? Guarantee { get; set; }
        public string? Notes { get; set; }
        public int? UserId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
