using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure.Repository
{
    public class TwitterProfileRepository : IRepository<TwitterProfile>
    {
        private TwitterContext _context;

        public TwitterProfileRepository(TwitterContext context)
        {
            _context = context;
        }

        public TwitterProfile Add(TwitterProfile obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public TwitterProfile Get(TwitterProfile obj)
        {
            return obj?.Name != null ? _context.TwitterProfiles.FirstOrDefault(profile => profile.Name == obj.Name) : null;
        }

        public IEnumerable<TwitterProfile> GetAll()
        {
            return _context.TwitterProfiles.ToList();
        }

        public IEnumerable<TwitterProfile> SearchList(TwitterProfile obj)
        {
            if (obj == null)
                return GetAll();

            return obj?.Name != null ? 
                _context.TwitterProfiles
                .Where(profile => profile.Name.Contains(obj.Name))
                .ToList() 
                : GetAll();
        }

        public TwitterProfile Search(TwitterProfile obj)
        {
            return obj?.Name != null ? _context.TwitterProfiles.FirstOrDefault(profile => profile.Name == obj.Name) : null;
        }

        public TwitterProfile Update(TwitterProfile obj)
        {
            return obj;
        }

        public TwitterProfile Remove(TwitterProfile obj)
        {
            return obj;
        }
    }
}
