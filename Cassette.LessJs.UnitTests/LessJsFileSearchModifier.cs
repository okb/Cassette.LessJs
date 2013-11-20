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
    public class LessJsFileSearchModifier_Tests
    {
        [Test]
        public void ModifyAddsLessPattern()
        {
            var modifier = new LessJsFileSearchModifier();
            var fileSearch = new FileSearch();
            modifier.Modify(fileSearch);
            Assert.That(fileSearch.Pattern, Is.StringContaining("*.less"));
        }
    }
}