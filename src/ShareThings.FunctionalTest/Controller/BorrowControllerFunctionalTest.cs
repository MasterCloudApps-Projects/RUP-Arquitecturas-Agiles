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
    public sealed class BorrowControllerFunctionalTest
    {
        private readonly ShareThingsScenarioBase shareThingsScenarioBase;

        public BorrowControllerFunctionalTest()
        {
            this.shareThingsScenarioBase = new ShareThingsScenarioBase();
        }

        [Fact]
        public async Task Post_Create_Update_ChangeDuration_AddComment_Reject_Confirm_Return_All_Ok()
        {
            using (TestServer sharethingsServer = new ShareThingsScenarioBase().CreateServer())
            {
                HttpClient productClient = shareThingsScenarioBase.CreateHttpClientLender(sharethingsServer);

                #region Create product
                HttpResponseMessage response = await productClient.GetAsync(ShareThingsScenarioBase.Product.Get.CreateProduct);
                var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                DateTime productStartDate = DateTime.Now.AddDays(-6);
                DateTime productEndDate = DateTime.Now.AddDays(26);

                HttpRequestMessage postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Product.Post.CreateProduct, antiForgeryValues)
                           .Set("Name", "TestName")
                           .Set("Description", "TestDescriptionCreated")
                           .Set("Type", "TestType")
                           .Set("Subtype", "TestSubtype")
                           .Set("Start", productStartDate.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("End", productEndDate.ToString("yyyy-MM-dd HH:mm:ss"))
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

                HttpClient borrowClient = shareThingsScenarioBase.CreateHttpClientBorrower(sharethingsServer);

                #region Create borrow 1
                response = await borrowClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.CreateBorrow(productId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                DateTime borrowDateStart = DateTime.Now.AddDays(-3);
                DateTime borrowDateEnd = DateTime.Now.AddDays(23);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Borrow.Post.CreateBorrow, antiForgeryValues)
                           .Set("Borrow.ProductId", productId.ToString())
                           .Set("Borrow.ProductStart", productStartDate.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.ProductEnd", productEndDate.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.Start", borrowDateStart.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.End", borrowDateEnd.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Build();

                response = await borrowClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);
                Assert.EndsWith(ShareThingsScenarioBase.Borrow.Get.GetBorrows, response.Headers.Location.OriginalString);

                // Get borrow Id
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetBorrows);
                responseString = await response.Content.ReadAsStringAsync();
                key = "/Borrows/Details/";
                index = responseString.IndexOf(key);
                int borrowId = Convert.ToInt32(responseString.Substring(index + key.Length, 1));
                #endregion

                #region Owner Product Details
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetOwnerDetails(borrowId));
                response.EnsureSuccessStatusCode();
                #endregion

                #region Change duration borrow
                DateTime borrowDateStartUpdated = DateTime.Now.AddDays(-2);
                DateTime borrowDateEndUpdated = DateTime.Now.AddDays(20);

                response = await borrowClient.GetAsync(ShareThingsScenarioBase.Borrow.Common.EditBorrow(borrowId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Borrow.Common.EditBorrow(borrowId), antiForgeryValues)
                           .Set("id", borrowId.ToString())
                           .Set("Borrow.BorrowId", borrowId.ToString())
                           .Set("Borrow.ProductId", productId.ToString())
                           .Set("Borrow.ProductStart", productStartDate.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.ProductEnd", productEndDate.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.Start", borrowDateStartUpdated.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.End", borrowDateEndUpdated.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Build();

                response = await borrowClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);
                Assert.EndsWith(ShareThingsScenarioBase.Borrow.Get.GetBorrows, response.Headers.Location.OriginalString);

                // Get detail borrowId
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetDetails(borrowId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.DoesNotContain(borrowDateStart.ToString("yyyy-MM-dd"), responseString);
                Assert.DoesNotContain(borrowDateEnd.ToString("yyyy-MM-dd"), responseString);
                Assert.Contains(borrowDateStartUpdated.ToString("yyyy-MM-dd"), responseString);
                Assert.Contains(borrowDateEndUpdated.ToString("yyyy-MM-dd"), responseString);
                #endregion

                #region Add comment
                string comment = "New comment";

                response = await borrowClient.GetAsync(ShareThingsScenarioBase.Borrow.Common.AddComment(borrowId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Borrow.Common.AddComment(borrowId), antiForgeryValues)
                           .Set("id", borrowId.ToString())
                           .Set("Borrow.BorrowId", borrowId.ToString())
                           .Set("Comment.Text", comment)
                           .Build();

                response = await borrowClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                // Get detail borrowId
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetDetails(borrowId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.Contains(comment, responseString);
                #endregion

                #region Set score
                int score = 5;

                response = await borrowClient.GetAsync(ShareThingsScenarioBase.Borrow.Common.Score(borrowId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Borrow.Common.Score(borrowId), antiForgeryValues)
                           .Set("id", borrowId.ToString())
                           .Set("Borrow.BorrowId", borrowId.ToString())
                           .Set("Borrow.Score", score.ToString())
                           .Build();

                response = await borrowClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);

                // Get detail borrowId
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetDetails(borrowId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.Contains("name=\"Borrow.Score\" value=\"" + score + "\"", responseString);
                #endregion

                #region Reject
                response = await borrowClient.GetAsync(ShareThingsScenarioBase.Borrow.Common.Reject(borrowId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Borrow.Common.Reject(borrowId), antiForgeryValues)
                           .Set("id", borrowId.ToString())
                           .Set("Borrow.BorrowId", borrowId.ToString())
                           .Build();

                response = await borrowClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);

                // Get detail borrowId
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetDetails(borrowId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.Contains("Rejected", responseString);
                #endregion

                #region Create borrow 2
                response = await borrowClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.CreateBorrow(productId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Borrow.Post.CreateBorrow, antiForgeryValues)
                           .Set("Borrow.ProductId", productId.ToString())
                           .Set("Borrow.ProductStart", productStartDate.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.ProductEnd", productEndDate.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.Start", borrowDateStart.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Set("Borrow.End", borrowDateEnd.ToString("yyyy-MM-dd HH:mm:ss"))
                           .Build();

                response = await borrowClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);
                Assert.EndsWith(ShareThingsScenarioBase.Borrow.Get.GetBorrows, response.Headers.Location.OriginalString);

                // Get borrow Id
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetBorrows);
                responseString = await response.Content.ReadAsStringAsync();
                key = "/Borrows/Details/";
                index = responseString.LastIndexOf(key);
                borrowId = Convert.ToInt32(responseString.Substring(index + key.Length, 1));
                #endregion

                #region Confirm
                response = await borrowClient.GetAsync(ShareThingsScenarioBase.Borrow.Common.Confirm(borrowId));
                antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(response);

                postRequest = new HttpRequestMessageBuilder(
                    HttpMethod.Post, ShareThingsScenarioBase.Borrow.Common.Confirm(borrowId), antiForgeryValues)
                           .Set("id", borrowId.ToString())
                           .Set("Borrow.BorrowId", borrowId.ToString())
                           .Build();

                response = await borrowClient.SendAsync(postRequest);
                Assert.Equal(HttpStatusCode.Found, response.StatusCode);

                // Get detail borrowId
                response = await productClient.GetAsync(ShareThingsScenarioBase.Borrow.Get.GetDetails(borrowId));
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                Assert.Contains("Accepted", responseString);
                #endregion
            }
        }
    }
}