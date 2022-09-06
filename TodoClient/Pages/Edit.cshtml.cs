using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Context;
using TodoClient.Services;

namespace TodoClient.Pages
{
    public class EditModel : PageModel
    {
        private readonly IAsyncTodoService _service;

        public EditModel(IAsyncTodoService service)
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

            Todo = todo;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _service.UpdateItemAsync(Todo);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }

            return Redirect($"/Details?id={Todo.Id}");
        }
    }
}
