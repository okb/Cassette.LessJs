using System;
using Cassette.BundleProcessing;

namespace Cassette.Stylesheets
{
    public class CompileLessWithJs : IBundleProcessor<StylesheetBundle>
    {
        private readonly ILessJsCompiler lessCompiler;
        private readonly CassetteSettings settings;

        public CompileLessWithJs(ILessJsCompiler lessCompiler, CassetteSettings settings)
        {
            this.lessCompiler = lessCompiler;
            this.settings = settings;
        }

        public void Process(StylesheetBundle bundle)
        {
            foreach (var asset in bundle.Assets)
            {
                if (asset.Path.EndsWith(".less", StringComparison.OrdinalIgnoreCase))
                {
                    asset.AddAssetTransformer(new CompileAsset(lessCompiler, settings.SourceDirectory));
                }
            }
        }
    }
}