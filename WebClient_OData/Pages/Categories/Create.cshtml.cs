using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebClient_OData.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace WebClient_OData.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly WebClient_OData.Models.MyStoreContext _context;
        private readonly string CategoryUrl = "http://localhost:5213/odata/Categories";
        private readonly HttpClient httpClient;

        public CreateModel(WebClient_OData.Models.MyStoreContext context)
        {
            _context = context;
            httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Categories == null || Category == null)
            {
                return Page();
            }
            /*
                        _context.Categories.Add(Category);
                        await _context.SaveChangesAsync();

                        */
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            string jsonStr = JsonConvert.SerializeObject(Category, settings);
            StringContent stringContent = new StringContent(jsonStr,System.Text.Encoding.UTF8,"application/json");
            HttpResponseMessage res = await httpClient.PostAsync(CategoryUrl,stringContent);

            if (res.IsSuccessStatusCode)
                return RedirectToPage("/Index");
            else
                return Page();
            
        }
    }
}
