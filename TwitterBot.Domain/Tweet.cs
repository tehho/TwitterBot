using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;

namespace TwitterBot.Domain
{
    public class Tweet : IStatus
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreateDate { get; set; }

        public int UserId { get; set; }
        public TwitterProfile User { get; set; }


    }
}
