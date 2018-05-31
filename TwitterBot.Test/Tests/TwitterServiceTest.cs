using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TwitterBot.Domain;

namespace TwitterBot.Test.Tests
{
    [TestFixture]
    public class TwitterServiceTest
    {
        private TwitterService _service;
        [SetUp]
        public void Init()
        {

            var option = new TwitterServiceOptions(
                20,
                new Token()
                {
                    Key = "FeODQH2RhO8RvnRA0eh8i7ezF",
                    Secret = "q9g6jh54sqDXDNXm8ekU3NGZDiv97n2GMorHxVRkxonTgxbo67"
                },
                new Token()
                {
                    Key = "1002092248362815488-NII3rriYEbnmtTfOoQC6W09snLK6js",
                    Secret = "OX2xY0MzlQpApMvNRsQIUm8Q0JLJdSfb9rteQfhnTXGGf"

                });
            _service = new TwitterService(option, null);
        }

        [Test]
        public async Task TestTweet()
        {
            Assert.IsTrue(await _service.PublishTweet(new Tweet(text: "Test")));
        }

        [Test]
        public void TestGetList()
        {
            Assert.DoesNotThrow(() => _service.GetAllTweetsFromProfile(new TwitterProfile("UBoat2018")));
        }
    }
}
