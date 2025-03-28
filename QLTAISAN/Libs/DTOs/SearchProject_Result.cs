namespace Libs.DTOs
{
    public class SearchProject_Result
    {
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public Nullable<int> ManagerProject { get; set; }
        public string? Address { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int Status { get; set; }
        public string? FullName { get; set; }
        public string? ProjectSymbol { get; set; }
        public Nullable<int> NumDevice { get; set; }
        public Nullable<int> TypeProject { get; set; }
    }
}
