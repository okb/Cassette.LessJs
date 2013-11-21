using System.IO;
using Cassette.BundleProcessing;
using Cassette.TinyIoC;
using Cassette.Utilities;
using Moq;
using NUnit.Framework;

namespace Cassette.Stylesheets
{
    [TestFixture]
    public class LessJsBundlePipelineModifier_Tests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            var minifier = Mock.Of<IStylesheetMinifier>();
            var urlGenerator = Mock.Of<IUrlGenerator>();
            var compiler = new Mock<ILessJsCompiler>();
            var settings = new CassetteSettings();

            var container = new TinyIoCContainer();
            container.Register(compiler.Object);
            container.Register(minifier);
            container.Register(urlGenerator);
            container.Register(settings);

            originalPipeline = new StylesheetPipeline(container, settings);
            var modifier = new LessJsBundlePipelineModifier();
            modifiedPipeline = modifier.Modify(originalPipeline);
        }

        #endregion

        private IBundlePipeline<StylesheetBundle> modifiedPipeline;
        private StylesheetPipeline originalPipeline;

        [Test]
        public void ModifiedPipelineIsSameObjectAsOriginalPipeline()
        {
            Assert.That(modifiedPipeline, Is.EqualTo(originalPipeline));
        }

        [Test]
        public void WhenModifiedPipelineProcessesBundle_ThenLessAssetHasCompileAssetTransformAdded()
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns("~/file.less");
            asset.Setup(a => a.OpenStream()).Returns(Stream.Null);
            var bundle = new StylesheetBundle("~");
            bundle.Assets.Add(asset.Object);

            modifiedPipeline.Process(bundle);

            asset.Verify(a => a.AddAssetTransformer(It.Is<IAssetTransformer>(t => t is CompileAsset)));
        }

        [Test]
        public void WhenModifiedPipelineProcessesBundle_ThenReferenceInLessAssetIsParsed()
        {
            var asset = new Mock<IAsset>();
            asset.SetupGet(a => a.Path).Returns("~/file.less");
            asset.Setup(a => a.OpenStream()).Returns(() => "// @reference 'other.less';".AsStream());
            var bundle = new StylesheetBundle("~");
            bundle.Assets.Add(asset.Object);

            modifiedPipeline.Process(bundle);

            asset.Verify(a => a.AddReference("other.less", 1));
        }
    }
}