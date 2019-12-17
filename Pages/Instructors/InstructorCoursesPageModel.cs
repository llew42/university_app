using BoiseStateUniversity.Data;
using BoiseStateUniversity.Models;
using BoiseStateUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace BoiseStateUniversity.Pages.Instructors
{
   public class InstructorCoursesPageModel : PageModel
   {
      public List<AssignedCourseData> AssignedCourseDataList;

      public void PopulateAssignedCourseData(SchoolContext context, Instructor instructor)
      {
         var allCourses = context.Courses;
         var instructorCourses = new HashSet<int>(
            instructor.CourseAssignments.Select(c => c.CourseID)
         );
         AssignedCourseDataList = new List<AssignedCourseData>();
         
         foreach (var course in allCourses)
         {
            AssignedCourseDataList.Add(new AssignedCourseData
            {
               CourseID = course.ID,
               Title = course.Title,
               Assigned = instructorCourses.Contains(course.ID)
            });
         }
      }

      public void UpdateInstructorCourses(SchoolContext context, string[]selectedCourses, Instructor instructorToUpdate)
      {
         if (selectedCourses == null)
         {
            instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
            return;
         }

         var selectedCoursesHS = new HashSet<string>(selectedCourses);
         var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.Course.ID));
         foreach (var course in context.Courses)
         {
            if (selectedCoursesHS.Contains(course.ID.ToString()))
            {
               if (!instructorCourses.Contains(course.ID))
               {
                  instructorToUpdate.CourseAssignments.Add(
                     new CourseAssignment
                     {
                        InstructorID = instructorToUpdate.ID,
                        CourseID = course.ID
                     }
                  );
               }
            }
            else
            {
               if (instructorCourses.Contains(course.ID))
               {
                  CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments.SingleOrDefault(i => i.CourseID == course.ID);
                  context.Remove(courseToRemove);
               }
            }
         }
      }
   }
}