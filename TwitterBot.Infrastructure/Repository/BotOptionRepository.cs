using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure.Repository
{
    public class BotOptionRepository : IRepository<BotOption>
    {
        private TwitterContext _context;

        public BotOptionRepository(TwitterContext context)
        {
            _context = context;
        }

        public BotOption Add(BotOption obj)
        {
            if (obj == null)
                return null;

            if (obj.Name == null)
                return null;

            if (obj.ProfileOccurances == null)
                return null;
            if (obj.ProfileOccurances.Count == 0)
                return null;

            _context.BotOptions.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public BotOption Get(BotOption obj)
        {
            if (obj == null)
                return null;

            if (obj.Id != null)
                return Search(option => option.Id == obj.Id);

            if (obj.Name != null)
                return Search(option => option.Name == obj.Name);

            return null;
        }

        public BotOption Update(BotOption obj)
        {
            var option = Get(obj);

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

            return option;

        }

        public BotOption Remove(BotOption obj)
        {
            var option = Get(obj);

            if (option != null)
                _context.BotOptions.Remove(option);
            return option;
        }

        public IEnumerable<BotOption> GetList(BotOption obj)
        {
            if (obj == null)
                return SearchList(option => true);

            if (obj.Name != null)
                return SearchList(option => option.Name.Contains(obj.Name));

            return SearchList(option => true);
        }

        public IEnumerable<BotOption> GetAll()
        {
            return GetList(null);
        }

        public IEnumerable<BotOption> SearchList(Predicate<BotOption> predicate)
        {
            //.Include(option => option.ProfileAlgorithms)
            //.Include(option => option.WordAlgorithms)

            return _context.BotOptions
                .Include(option => option.ProfileOccurances)
                .ThenInclude(occ => occ.Profile)
                .Where(option => predicate(option)).ToList();
        }

        public BotOption Search(Predicate<BotOption> predicate)
        {
            return _context.BotOptions.Include(option => option.ProfileAlgorithms)
                .Include(option => option.WordAlgorithms)
                .Include(option => option.ProfileOccurances)
                .ThenInclude(occ => occ.Profile)
                .SingleOrDefault(option => predicate(option));
        }

        public bool Exists(BotOption obj)
        {
            return Get(obj) != null;
        }
    }
}
