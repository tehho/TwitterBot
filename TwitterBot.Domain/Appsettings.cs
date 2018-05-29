using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterBot.Domain;

namespace TwitterBot.Domain
{
    public class Appsettings
    {
        public int TweetCount { get; set; }
        public Token Customer { get; set; }
        public Token Access { get; set; }
        public string Logfile { get; set; }
    }
}
