using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
public class DEBUG_LocalAchievementsPrinter : MonoBehaviour
{
	// Token: 0x06003DE4 RID: 15844 RVA: 0x00031D39 File Offset: 0x0002FF39
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x0011AA5C File Offset: 0x00118C5C
	public void OnGUI()
	{
		if (Time.frameCount < 120)
		{
			return;
		}
		IList<LocalAchievementsManager.Achievement> unlockedAchievements = LocalAchievementsManager.GetUnlockedAchievements();
		foreach (string text in DEBUG_LocalAchievementsPrinter.AllAchievements)
		{
			bool flag = unlockedAchievements.Contains((LocalAchievementsManager.Achievement)Enum.Parse(typeof(LocalAchievementsManager.Achievement), text));
			this.builder.AppendFormat("{0}....{1}\n", (!flag) ? "L" : "U", text);
		}
		GUIStyle guistyle = new GUIStyle(GUI.skin.GetStyle("Box"));
		guistyle.alignment = 0;
		GUI.Box(new Rect(0f, 0f, 200f, 500f), this.builder.ToString(), guistyle);
		this.builder.Length = 0;
	}

	// Token: 0x040031BF RID: 12735
	public static readonly string[] AllAchievements = Enum.GetNames(typeof(LocalAchievementsManager.Achievement));

	// Token: 0x040031C0 RID: 12736
	public StringBuilder builder = new StringBuilder();
}
