var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<QuanLyTaiSanCtyDATNContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

}, ServiceLifetime.Transient);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IScheduleTestService , ScheduleTestService>();
builder.Services.AddScoped<IRoleService , RoleService>();
builder.Services.AddScoped<IRequestDeviceService , RequestDeviceService>();
builder.Services.AddScoped<IRepairTypeService, RepairTypeService>();
builder.Services.AddScoped<IRepairDetailsService, RepairDetailsService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDeviceTypeService, DeviceTypeService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
