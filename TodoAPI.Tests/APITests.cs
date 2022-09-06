using Microsoft.AspNetCore.Mvc.Testing;

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

        public async void Get_Uncompleted_Todo_Items ()
        {

        }
    }
}