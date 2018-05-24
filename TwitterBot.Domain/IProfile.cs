using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Domain
{
    public interface IProfile
    {
        [Required]
        string Name { get; set; }
        List<WordContainerOccurrence> WordList { get; set; }

        IReadOnlyList<Word> Vocabulary { get; }

        IReadOnlyList<WordContainer> Containers { get; }
         WordContainer AddWordContainer(WordContainer word);
    }
}