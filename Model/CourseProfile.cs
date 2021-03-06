﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSupport.KC.OpleidingsplanGenerator.Models
{
    public class CourseProfile
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<CoursePriority> Courses { get; set; } = new List<CoursePriority>();

        public override string ToString()
        {
            return Name;
        }
    }
    public class CoursePriority
    {
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public int ProfileId { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Code: {1}, Priority: {2}, ProfileId: {3}", Id, Code, Priority, ProfileId);
        }
    }
}
