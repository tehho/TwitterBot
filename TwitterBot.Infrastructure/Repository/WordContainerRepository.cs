using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;

namespace TwitterBot.Infrastructure.Repository
{
    public class WordContainerRepository : IRepository<WordContainer>
    {
        private readonly TwitterContext _context;
        public WordContainerRepository(TwitterContext context)
        {
            _context = context;
        }

        public WordContainer Add(WordContainer obj)
        {
            if (obj == null)
                return null;
            if (obj.Word == null)
                return null;

            _context.Containers.Add(obj);

            return obj;
        }

        public WordContainer Get(WordContainer obj)
        {
            return obj?.Id != null ? Search(container => container.Id == obj.Id) : null;
        }

        public WordContainer Update(WordContainer obj)
        {
            if (obj == null)
                return null;

            var container = Get(obj);

            if (container != null)
            {
                if (obj.Occurrances != null)
                    container.Occurrances = obj.Occurrances;
            }

            return container;
        }

        public WordContainer Remove(WordContainer obj)
        {
            var container = Get(obj);

            if (container != null)
                _context.Containers.Remove(container);

            return container;
        }

        public IEnumerable<WordContainer> GetList(WordContainer obj)
        {
            if (obj == null)
                return null;

            if (obj.Word != null)
                return SearchList(container => container.Word == obj.Word);

            return null;
        }

        public IEnumerable<WordContainer> GetAll()
        {
            return SearchList(container => true);

        }

        public IEnumerable<WordContainer> SearchList(Predicate<WordContainer> predicate)
        {
            return _context.Containers.Include(container => container.Word).Include(container => container.Occurrances).ThenInclude(occ => occ.Word).Where(container => predicate(container)).ToList();
        }

        public WordContainer Search(Predicate<WordContainer> predicate)
        {
            return _context.Containers.Include(container => container.Word).Include(container => container.Occurrances).ThenInclude(occ => occ.Word).SingleOrDefault(container => predicate(container));
        }

        public bool Exists(WordContainer obj)
        {
            return Get(obj) != null;
        }
    }
}
