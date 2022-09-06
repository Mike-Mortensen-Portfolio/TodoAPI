using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Context;
using TodoClient.Services;

namespace TodoClient.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IAsyncTodoService _service;

        public DeleteModel(IAsyncTodoService service)
        {
            _service = service;
        }

        [BindProperty]
        public Todo Todo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _service.GetTodoAsync(id.Value);

            if (todo == null)
            {
                return NotFound();
            }
            else
            {
                Todo = todo;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var success = await _service.SoftDeleteItemAsync(id.Value);

            if (success)
                return Redirect("/Index");

            return Page();
        }
    }
}
