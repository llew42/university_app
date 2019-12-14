using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoiseStateUniversity.Models;

namespace BoiseStateUniversity.Pages.Courses
{
   public class IndexModel : PageModel
    {
        private readonly BoiseStateUniversity.Data.SchoolContext _context;

        public IndexModel(BoiseStateUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get;set; }

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
