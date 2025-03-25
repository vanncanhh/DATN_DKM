namespace Libs.Models
{
    public partial class UserLogin
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public string? GroupId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Status { get; set; }

        public virtual UserGroup? Group { get; set; }
    }
}
