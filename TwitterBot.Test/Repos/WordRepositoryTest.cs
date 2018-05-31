using System;
using System.Collections.Generic;
using System.Linq;
using TwitterBot.Domain;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Test
{
    internal class WordRepositoryTest : IRepository<Word>
    {
        private List<Word> _list;

        public WordRepositoryTest()
        {
            _list = new List<Word>();
        }

        public Word Add(Word obj)
        {
            if (obj == null)
                return null;

            if (obj.Value == null)
                return null;

            _list.Add(obj);

            return obj;
        }

        public Word Get(Word obj)
        {
            if (obj == null)
                return null;

            if (obj.Value == null)
                return null;

            return Search(word => word.Value == obj.Value);
        }


        public Word Update(Word obj)
        {
            throw new InvalidOperationException();
        }

        public Word Remove(Word obj)
        {
            var word = Get(obj);

            if (word == null)
                return null;

            _list.Remove(word);

            return word;
        }

        public IEnumerable<Word> GetList(Word obj)
        {
            if (obj == null)
                return null;

            if (obj.Value == null)
                return null;

            return SearchList(word => word.Value.Contains(obj.Value));
        }

        public IEnumerable<Word> GetAll()
        {
            return SearchList(word => true);
        }

        public IEnumerable<Word> SearchList(Predicate<Word> predicate)
        {
            return _list.Where(word => predicate(word)).ToList();
        }

        public Word Search(Predicate<Word> predicate)
        {

            return _list.SingleOrDefault(word => predicate(word));
        }

        public bool Exists(Word obj)
        {
            return Get(obj) != null;
        }
    }
}