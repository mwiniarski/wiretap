using System;
using System.Net.Configuration;
using WindowsStalker;
using NUnit.Framework;

namespace WindowsStalkerTest
{
    [TestFixture]
    public class ConnectionTest
    {
        [Test]
        public void GetComputerIDTest()
        {
            string id = Program.GetComputerID();
            string id2 = Program.GetComputerID();

            Assert.AreEqual(id, id2);
            Assert.AreEqual(string.IsNullOrEmpty(id), false);
        }
    }
}