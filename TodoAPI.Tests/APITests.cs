using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TodoAPI.Context;

namespace TodoAPI.Tests
{
    public class APITests
    {
        [Fact]
        public async void Hello_World_Root_Enpoint()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            var response = await client.GetStringAsync("/");

            Assert.Equal("Hello, World!", response);
        }

        [Theory]
        [InlineData("Test 1", 1, false, Priority.Low)]
        [InlineData("Test 2", 2, false, Priority.Normal)]
        [InlineData("Test 3", 3, true, Priority.High)]
        public async void Todo_Item_201_Created(string desc, int id, bool isComplete, Priority priority)
        {
            await using var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var result = await client.PostAsJsonAsync("/todoitems", new Todo
            {
                CreatedTime = DateTime.Now,
                Description = desc,
                Id = id,
                IsComplete = isComplete,
                Priority = priority
            });

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

        [Theory]
        [InlineData(0, "Test 1", -1, false, Priority.Low)]
        [InlineData(+5, "Test 2", 2, false, Priority.Normal)]
        public async void Todo_Item_400_BadRequest(int datetimeOffset, string desc, int id, bool isComplete, Priority priority)
        {
            await using var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var result = await client.PostAsJsonAsync("/todoitems", new Todo
            {
                CreatedTime = DateTime.Now.AddDays(datetimeOffset),
                Description = desc,
                Id = id,
                IsComplete = isComplete,
                Priority = priority
            });

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}