namespace Libs.Models
{
    public class Credential
    {
        public string UserGroupId { get; set; }
        public string RoleId { get; set; }
        public int Id { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
