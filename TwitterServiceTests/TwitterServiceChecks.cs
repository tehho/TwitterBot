using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterBot.Domain;

namespace TwitterServiceTests
{
    [TestClass]
    public class TwitterServiceTests
    {

        [TestMethod]
        public void CheckIfUserExists()
        {

            TwitterProfile profileThatExists = new TwitterProfile()
            {
                Name = "VP"
            };


            var result = TestData.NewTwitterService().DoesTwitterUserExist(profileThatExists);


            Assert.IsTrue(result);

        }

        [TestMethod]
        public void CheckIfUserDoesntExists()
        {

            TwitterProfile profileThatDoesntExist = new TwitterProfile()
            {
                Name = "INJsdfmsfomswfm11111"
            };


            var result = TestData.NewTwitterService().DoesTwitterUserExist(profileThatDoesntExist);

            Assert.IsFalse(result);

        }

        [TestMethod]
        public void CheckIfUserHasTweets()
        {

            TwitterProfile profileThatHasTweets = new TwitterProfile()
            {
                Name = "1d11fxhn1xfgh51fxh51fxh51fgh51"
            };


            var result = TestData.NewTwitterService().ProfileTimeLineHasTweets(profileThatHasTweets);

            Assert.IsTrue(result);

        }

        [TestMethod]
        public void CheckIfUserDoesntHaveTweets()
        {

            TwitterProfile profileThatDoesntExist = new TwitterProfile()
            {
                Name = "VP"
            };


            var result = TestData.NewTwitterService().DoesTwitterUserExist(profileThatDoesntExist);

            Assert.IsTrue(result);

        }
    }
}
