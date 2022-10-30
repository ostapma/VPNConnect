using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.Net;

namespace VPNConnectTests.Unit
{
    [TestClass]
    public class NetUtilsTest
    {
        [TestMethod]
        public void IpToIntTest()
        {
            string testValidIp = "192.168.1.154";
            var actual = NetUtils.IpToInt(testValidIp);
            var expected = 0xc0a8019a;
            Assert.AreEqual(expected, actual, $"expected {expected}, actual {actual}");
            string invalidIp = "123.2.";
            actual = NetUtils.IpToInt(invalidIp);
            Assert.AreEqual(0, actual, $"expected 0, actual {actual}");
        }

    }
}
