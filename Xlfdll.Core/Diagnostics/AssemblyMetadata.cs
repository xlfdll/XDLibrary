using System;
using System.IO;
using System.Reflection;

namespace Xlfdll.Diagnostics
{
    public class AssemblyMetadata
    {
        public AssemblyMetadata(String path)
        {
            this.AssemblyObject = Assembly.LoadFile(path);
        }

        public AssemblyMetadata(Assembly assembly)
        {
            this.AssemblyObject = assembly;
        }

        public Assembly AssemblyObject { get; }

        public String AssemblyLocation => this.AssemblyObject.Location;

        #region Assembly Internal Name Accessors

        public String AssemblyInternalFileName => Path.GetFileName(this.AssemblyObject.Location);
        public String AssemblyInternalName => Path.GetFileNameWithoutExtension(this.AssemblyObject.Location);

        #endregion

        #region Assembly Attribute Accessors

        public String AssemblyTitle
        {
            get
            {
                Object[] attributes = this.AssemblyObject.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];

                    if (titleAttribute.Title != String.Empty)
                    {
                        return titleAttribute.Title;
                    }
                }

                return Path.GetFileNameWithoutExtension(this.AssemblyObject.CodeBase);
            }
        }

        public String AssemblyDescription
        {
            get
            {
                Object[] attributes = this.AssemblyObject.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                if (attributes.Length == 0)
                {
                    return String.Empty;
                }

                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public String AssemblyVersion
        {
            get
            {
                return this.AssemblyObject.GetName().Version.ToString();
            }
        }

        public String AssemblyProduct
        {
            get
            {
                Object[] attributes = this.AssemblyObject.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                if (attributes.Length == 0)
                {
                    return String.Empty;
                }

                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public String AssemblyCompany
        {
            get
            {
                Object[] attributes = this.AssemblyObject.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                if (attributes.Length == 0)
                {
                    return String.Empty;
                }

                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public String AssemblyCopyright
        {
            get
            {
                Object[] attributes = this.AssemblyObject.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                if (attributes.Length == 0)
                {
                    return String.Empty;
                }

                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion
    }
}