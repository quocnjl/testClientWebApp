using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientWebApp.Services
{

    public class TodoService
    {
        public const string CacheKey = "todos";
        public static async Task<string[]> GetTodosAsync(IDatabase database)
        {
            var v = (await database.ListRangeAsync(CacheKey));
            return v.Select(x => x.ToString()).ToArray();
        }

        public static async Task CreateTodo(IDatabase database, string todo)
        {
            await database.ListRightPushAsync(CacheKey, todo);
        }
        public static async Task ClearTodo(IDatabase database)
        {
            await database.KeyDeleteAsync(CacheKey);
        }
    }
}
