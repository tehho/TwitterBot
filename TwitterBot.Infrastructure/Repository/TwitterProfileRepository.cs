﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            if (obj?.Name == null)
                return null;

            _context.TwitterProfiles.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public TwitterProfile Get(TwitterProfile obj)
        {
            if (obj == null)
                return null;

            if (obj.Name != null)
            {
                return Search(profile => profile.Name == obj.Name);
            }

            //if (obj.Id != null)
            //{
            //    return Search(profile => profile.Id == obj.Id);
            //}

            

            return null;
        }

        public IEnumerable<TwitterProfile> GetList(TwitterProfile obj)
        {
            if (obj == null)
                return SearchList(profile => true);

            if (obj.Id != null)
                return SearchList(profile => profile.Id == obj.Id);

            if (obj.Name != null)
                return SearchList(profile => profile.Name == obj.Name);

            return GetAll();
        }

        public IEnumerable<TwitterProfile> GetAll()
        {
            return GetList(null);
        }

        public IEnumerable<TwitterProfile> SearchList(Predicate<TwitterProfile> predicate)
        {
            return _context.TwitterProfiles.Include(p => p.Words).ThenInclude(list => list.Word).Where(profile => predicate(profile)).ToList();
        }

        public TwitterProfile Search(Predicate<TwitterProfile> predicate)
        {
            return _context.TwitterProfiles
                .Include(p => p.Words)
                .ThenInclude(w => w.Word)
                .Include(p => p.Words)
                .ThenInclude(w => w.NextWords)
                .ThenInclude(n => n.Word)
                .SingleOrDefault(profile => predicate(profile));
        }

        public bool Exists(TwitterProfile obj)
        {
            return _context.TwitterProfiles.Any(profile => profile.Name == obj.Name);
        }

        public TwitterProfile Update(TwitterProfile obj)
        {
            if (obj?.Words == null)
                return null;

            var profile = Get(obj);

            if (profile == null)
            {
                return Add(obj);
            }

            profile.Words = obj.Words;
            _context.SaveChanges();

            return profile;
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
