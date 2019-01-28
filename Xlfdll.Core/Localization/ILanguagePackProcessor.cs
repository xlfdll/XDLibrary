using System;

namespace Xlfdll.Localization
{
    public interface ILanguagePackProcessor
    {
        LanguageDictionary LoadLanguagePack(String path);
    }
}