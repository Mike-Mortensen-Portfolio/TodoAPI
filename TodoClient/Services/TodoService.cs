using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using TodoAPI.Context;

namespace TodoClient.Services
{
    public class TodoService : IAsyncTodoService
    {
        public TodoService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }


        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        private async Task<HttpClient> BuildClient()
        {
            var client = new HttpClient();
            var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            var authHeader = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.BaseAddress = new Uri($"Https://localhost:{_configuration.GetSection("ApiSettings")["port"]}/{TodoConstants.API_TODO_BASE}");

            return client;
        }

        public async Task<bool> CreateItemAsync(Todo item)
        {
            using var client = await BuildClient();

            var result = await client.PostAsJsonAsync(TodoConstants.API_TODO_CREATE, item);

            return result.StatusCode == System.Net.HttpStatusCode.Created;
        }

        public async Task<IEnumerable<Todo>> GetItemsAsync(bool includeCompleted = false)
        {
            using var client = await BuildClient();

            if (includeCompleted)
                return await client.GetFromJsonAsync<IEnumerable<Todo>>(TodoConstants.API_TODO_GET_ALL);

            return await client.GetFromJsonAsync<IEnumerable<Todo>>(TodoConstants.API_TODO_GET_UNCOMPLETED);
        }

        public async Task<Todo> GetTodoAsync(int itemId)
        {
            using var client = await BuildClient();

            var result = await client.GetAsync($"{TodoConstants.API_TODO_GET}{itemId}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return await result.Content.ReadFromJsonAsync<Todo>();

            throw new Exception("No such item found!");
        }

        public async Task<bool> SoftDeleteItemAsync(int itemId)
        {
            using var client = await BuildClient();

            var result = await client.DeleteAsync($"{TodoConstants.API_TODO_SOFT_DELETE}{itemId}");

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> DeleteItemAsync(int itemId)
        {
            using var client = await BuildClient();

            var result = await client.DeleteAsync($"{TodoConstants.API_TODO_DELETE}{itemId}");

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task UpdateItemAsync(Todo item)
        {
            using var client = await BuildClient();

            var result = await client.PutAsJsonAsync($"{TodoConstants.API_TODO_UPDATE}{item.Id}", item);

            if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new Exception("No such item found!");
        }
    }
}
