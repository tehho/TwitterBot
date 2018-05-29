using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterBot.Domain;

namespace TwitterBot.Api.Model
{
    public class BotOptionApi
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public List<TwitterProfile> Profiles { get; set; }

        public BotOptionApi()
        {
            Id = null;
            Name = "";
            Profiles = new List<TwitterProfile>();
        }

        public static implicit operator BotOptions(BotOptionApi value)
        {
            return new BotOptions { Name = value.Name };
        }
    }
}
