using System;
using System.Collections.Generic;
using System.Text;
using TwitterBot.Domain;

namespace TwitterServiceTests
{
    class TestData
    {
        public static TwitterService NewTwitterService()
        {

            return new TwitterService(null
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


        }

    }
}
