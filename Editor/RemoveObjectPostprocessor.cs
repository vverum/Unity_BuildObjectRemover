﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vverum.Tools.BuildObjectRemover
{
	public class RemoveObjectPostprocessor : IPreprocessBuildWithReport, IProcessSceneWithReport
	{
		public int callbackOrder => 0;

		public void OnPreprocessBuild(BuildReport report)
		{
			var settings = GetTagsToRemove(report);
			var logMessage = new StringBuilder();
			logMessage.Append("Removing object with tags: ");
			foreach (var item in settings)
			{
				logMessage.Append(item.tag);
				logMessage.Append("; ");
			}
			Debug.Log(logMessage);
		}

		public void OnProcessScene(Scene scene, BuildReport report)
		{
			var settings = GetTagsToRemove(report);
			foreach (var item in settings)
			{
				//TODO: choose better method of removing objs
				//RemoveObjectWithTagInScene(scene, item.tag);
				RemoveChildObjectWithTagOptymize(item.tag);
			}
		}

		private List<RemoveTagData> GetTagsToRemove(BuildReport report)
		{
			var buildType = GetBuildType(report);
			var buildTarget = GetBuildTarget(report);
			var tagsToRemove = BuildObjectRemoverSettingsProvider.LoadSettings()
				.Where(x => (x.enable
					&& ((RemoverState)x.runType).HasFlag(buildType)
					&& (x.buildTarget == buildTarget || x.buildTarget == BuildTarget.NoTarget)))
				.GroupBy(x => x.tag)
				.Select(x => x.First()).ToList();
			return tagsToRemove;
		}

		private RemoverState GetBuildType(BuildReport report)
		{
			if (report is null)
			{
				return RemoverState.EditorPlayTime;
			}
			return (report.summary.options.HasFlag(BuildOptions.Development)) ? RemoverState.Development : RemoverState.Release;
		}

		private BuildTarget GetBuildTarget(BuildReport report)
		{
			if (report is null)
			{
				return EditorUserBuildSettings.activeBuildTarget;
			}
			return report.summary.platform;
		}

		private void RemoveObjectWithTagInScene(Scene scene, in string tag)
		{
			foreach (var item in scene.GetRootGameObjects())
			{
				RemoveChildObjectWithTag(item, tag);
			}
		}

		private void RemoveChildObjectWithTag(GameObject obj, in string tag)
		{
			if (obj.CompareTag(tag))
			{
				GameObject.DestroyImmediate(obj);
				return;
			}
			foreach (Transform item in obj.transform)
			{
				RemoveChildObjectWithTag(item.gameObject, tag);
			}
		}

		private void RemoveChildObjectWithTagOptymize(in string tag)
		{
			var objsWithTag = GameObject.FindGameObjectsWithTag(tag);
			foreach (var item in objsWithTag)
			{
				GameObject.DestroyImmediate(item);
			}
		}

	}
}