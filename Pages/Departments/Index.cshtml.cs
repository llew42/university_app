using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoiseStateUniversity.Models;

namespace BoiseStateUniversity.Pages.Departments
{
   public class IndexModel : PageModel
    {
        private readonly BoiseStateUniversity.Data.SchoolContext _context;

        public IndexModel(BoiseStateUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get;set; }

        public async Task OnGetAsync()
        {
            Department = await _context.Departments
                .Include(d => d.Administrator).ToListAsync();
        }
    }
}
