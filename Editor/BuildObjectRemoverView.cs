using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vverum.Tools.BuildObjectRemover
{
	public class BuildObjectRemoverView : SettingsProvider
	{
		private static readonly string viewTitle = $"Project/Build Object Remover";

		private VisualTreeAsset containerTemplate;
		private VisualTreeAsset rowTemplate;
		private VisualElement rootVisualElement;
		private List<RemoveTagDataHolder> rowsData = new List<RemoveTagDataHolder>();
		private Dictionary<BuildTarget, VisualElement> buildTypesConteiners = new Dictionary<BuildTarget, VisualElement>();

		bool HasSearchInterestHandler(string searchContext) => true;

		public BuildObjectRemoverView() : base(viewTitle, SettingsScope.Project)
		{
			hasSearchInterestHandler = HasSearchInterestHandler;
		}

#if UNITY_2019_4_OR_NEWER
		[SettingsProvider]
#endif
		public static SettingsProvider PreferenceSettingsProvider()
		{
			return new BuildObjectRemoverView();
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			rootVisualElement = rootElement;

			containerTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BuildObjectRemoverConstants.PACKAGE_PATH + "/SettingsBuildTargetContainerTemplate.uxml");
			rowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BuildObjectRemoverConstants.PACKAGE_PATH + "/SettingsTagViewTemplate.uxml");

			SetupView(rootElement);
			FillViewContent();

			base.OnActivate(searchContext, rootElement);
			if (!BuildObjectRemoverSettingsProvider.HasSettings())
			{
				AddDefaultTag();
			}
		}

		public override void OnDeactivate()
		{
			//TODO: ask to save changes

			rootVisualElement?.Clear();
			buildTypesConteiners?.Clear();
		}

		private void SetupView(VisualElement root)
		{
			var viewTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BuildObjectRemoverConstants.PACKAGE_PATH + "/BuildObjectRemoverView.uxml");
			viewTemplate.CloneTree(root);
			SetupEvents(root);
			SetAllBuilds(root);
			SetRowEditorOnly(root);
		}

		private void SetupEvents(VisualElement root)
		{
			root.Q<Button>("addBuildTargetButton").clicked += ShowNewBuildTargetSelector;
			root.Q<Button>("AddSelectedBuildTarget").clicked += AddBuildTargetFromSelector;
			root.Q<Button>("applyButton").clicked += ApplyChanges;
			root.Q<Button>("revertButton").clicked += RevertChanges;
		}

		private void SetAllBuilds(VisualElement root)
		{
			if (!buildTypesConteiners.ContainsKey(BuildTarget.NoTarget))
			{
				var allBuildContainer = root.Q("allBuildTarget");
				allBuildContainer.Q<Button>("AddTagButton").clicked += () => AddValue(allBuildContainer.Q("contentTags"), BuildTarget.NoTarget);
				buildTypesConteiners.Add(BuildTarget.NoTarget, allBuildContainer);
			}
		}

		private void SetRowEditorOnly(VisualElement root)
		{
			var editorOnlyRow = root.Q("editorOnlyRow");
			editorOnlyRow.Q<Toggle>("removeEnable").SetEnabled(false);
			editorOnlyRow.Q<TagField>("tag").SetEnabled(false);
			editorOnlyRow.Q<EnumFlagsField>("buildType").SetEnabled(false);
			editorOnlyRow.Q<Button>("removeButton").SetEnabled(false);
		}

		private void FillViewContent()
		{
			var data = BuildObjectRemoverSettingsProvider.LoadSettings();
			foreach (var item in data)
			{
				buildTypesConteiners.TryGetValue(item.buildTarget, out VisualElement container);
				AddValue((container ?? AddBuildTargetContainer(item.buildTarget)).Q("contentTags"), item);
			}
		}

		private void ShowNewBuildTargetSelector()
		{
			rootVisualElement.Q<EnumField>("selectedBuildTarget").Init(BuildTarget.StandaloneWindows);
			rootVisualElement.Q("addBuildTargetSelector").style.display = DisplayStyle.Flex;
			rootVisualElement.Q("addBuildTargetButton").style.display = DisplayStyle.None;
		}

		private void AddBuildTargetFromSelector()
		{
			var buildTargetField = rootVisualElement.Q<EnumField>("selectedBuildTarget");
			if (!buildTypesConteiners.ContainsKey((BuildTarget)buildTargetField.value))
			{
				AddBuildTargetContainer((BuildTarget)buildTargetField.value);
				rootVisualElement.Q("addBuildTargetSelector").style.display = DisplayStyle.None;
				rootVisualElement.Q("addBuildTargetButton").style.display = DisplayStyle.Flex;
			}
		}

		private VisualElement AddBuildTargetContainer(BuildTarget buildTarget)
		{
			var mainContent = rootVisualElement.Q("container");
			var newBuildTypeConteiner = containerTemplate.CloneTree();
			newBuildTypeConteiner.Q<Label>("buildTarget").text = buildTarget.ToString();
			newBuildTypeConteiner.Q<Button>("AddTagButton").clicked += () => AddValue(newBuildTypeConteiner.Q("contentTags"), buildTarget);
			buildTypesConteiners.Add(buildTarget, newBuildTypeConteiner);
			mainContent.Add(newBuildTypeConteiner);
			return newBuildTypeConteiner;
		}

		private void AddValue(VisualElement container, RemoveTagData data)
		{
			var newDataHolder = (RemoveTagDataHolder)ScriptableObject.CreateInstance(typeof(RemoveTagDataHolder));
			newDataHolder.SetData(data);
			AddNewRow(container, newDataHolder);
		}

		private void AddValue(VisualElement container, BuildTarget buildTarget)
		{
			var newDataHolder = (RemoveTagDataHolder)ScriptableObject.CreateInstance(typeof(RemoveTagDataHolder));
			newDataHolder.SetTarget(buildTarget);
			AddNewRow(container, newDataHolder);
		}

		private void AddNewRow(VisualElement container, RemoveTagDataHolder dataHolder)
		{
			rowsData.Add(dataHolder);
			var row = rowTemplate.CloneTree();
			row.Q<EnumFlagsField>("buildType").Init(RemoverState.Development);
			row.Bind(new SerializedObject(dataHolder));
			row.Q<Button>("removeButton").clicked += () => { RemoveRow(row, dataHolder); };
			container.Add(row);
		}

		private void RemoveRow(VisualElement element, RemoveTagDataHolder data)
		{
			rowsData.Remove(data);
			element.RemoveFromHierarchy();
		}

		private void ApplyChanges()
		{
			BuildObjectRemoverSettingsProvider.SaveSettings(rowsData.Select(x => x.GetData()).ToList());
		}

		private void RevertChanges()
		{
			rootVisualElement.Clear();
			buildTypesConteiners.Clear();
			rowsData.Clear();
			SetupView(rootVisualElement);
			FillViewContent();
		}

		private void AddDefaultTag()
		{
			string sampleTag = "DevelopmentOnly";
			if (!UnityEditorInternal.InternalEditorUtility.tags.Contains(sampleTag))
			{
				UnityEditorInternal.InternalEditorUtility.AddTag(sampleTag);
			}
		}

	}
}