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
        public Dictionary<string, int> Dictionary
        {
            get
            {
                Dictionary<string, int> dicReturn = new Dictionary<string, int>();

                profiles.ForEach(p =>
                {
                    var dic = p.GenerateMetaData();
                    var keys = dic.Keys.ToList();
                    keys.ForEach(k =>
                    {
                        if (dicReturn.ContainsKey(k))
                            dicReturn[k] += dic[k];
                        else
                            dicReturn.Add(k, dic[k]);
                    });
                });

                return dicReturn;
            }
        }

        private List<TwitterProfile> profiles;
    }
}
