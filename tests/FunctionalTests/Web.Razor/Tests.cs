﻿using ApplicationCore.Entities;
using Web.Razor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace FunctionalTests.Web.Razor
{
    [TestClass]
    public class Tests : FunctionalTests<Startup>
    {

        [DataTestMethod]
        [DataRow("Products")]
        [DataRow("Products/Create")]
        [DataRow("Products/Details/1")]
        [DataRow("Products/Edit/1")]
        [DataRow("Products/Delete/1")]
        [DataRow("Products/Details/2")]
        [DataRow("Products/Edit/2")]
        [DataRow("Products/Delete/2")]
        [DataRow("Products/Details/3")]
        [DataRow("Products/Edit/3")]
        [DataRow("Products/Delete/3")]
        public async Task GetRequestOfExistingProducts_ShouldGiveOkResponse(string url)
        {
            // Arrange
            HttpClient client = CreateAuthorizedTestHttpClient();

            // Act
            HttpResponseMessage getResponse = await client.GetAsync(url);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CreatingANewProductWithAPostRequest_ShouldResultInSuccessfulGetRequest()
        {
            // Arrange
            HttpClient client = CreateAuthorizedTestHttpClient();
            var newProduct = new Product()
            {
                Title = "TestTitle"
            };
            StringContent serializedProduct = SerializeObject(newProduct);
            int idOfCreatedProduct = 4;

            // Act
            HttpResponseMessage getResponseBeforeCreation = await client.GetAsync("/Products/Details/" + idOfCreatedProduct);
            HttpResponseMessage creationResponse = await client.PostAsync("/Products/Create", serializedProduct);
            HttpResponseMessage getResponseAfterCreation = await client.GetAsync("/Products/Details/" + idOfCreatedProduct);

            // Assert
            getResponseBeforeCreation.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            creationResponse.StatusCode.ShouldBe(HttpStatusCode.Redirect);
            creationResponse.Headers.Location.OriginalString.ShouldStartWith("/Products/Details/4");
            getResponseAfterCreation.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GettingTheDetailsOfAProductWithNonExistantID_ShouldResultInNotFoundResponse()
        {
            // Arrange
            HttpClient client = CreateAuthorizedTestHttpClient();

            int nonExistantProductId = 4;

            // Act
            HttpResponseMessage getResponse = await client.GetAsync("/Products/Details/" + nonExistantProductId);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task DeletingAProduct_AfterwardsShouldResultInANotFoundResponseWhenGettingTheProduct()
        {
            // Arrange
            HttpClient client = CreateAuthorizedTestHttpClient();

            int productId = 3;
            StringContent serializedProductId = SerializeObject(productId);

            // Act
            HttpResponseMessage getResponseBeforeDeletion = await client.GetAsync("Products/Details/" + productId);
            HttpResponseMessage deletionResponse = await client.PostAsync("/Products/Delete/" + productId, serializedProductId);
            HttpResponseMessage getResponseAfterDeletion = await client.GetAsync("/Products/Details/" + productId);

            // Assert
            getResponseBeforeDeletion.StatusCode.ShouldBe(HttpStatusCode.OK);
            deletionResponse.StatusCode.ShouldBe(HttpStatusCode.Redirect);
            deletionResponse.Headers.Location.OriginalString.ShouldStartWith("/Products");
            getResponseAfterDeletion.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        // The reason this test is failing is because despite the fact that we are sending a serialized product
        // within the post request, the ProductsController class seems to always use a null product.
        [TestMethod]
        public async Task EditingAnExistingProductWithAPostRequest_ShouldRespondWithARedirect()
        {
            // Arrange
            HttpClient client = CreateAuthorizedTestHttpClient();
            int idOfEditedProduct = 3;
            var editedProduct = new Product()
            {
                ProductId = idOfEditedProduct,
                Title = "New Title"
            };
            StringContent serializedProduct = SerializeObject(editedProduct);

            // Act
            HttpResponseMessage getResponse = await client.GetAsync("Products/Details/" + idOfEditedProduct);
            HttpResponseMessage editPostResponse = await client.PostAsync("/Products/Edit/" + idOfEditedProduct, serializedProduct);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            editPostResponse.StatusCode.ShouldBe(HttpStatusCode.Redirect);
            editPostResponse.Headers.Location.OriginalString.ShouldStartWith("/Products/Details/");
        }

        [TestMethod]
        public async Task EditingANonExistentProductWithAPostRequest_ShouldRespondWithANotFoundStatusCode()
        {
            // Arrange
            HttpClient client = CreateAuthorizedTestHttpClient();

            var nonExistentProduct = new Product()
            {
                ProductId = 0,
            };
            StringContent serializedProduct = SerializeObject(nonExistentProduct);

            // Act
            HttpResponseMessage getResponse = await client.PostAsync("Products/Edit", serializedProduct);

            // Assert
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

    }
}
