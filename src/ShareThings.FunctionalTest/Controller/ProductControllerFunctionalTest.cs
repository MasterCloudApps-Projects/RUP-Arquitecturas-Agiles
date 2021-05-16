using FluentAssertions;
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
    public sealed class ProductControllerFunctionalTest
    {
        private readonly ShareThingsScenarioBase shareThingsScenarioBase;

        public ProductControllerFunctionalTest()
        {
            this.shareThingsScenarioBase = new ShareThingsScenarioBase();
        }

        [Fact]
        public async Task Post_Create_Update_ChangeStatus_Delete_Product_Return_All_Ok()
        {
            using (TestServer sharethingsServer = new ShareThingsScenarioBase().CreateServer())
            {
                HttpClient productClient = shareThingsScenarioBase.CreateHttpClientLender(sharethingsServer);

                #region Create product
                HttpResponseMessage response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.CreateProduct);
                var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                HttpRequestMessage postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Product.Post.CreateProduct, antiForgeryValues)
                           .Set("Name", "TestName")
                           .Set("Description", "TestDescriptionCreated")
                           .Set("Type", "TestType")
                           .Set("Subtype", "TestSubtype")
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
                int productId = Convert.ToInt32(responseString.Substring(index + key.Length, 1));
                #endregion

                #region Get detail productId
                response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.GetDetails(productId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.Contains("TestDescriptionCreated", responseString);
                Assert.DoesNotContain("TestDescriptionUpdated", responseString);
                #endregion

                #region Edit product
                response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Common.EditProduct(productId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Product.Common.EditProduct(productId), antiForgeryValues)
                        .Set("ProductId", productId.ToString())
                        .Set("Name", "TestName")
                        .Set("Description", "TestDescriptionUpdated")
                        .Set("Type", "TestType")
                        .Set("Subtype", "TestSubtype")
                        .Build();

                response = await productClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);
                Assert.EndsWith(ShareThingsScenarioBase.Product.Get.GetProducts, response.Headers.Location.OriginalString);

                // Get detail productId
                response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.GetDetails(productId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.DoesNotContain("TestDescriptionCreated", responseString);
                Assert.Contains("TestDescriptionUpdated", responseString);
                Assert.Contains("<a href=\"/Products/StatusShary/" + productId + "\">UnShary</a>", responseString);
                Assert.DoesNotContain("<a href=\"/Products/StatusShary/" + productId + "\">Shary</a>", responseString);
                #endregion

                #region Change status
                response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Common.ChangeStatus(productId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Product.Common.ChangeStatus(productId), antiForgeryValues)
                        .Build();

                response = await productClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);
                Assert.EndsWith(ShareThingsScenarioBase.Product.Get.GetProducts, response.Headers.Location.OriginalString);

                response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.GetDetails(productId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.DoesNotContain("<a href=\"/Products/StatusShary/" + productId + "\">UnShary</a>", responseString);
                Assert.Contains("<a href=\"/Products/StatusShary/" + productId + "\">Shary</a>", responseString);
                #endregion

                #region Delete product
                response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Common.DeleteProduct(productId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Product.Common.DeleteProduct(productId), antiForgeryValues)
                        .Build();

                response = await productClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);
                Assert.EndsWith(ShareThingsScenarioBase.Product.Get.GetProducts, response.Headers.Location.OriginalString);

                Func<Task> act = async () =>
                {
                    await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.GetDetails(productId));
                };
                act.Should().ThrowExactly<ArgumentException>().WithMessage("Product with Id 1 not exist (Parameter 'productId')"); 
                #endregion
            }
        }
    }
}