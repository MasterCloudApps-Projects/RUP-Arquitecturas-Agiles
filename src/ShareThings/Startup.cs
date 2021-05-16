using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShareThings.Areas.Identity.Data;
using ShareThings.Data.Context;
using ShareThings.Data.Repositories;
using ShareThings.Domain;
using ShareThings.Services.Business;
using ShareThings.Services.External.Sendgrid;
using ShareThings.Services.External.Storage;
using ShareThings.Services.External.Storage.AzureBlobStorage;

namespace ShareThings
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            LoadOptions(services);
            LoadIdentityServices(services);
            LoadExternalServices(services);
            LoadBusinessServices(services);
            LoadDbContextServices(services);
            LoadRepositoriesServices(services);
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages("text/html", "<h1>Ooops! Something was wrong!!</h1><h2>Status Code {0}</h2><a href=\"#\" onclick=\"window.history.back();\">Back</a>");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            ConfigureAuth(app);

            ConfigureEnpoints(app);
        }

        protected void ConfigureEnpoints(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{id?}/{action=Index}/{id2?}");
                endpoints.MapRazorPages();
            });
        }

        protected void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private void LoadOptions(IServiceCollection services)
        {
            services.Configure<SendGridEmailSenderOptions>(options =>
            {
                options.ApiKey = Configuration["ExternalProviders:SendGrid:ApiKey"];
                options.SenderEmail = Configuration["ExternalProviders:SendGrid:SenderEmail"];
                options.SenderName = Configuration["ExternalProviders:SendGrid:SenderName"];
            });
            services.Configure<BlobOptions>(options =>
            {
                options.ConnectionString = Configuration["ExternalProviders:BlobStorage:ConnectionString"];
                options.ContainerName = Configuration["ExternalProviders:BlobStorage:ContainerName"];
                options.DefaultContainer = Configuration["ExternalProviders:BlobStorage:DefaultContainer"];
                options.DefaultImage = Configuration["ExternalProviders:BlobStorage:DefaultImage"];
            });
        }

        protected virtual void LoadExternalServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSenderSendgrid>()
                    .AddTransient<IDocumentService, BlobService>();
        }

        private static void LoadBusinessServices(IServiceCollection services)
        {
            services.AddTransient<ProductService>()
                    .AddTransient<PhotoService>()
                    .AddTransient<BorrowService>();
        }

        private static void LoadIdentityServices(IServiceCollection services)
        {
            services.AddTransient<IShareThingsUserManager, ShareThingsUserManager>();
        }

        protected virtual void LoadDbContextServices(IServiceCollection services)
        {
            services.AddDbContextPool<ShareThingsDbContext>(
                dbContextOptions => dbContextOptions
                    .UseSqlServer(Configuration.GetConnectionString("ShareThingsDatabase"), 
                        b => b.MigrationsAssembly(typeof(ShareThingsDbContext).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

            services.AddDbContext<ShareThingsIdentityContext>(optionsAction: options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ShareThingsDatabase")));
        }

        private static void LoadRepositoriesServices(IServiceCollection services)
        {
            services
                .AddDefaultIdentity<ShareThingsUser>(configureOptions: options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ShareThingsIdentityContext>()
                .AddUserStore<ShareThingUserStore>();

            services.AddScoped<IShareThingsDbContext>(provider => provider.GetService<ShareThingsDbContext>());

            services.AddTransient<IProductRepository, ProductRepository>()
                    .AddTransient<IBorrowRepository, BorrowRepository>();
        }
    }
}