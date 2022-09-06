using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Context;
using TodoClient.Services;

namespace TodoClient.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public IndexModel(IAsyncTodoService service)
        {
            _service = service;
        }

        private readonly IAsyncTodoService _service;

        public IList<Todo> Todo { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public bool DisplayAll { get; set; }

        public async Task OnGetAsync(bool? includeCompleted = false)
        {
            DisplayAll = includeCompleted.Value;

            if (_service.GetItemsAsync() != null)
            {
                Todo = (await _service.GetItemsAsync(includeCompleted ?? false))
                    .ToList();
            }
        }
    }
}
