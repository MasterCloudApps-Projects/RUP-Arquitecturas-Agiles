using Microsoft.AspNetCore.TestHost;
using ShareThings.FunctionalTest.Base;
using ShareThings.FunctionalTest.Security;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShareThings.FunctionalTest.Controller
{
    public sealed class HomeControllerFunctionalTest
    {
        private readonly ShareThingsScenarioBase shareThingsScenarioBase;

        public HomeControllerFunctionalTest()
        {
            this.shareThingsScenarioBase = new ShareThingsScenarioBase();
        }

        [Fact]
        public async Task Get_Filter_By_Family_And_SubFamily_Return_All_Ok()
        {
            using (TestServer sharethingsServer = new ShareThingsScenarioBase().CreateServer())
            {
                HttpClient productClient = shareThingsScenarioBase.CreateHttpClientLender(sharethingsServer);
                HttpClient homeClient = shareThingsScenarioBase.CreateHttpClientLender(sharethingsServer);

                string type = "TestType";
                string subtype = "TestSubtype";
                int productId;

                #region Create product
                HttpResponseMessage response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.CreateProduct);
                var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                HttpRequestMessage postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Product.Post.CreateProduct, antiForgeryValues)
                           .Set("Name", "TestName")
                           .Set("Description", "TestDescriptionCreated")
                           .Set("Type", type)
                           .Set("Subtype", subtype)
                           .Set("Start", DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("End", DateTime.Now.AddDays(26).ToString("yyyy-MM-dd HH:mm:ss"))
                           .Build();

                response = await productClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);
                Assert.EndsWith(ShareThingsScenarioBase.Product.Get.GetProducts, response.Headers.Location.OriginalString);

                // Get Id product
                response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.GetProducts);
                string responseString = await response.Content.ReadAsStringAsync();
                string key = "/Products/Edit/";
                int index = responseString.IndexOf(key);
                productId = Convert.ToInt32(responseString.Substring(index + key.Length, 1));
                #endregion

                #region Home
                response = await homeClient.GetAsync(ShareThingsScenarioBase.Home.Get.GetHome);
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();

                key = string.Format("/Products/Details/{0}", productId);
                Assert.Contains(key, responseString);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Get, ShareThingsScenarioBase.Home.Get.GetHome, antiForgeryValues)
                           .Set("Family", type)
                           .Set("SubFamily", subtype)
                           .Build();

                response = await homeClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                #endregion
            }
        }
    }
}