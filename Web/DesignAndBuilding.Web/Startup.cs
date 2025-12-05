namespace DesignAndBuilding.Web
{
    using Amazon.Runtime;
    using Amazon.S3;
    using AutoMapper;
    using DesignAndBuilding.Data;
    using DesignAndBuilding.Data.Common;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Data.Repositories;
    using DesignAndBuilding.Data.Seeding;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Services.Mapping;
    using DesignAndBuilding.Services.Messaging;
    using DesignAndBuilding.Services.Storage;
    using DesignAndBuilding.Web.Hubs;
    using DesignAndBuilding.Web.ViewModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Reflection;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            })
            .AddMessagePackProtocol();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddAutoMapper(typeof(Startup), typeof(MappingProfile));

            services.AddMemoryCache();

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.Configure<SmtpOptions>(this.configuration.GetSection("Smtp"));

            // TODO: set IEmailSender implementation
            services.AddTransient<IEmailSender, SmtpEmailSender>();

            services.AddSingleton<IAmazonS3>(sp => {
                var s3cfg = new Amazon.S3.AmazonS3Config
                {
                    ServiceURL = $"https://{R2CloudflareConfig.AccountId}.r2.cloudflarestorage.com",
                    ForcePathStyle = true,
                };
                return new AmazonS3Client(
                    new BasicAWSCredentials(R2CloudflareConfig.AccessKeyId, R2CloudflareConfig.SecretAccessKey),
                    s3cfg
                );
            });
            services.AddTransient<IObjectStorageService, R2StorageService>();
            services.AddTransient<IBuildingsService, BuildingsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IAssignmentsService, AssignmentsService>();
            services.AddTransient<IBidsService, BidsService>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<AuthenticationService>();
            services.AddTransient<IFilesService, FilesService>();
            services.AddTransient<IDescriptionFilesService, DescriptionFilesService>();
            services.AddTransient<IQandAService, QandAService>();

            // Auto Mapper Configurations
            services.AddSingleton(provider => new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<NotificationsHub>("/notificationshub");
                        endpoints.MapHub<BidsHub>("/bidshub");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
