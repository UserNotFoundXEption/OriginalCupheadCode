using System;
using System.Collections.Generic;

// Token: 0x020000E2 RID: 226
[Serializable]
public class GitLevelProperties
{
	// Token: 0x06000AA5 RID: 2725 RVA: 0x00009A07 File Offset: 0x00007C07
	public GitLevelProperties()
	{
		this.levels = new List<GitLevelProperties.GitLevel>();
	}

	// Token: 0x04000851 RID: 2129
	public const string UNITY_PATH = "/_CUPHEAD/_Generated/git_data.xml";

	// Token: 0x04000852 RID: 2130
	public const string GIT_TOOLS_PATH = "Assets/_CUPHEAD/_Generated/git_data.xml";

	// Token: 0x04000853 RID: 2131
	public List<GitLevelProperties.GitLevel> levels;

	// Token: 0x02000954 RID: 2388
	[Serializable]
	public class GitLevel
	{
		// Token: 0x04004625 RID: 17957
		public string name;

		// Token: 0x04004626 RID: 17958
		public string levelClassPath;

		// Token: 0x04004627 RID: 17959
		public string levelObjectPath;
	}
}
