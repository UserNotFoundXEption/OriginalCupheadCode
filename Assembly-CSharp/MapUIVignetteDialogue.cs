using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class MapUIVignetteDialogue : AbstractMonoBehaviour
{
	// Token: 0x170003BE RID: 958
	// (get) Token: 0x0600334C RID: 13132 RVA: 0x0002A7D5 File Offset: 0x000289D5
	// (set) Token: 0x0600334D RID: 13133 RVA: 0x0002A7DC File Offset: 0x000289DC
	public static MapUIVignetteDialogue Current { get; set; }

	// Token: 0x0600334E RID: 13134 RVA: 0x0002A7E4 File Offset: 0x000289E4
	public override void Awake()
	{
		base.Awake();
		MapUIVignetteDialogue.Current = this;
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x0002A802 File Offset: 0x00028A02
	public void LateUpdate()
	{
		base.transform.position = CupheadMapCamera.Current.transform.position;
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x0002A81E File Offset: 0x00028A1E
	public void FadeIn()
	{
		this.Fade(1f);
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x0002A82B File Offset: 0x00028A2B
	public void FadeOut()
	{
		this.Fade(0f);
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x0002A838 File Offset: 0x00028A38
	public void Fade(float target)
	{
		base.StartCoroutine(this.fade_cr(this.canvasGroup.alpha, target));
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x000F3D78 File Offset: 0x000F1F78
	public IEnumerator fade_cr(float startOpacity, float endOpacity)
	{
		float t = 0f;
		while (t < MapUIVignetteDialogue.fadeTime)
		{
			yield return null;
			t += CupheadTime.Delta;
			this.canvasGroup.alpha = Mathf.Lerp(startOpacity, endOpacity, t / MapUIVignetteDialogue.fadeTime);
		}
		this.canvasGroup.alpha = endOpacity;
		yield break;
	}

	// Token: 0x04002A65 RID: 10853
	public static float fadeTime = 0.5f;

	// Token: 0x04002A66 RID: 10854
	[SerializeField]
	public CanvasGroup canvasGroup;
}
