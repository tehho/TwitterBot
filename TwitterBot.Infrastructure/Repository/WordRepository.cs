using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure.Repository
{
    public class WordRepository : IRepository<Word>
    {
        public TwitterContext _context;

        public WordRepository(TwitterContext context)
        {
            _context = context;
        }

        public Word Add(Word obj)
        {
            if (obj?.Value == null)
                return null;

            var temp = obj;
            temp.Id = null;

            if (Exists(temp))
                return Get(temp);

            try
            {
                _context.Words.Add(temp);
                _context.SaveChanges();
            } 
            catch (Exception e)
            {
                return null;
            }

            return temp;
        }

        public Word Get(Word obj)
        {
            return obj?.Value != null ? Search(word => word.Value == obj.Value) : null;
        }

        public Word Update(Word obj)
        {
            var word = Get(obj);

            return word;
        }

        public Word Remove(Word obj)
        {
            var word = Get(obj);

            if (word != null)
                _context.Words.Remove(word);

            return word;
        }

        public IEnumerable<Word> GetList(Word obj)
        {
            if (obj == null)
                return SearchList(x => true);

            return obj.Value != null ? SearchList(word => word.Value.Contains(obj.Value)) : null;
        }

        public IEnumerable<Word> GetAll()
        {
            return GetList(null);
        }

        public IEnumerable<Word> SearchList(Predicate<Word> predicate)
        {
            return _context.Words.Where(word => predicate(word)).ToList();
        }

        public Word Search(Predicate<Word> predicate)
        {
            return _context.Words.SingleOrDefault(word => predicate(word));
        }

        public bool Exists(Word obj)
        {
            return Get(obj) != null;
        }
    }
}
