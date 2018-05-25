using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public class TwitterProfile : Entity, IProfile
    {
        public string Name { get; set; }
        
        public IReadOnlyList<Word> Vocabulary => WordList?.Where(occurrence => occurrence.ProfileId == Id).Select(w => w.WordContainer.Word).ToList();
        public IReadOnlyList<WordContainer> Containers => WordList?.Select(wco => wco.WordContainer).ToList();

        public List<WordContainerOccurrence> WordList { get; set; }

        public TwitterProfile()
        {
            Name = "";
            WordList = new List<WordContainerOccurrence>();
        }

        public TwitterProfile(string name)
        {
            Name = name;
            WordList = new List<WordContainerOccurrence>();
        }

        public WordContainer AddWordContainer(WordContainer word)
        {
            if (word == null)
                throw new NullReferenceException();

            if (word.Word == null)
                throw new ArgumentNullException();

            if (word.Word.Value == null)
                throw new ArgumentNullException();

            if (WordList == null)
                WordList = new List<WordContainerOccurrence>();

            var container = WordList.SingleOrDefault(wco => wco.WordContainer.Word.Equals(word.Word));

            if (container == null)
            {
                container = new WordContainerOccurrence(word, this);
                WordList.Add(container);
            }
            else
            {
                container.Occurrence++;
            }

            return container.WordContainer;
        }
    }
}