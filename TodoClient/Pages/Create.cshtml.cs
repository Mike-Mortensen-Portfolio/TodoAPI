using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoAPI.Context;
using TodoClient.Services;

namespace TodoClient.Pages
{
    public class CreateModel : PageModel
    {
        public CreateModel(IAsyncTodoService service)
        {
            _service = service;
        }

        private readonly IAsyncTodoService _service;

        public IActionResult OnGet()
        {
            Todo = new Todo();
            Todo.Priority = Priority.Low;
            Todo.CreatedTime = DateTime.Now;

            return Page();
        }

        [BindProperty]
        public Todo Todo { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Todo == null)
            {
                return Page();
            }

            var currentTop = (await _service.GetItemsAsync(true))
                .OrderBy(i => i.Id)
                .LastOrDefault()?.Id ?? 1;

            Todo.Id = currentTop + 1;

            await _service.CreateItemAsync(Todo);

            return Redirect($"/Details?id={Todo.Id}");
        }
    }
}
