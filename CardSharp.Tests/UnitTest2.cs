using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardSharp.Rules;

namespace CardSharp.Tests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void RuleSingle()
        {
            var card = "3".ToCards().ToList().ExtractCardGroups();
            var card2 = "4".ToCards().ToList().ExtractCardGroups();
            Assert.IsTrue(new RuleSingle().IsMatch(card, null));
            Assert.IsTrue(new RuleSingle().IsMatch(card2, card));
            Assert.IsFalse(new RuleSingle().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleDouble()
        {
            var card = "33".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "44".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new RuleDouble().IsMatch(card, null));
            Assert.IsTrue(new RuleDouble().IsMatch(card2, card));
            Assert.IsFalse(new RuleDouble().IsMatch(card, card2));
        }
        [TestMethod]
        public void Rule3()
        {
            var card = "333".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "444".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new Rule3().IsMatch(card, null));
            Assert.IsTrue(new Rule3().IsMatch(card2, card));
            Assert.IsFalse(new Rule3().IsMatch(card, card2));
        }
        [TestMethod]
        public void Rule3With1()
        {
            var card = "3334".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "4445".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new Rule3With1().IsMatch(card, null));
            Assert.IsTrue(new Rule3With1().IsMatch(card2, card));
            Assert.IsFalse(new Rule3With1().IsMatch(card, card2));
        }
        [TestMethod]
        public void Rule3With2()
        {
            var card = "33344".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "44455".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new Rule3With2().IsMatch(card, null));
            Assert.IsTrue(new Rule3With2().IsMatch(card2, card));
            Assert.IsFalse(new Rule3With2().IsMatch(card, card2));
        }
        [TestMethod]
        public void Rule4With2()
        {
            var card = "333344".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "444455".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new Rule4With2().IsMatch(card, null));
            Assert.IsTrue(new Rule4With2().IsMatch(card2, card));
            Assert.IsFalse(new Rule4With2().IsMatch(card, card2));
        }
        [TestMethod]
        public void Rule4With4()
        {
            var card = "33334455".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "44445566".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new Rule4With4().IsMatch(card, null));
            Assert.IsTrue(new Rule4With4().IsMatch(card2, card));
            Assert.IsFalse(new Rule4With4().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleAirplane()
        {
            var card = "333444555".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "444555666".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new RuleAirplane().IsMatch(card, null));
            Assert.IsTrue(new RuleAirplane().IsMatch(card2, card));
            Assert.IsFalse(new RuleAirplane().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleAirplain1()
        {
            var card = "333444555678".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "444555666789".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new RuleAirplain1().IsMatch(card, null));
            Assert.IsTrue(new RuleAirplain1().IsMatch(card2, card));
            Assert.IsFalse(new RuleAirplain1().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleAirplain2()
        {
            var card = "333444555667788".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "444555666778899".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new RuleAirplain2().IsMatch(card, null));
            Assert.IsTrue(new RuleAirplain2().IsMatch(card2, card));
            Assert.IsFalse(new RuleAirplain2().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleChain()
        {
            var card = "3456789".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "45678910".ToCards().ToListAndSort().ExtractCardGroups();
            //Assert.IsTrue(new RuleChain().IsMatch(card, null));
            Assert.IsTrue(new RuleChain().IsMatch(card2, card));
            Assert.IsFalse(new RuleChain().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleChain2()
        {
            var card = "33445566".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "44556677".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new RuleChain2().IsMatch(card, null));
            Assert.IsTrue(new RuleChain2().IsMatch(card2, card));
            Assert.IsFalse(new RuleChain2().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleBomb()
        {
            var card = "3333".ToCards().ToListAndSort().ExtractCardGroups();
            var card2 = "4444".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new RuleBomb().IsMatch(card, null));
            Assert.IsTrue(new RuleBomb().IsMatch(card2, card));
            Assert.IsFalse(new RuleBomb().IsMatch(card, card2));
        }
        [TestMethod]
        public void RuleRocket()
        {
            var card = "鬼王".ToCards().ToListAndSort().ExtractCardGroups();
            Assert.IsTrue(new RuleRocket().IsMatch(card, null));
        }
    }
}
