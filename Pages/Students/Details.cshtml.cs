using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoiseStateUniversity.Models;

namespace BoiseStateUniversity.Pages.Students
{
   public class DetailsModel : PageModel
    {
        private readonly BoiseStateUniversity.Data.SchoolContext _context;

        public DetailsModel(BoiseStateUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
