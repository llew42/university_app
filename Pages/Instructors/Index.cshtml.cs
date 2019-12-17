using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoiseStateUniversity.Models;
using BoiseStateUniversity.Models.SchoolViewModels;
using System.Linq;
using System;

namespace BoiseStateUniversity.Pages.Instructors
{
   public class IndexModel : PageModel
    {
        private readonly BoiseStateUniversity.Data.SchoolContext _context;

        public IndexModel(BoiseStateUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id, int? courseID)
        {
            InstructorData = new InstructorIndexData();
            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                // EAGER LOADING METHOD 
                // .Include(i => i.CourseAssignments)
                //     .ThenInclude(i => i.Course)
                //         .ThenInclude(i => i.Enrollments)
                //             .ThenInclude(i => i.Student)
                .OrderBy(i => i.LastName)
                .ToListAsync();
            
            Console.WriteLine("InstructorType ," );
            Console.WriteLine(typeof(Instructor));
            if (id != null)
            {
                InstructorID = id.Value;
                Instructor instructor = InstructorData.Instructors
                    .SingleOrDefault(i => i.ID == id.Value);
                InstructorData.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                CourseID = courseID.Value;
                var selectedCourse = InstructorData.Courses
                    .SingleOrDefault(x => x.ID == courseID);
                // EXPLICIT LOADING METHOD
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }
        public IList<Instructor> Instructor { get; set; }
    }
}
