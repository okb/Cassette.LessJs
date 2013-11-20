#region License

// --------------------------------------------------
// Copyright © OKB. All Rights Reserved.
// 
// This software is proprietary information of OKB.
// USE IS SUBJECT TO LICENSE TERMS.
// --------------------------------------------------

#endregion

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