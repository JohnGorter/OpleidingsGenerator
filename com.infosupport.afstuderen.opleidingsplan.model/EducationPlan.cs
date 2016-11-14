﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.model
{
    public class EducationPlan
    {
        public long ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime InPaymentFrom { get; set; }
        public DateTime EmployableFrom { get; set; }
        public string NameEmployee { get; set; }
        public string NameTeacher { get; set; }
        public string KnowledgeOf { get; set; }
        public string Profile { get; set; }


        public List<EducationPlanCourse> PlannedCourses { get; set; }
        public List<EducationPlanCourse> NotPlannedCourses { get; set; }
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
    }
}