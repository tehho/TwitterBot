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
        public string Name { get; set; }
        public List<TwitterProfile> Profiles { get; set; }
        public BotOptionApi()
        {
            Name = "";
            Profiles = new List<TwitterProfile>();
        }

        public static implicit operator BotOption(BotOptionApi value)
        {
            return new BotOption { Name = value.Name };
        }
    }
}
