using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SettingsManagement;
using UnityEditor;
using System;
using System.Linq;

namespace Vverum.Tools.BuildObjectRemover
{
	public static class BuildObjectRemoverSettingsProvider
	{
		private const string TAGS_DATA_KEY = "TagsData";

		private static Settings settingsProvider;

		internal static Settings SettingsProvider
		{
			get
			{
				if (settingsProvider == null)
				{
					settingsProvider = new Settings(BuildObjectRemoverConstants.PACKAGE_NAME);
				}
				return settingsProvider;
			}
		}

		public static void SaveSettings(List<RemoveTagData> data)
		{
			if (data == null)
			{
				data = new List<RemoveTagData>();
			}

			SettingsProvider.Set<RemoveTagData[]>(TAGS_DATA_KEY, data.ToArray(), SettingsScope.Project);
		}

		public static List<RemoveTagData> LoadSettings()
		{
			var data = SettingsProvider.Get<RemoveTagData[]>(TAGS_DATA_KEY, SettingsScope.Project, default) ?? new RemoveTagData[0];
			return data.ToList();
		}

	}
}