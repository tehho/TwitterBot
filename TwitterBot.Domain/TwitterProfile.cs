using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace TwitterBot.Domain
{
    public class TwitterProfile
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<IStatus> _tweets { get; set; }

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
