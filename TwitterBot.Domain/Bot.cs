using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterBot.Domain
{
    public class Bot : Entity
    {
        private Dictionary<Profile, int> profiles;

        [Required]
        public string Name { get; set; }
        
        public Bot(string name)
        {
            Name = name;
            profiles = new Dictionary<Profile, int>();
        }
    }
}
