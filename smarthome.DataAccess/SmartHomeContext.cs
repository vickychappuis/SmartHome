using Microsoft.EntityFrameworkCore;
using smarthome.Domain;

namespace smarthome.DataAccess;

public class SmartHomeContext : DbContext
{
    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<HomeDevice> HomeDevices { get; set; }

    public virtual DbSet<HomeMember> HomeMember { get; set; }

    public virtual DbSet<HomeMemberNotification> HomeMemberNotification { get; set; }

    public virtual DbSet<Notification> Notification { get; set; }

    public virtual DbSet<SecurityCamera> SecurityCameras { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WindowSensor> WindowSensors { get; set; }

    public SmartHomeContext(DbContextOptions<SmartHomeContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HomeMember>().HasKey(hm => new { hm.HomeId, hm.UserId });

        modelBuilder
            .Entity<HomeMemberNotification>()
            .HasKey(hmn => new
            {
                hmn.HomeId,
                hmn.UserId,
                hmn.NotificationId,
            });

        modelBuilder
            .Entity<Device>()
            .HasDiscriminator(b => b.DeviceType)
            .HasValue<SecurityCamera>((DeviceType)0)
            .HasValue<WindowSensor>((DeviceType)1)
            .HasValue<SmartLamp>((DeviceType)2)
            .HasValue<MotionSensor>((DeviceType)3);
    }
}
