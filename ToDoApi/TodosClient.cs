using System.Text.Json.Serialization;

namespace ToDoApi
{
    public class TodosClient(HttpClient httpClient)
        : ITodosClient
    {
        public async Task<IEnumerable<TodoModel>> GetTodosAsync(CancellationToken cancellationToken)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<TodoModel>>("/todos", cancellationToken) ?? [];
        }
    }

    public interface ITodosClient
    {
        Task<IEnumerable<TodoModel>> GetTodosAsync(CancellationToken cancellationToken);
    }

    public record TodoModel(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("title")] string Title);
}
