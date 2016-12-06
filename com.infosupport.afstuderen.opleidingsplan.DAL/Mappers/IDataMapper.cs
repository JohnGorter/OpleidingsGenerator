using System;
using System.Collections.Generic;

namespace com.infosupport.afstuderen.opleidingsplan.dal.mappers
{
    public interface IDataMapper<T>
    {
        T FindById(long id);
        void Insert(T data);
        IEnumerable<T> FindAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
        void Update(T data);
        void Delete(T data);
    }
}