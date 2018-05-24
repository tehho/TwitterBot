using TwitterBot.Domain;
using TwitterBot.Infrastructure;
using TwitterBot.Infrastructure.Repository;
using NUnit.Framework;

namespace TwitterBot.Test
{
    [TestFixture]
    public class ProfileTrainerTest
    {
        private IRepository<Word> _words;
        private TwitterProfileTrainer _trainer;

        [SetUp]
        public void Init()
        {
            _words = new WordRepositoryTest();

            _trainer = new TwitterProfileTrainer(_words);

        }

        [Test]
        [TestCaseSource(typeof(TestData), "ProfilesAndTweets")]
        public int TestTrainProfile(IProfile profile, TextContent text)
        {
            _trainer.Train(profile, text);

            return profile.WordList.Count;

        }
    }
}
