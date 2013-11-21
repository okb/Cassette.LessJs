using NUnit.Framework;

namespace Cassette.Stylesheets
{
    [TestFixture]
    public class LessJsCompileException_Tests
    {
        [Test]
        public void LessCompileExceptionConstructorAcceptsMessage()
        {
            Assert.That(new LessJsCompileException("test").Message, Is.EqualTo("test"));
        }
    }
}