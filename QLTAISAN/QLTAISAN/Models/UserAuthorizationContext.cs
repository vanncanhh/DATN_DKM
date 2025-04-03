using System.Diagnostics;

namespace QLTAISAN.Models
{
    public class UserAuthorizationContext : DbContext
    {
        public UserAuthorizationContext(DbContextOptions<UserAuthorizationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemFeature>().ToTable("SystemFeature");
            modelBuilder.Entity<UserAuthorization>().ToTable("UserAuthorization");
            modelBuilder.Entity<AspNetRoles>().ToTable("AspNetRoles");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SystemFeature> SystemFeature { get; set; }
        public DbSet<UserAuthorization> UserAuthorization { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
    }


    public class UserAuthorizationDatabseAction
    {
        private readonly UserAuthorizationContext _dbContext;

        public UserAuthorizationDatabseAction(UserAuthorizationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public SystemFeature GetFeatureById(int id)
        {
            return _dbContext.SystemFeature.FirstOrDefault(x => x.Id == id);
        }

        public List<SystemFeature> GetAllFeatureRecords()
        {
            return _dbContext.SystemFeature.ToList();
        }

        public List<SystemFeature> GetFeaturesByRoleName(string name)
        {
            var lstId = _dbContext.UserAuthorization
                .Where(p => p.RoleName.ToLower().Equals(name.ToLower()))
                .Select(k => k.FeatureId)
                .ToList();
            return _dbContext.SystemFeature
                .Where(k => lstId.Contains(k.Id))
                .ToList();
        }

        public int AddNewFeature(SystemFeature user)
        {
            try
            {
                _dbContext.SystemFeature.Add(user);
                _dbContext.SaveChanges();
                return user.Id;
            }
            catch
            {
                return 0;
            }
        }

        public int UpdateFeature(SystemFeature user)
        {
            try
            {
                var current = _dbContext.SystemFeature.FirstOrDefault(k => k.Id == user.Id);
                if (current != null)
                {
                    current.Name = user.Name;
                    current.ActionName = user.ActionName;
                    current.ControllerName = user.ControllerName;
                    _dbContext.SaveChanges();
                    return user.Id;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public bool DeleteListFeature(List<int> lstId)
        {
            try
            {
                foreach (var id in lstId)
                {
                    var current = _dbContext.SystemFeature.FirstOrDefault(k => k.Id == id);
                    if (current != null)
                    {
                        _dbContext.SystemFeature.Remove(current);
                    }
                }
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddRangeUserAuthorization(int[] lstId, string roleName)
        {
            try
            {
                var range = lstId.Select(item => new UserAuthorization
                {
                    RoleName = roleName,
                    FeatureId = item
                }).ToList();

                _dbContext.UserAuthorization.AddRange(range);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public bool DeleteUserAuthorizationByRoleName(string roleName)
        {
            _dbContext.Database.ExecuteSqlRaw("DELETE FROM [UserAuthorization] WHERE RoleName = {0}", roleName);
            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateUserAuthorizationByRoleName(string roleName, string newRoleName)
        {
            var range = _dbContext.UserAuthorization
                .Where(k => k.RoleName.ToLower().Equals(roleName.ToLower()))
                .Select(k => k.FeatureId)
                .ToArray();

            DeleteUserAuthorizationByRoleName(roleName);
            AddRangeUserAuthorization(range, newRoleName);
            return true;
        }

        public AspNetRoles GetRoleById(string id)
        {
            return _dbContext.AspNetRoles.FirstOrDefault(k => k.Id.Equals(id));
        }

        public List<AspNetRoles> GetAllRole()
        {
            return _dbContext.AspNetRoles.ToList();
        }
    }
}
