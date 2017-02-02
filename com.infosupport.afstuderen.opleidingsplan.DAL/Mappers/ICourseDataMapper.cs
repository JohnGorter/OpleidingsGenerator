using InfoSupport.KC.OpleidingsplanGenerator.Models;
using System.Collections.Generic;

namespace InfoSupport.KC.OpleidingsplanGenerator.Dal.Mappers
{
    public interface ICourseDataMapper
    {
        void Delete(CoursePriority data);
        void Insert(CoursePriority data);
        void Update(CoursePriority data);
    }
}