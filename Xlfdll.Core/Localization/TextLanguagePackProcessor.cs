using System;
using System.IO;
using System.Text;

namespace Xlfdll.Localization
{
    public class TextLanguagePackProcessor : ILanguagePackProcessor
    {
        public LanguageDictionary LoadLanguagePack(String path)
        {
            LanguageDictionary languageDictionary = null;

            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                String line = String.Empty;
                String currentSectionName = String.Empty;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    try
                    {
                        if (line.StartsWith("Language"))
                        {
                            languageDictionary = new LanguageDictionary(line.Substring(line.IndexOf('=') + 1).Trim());
                        }
                        else if (line.StartsWith("[") && !line.Equals("[Metadata]"))
                        {
                            currentSectionName = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1);
                            languageDictionary.AddSection(currentSectionName);
                        }
                        else if (line.StartsWith("$"))
                        {
                            languageDictionary[currentSectionName].Add(line.Substring(line.IndexOf('$') + 1, line.IndexOf('=') - line.IndexOf('$') - 1).Trim(), line.Substring(line.IndexOf('=') + 1).Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException("Invalid format on line: " + line, ex);
                    }
                }
            }

            return languageDictionary;
        }
    }
}