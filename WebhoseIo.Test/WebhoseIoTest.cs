using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webhoseio;

namespace WebhoseIo.Test
{
    [TestClass]
    public class WebhoseIoTest
    {
        [TestMethod]
        public void Should_Return_Data_By_Orgnaization_newsType()
        {
            var client = new WebhoseClient();
            var q = " ";
            var organization = new List<string> {"cisco"};
            var siteType = new List<string> {"news"};
            var beginDate = new DateTime(2017, 02, 24);
            var endDate = new DateTime(2017, 02, 25);
            var data = new Dictionary<string, string>();
            var expected = client.Query("search", q, organization, siteType, 2, null, null);
            var posts = expected["posts"].Count();
            var moreResult = (int) expected["moreResultsAvailable"];
            var pages = 0;
            var numpages = 10;
            while (pages < numpages && moreResult != 0)
            {
                var result = expected.GetNext();
                moreResult = (int) result["moreResultsAvailable"];
                pages++;
            }

            Assert.IsTrue(posts > 0);
        }


        [TestMethod]
        public void Should_Match_OrgnaizationName_Format()
        {
            var organization = new List<string> {"Atena", "cigna"};
            var actual = organization.GetOrganizationName();
            var expected = "(organization:\"Atena\"),(organization:\"cigna\")";
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void Should_Match_UnixTimeStamp_ByDate()
        {
            var date = new DateTime(2017, 02, 26);
            var actual = Helpers.GetUnixTimeStamp(date);
            var exected = 1488067200000;
            Assert.AreEqual(actual, exected);
        }

        [TestMethod]
        public void Should_Match_DateByRange()
        {
            var beginDate = new DateTime(2017, 02, 24);
            var endDate = new DateTime(2017, 02, 25);
            var actual = beginDate.GetDateByRange(endDate);
            var exected = "(published:>1487894400000)-(published:>1487980800000)";
            Assert.AreEqual(actual, exected);
        }
    }
}