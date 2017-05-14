using System;
using NUnit.Framework;

namespace WindowsStalkerTest
{
    [TestFixture]
    public class SerlializerTest
    {
        [Test]
        public void BytesCountTest()
        {
            int framesCount = 789;
            byte byte1 = (byte) (framesCount % 256);
            byte byte2 = 65537/65536;
            //Your code goes here
            Console.WriteLine(byte1 + " " + byte2);
        }
    }
}