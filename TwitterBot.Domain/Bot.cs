using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class Bot : Entity
    {
        public string Name { get; set; }
        public Dictionary<Word, int> Dictionary { get; set; }
    }
}
