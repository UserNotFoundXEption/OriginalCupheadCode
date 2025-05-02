using System;
using UnityEngine;

// Token: 0x02000519 RID: 1305
public class PlayerScreenEffectController : AbstractMonoBehaviour
{
	// Token: 0x06003755 RID: 14165 RVA: 0x0002D351 File Offset: 0x0002B551
	public void Update()
	{
		if (!this.dontCenter)
		{
			this.UpdateToCamera();
		}
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x0002D364 File Offset: 0x0002B564
	public void LateUpdate()
	{
		if (!this.dontCenter)
		{
			this.UpdateToCamera();
		}
	}

	// Token: 0x06003757 RID: 14167 RVA: 0x00102FD8 File Offset: 0x001011D8
	public void UpdateToCamera()
	{
		Camera main = Camera.main;
		Transform transform = main.transform;
		base.transform.position = transform.position;
		base.transform.localScale = Vector3.one * (main.orthographicSize / 360f);
		base.transform.rotation = transform.rotation;
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x0002D377 File Offset: 0x0002B577
	public void SetSpriteLayer(int index, SpriteLayer layer)
	{
		this.spriteRenderers[index].sortingLayerName = layer.ToString();
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x0002D393 File Offset: 0x0002B593
	public void SetSpriteOrder(int index, int order)
	{
		this.spriteRenderers[index].sortingOrder = order;
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x00103038 File Offset: 0x00101238
	public void ResetSprites()
	{
		for (int i = 0; i < this.spriteRenderers.Length; i++)
		{
			this.spriteRenderers[i].sortingOrder = -2010 - i;
			this.spriteRenderers[i].sortingLayerName = "Player";
			this.spriteRenderers[i].sprite = null;
		}
	}

	// Token: 0x04002C81 RID: 11393
	[SerializeField]
	public bool dontCenter;

	// Token: 0x04002C82 RID: 11394
	[SerializeField]
	public SpriteRenderer[] spriteRenderers;
}
