using System;
using UnityEngine;

// Token: 0x020005FC RID: 1532
public class DialoguerExampleStart : MonoBehaviour
{
	// Token: 0x06003EC5 RID: 16069 RVA: 0x0003271E File Offset: 0x0003091E
	public void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0011DCF0 File Offset: 0x0011BEF0
	public void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Start!"))
		{
			Dialoguer.StartDialogue(3);
		}
		string text = "Open this file (DialoguerExampleStart.cs) to see how to start using Dialoguer";
		GUI.Label(new Rect(10f, 50f, 500f, 500f), text);
	}
}
