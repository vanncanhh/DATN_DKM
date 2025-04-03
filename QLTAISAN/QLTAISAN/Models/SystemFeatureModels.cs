namespace QLTAISAN.Models
{
    public class SystemFeature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }

    public class UserAuthorization
    {
        public string Id { get; set; }
        public int FeatureId { get; set; }
        public string RoleName { get; set; }
    }

    public class AspNetRoles
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
