using System;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class PlayerCameraController : AbstractPlayerComponent
{
	// Token: 0x06003AB2 RID: 15026 RVA: 0x00110EA0 File Offset: 0x0010F0A0
	public void LevelInit()
	{
		this.rect = default(Rect);
		this.rect.x = base.transform.position.x - 100f;
		this.rect.width = 200f;
		this.rect.y = base.transform.position.y - 150f;
		this.rect.height = 300f;
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x00110F24 File Offset: 0x0010F124
	public void Update()
	{
		if (base.basePlayer.right > this.rect.x + this.rect.width)
		{
			this.rect.x = base.basePlayer.right - 200f;
		}
		if (base.basePlayer.left < this.rect.xMin)
		{
			this.rect.x = base.basePlayer.left;
		}
		if (base.basePlayer.top > this.rect.y + 300f)
		{
			this.rect.y = base.basePlayer.top - 300f;
		}
		if (base.basePlayer.bottom < this.rect.y)
		{
			this.rect.y = base.basePlayer.bottom;
		}
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x00111014 File Offset: 0x0010F214
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (!PlayerDebug.Enabled)
		{
			return;
		}
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawWireCube(this.rect.center, new Vector3(this.rect.width, this.rect.height, 1f));
		Gizmos.color = Color.white;
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x0002FBAA File Offset: 0x0002DDAA
	public Vector2 center
	{
		get
		{
			return this.rect.center;
		}
	}

	// Token: 0x04002F13 RID: 12051
	public const float WIDTH = 200f;

	// Token: 0x04002F14 RID: 12052
	public const float HEIGHT = 300f;

	// Token: 0x04002F15 RID: 12053
	public Rect rect = default(Rect);
}
