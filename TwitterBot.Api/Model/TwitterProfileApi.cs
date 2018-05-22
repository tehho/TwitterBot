using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.Internal;
using TwitterBot.Domain;

namespace TwitterBot.Api.Model
{
    public class TwitterProfileApi
    {
        public string Name { get; set; }
        public List<IStatus> _tweets { get; set; }

        public TwitterProfileApi()
        {
            Name = "";
            _tweets = new List<IStatus>();
        }

        public TwitterProfile ToDomain()
        {
            return new TwitterProfile
            {
                Name = Name,
            };
        }
    }
}
