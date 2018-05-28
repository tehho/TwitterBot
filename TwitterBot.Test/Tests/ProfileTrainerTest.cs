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
        //private IRepository<WordContainer> _container;
        private TwitterProfileTrainer _trainer;

        [SetUp]
        public void Init()
        {
            _words = new WordRepositoryTest();
            //_container = new WordContainerRepositoryTest();
            //_trainer = new TwitterProfileTrainer(_words,_container);

        }

        [Test]
        [TestCaseSource(typeof(TestData), "ProfilesAndTweets")]
        public int TestTrainProfile(TwitterProfile profile, TextContent text)
        {
            _trainer.Train(profile, text);

            return profile.Words.Count;

        }
    }
}
