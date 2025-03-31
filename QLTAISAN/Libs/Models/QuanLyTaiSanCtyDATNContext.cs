namespace Libs.Models
{
    public partial class QuanLyTaiSanCtyDATNContext : DbContext
    {
        public QuanLyTaiSanCtyDATNContext()
        {
        }

        public QuanLyTaiSanCtyDATNContext(DbContextOptions<QuanLyTaiSanCtyDATNContext> options)
            : base(options)
        {
        }
        public virtual DbSet<SearchDevice_Result> SearchDevice_Results { get; set; }
        public virtual DbSet<SearchProject_Result> SearchProject_Results { get; set; }
        public virtual DbSet<DeviceById_Result> DeviceById_Results { get; set; }
        public virtual DbSet<DeviceHistory_Result> DeviceHistory_Results { get; set; }
        public virtual DbSet<SearchRepairDetails_Result> SearchRepairDetails_Results { get; set; }
        public virtual DbSet<SearchUseDevice_Result> SearchUseDevice_Results { get; set; }
        public virtual DbSet<TypeComponantOfDevice_Result> TypeComponantOfDevice_Results { get; set; }
        public virtual DbSet<ChildrenOfDevice_Result> ChildrenOfDevice_Results { get; set; }
        public virtual DbSet<SearchScheduleTest_Result> SearchScheduleTests { get; set; }
        public virtual DbSet<OderCode> OrderCode { get; set; }
        public virtual DbSet<StatisticalDevice_Result> StatisticalDevice_Results { get; set; }
        public virtual DbSet<SearchRequestDeviceNew_Result> SearchRequestDeviceNew_Results { get; set; }
        public virtual DbSet<DeviceOfProjectAll_Result> DeviceOfProjectAll_Results { get; set; }
        public virtual DbSet<StatisticProject_Result> StatisticProject_Results { get; set; }
        //
        public virtual DbSet<Credential> Credentials { get; set; } = null!;
        public virtual DbSet<DestructiveType> DestructiveTypes { get; set; } = null!;
        public virtual DbSet<DestructivelDevice> DestructivelDevices { get; set; } = null!;
        public virtual DbSet<Device> Devices { get; set; } = null!;
        public virtual DbSet<DeviceDevice> DeviceDevices { get; set; } = null!;
        public virtual DbSet<DeviceOfProject> DeviceOfProjects { get; set; } = null!;
        public virtual DbSet<DeviceType> DeviceTypes { get; set; } = null!;
        public virtual DbSet<DeviceTypeComponantType> DeviceTypeComponantTypes { get; set; } = null!;
        public virtual DbSet<ProjectDkc> ProjectDkcs { get; set; } = null!;
        public virtual DbSet<RepairDetail> RepairDetails { get; set; } = null!;
        public virtual DbSet<RepairType> RepairTypes { get; set; } = null!;
        public virtual DbSet<RequestDevice> RequestDevices { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ScheduleTest> ScheduleTests { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<UsageDevice> UsageDevices { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserGroup> UserGroups { get; set; } = null!;
        public virtual DbSet<UserLogin> UserLogins { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-3T5GPVS5\\SQLEXPRESS; Database=QuanLyTaiSanCtyDATN; Trusted_Connection=True; TrustServerCertificate=True;");
            }
        }
        public IQueryable<SearchDevice_Result> SearchDevice(Nullable<int> status, Nullable<int> devicetype, Nullable<int> guarantee, Nullable<int> projectid, string devicecode)
        {
            return SearchDevice_Results.FromSqlInterpolated(
                $"EXEC dbo.SearchDevice {status}, {devicetype}, {guarantee}, {projectid}, {devicecode}");
        }
        public IQueryable<SearchProject_Result> SearchProject(Nullable<int> managerProject, Nullable<int> status, Nullable<int> typeProject, string projectSymbol)
        {
            return SearchProject_Results.FromSqlInterpolated(
                $"EXEC dbo.SearchProject {managerProject}, {status}, {typeProject}, {projectSymbol}");
        }
        public int AddDevice(string deviceCode, string newCode, string deviceName, Nullable<int> typeOfDevice, Nullable<int> parentId,
            string configuration, Nullable<double> price, string purchaseContract, Nullable<System.DateTime> dateOfPurchase,
            Nullable<int> supplierId, Nullable<int> projectId, Nullable<System.DateTime> guarantee, string notes,
            Nullable<int> userId, Nullable<int> status)            
        {
            // Sử dụng ExecuteSqlInterpolated để truyền tham số một cách an toàn
            var result = this.Database.ExecuteSqlInterpolated(
                $"EXEC dbo.AddDevice {deviceCode}, {newCode}, {deviceName}, {typeOfDevice}, {parentId}, {configuration}, {price}, {purchaseContract}, {dateOfPurchase}, {supplierId}, {projectId}, {guarantee}, {notes}, {userId}, {status}");
            return result;
        }
        public IQueryable<DeviceById_Result> DeviceById(Nullable<int> id)
        {
            return DeviceById_Results.FromSqlInterpolated($"EXEC dbo.DeviceById {id}");
        }

        // SP DeviceHistory
        public IQueryable<DeviceHistory_Result> DeviceHistory()
        {
            return DeviceHistory_Results.FromSqlInterpolated($"EXEC dbo.DeviceHistory");
        }

        // SP SearchRepairDetails
        public IQueryable<SearchRepairDetails_Result> SearchRepairDetails(int? repairtypes, int? user, int? iddevice, int? status)
        {
            return SearchRepairDetails_Results.FromSqlInterpolated($"EXEC dbo.SearchRepairDetails {repairtypes}, {user}, {iddevice}, {status}");
        }

        public IQueryable<SearchUseDevice_Result> SearchUseDevice(int deviceId)
        {
            return SearchUseDevice_Results.FromSqlInterpolated($"EXEC dbo.SearchUseDevice {deviceId}");
        }

        public IQueryable<TypeComponantOfDevice_Result> TypeComponantOfDevice(int typeParent)
        {
            return TypeComponantOfDevice_Results.FromSqlInterpolated($"EXEC dbo.TypeComponantOfDevice {typeParent}");
        }
        public IQueryable<ChildrenOfDevice_Result> ChildrenOfDevice(Nullable<int> deviceCodeParents, Nullable<int> typeSymbolChildren)
        {
            return ChildrenOfDevice_Results.FromSqlInterpolated($"EXEC dbo.ChildrenOfDevice {deviceCodeParents}, {typeSymbolChildren}");
        }
        public int UpdateDevice(int id, string deviceCode, string newCode, string deviceName, int? typeOfDevice, int? parentId,
                                string configuration, double price, string purchaseContract, DateTime? dateOfPurchase,
                                int? supplierId, DateTime? guarantee, int? userId, string notes, DateTime? createdDate, int status)
        {
            return Database.ExecuteSqlInterpolated(
                $"EXEC dbo.UpdateDevice {id}, {deviceCode}, {newCode}, {deviceName}, {typeOfDevice}, {parentId}, {configuration}, {price}, {purchaseContract}, {dateOfPurchase}, {supplierId}, {guarantee}, {userId}, {notes}, {createdDate}, {status}");
        }
        public int UpdateUserDevice(int idDv, int idUser)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.UpdateUserDevice {idDv}, {idUser}");
        }
        public IQueryable<SearchScheduleTest_Result> GetSearchScheduleTest(int? user, int? status)
        {
            return SearchScheduleTests.FromSqlInterpolated($"EXEC dbo.SearchScheduleTest {user}, {status}");
        }
        public int ReturnDeviceOfProject(Nullable<int> projectId, Nullable<int> deviceId, string notes)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.ReturnDeviceOfProject {projectId}, {deviceId}, {notes}");
        }
        public int ReturnDeviceProject(Nullable<int> deviceId)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.ReturnDeviceProject {deviceId}");
        }
        public int AddSupplier(string name, string email, string phone, string address, string support)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddSupplier {name}, {email}, {phone}, {address}, {support}");
        }
        public int AddUser(string userName, string passWord, string fullName, string email, string phoneNumber, string address, string department, string position, Nullable<int> roleId, Nullable<int> status)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddUser {userName}, {passWord}, {fullName}, {email}, {phoneNumber}, {address}, {department}, {position}, {roleId}, {status}");
        }
        public int AddRepairDetails(Nullable<int> deviceId, Nullable<System.DateTime> dateOfRepair, Nullable<System.DateTime> nextDateOfRepair, Nullable<int> timeOrder, Nullable<int> typeOfRepair, string addressOfRepair, Nullable<int> userId, string notes)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddRepairDetails {deviceId}, {dateOfRepair}, {nextDateOfRepair}, {timeOrder}, {typeOfRepair}, {addressOfRepair}, {userId}, {notes}");
        }
        public int AddDeviceType(string typeName, string typeSymbol, string notes)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddDeviceType {typeName}, {typeSymbol}, {notes}");
        }
        public int AddDeviceOfProject(Nullable<int> projectId, Nullable<int> deviceId, string notes)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddDeviceOfProject {projectId}, {deviceId}, {notes}");
        }
        public IQueryable<OderCode> OderCode(Nullable<int> id)
        {
            return OrderCode.FromSqlInterpolated($"EXEC dbo.OderCode {id}");
        }
        public int LiquidationDevice(string id)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.LiquidationDevice {id}");
        }
        public int DeleteDeviceOfDevice(Nullable<int> idParent, Nullable<int> idChild, string resons)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.DeleteDeviceOfDevice {idParent}, {idChild}, {resons}");
        }
        public int DeleteDevice1(string id)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.DeleteDevice1 {id}");
        }
        public IQueryable<StatisticalDevice_Result> StatisticalDevice()
        {
            return StatisticalDevice_Results.FromSqlInterpolated($"EXEC dbo.StatisticalDevice");
        }
        public IQueryable<StatisticProject_Result> StatisticProject()
        {
            return StatisticProject_Results.FromSqlInterpolated($"EXEC dbo.StatisticProject");
        }
        public IQueryable<DeviceOfProjectAll_Result> DeviceOfProjectAll(Nullable<int> projectId)
        {
            return DeviceOfProjectAll_Results.FromSqlInterpolated($"EXEC dbo.DeviceOfProjectAll {projectId}");
        }
        public IQueryable<SearchRequestDeviceNew_Result> SearchRequestDeviceNew(Nullable<int> status, Nullable<bool> approved)
        {
            return SearchRequestDeviceNew_Results.FromSqlInterpolated($"EXEC dbo.SearchRequestDeviceNew {status}, {approved}");
        }
        public int UpdateUser(Nullable<int> id, string userName, string passWord, string fullName, string email, string phoneNumber, string address, string department, string position, Nullable<int> roleId, Nullable<int> status)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.UpdateUser {id}, {userName}, {passWord}, {fullName}, {email}, {phoneNumber}, {address}, {department}, {position}, {roleId}, {status}");
        }
        public int AddTypeChidren(Nullable<int> typeChidren, Nullable<int> typeParent, Nullable<int> type_TypeCom)
        { 
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddTypeChidren {typeChidren}, {typeParent}, {type_TypeCom}");
        }
        public int AddDeviceOfDevice(Nullable<int> idParent, Nullable<int> idChild, Nullable<int> typeChild, Nullable<int> typeParent, Nullable<int> typeComponant)
        { 
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddDeviceOfDevice {idParent}, {idChild}, {typeChild}, {typeParent}, {typeComponant}");
        }
        public int DeleteTypeParentTypeChild(Nullable<int> typeChidren, Nullable<int> typeParent)
        { 
            return Database.ExecuteSqlInterpolated($"EXEC dbo.DeleteTypeParentTypeChild {typeChidren}, {typeParent}");
        }
        public int AddRepairType(string typeName, string notes)
        { 
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddRepairType {typeName}, {notes}");
        }
        public int AddProject_(string projectSymbol, string projectName, Nullable<int> managerProject, string address, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> status)
        { 
            return Database.ExecuteSqlInterpolated($"EXEC dbo.AddProject {projectSymbol}, {projectName}, {managerProject}, {address}, {fromDate}, {toDate}, {status}");
        }
        public int UpdateProject(Nullable<int> id, string projectSymbol, string projectName, Nullable<int> managerProject, string address, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<System.DateTime> createdDate, Nullable<System.DateTime> modifiedDate, Nullable<int> status, Nullable<bool> isDeleted)
        { 
            return Database.ExecuteSqlInterpolated($"EXEC dbo.UpdateProject {id}, {projectSymbol}, {projectName}, {managerProject}, {address}, {fromDate}, {toDate}, {createdDate}, {modifiedDate}, {status}, {isDeleted}");
        }
        public int DeleteProject1(string id)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.DeleteProject1 {id}");
        }
        public int DeleteProject(Nullable<int> id)
        {
            return Database.ExecuteSqlInterpolated($"EXEC dbo.DeleteProject {id}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchDevice_Result>().HasNoKey();
            modelBuilder.Entity<SearchProject_Result>().HasNoKey();
            modelBuilder.Entity<DeviceById_Result>().HasNoKey();
            modelBuilder.Entity<DeviceHistory_Result>().HasNoKey();
            modelBuilder.Entity<SearchRepairDetails_Result>().HasNoKey();
            modelBuilder.Entity<SearchUseDevice_Result>().HasNoKey();
            modelBuilder.Entity<TypeComponantOfDevice_Result>().HasNoKey();
            modelBuilder.Entity<ChildrenOfDevice_Result>().HasNoKey();
            modelBuilder.Entity<SearchScheduleTest_Result>().HasNoKey();
            modelBuilder.Entity<StatisticalDevice_Result>().HasNoKey();
            modelBuilder.Entity<DeviceOfProjectAll_Result>().HasNoKey();
            modelBuilder.Entity<StatisticProject_Result>().HasNoKey();
            modelBuilder.Entity<OderCode>().HasNoKey();
            modelBuilder.Entity<Credential>(entity =>
            {
                entity.HasKey(e => new { e.UserGroupId, e.RoleId });

                entity.ToTable("Credential");

                entity.Property(e => e.UserGroupId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserGroupID");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RoleID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Credentials)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Credential_UserGroup");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.Credentials)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Credential_UserGroup1");
            });

            modelBuilder.Entity<DestructiveType>(entity =>
            {
                entity.ToTable("DestructiveType");

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.Property(e => e.TypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<DestructivelDevice>(entity =>
            {
                entity.ToTable("DestructivelDevice");

                entity.Property(e => e.AddressOfDestructive).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.DateOfDestructive).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DestructivelDevices)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_CancelDevice_Device");

                entity.HasOne(d => d.TypeOfDestructiveNavigation)
                    .WithMany(p => p.DestructivelDevices)
                    .HasForeignKey(d => d.TypeOfDestructive)
                    .HasConstraintName("FK_CancelDevice_CancelType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DestructivelDevices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CancelDevice_User");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Configuration).HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.DateOfPurchase).HasColumnType("date");

                entity.Property(e => e.DeviceCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DeviceName).HasMaxLength(200);

                entity.Property(e => e.Guarantee).HasColumnType("date");

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.NewCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.Property(e => e.PurchaseContract).HasMaxLength(500);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Device_Supplier");

                entity.HasOne(d => d.TypeOfDeviceNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.TypeOfDevice)
                    .HasConstraintName("FK_Device_DeviceType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Device_User");
            });

            modelBuilder.Entity<DeviceDevice>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DeviceDevice");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.HasOne(d => d.DeviceCodeChildrenNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.DeviceCodeChildren)
                    .HasConstraintName("FK_DeviceDevice_Device");

                entity.HasOne(d => d.DeviceCodeParentsNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.DeviceCodeParents)
                    .HasConstraintName("FK_DeviceDevice_Device1");
            });

            modelBuilder.Entity<DeviceOfProject>(entity =>
            {
                entity.ToTable("DeviceOfProject");

                entity.Property(e => e.DateOfDelivery).HasColumnType("date");

                entity.Property(e => e.DateOfReturn).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceOfProjects)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_DeviceOfProject_Device");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.DeviceOfProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_DeviceOfProject_ProjectDKC");
            });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.ToTable("DeviceType");

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.Property(e => e.OrderCode).HasDefaultValueSql("((1))");

                entity.Property(e => e.TypeName).HasMaxLength(100);

                entity.Property(e => e.TypeSymbol)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<DeviceTypeComponantType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DeviceTypeComponantType");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.HasOne(d => d.TypeSymbolChildrenNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.TypeSymbolChildren)
                    .HasConstraintName("FK_DeviceTypeComponantType_DeviceType");

                entity.HasOne(d => d.TypeSymbolParentsNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.TypeSymbolParents)
                    .HasConstraintName("FK_DeviceTypeComponantType_DeviceType1");
            });

            modelBuilder.Entity<ProjectDkc>(entity =>
            {
                entity.ToTable("ProjectDKC");

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.ProjectName).HasMaxLength(200);

                entity.Property(e => e.ProjectSymbol)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.HasOne(d => d.ManagerProjectNavigation)
                    .WithMany(p => p.ProjectDkcs)
                    .HasForeignKey(d => d.ManagerProject)
                    .HasConstraintName("FK_ProjectDKC_User");
            });

            modelBuilder.Entity<RepairDetail>(entity =>
            {
                entity.Property(e => e.AddressOfRepair).HasMaxLength(500);

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.DateOfRepair).HasColumnType("date");

                entity.Property(e => e.NextDateOfRepair).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.RepairDetails)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_RepairDetails_Device");

                entity.HasOne(d => d.TypeOfRepairNavigation)
                    .WithMany(p => p.RepairDetails)
                    .HasForeignKey(d => d.TypeOfRepair)
                    .HasConstraintName("FK_RepairDetails_RepairType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RepairDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RepairDetails_User");
            });

            modelBuilder.Entity<RepairType>(entity =>
            {
                entity.ToTable("RepairType");

                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.Property(e => e.TypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<RequestDevice>(entity =>
            {
                entity.ToTable("RequestDevice");

                entity.Property(e => e.Configuration).HasMaxLength(2000);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.DateOfRequest).HasColumnType("date");

                entity.Property(e => e.DateOfUse).HasColumnType("date");

                entity.Property(e => e.DeviceName).HasMaxLength(200);

                entity.Property(e => e.NameUserApproved).HasMaxLength(100);

                entity.Property(e => e.NoteProcess).HasMaxLength(2000);

                entity.Property(e => e.NoteReasonRefuse).HasMaxLength(2000);

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.HasOne(d => d.TypeOfDeviceNavigation)
                    .WithMany(p => p.RequestDevices)
                    .HasForeignKey(d => d.TypeOfDevice)
                    .HasConstraintName("FK_RequestDevice_DeviceType");

                entity.HasOne(d => d.UserRequestNavigation)
                    .WithMany(p => p.RequestDevices)
                    .HasForeignKey(d => d.UserRequest)
                    .HasConstraintName("FK_RequestDevice_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ScheduleTest>(entity =>
            {
                entity.ToTable("ScheduleTest");

                entity.Property(e => e.CategoryTest).HasMaxLength(2000);

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.DateOfTest).HasColumnType("date");

                entity.Property(e => e.NextDateOfTest).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(2000);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.ScheduleTests)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_ScheduleTest_Device");

                entity.HasOne(d => d.UserTestNavigation)
                    .WithMany(p => p.ScheduleTests)
                    .HasForeignKey(d => d.UserTest)
                    .HasConstraintName("FK_ScheduleTest_User");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Support).HasMaxLength(500);
            });

            modelBuilder.Entity<UsageDevice>(entity =>
            {
                entity.ToTable("UsageDevice");

                entity.Property(e => e.DateUse).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.UsageDevices)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_UsageDevice_Device");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.UsageDevices)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_UsageDevice_ProjectDKC");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsageDevices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UsageDevice_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Department).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.GroupId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("GroupID")
                    .HasDefaultValueSql("('MEMBER')");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Position).HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("UserGroup");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.ToTable("UserLogin");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.GroupId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("GroupID");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserLogins)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_UserLogin_UserGroup");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
