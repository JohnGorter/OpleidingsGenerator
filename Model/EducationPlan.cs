using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
{
    public class EducationPlan
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime InPaymentFrom { get; set; }
        public DateTime EmployableFrom { get; set; }
        public List<DateTime> BlockedDates { get; set; }
        public string NameEmployee { get; set; }
        public string NameTeacher { get; set; }
        public string KnowledgeOf { get; set; }
        public string Profile { get; set; }
        public int ProfileId { get; set; }

        public IEnumerable<EducationPlanCourse> CoursesJustBeforeStart { get; set; }
        public IEnumerable<EducationPlanCourse> PlannedCourses { get; set; }
        public IEnumerable<EducationPlanCourse> NotPlannedCourses { get; set; }
        public decimal PlannedCoursesTotalPrice
        {
            get
            {
                return PlannedCourses.Sum(course => course.Price);
            }
        }

        public decimal PlannedCoursesTotalPriceWithDiscount
        {
            get
            {
                return PlannedCourses.Sum(course => course.PriceWithDiscount);
            }
        }

        public decimal NotPlannedCoursesTotalPrice
        {
            get
            {
                return NotPlannedCourses.Sum(course => course.Price);
            }
        }

        public decimal NotPlannedCoursesTotalPriceWithDiscount
        {
            get
            {
                return NotPlannedCourses.Sum(course => course.PriceWithDiscount);
            }
        }


        public override string ToString()
        {
            return string.Format("Id: {0}, NameEmployee: {1}, NameTeacher: {2}", Id, NameEmployee, NameTeacher);
        }
    }
}
