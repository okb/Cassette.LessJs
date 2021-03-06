﻿using Cassette.BundleProcessing;
using Moq;
using NUnit.Framework;

namespace Cassette.Stylesheets
{
    [TestFixture]
    public class CompileLessWithJs_Tests
    {
        [Test]
        public void GivenACompiler_WhenProcessCalled_ThenCompileAssetTransformerAddedToLessAsset()
        {
            var processor = new CompileLessWithJs(Mock.Of<ILessJsCompiler>(), new CassetteSettings());
            var bundle = new StylesheetBundle("~");
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns("test.less");
            bundle.Assets.Add(asset.Object);

            processor.Process(bundle);

            asset.Verify(a => a.AddAssetTransformer(It.Is<IAssetTransformer>(at => at is CompileAsset)));
        }

        [Test]
        public void GivenACompiler_WhenProcessCalled_ThenCompileAssetTransformerNotAddedToCssAsset()
        {
            var processor = new CompileLessWithJs(Mock.Of<ILessJsCompiler>(), new CassetteSettings());
            var bundle = new StylesheetBundle("~");
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns("test.css");
            bundle.Assets.Add(asset.Object);

            processor.Process(bundle);

            asset.Verify(a => a.AddAssetTransformer(It.Is<IAssetTransformer>(at => at is CompileAsset)), Times.Never());
        }
    }
}