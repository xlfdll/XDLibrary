using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Xlfdll.Localization;

namespace Xlfdll.Windows.Forms.Localization
{
    public static class FormLanguagePackBinder
    {
        public static void BindLanguagePack(this Form form, LanguageDictionary languageDictionary)
        {
            if (languageDictionary.ContainsSection(form.Name))
            {
                if (languageDictionary[form.Name].ContainsKey("this"))
                {
                    form.Text = languageDictionary[form.Name]["this"];
                }

                foreach (KeyValuePair<String, String> pair in languageDictionary[form.Name])
                {
                    Control[] controls = form.Controls.Find(pair.Key, true);

                    if (controls.Length > 0)
                    {
                        controls[0].Text = pair.Value;
                    }
                }
            }
        }
    }
}