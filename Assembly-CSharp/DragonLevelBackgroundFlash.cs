using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class DragonLevelBackgroundFlash : MonoBehaviour
{
	// Token: 0x060017FC RID: 6140 RVA: 0x000A2C64 File Offset: 0x000A0E64
	public void SetFlash1()
	{
		List<SpriteRenderer> copyRenderers = this.scrollSprite.copyRenderers;
		for (int i = 0; i < copyRenderers.Count; i++)
		{
			copyRenderers[i].sprite = this.flashSprite1;
		}
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x000A2CA8 File Offset: 0x000A0EA8
	public void SetFlash2()
	{
		List<SpriteRenderer> copyRenderers = this.scrollSprite.copyRenderers;
		for (int i = 0; i < copyRenderers.Count; i++)
		{
			copyRenderers[i].sprite = this.flashSprite2;
		}
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x000A2CEC File Offset: 0x000A0EEC
	public void SetNormal()
	{
		List<SpriteRenderer> copyRenderers = this.scrollSprite.copyRenderers;
		for (int i = 0; i < copyRenderers.Count; i++)
		{
			copyRenderers[i].sprite = this.normalSprite;
		}
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x0001479C File Offset: 0x0001299C
	public void OnDestroy()
	{
		this.normalSprite = null;
		this.flashSprite1 = null;
		this.flashSprite2 = null;
	}

	// Token: 0x0400136F RID: 4975
	[SerializeField]
	public Sprite normalSprite;

	// Token: 0x04001370 RID: 4976
	[SerializeField]
	public Sprite flashSprite1;

	// Token: 0x04001371 RID: 4977
	[SerializeField]
	public Sprite flashSprite2;

	// Token: 0x04001372 RID: 4978
	[SerializeField]
	public ScrollingSprite scrollSprite;
}
