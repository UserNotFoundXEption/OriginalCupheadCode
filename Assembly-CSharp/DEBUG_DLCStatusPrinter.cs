using System;
using UnityEngine;

// Token: 0x020005C7 RID: 1479
public class DEBUG_DLCStatusPrinter : MonoBehaviour
{
	// Token: 0x06003DDD RID: 15837 RVA: 0x00031C98 File Offset: 0x0002FE98
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003DDE RID: 15838 RVA: 0x0011A7CC File Offset: 0x001189CC
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
		GUI.Box(new Rect(0f, 0f, 200f, 100f), "DLC Enabled: " + DLCManager.DLCEnabled());
	}

	// Token: 0x040031B3 RID: 12723
	public GUIStyle style;
}
