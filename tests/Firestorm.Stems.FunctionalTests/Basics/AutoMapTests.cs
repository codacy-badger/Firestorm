﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.FunctionalTests.Models;
using Firestorm.Stems.FunctionalTests.Web;
using Firestorm.Stems.Roots.DataSource;
using Firestorm.Testing.Http;
using Newtonsoft.Json;
using Xunit;

namespace Firestorm.Stems.FunctionalTests.Basics
{
    public class AutoMapTests : IClassFixture<ExampleFixture<AutoMapTests>>
    {
        private HttpClient HttpClient { get; }

        public AutoMapTests(ExampleFixture<AutoMapTests> fixture)
        {
            HttpClient = fixture.HttpClient;
        }

        [DataSourceRoot]
        public class ArtistsStem : Stem<Artist>
        {
            [Identifier, AutoExpr]
            public static int ArtistID { get; set; }

            [Get, AutoExpr]
            public static string Name { get; set; }

            [Get, AutoExpr]
            public static DateTime StartDate { get; set; }
        }

        [Fact]
        public async Task ArtistsCollection_Get_StatusOK()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists");

            ResponseAssert.Success(response);
        }

        [Fact]
        public async Task ArtistItem_Get_Deserialises()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/1");
            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            object obj = JsonConvert.DeserializeObject(json);

        }

        [Fact]
        public async Task ArtistNameScalar_Get_Correct()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists/1/name");

            ResponseAssert.Success(response);

            string responseStr = await response.Content.ReadAsStringAsync();

            Assert.Equal("\"Eminem\"", responseStr);
        }

        [Fact]
        public async Task ArtistsCollection_GetWithFields_DeserialisesAndCorrect()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("/artists?fields=start_date,name");
            ResponseAssert.Success(response);

            string json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            string name = obj[0].name;
            Assert.Equal("Eminem", name);
        }
    }
}