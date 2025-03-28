using System.ComponentModel.DataAnnotations.Schema;

namespace Libs.DTOs
{
    public class ChildrenOfDevice_Result
    {
        public int Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public string? Configuration { get; set; }
        public string? Price { get; set; }
        public string? PurchaseContract { get; set; }
        public Nullable<System.DateTime> DateOfPurchase { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> StatusRepair { get; set; }
        public string? TypeName { get; set; }
        public Nullable<int> DeviceCodeParents { get; set; }
        public Nullable<int> DeviceCodeChildren { get; set; }
        public Nullable<int> TypeSymbolParents { get; set; }
        public Nullable<int> TypeSymbolChildren { get; set; }
        public Nullable<int> TypeComponant { get; set; }
        [NotMapped]
        public object numbers { get; set; }
    }
}
