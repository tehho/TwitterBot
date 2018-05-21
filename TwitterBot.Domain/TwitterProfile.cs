using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TwitterBot.Domain
{
    public class TwitterProfile
    {
        public string Name { get; set; }

        private List<Tweet> _tweets { get; set; }

        public TwitterProfile()
        {

        }

        public Dictionary<string, int> GenerateMetaData()
        {
            var regex = new Regex("/(\\.| |!|\\?)/");

            var dicReturn = new Dictionary<string, int>();

            _tweets.ForEach(tweet =>
            {
                var list = regex.Split(tweet.Message).ToList();

                list.Where(string.IsNullOrWhiteSpace).ToList().ForEach(word =>
                {
                    if (dicReturn.ContainsKey(word))
                        dicReturn[word]++;
                    else
                        dicReturn.Add(word, 1);
                });

            });


            return dicReturn;
        }
    }
}
