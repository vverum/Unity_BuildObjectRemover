using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vverum.Tools.BuildObjectRemover
{
	internal class RemoveTagDataHolder : ScriptableObject
	{
		[SerializeField]
		private bool enable;
		[SerializeField]
		private string tag;
		[SerializeField]
		private RemoverState runType;

		private BuildTarget buildTarget;

		public BuildTarget BuildTarget { get => buildTarget; }

		public void SetTarget(BuildTarget buildTarget)
		{
			if (string.IsNullOrEmpty(tag)) 
				SetData(null);
			this.buildTarget = buildTarget;
		}

		public void SetData(RemoveTagData data)
		{
			if (data == null)
			{
				buildTarget = BuildTarget.NoTarget;
				enable = false;
				tag = "Untagged";
				runType = 0;
			}
			else
			{
				buildTarget = data.buildTarget;
				enable = data.enable;
				tag = data.tag;
				runType = (RemoverState)data.runType;
			}
		}

		public RemoveTagData GetData()
		{
			var newData = new RemoveTagData()
			{
				buildTarget = this.buildTarget,
				enable = this.enable,
				tag = this.tag,
				runType = (int)this.runType
			};

			return newData;
		}

	}
}