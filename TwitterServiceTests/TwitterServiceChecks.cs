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

            var tweetService = new TwitterService(null
                , new Token
                {
                    Key = "GjMrzt4a9YJqKXRTNKjLN2CVi",
                    Secret = "w3koS8pDXMxDscBZnT7VFgGFeoNgv0qxgUa5YYcvrv2WoysfRD"
                },
                new Token()
                {
                    Key = "998554298735845382-cHyJyzufzzSUzceD79y8zb0IkbfrPxi",
                    Secret = "B72OlpxIme0yz3ZHRVw0mCMDxKukXTcNuOvhD9d0ySCX8"
                });

            TwitterProfile profileThatExists = new TwitterProfile()
            {
                Name = "VP"
            };

            TwitterProfile profileThatDoesntExist = new TwitterProfile()
            {
                Name = "INJsdfmsfomswfm11111"
            };

            var shouldBeTrue = tweetService.DoesTwitterUserExist(profileThatExists);

            var shouldBeFalse = tweetService.DoesTwitterUserExist(profileThatDoesntExist);

            Assert.IsTrue(shouldBeTrue);
            Assert.IsFalse(shouldBeFalse);

        }
    }
}
