using System.Collections.Generic;
using System.Linq;
using CardSharp.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CardSharp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Set()
        {
            for (int i = 0; i < 100000; i++)
            {
                var list = new List<Card> {new Card(1), new Card(1), new Card(1), new Card(1)};
                var isMatch = SetMatch.IsMatch(list, list, 4);
                Assert.IsFalse(isMatch);
            }
        }

        [TestMethod]
        public void CardGroup()
        {
            for (int i = 0; i < 100000; i++)
            {
                var list = new List<Card> {new Card(1), new Card(1), new Card(1), new Card(1)};
                var group = list.ExtractCardGroups();
                var isMatch = SingleGroupMatch.IsMatch(group, group, 4);
                
                Assert.IsFalse(isMatch);
            }
        }

        [TestMethod]
        public void Test3WithN()
        {
            var list = new List<Card> { new Card(1), new Card(1), new Card(1), new Card(2) };
            var list2 = new List<Card> { new Card(1), new Card(1), new Card(1), new Card(2) };
            var list3 = new List<Card> { new Card(3), new Card(3), new Card(3), new Card(9) };
            //Assert.IsFalse(new Rule3WithN().IsMatch(list.ExtractCardGroups(), list2.ExtractCardGroups()));
            //Assert.IsTrue(new Rule3WithN().IsMatch(list3.ExtractCardGroups(), list.ExtractCardGroups()));
        }

        [TestMethod]
        public void TestChain()
        {
            var list = new List<Card> { new Card(1), new Card(2), new Card(3), new Card(4), new Card(5), new Card(6), new Card(7), new Card(8) };
            var group = list.ExtractCardGroups();
            for (int i = 0; i < 1000000; i++)
            {
                new RuleChain().IsMatch(group, null);
            }
        }

        [TestMethod]
        public void TestBigPlain()
        {
            Assert.IsTrue(new RuleAirplain2().IsMatch("333444555667788".ToCards().ToList().ExtractCardGroups(), null));
        }
    }
}
