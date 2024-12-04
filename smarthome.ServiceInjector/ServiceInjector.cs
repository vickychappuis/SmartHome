using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using smarthome.BusinessLogic;
using smarthome.DataAccess;
using smarthome.DataAccess.Repositories;
using smarthome.Domain;
using smarthome.IBusinessLogic;
using smarthome.IDataAccess;

namespace smarthome.ServiceInjector
{
    public static class ServiceInjector
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddDataServices(services, configuration);
            AddBusinessServices(services);

            return services;
        }

        private static void AddDataServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, SmartHomeContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    providerOptions => providerOptions.EnableRetryOnFailure()
                )
            );

            // Repository registrations
            services.AddScoped<IGenericRepository<Company>, GenericRepository<Company>>();
            services.AddScoped<IGenericRepository<Device>, GenericRepository<Device>>();
            services.AddScoped<IGenericRepository<Home>, GenericRepository<Home>>();
            services.AddScoped<IGenericRepository<HomeDevice>, GenericRepository<HomeDevice>>();
            services.AddScoped<IGenericRepository<HomeMember>, GenericRepository<HomeMember>>();
            services.AddScoped<IGenericRepository<HomeMemberNotification>, GenericRepository<HomeMemberNotification>>();
            services.AddScoped<IGenericRepository<Notification>, GenericRepository<Notification>>();
            services.AddScoped<IGenericRepository<SecurityCamera>, GenericRepository<SecurityCamera>>();
            services.AddScoped<IGenericRepository<Session>, GenericRepository<Session>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<WindowSensor>, GenericRepository<WindowSensor>>();
            services.AddScoped<IGenericRepository<SmartLamp>, GenericRepository<SmartLamp>>();
            services.AddScoped<IGenericRepository<MotionSensor>, GenericRepository<MotionSensor>>();
            services.AddScoped<IGenericRepository<Room>, GenericRepository<Room>>();
        }

        private static void AddBusinessServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}