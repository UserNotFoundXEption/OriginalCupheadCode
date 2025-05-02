using System;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class FlyingCowboyLevelOverlayScrollingSprite : ScrollingSprite
{
	// Token: 0x06001BBB RID: 7099 RVA: 0x000AC790 File Offset: 0x000AA990
	public override void Start()
	{
		base.Start();
		this.leftRenderer = base.GetComponent<SpriteRenderer>();
		this.rightRenderer = base.copyRenderers.Find((SpriteRenderer renderer) => renderer.transform.position.x > this.leftRenderer.transform.position.x);
		this.rightOverlayRenderers = new SpriteRenderer[this.overlayRenderers.Length];
		for (int i = 0; i < this.overlayRenderers.Length; i++)
		{
			SpriteRenderer spriteRenderer = this.overlayRenderers[i];
			GameObject gameObject = new GameObject(spriteRenderer.gameObject.name);
			SpriteRenderer spriteRenderer2 = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer2.sprite = spriteRenderer.sprite;
			spriteRenderer2.sortingLayerID = spriteRenderer.sortingLayerID;
			spriteRenderer2.sortingOrder = spriteRenderer.sortingOrder;
			spriteRenderer2.enabled = false;
			gameObject.transform.SetParent(this.rightRenderer.transform, false);
			this.rightOverlayRenderers[i] = spriteRenderer2;
		}
		this.leftOverlaysEnabled = new bool[this.overlayRenderers.Length];
		this.rightOverlaysEnabled = new bool[this.overlayRenderers.Length];
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x000AC88C File Offset: 0x000AAA8C
	public override void onLoop()
	{
		base.onLoop();
		bool[] array = this.leftOverlaysEnabled;
		this.leftOverlaysEnabled = this.rightOverlaysEnabled;
		this.rightOverlaysEnabled = array;
		for (int i = 0; i < this.rightOverlaysEnabled.Length; i++)
		{
			this.rightOverlaysEnabled[i] = (Random.value < this.overlayProbability);
		}
		FlyingCowboyLevelOverlayScrollingSprite.toggleOverlays(this.overlayRenderers, this.leftOverlaysEnabled);
		FlyingCowboyLevelOverlayScrollingSprite.toggleOverlays(this.rightOverlayRenderers, this.rightOverlaysEnabled);
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x000AC90C File Offset: 0x000AAB0C
	public static void toggleOverlays(SpriteRenderer[] overlayRenderers, bool[] activeStatus)
	{
		for (int i = 0; i < overlayRenderers.Length; i++)
		{
			overlayRenderers[i].enabled = activeStatus[i];
		}
	}

	// Token: 0x04001688 RID: 5768
	[SerializeField]
	[Range(0f, 1f)]
	public float overlayProbability;

	// Token: 0x04001689 RID: 5769
	[SerializeField]
	public SpriteRenderer[] overlayRenderers;

	// Token: 0x0400168A RID: 5770
	public SpriteRenderer leftRenderer;

	// Token: 0x0400168B RID: 5771
	public SpriteRenderer rightRenderer;

	// Token: 0x0400168C RID: 5772
	public SpriteRenderer[] rightOverlayRenderers;

	// Token: 0x0400168D RID: 5773
	public bool[] leftOverlaysEnabled;

	// Token: 0x0400168E RID: 5774
	public bool[] rightOverlaysEnabled;
}
