namespace Libs.DTOs
{
    public class SearchRequestDeviceNew_Result
    {
        public int Id { get; set; }
        public Nullable<int> UserRequest { get; set; }
        public Nullable<System.DateTime> DateOfRequest { get; set; }
        public Nullable<System.DateTime> DateOfUse { get; set; }
        public string DeviceName { get; set; }
        public Nullable<int> TypeOfDevice { get; set; }
        public string Configuration { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> Approved { get; set; }
        public Nullable<int> UserApproved { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string FullName { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> NumDevice { get; set; }
    }
}
