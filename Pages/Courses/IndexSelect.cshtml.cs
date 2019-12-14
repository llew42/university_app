using BoiseStateUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoiseStateUniversity.Pages.Courses
{
   public class IndexSelectModel : PageModel
   {
      private readonly BoiseStateUniversity.Data.SchoolContext _context;

      public IndexSelectModel(BoiseStateUniversity.Data.SchoolContext context)
      {
         _context = context;
      }

      #region snippet_RevisedIndexMethod
      public IList<CourseViewModel> CourseVM { get; set; }

      public async Task OnGetAsync()
      {
         CourseVM = await _context.Courses
            .Select(p => new CourseViewModel
            {
               CourseID = p.ID,
               Title = p.Title,
               Credits = p.Credits,
               DepartmentName = p.Department.Name
            }).ToListAsync();
      }
      #endregion
   }
}