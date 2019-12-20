using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoiseStateUniversity.Data;
using BoiseStateUniversity.Models;

namespace BoiseStateUniversity.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly BoiseStateUniversity.Data.SchoolContext _context;

        public DeleteModel(BoiseStateUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }
        public string ConcurrencyErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? concurrencyError)
        {
            Department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (Department == null)
            {
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ConcurrencyErrorMessage = "The record you attempted to delete was modified by another user after you selected delete. The delete operation was canceled and the current values in the database have been displayed. If you still want to delete this record, click Delete again.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (await _context.Departments.AnyAsync(
                    m => m.ID == id
                ))
                {
                    _context.Departments.Remove(Department);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToPage("./Delete", new { concurrencyError = true, id = id});   
            }

        }
    }
}
