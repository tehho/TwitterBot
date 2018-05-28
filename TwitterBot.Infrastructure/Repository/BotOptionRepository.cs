using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure.Repository
{
    public class BotOptionRepository : IRepository<BotOptions>
    {
        private TwitterContext _context;

        public BotOptionRepository(TwitterContext context)
        {
            _context = context;
        }

        public BotOptions Add(BotOptions obj)
        {
            if (obj == null)
                return null;

            if (obj.Name == null)
                return null;

            _context.BotOptions.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public BotOptions Get(BotOptions obj)
        {
            if (obj == null)
                return null;

            if (obj.Id != null)
                return Search(option => option.Id == obj.Id);

            if (obj.Name != null)
                return Search(option => option.Name == obj.Name);

            return null;
        }

        public BotOptions Update(BotOptions obj)
        {
            var option = Get(obj);
            try
            {
                if (option != null)
                {
                    if (obj.ProfileOccurances != null)
                        option.ProfileOccurances = obj.ProfileOccurances;

                    if (obj.ProfileAlgorithms != null)
                        option.ProfileAlgorithms = obj.ProfileAlgorithms;

                    if (obj.WordAlgorithms != null)
                        option.WordAlgorithms = obj.WordAlgorithms;

                    _context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            

            return option;

        }

        public BotOptions Remove(BotOptions obj)
        {
            var option = Get(obj);

            if (option != null)
                _context.BotOptions.Remove(option);
            return option;
        }

        public IEnumerable<BotOptions> GetList(BotOptions obj)
        {
            if (obj == null)
                return SearchList(option => true);

            if (obj.Name != null)
                return SearchList(option => option.Name.Contains(obj.Name));

            return SearchList(option => true);
        }

        public IEnumerable<BotOptions> GetAll()
        {
            return GetList(null);
        }

        public IEnumerable<BotOptions> SearchList(Predicate<BotOptions> predicate)
        {
            //.Include(option => option.ProfileAlgorithms)
            //.Include(option => option.WordAlgorithms)

            return _context.BotOptions
                .Include(option => option.ProfileOccurances)
                .ThenInclude(occ => occ.Profile)
                .Where(option => predicate(option)).ToList();
        }

        public BotOptions Search(Predicate<BotOptions> predicate)
        {
            //.Include(option => option.ProfileAlgorithms)
            //.Include(option => option.WordAlgorithms)
            return _context.BotOptions
                .Include(option => option.ProfileOccurances)
                .ThenInclude(occ => occ.Profile)
                .ThenInclude(p => p.Words)
                .ThenInclude(w => w.Word)

                .Include(option => option.ProfileOccurances)
                .ThenInclude(occ => occ.Profile)
                .ThenInclude(p => p.Words)
                .ThenInclude(w => w.NextWordOccurrences)
                .ThenInclude(wno => wno.Word)
                
                .SingleOrDefault(option => predicate(option));
        }

        public bool Exists(BotOptions obj)
        {
            return Get(obj) != null;
        }
    }
}
