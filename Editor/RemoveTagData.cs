using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vverum.Tools.BuildObjectRemover
{
	[Serializable]
	internal class RemoveTagData
	{
		public BuildTarget buildTarget = BuildTarget.NoTarget;
		public bool enable;
		public string tag;
		public int runType;
	}
}