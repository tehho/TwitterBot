using System.Collections;
using NUnit.Framework;
using TwitterBot.Domain;

namespace TwitterBot.Test
{
    public class TestData
    {
        public static IEnumerable ProfilesAndTweets
        {
            get
            {
                yield return new TestCaseData(new TwitterProfile("Test"), new Tweet("Jag �r bra")).Returns(3);
                yield return new TestCaseData(new TwitterProfile("Test"), new Tweet("Jag �r bra. Hej p� dig.")).Returns(7);
            }
        }
    }
}