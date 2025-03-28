namespace Libs.DTOs
{
    public class SearchUseDevice_Result
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> DateUse { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string? Notes { get; set; }
        public Nullable<int> DeviceId { get; set; }
        public string? ProjectName { get; set; }
    }
}
