#region License

// --------------------------------------------------
// Copyright © OKB. All Rights Reserved.
// 
// This software is proprietary information of OKB.
// USE IS SUBJECT TO LICENSE TERMS.
// --------------------------------------------------

#endregion

using Cassette.TinyIoC;

namespace Cassette.Stylesheets
{
    [ConfigurationOrder(20)]
    public class LessJsServices : IConfiguration<TinyIoCContainer>
    {
        public void Configure(TinyIoCContainer container)
        {
            container.Register<ILessJsCompiler, LessJsCompiler>().AsMultiInstance();
            container.Register<IFileSearchModifier<StylesheetBundle>, LessJsFileSearchModifier>().AsSingleton();
        }
    }
}