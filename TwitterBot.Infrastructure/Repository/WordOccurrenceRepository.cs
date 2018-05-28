using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure.Repository
{
    public class WordOccurrenceRepository : IRepository<WordOccurrence>
    {
        private readonly TwitterContext _context;

        public WordOccurrenceRepository(TwitterContext context)
        {
            _context = context;
        }

        public WordOccurrence Add(WordOccurrence obj)
        {
            if (obj == null)
                return null;

            _context.WordOccurrences.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public WordOccurrence Get(WordOccurrence obj)
        {
            if (obj == null)
                return null;

            if (obj.Id != null)
                return Search(occ => occ.Id == obj.Id);

            if (obj.Word != null)
                return Search(occ => occ.Word.Equals(obj.Word));

            if (obj.TwitterProfile != null)
                return Search(occ => occ.TwitterProfile.Id == obj.TwitterProfile.Id);

            return null;
        }

        public WordOccurrence Update(WordOccurrence obj)
        {
            if (obj == null)
                return null;

            var occ = Get(obj);

            if (obj.NextWordOccurrences != null)
                occ.NextWordOccurrences = obj.NextWordOccurrences;
            
            return obj;
        }

        public WordOccurrence Remove(WordOccurrence obj)
        {
            if (obj == null)
                return null;

            var occ = Get(obj);

            if (occ != null)
            {
                _context.WordOccurrences.Remove(occ);
                _context.SaveChanges();
            }

            return occ;
        }

        public IEnumerable<WordOccurrence> GetList(WordOccurrence obj)
        {
            if (obj == null)
                return SearchList(occ => true);

            if (obj.Word != null)
                return SearchList(occ => occ.Word.Value.Contains(occ.Word.Value));

            return GetList(null);
        }

        public IEnumerable<WordOccurrence> GetAll()
        {
            return GetList(null);
        }

        public IEnumerable<WordOccurrence> SearchList(Predicate<WordOccurrence> predicate)
        {
            return _context.WordOccurrences
                    .Include(occ => occ.TwitterProfile)
                    .Include(occ => occ.Word)
                    .Include(occ => occ.NextWordOccurrences)
                    .ThenInclude(nwo => nwo.FollowedBy)
                    .Where(occ => predicate(occ)).ToList();
        }

        public WordOccurrence Search(Predicate<WordOccurrence> predicate)
        {
            return _context.WordOccurrences
                .Include(occ => occ.TwitterProfile)
                .Include(occ => occ.Word)
                .Include(occ => occ.NextWordOccurrences)
                .ThenInclude(nwo => nwo.FollowedBy)
                .SingleOrDefault(occ => predicate(occ));
        }

        public bool Exists(WordOccurrence obj)
        {
            return Get(obj) != null;
        }
    }
}
