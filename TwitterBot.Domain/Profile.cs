using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public abstract class Profile : Entity, ITrainableFromText
    {
        [Required]
        public string Name { get; set; }
        public List<Word> Words { get; set; }

        protected Profile(string name)
        {
            Name = name;
            Words = new List<Word>();
        }

        public void TrainFromText(TextContent text)
        {
            throw new NotImplementedException();
        }
    }
}
