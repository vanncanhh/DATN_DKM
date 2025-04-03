namespace Libs.Common
{
    [Serializable]
    public class UserLoginSec
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string GroupID { get; set; }
    }

    public class InformationUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string RoleGroupID { get; set; }
        public string RoleGroupName { get; set; }
        public string FullName { get; set; }
    }
}
