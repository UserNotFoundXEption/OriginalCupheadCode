using System;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class GenericTextHandler : AbstractPausableComponent
{
	// Token: 0x060008E3 RID: 2275 RVA: 0x00076CF8 File Offset: 0x00074EF8
	public void Start()
	{
		foreach (GameObject gameObject in this.otherText)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00008770 File Offset: 0x00006970
	public void ShowText()
	{
		this.textChosen.SetActive(true);
	}

	// Token: 0x040006C2 RID: 1730
	[SerializeField]
	public GameObject textChosen;

	// Token: 0x040006C3 RID: 1731
	[SerializeField]
	public GameObject[] otherText;
}
