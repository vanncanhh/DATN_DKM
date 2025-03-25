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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
