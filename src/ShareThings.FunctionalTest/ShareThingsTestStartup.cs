using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ShareThings.Areas.Identity.Data;
using ShareThings.Data.Context;
using ShareThings.Domain;
using ShareThings.FunctionalTest.Authorization;
using ShareThings.FunctionalTest.Security;
using ShareThings.Services.External.Storage;
using System;
using System.Linq;

[assembly: HostingStartup(typeof(ShareThings.FunctionalTest.ShareThingsTestStartup))]
namespace ShareThings.FunctionalTest
{
    public class ShareThingsTestStartup : Startup
    {
        public ShareThingsTestStartup(IConfiguration env) : base(env) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddAntiforgery(t =>
            {
                t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
            });

            services.AddAuthentication("TestSchema")
                .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerTest>(
                    "TestSchema", options => { });
        }

        protected override void LoadDbContextServices(IServiceCollection services)
        {
            Guid guidForParalellThreads = Guid.NewGuid();
            services.AddDbContext<ShareThingsDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting" + guidForParalellThreads.ToString());
            });

            services.AddDbContext<ShareThingsIdentityContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting" + guidForParalellThreads.ToString());
            });
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            InitializeDbForTests(serviceScope);
        }

        private static void InitializeDbForTests(IServiceScope serviceScope)
        {
            ShareThingsDbContext context = serviceScope.ServiceProvider.GetService<ShareThingsDbContext>();
            if (!context.Users.Any())
            {
                context.Users.Add(new User(IdentitySingleton.Instance.GetBorrower().Id));
                context.Users.Add(new User(IdentitySingleton.Instance.GetLender().Id));

                context.SaveChanges();
            }

            ShareThingsIdentityContext contextIdentity = serviceScope.ServiceProvider.GetService<ShareThingsIdentityContext>();
            if (!contextIdentity.Users.Any())
            {
                contextIdentity.Users.Add(IdentitySingleton.Instance.GetBorrower());
                contextIdentity.Users.Add(IdentitySingleton.Instance.GetLender());

                contextIdentity.SaveChanges();
            }
        }

        protected override void LoadExternalServices(IServiceCollection services)
        {
            Mock<IEmailSender> emailSender = new Mock<IEmailSender>();
            Mock<IDocumentService> blobstorage = new Mock<IDocumentService>();
            blobstorage.Setup(b => b.LoadDefault()).Returns(new Uri("file://Sin_imagen_disponible.jpg"));
            services.AddSingleton<IEmailSender>(emailSender.Object)
                    .AddSingleton<IDocumentService>(blobstorage.Object);
        }
    }
}