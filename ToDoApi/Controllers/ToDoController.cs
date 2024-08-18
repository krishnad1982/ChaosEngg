using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController(ITodosClient clientService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetPosts()
        {
            var result = await clientService.GetTodosAsync(new CancellationToken());
            return result.Any() ? Ok() : StatusCode(500);
        }
    }
}
