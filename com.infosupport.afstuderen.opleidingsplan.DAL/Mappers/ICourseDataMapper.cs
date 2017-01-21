using com.infosupport.afstuderen.opleidingsplan.models;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public interface ICourseDataMapper
    {
        void Delete(CoursePriority data);
        void Insert(CoursePriority data);
        void Update(CoursePriority data);
    }
}