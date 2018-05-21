using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;

namespace TwitterBot.Domain
{
    class Tweet
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }
        public TwitterProfile User { get; set; }


    }
}
