using com.infosupport.afstuderen.opleidingsplan.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.DAL.mapper
{
    public class ProfileDataMapper : IDataMapper<Profile>
    {
        private string _path;

        public ProfileDataMapper(string path)
        {
            _path = path;
        }

        public void Delete(Profile data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Profile> Find(Func<Profile, bool> predicate)
        {
            return GetAllProfiles().Where(predicate);
        }

        public IEnumerable<Profile> FindAll()
        {
            return GetAllProfiles();
        }

        public void Insert(Profile profile)
        {
            throw new NotImplementedException();
        }

        public void Update(Profile profile)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Profile> GetAllProfiles()
        {
            string profiles = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<Profile>>(profiles);
        }
    }
}
