using System;
using System.Collections;
using System.Collections.Generic;

namespace Xlfdll.Configuration
{
	public class Configuration : IEnumerable<KeyValuePair<String, ConfigurationSection>>
	{
		public Configuration()
		{
			configurationSectionDictionary = new Dictionary<String, ConfigurationSection>(StringComparer.InvariantCultureIgnoreCase);
		}

		private Dictionary<String, ConfigurationSection> configurationSectionDictionary;

		public Int32 SectionCount => configurationSectionDictionary.Count;

		public Dictionary<String, ConfigurationSection>.KeyCollection SectionNames
			=> configurationSectionDictionary.Keys;
		public Dictionary<String, ConfigurationSection>.ValueCollection Sections
			=> configurationSectionDictionary.Values;

		public Boolean ContainsSection(String sectionName)
		{
			return configurationSectionDictionary.ContainsKey(sectionName);
		}

		public ConfigurationSection this[String sectionName]
		{
			get
			{
				if (String.IsNullOrEmpty(sectionName))
				{
					throw new ArgumentException("The name of the configuration section is null or empty.");
				}
				else if (!this.ContainsSection(sectionName))
				{
					throw new ArgumentException("The configuration section is not in the current configuration data.");
				}

				return configurationSectionDictionary[sectionName];
			}
		}

		public void AddSection(ConfigurationSection section)
		{
			if (this.ContainsSection(section.Name))
			{
				throw new ArgumentException("The configuration section is already in the current configuration data.");
			}

			configurationSectionDictionary.Add(section.Name, section);
		}

		public void AddSection(String sectionName)
		{
			if (String.IsNullOrEmpty(sectionName))
			{
				throw new ArgumentException("The name of the configuration section is null or empty.");
			}
			else if (this.ContainsSection(sectionName))
			{
				throw new ArgumentException("The configuration section is already in the current configuration data.");
			}

			configurationSectionDictionary.Add(sectionName, new ConfigurationSection(sectionName));
		}

		public void RemoveSection(String sectionName)
		{
			if (String.IsNullOrEmpty(sectionName))
			{
				throw new ArgumentException("The name of the configuration section is null or empty.");
			}
			else if (!this.ContainsSection(sectionName))
			{
				throw new ArgumentException("The configuration section is not in the current configuration data.");
			}

			configurationSectionDictionary.Remove(sectionName);
		}

		public void AddValue(String sectionName, String key, String value)
		{
			if (!this.ContainsSection(sectionName))
			{
				this.AddSection(sectionName);
			}

			this[sectionName].Add(key, value);
		}

		public void RemoveValue(String sectionName, String key)
		{
			this[sectionName].Remove(key);

			if (this[sectionName].Count == 0)
			{
				this.RemoveSection(sectionName);
			}
		}

		public Boolean TryAddSection(ConfigurationSection section)
		{
			Boolean result = !this.ContainsSection(section.Name);

			if (result)
			{
				configurationSectionDictionary.Add(section.Name, section);
			}

			return result;
		}

		public Boolean TryAddSection(String sectionName)
		{
			Boolean result = !String.IsNullOrEmpty(sectionName) && !this.ContainsSection(sectionName);

			if (result)
			{
				configurationSectionDictionary.Add(sectionName, new ConfigurationSection(sectionName));
			}

			return result;
		}

		public Boolean TryRemoveSection(String sectionName)
		{
			Boolean result = !String.IsNullOrEmpty(sectionName) && this.ContainsSection(sectionName);

			if (result)
			{
				configurationSectionDictionary.Remove(sectionName);
			}

			return result;
		}

		public Boolean TryAddValue(String sectionName, String key, String value)
		{
			Boolean result = this.ContainsSection(sectionName) || this.TryAddSection(sectionName);

			if (result)
			{
				result &= !this[sectionName].ContainsKey(key);

				if (result)
				{
					this[sectionName].Add(key, value);
				}
			}

			return result;
		}

		public Boolean TryRemoveValue(String sectionName, String key)
		{
			Boolean result = this.ContainsSection(sectionName);

			if (result)
			{
				result &= this[sectionName].ContainsKey(key);

				if (result)
				{
					this[sectionName].Remove(key);

					if (this[sectionName].Count == 0)
					{
						this.RemoveSection(sectionName);
					}
				}
			}

			return result;
		}

		public void Clear()
		{
			configurationSectionDictionary.Clear();
		}

		#region IEnumerable<KeyValuePair<String, ConfigurationSection>>

		public IEnumerator<KeyValuePair<String, ConfigurationSection>> GetEnumerator()
			=> configurationSectionDictionary.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();

		#endregion
	}
}