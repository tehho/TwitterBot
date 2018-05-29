using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public abstract class Profile : Entity, IProfile
    {
        public string Name { get; set; }
        public IList<WordOccurrence> Words { get; set; }

        public IReadOnlyList<Word> Vocabulary { get; }
    }
}
