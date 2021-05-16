using ShareThings.FunctionalTest.Base;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShareThings.FunctionalTest.Controller
{
    public sealed class GetControllerFunctionalTest
    {
        private readonly ShareThingsScenarioBase shareThingsScenarioBase;

        public GetControllerFunctionalTest()
        {
            this.shareThingsScenarioBase = new ShareThingsScenarioBase();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Privacy")]
        [InlineData("/Identity/Account/Register")]
        [InlineData("/Identity/Account/ForgotPassword")]
        [InlineData("/Identity/Account/Login")]
        [InlineData("/Identity/Account/Manage")]
        [InlineData("/Identity/Account/Manage/PersonalData")]
        [InlineData("/Borrows")]
        [InlineData("/Products")]
        [InlineData("/Products/Create")]
        [InlineData("/Identity/Account/Logout")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            using var sharethingsServer = shareThingsScenarioBase.CreateServer();
            HttpClient client = shareThingsScenarioBase.CreateHttpClientBorrower(sharethingsServer);

            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Fake")]
        public async Task Get_FakeEndpointsReturnNotFoundStatusCode(string url)
        {
            using var sharethingsServer = shareThingsScenarioBase.CreateServer();
            HttpClient client = shareThingsScenarioBase.CreateHttpClientBorrower(sharethingsServer);

            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}