using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class Bot : Entity
    {
        private List<Word> words;
        public List<Profile> profiles;

        public string Name { get; set; }
    }
}
