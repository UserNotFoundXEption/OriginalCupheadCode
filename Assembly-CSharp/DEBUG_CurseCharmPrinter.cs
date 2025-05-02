using System;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
public class DEBUG_CurseCharmPrinter : MonoBehaviour
{
	// Token: 0x06003DDA RID: 15834 RVA: 0x00031C83 File Offset: 0x0002FE83
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003DDB RID: 15835 RVA: 0x0011A720 File Offset: 0x00118920
	public void OnGUI()
	{
		if (Time.frameCount < 120)
		{
			return;
		}
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.GetStyle("Box"));
			this.style.alignment = 0;
		}
		if (PlayerData.Data == null)
		{
			return;
		}
		string text = string.Format("Curse: {0} / {1} / {2}", CharmCurse.CalculateLevel(PlayerId.PlayerOne), PlayerData.Data.CalculateCurseCharmAccumulatedValue(PlayerId.PlayerOne, CharmCurse.CountableLevels), CharmCurse.IsMaxLevel(PlayerId.PlayerOne));
		GUI.Box(new Rect(0f, 0f, 200f, 100f), text);
	}

	// Token: 0x040031B2 RID: 12722
	public GUIStyle style;
}
