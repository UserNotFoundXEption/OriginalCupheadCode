using System;
using UnityEngine;

// Token: 0x02000580 RID: 1408
public class PlayerRecolorHandler : MonoBehaviour
{
	// Token: 0x06003B0D RID: 15117 RVA: 0x0002FFDC File Offset: 0x0002E1DC
	public void OnEnable()
	{
		EventManager.Instance.AddListener<ChaliceRecolorEvent>(new EventManager.EventDelegate<ChaliceRecolorEvent>(this.chaliceRecolorEventHandler));
	}

	// Token: 0x06003B0E RID: 15118 RVA: 0x0002FFF4 File Offset: 0x0002E1F4
	public void OnDisable()
	{
		EventManager.Instance.RemoveListener<ChaliceRecolorEvent>(new EventManager.EventDelegate<ChaliceRecolorEvent>(this.chaliceRecolorEventHandler));
	}

	// Token: 0x06003B0F RID: 15119 RVA: 0x0003000C File Offset: 0x0002E20C
	public void chaliceRecolorEventHandler(ChaliceRecolorEvent e)
	{
		PlayerRecolorHandler.SetChaliceRecolorEnabled(this.chaliceRenderer.sharedMaterial, e.enabled);
	}

	// Token: 0x06003B10 RID: 15120 RVA: 0x00030024 File Offset: 0x0002E224
	public static void SetChaliceRecolorEnabled(Material sharedMaterial, bool enabled)
	{
		sharedMaterial.SetFloat("_RecolorFactor", (float)((!enabled) ? 0 : 1));
	}

	// Token: 0x04002F46 RID: 12102
	[SerializeField]
	public SpriteRenderer chaliceRenderer;
}
