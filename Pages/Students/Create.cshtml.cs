using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BoiseStateUniversity.Models;

namespace BoiseStateUniversity.Pages.Students
{
   public class CreateModel : PageModel
    {
        private readonly BoiseStateUniversity.Data.SchoolContext _context;

        public CreateModel(BoiseStateUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate
            ))
            {
                _context.Students.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
