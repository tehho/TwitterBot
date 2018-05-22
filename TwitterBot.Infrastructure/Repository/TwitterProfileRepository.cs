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
        private readonly TwitterContext _context;

        public TwitterProfileRepository(TwitterContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public TwitterProfile Add(TwitterProfile obj)
        {
            _context.TwitterProfiles.Add(obj);
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

        public IEnumerable<TwitterProfile> SearchList(Predicate<TwitterProfile> predicate)
        {
            return _context.TwitterProfiles.Where(profile => predicate(profile)).ToList();
        }

        public TwitterProfile Search(TwitterProfile obj)
        {
            return obj?.Name != null ? _context.TwitterProfiles.FirstOrDefault(profile => profile.Name == obj.Name) : null;
        }

        public bool Exists(TwitterProfile obj)
        {
            return _context.TwitterProfiles.Any(profile => profile.Name == obj.Name);
        }

        public TwitterProfile Update(TwitterProfile obj)
        {
            return obj;
        }

        public TwitterProfile Remove(TwitterProfile obj)

        {
            var profile = Get(obj);

            if (profile != null)
            {
                _context.TwitterProfiles.Remove(profile);
                _context.SaveChanges();
            }

            return profile;
        }
    }
}
