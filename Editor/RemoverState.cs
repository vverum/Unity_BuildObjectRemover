using System;

namespace Vverum.Tools.BuildObjectRemover
{
	[Flags]
	public enum RemoverState
	{
		EditorPlayTime = 1,
		Development = 2,
		Release = 4
	}
}