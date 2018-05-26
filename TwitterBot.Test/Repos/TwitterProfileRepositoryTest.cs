using System;
using System.Collections.Generic;
using System.Linq;
using TwitterBot.Domain;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Test
{
    internal class TwitterProfileRepositoryTest : IRepository<TwitterProfile>
    {
        private List<TwitterProfile> _list;

        public TwitterProfileRepositoryTest()
        {
            _list = new List<TwitterProfile>();
        }

        public TwitterProfile Add(TwitterProfile obj)
        {
            _list.Add(obj);
            return obj;
        }

        public TwitterProfile Get(TwitterProfile obj)
        {
            if (obj?.Name == null)
                return null;

            return Search(profile => profile.Name == obj.Name);

        }

        public TwitterProfile Update(TwitterProfile obj)
        {
            if (obj?.WordList == null)
                return null;

            var profile = Get(obj);

            if (profile == null)
                return null;

            profile.WordList = obj.WordList;

            return profile;
        }

        public TwitterProfile Remove(TwitterProfile obj)
        {
            var profile = Get(obj);

            if (profile == null)
                return null;

            _list.Remove(profile);

            return profile;
        }

        public IEnumerable<TwitterProfile> GetList(TwitterProfile obj)
        {
            if (obj == null)
                return null;

            if (obj.Name == null)
                return null;

            return SearchList(profile => profile.Name.Contains(obj.Name));
        }

        public IEnumerable<TwitterProfile> GetAll()
        {
            return SearchList(profile => true);
        }

        public IEnumerable<TwitterProfile> SearchList(Predicate<TwitterProfile> predicate)
        {
            return _list.Where(profile => predicate(profile)).ToList();
        }

        public TwitterProfile Search(Predicate<TwitterProfile> predicate)
        {
            return _list.SingleOrDefault(profile => predicate(profile));
        }

        public bool Exists(TwitterProfile obj)
        {
            return Get(obj) != null;
        }
    }
}