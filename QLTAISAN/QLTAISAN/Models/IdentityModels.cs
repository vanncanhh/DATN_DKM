using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QLTAISAN.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Thêm trường LastLoginTime vào user
        public DateTime? LastLoginTime { get; set; }

        // Phương thức tạo ClaimsIdentity
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tạo Identity cho user
            var userIdentity = await manager.CreateAsync(this);

            // Thêm custom claims nếu cần
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, this.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.Id));

            if (this.LastLoginTime.HasValue)
            {
                identity.AddClaim(new Claim("LastLoginTime", this.LastLoginTime.Value.ToString()));
            }

            // Bạn có thể thêm thêm custom claims vào đây
            await manager.AddClaimsAsync(this, identity.Claims);

            return identity;
        }
    }

    // DbContext kế thừa IdentityDbContext cho ASP.NET Core Identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Cấu hình DbSet cho các bảng khác nếu cần
        // public DbSet<OtherModel> OtherModels { get; set; }
    }
}
