using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebClient_OData.Models;

namespace WebClient_OData.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly WebClient_OData.Models.MyStoreContext _context;
        private readonly string CategoryUrl = "http://localhost:5213/odata/Categories";
        private readonly HttpClient httpClient;
        public IndexModel(WebClient_OData.Models.MyStoreContext context)
        {
            _context = context;
            httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            /*if (_context.Categories != null)
            {
                Category = await _context.Categories.ToListAsync();
            }*/
            HttpResponseMessage res = await httpClient.GetAsync(CategoryUrl);
            string jsonStr = await res.Content.ReadAsStringAsync();
            var temp = JObject.Parse(jsonStr);
            dynamic list = temp["value"];
            Category = JsonConvert.DeserializeObject<List<Category>>(list.ToString());
        }
    }
}
