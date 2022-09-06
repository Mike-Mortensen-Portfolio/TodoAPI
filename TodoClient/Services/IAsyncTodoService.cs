using TodoAPI.Context;

namespace TodoClient.Services
{
    public interface IAsyncTodoService
    {
        Task<bool> CreateItemAsync(Todo item);
        Task<IEnumerable<Todo>> GetItemsAsync(bool includeCompleted = false);
        Task<Todo> GetTodoAsync(int itemId);
        Task UpdateItemAsync(Todo item);
        Task<bool> SoftDeleteItemAsync(int itemId);
        Task<bool> DeleteItemAsync(int itemId);
    }
}
