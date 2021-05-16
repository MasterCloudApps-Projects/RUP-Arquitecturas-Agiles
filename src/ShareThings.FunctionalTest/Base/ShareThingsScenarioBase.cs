using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using ShareThings.FunctionalTest.Authorization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace ShareThings.FunctionalTest.Base
{
    public class ShareThingsScenarioBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(ShareThingsScenarioBase))
               .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration((context, cb) =>
                {
                    context.HostingEnvironment.ApplicationName = typeof(Startup).Assembly.GetName().Name;
                    cb.AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                })
                .UseStartup<ShareThingsTestStartup>();

            return new TestServer(hostBuilder);
        }

        public HttpClient CreateHttpClientBorrower(TestServer sharethingsServer)
        {
            HttpClient productClient = sharethingsServer.CreateClient();
            productClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(IdentitySingleton.KeyBorrower);
            return productClient;
        }

        public HttpClient CreateHttpClientLender(TestServer sharethingsServer)
        {
            HttpClient productClient = sharethingsServer.CreateClient();
            productClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(IdentitySingleton.KeyLender);
            return productClient;
        }

        public static class Home
        {
            private const string ApiUrlBase = "/";

            public static class Get
            {
                public static string GetHome = $"{ApiUrlBase}";
            }
        }

        public static class Product
        {
            private const string ApiUrlBase = "Products";

            public static class Common
            {
                public static string EditProduct(int id)
                {
                    return $"{ApiUrlBase}/Edit/{id}";
                }

                public static string DeleteProduct(int id)
                {
                    return $"{ApiUrlBase}/Delete/{id}";
                }

                public static string ChangeStatus(int id)
                {
                    return $"{ApiUrlBase}/StatusShary/{id}";
                }
            }

            public static class Get
            {
                public static string GetProducts = $"{ApiUrlBase}";
                public static string CreateProduct = $"{ApiUrlBase}/Create";

                public static string GetDetails(int id)
                {
                    return $"{ApiUrlBase}/Details/{id}";
                }
            }

            public static class Post
            {
                public static string CreateProduct = $"{ApiUrlBase}/Create/";
            }
        }

        public static class Borrow
        {
            private const string ApiUrlBase = "Borrows";

            public static class Common
            {
                public static string EditBorrow(int id)
                {
                    return $"{ApiUrlBase}/Edit/{id}";
                }

                public static string AddComment(int id)
                {
                    return $"{ApiUrlBase}/AddComment/{id}";
                }

                public static string Score(int id)
                {
                    return $"{ApiUrlBase}/Score/{id}";
                }

                public static string Confirm(int id)
                {
                    return $"{ApiUrlBase}/Confirm/{id}";
                }

                public static string Reject(int id)
                {
                    return $"{ApiUrlBase}/Reject/{id}";
                }
            }

            public static class Get
            {
                public static string GetBorrows = $"{ApiUrlBase}";

                public static string CreateBorrow(int id)
                {
                    return $"{ApiUrlBase}/Create?idProduct={id}";
                }

                public static string GetDetails(int id)
                {
                    return $"{ApiUrlBase}/Details/{id}";
                }

                public static string GetOwnerDetails(int id)
                {
                    return $"{ApiUrlBase}/OwnerDetail/{id}";
                }
            }

            public static class Post
            {
                public static string CreateBorrow = $"{ApiUrlBase}/Create/";
            }
        }
    }
}