namespace Libs.DTOs
{
    public class HistoryScheduleTestById_Result
    {
        public int Id { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public Nullable<System.DateTime> DateOfTest { get; set; }
        public Nullable<System.DateTime> NextDateOfTest { get; set; }
        public string? CategoryTest { get; set; }
        public Nullable<int> UserTest { get; set; }
        public string? Notes { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string? FullName { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceCode { get; set; }
    }
}
