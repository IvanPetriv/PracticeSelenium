using Locators.Utils;

namespace Locators.Tests {
    [TestFixture]
    internal class XmlTest {
        [Test]
        public void TestXml() {
            XmlTestsParser parser = new(@"D:\LPNU\University\3-year\2-term\STP\Code-2\Locators\TestData\testsDev.xml");

            Console.WriteLine(parser.TestCases);
        }
    }
}
