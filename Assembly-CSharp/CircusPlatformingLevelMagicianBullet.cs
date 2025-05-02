using System;
using UnityEngine;

// Token: 0x0200040B RID: 1035
public class CircusPlatformingLevelMagicianBullet : BasicProjectile
{
	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06002D2F RID: 11567 RVA: 0x000DC2BC File Offset: 0x000DA4BC
	// (remove) Token: 0x06002D30 RID: 11568 RVA: 0x000DC2F4 File Offset: 0x000DA4F4
	public event Action OnProjectileDeath;

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06002D31 RID: 11569 RVA: 0x00025BA3 File Offset: 0x00023DA3
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002D32 RID: 11570 RVA: 0x000DC32C File Offset: 0x000DA52C
	public override void Start()
	{
		base.Start();
		AudioManager.PlayLoop("circus_magician_magic_loop");
		this.emitAudioFromObject.Add("circus_magician_magic_loop");
		this.puffs.flipX = Rand.Bool();
		this.puffs.flipY = Rand.Bool();
		this.DestroyDistance = 0f;
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x00025BAA File Offset: 0x00023DAA
	public override void OnDestroy()
	{
		AudioManager.Stop("circus_magician_magic_loop");
		if (this.OnProjectileDeath != null)
		{
			this.OnProjectileDeath();
		}
		base.OnDestroy();
	}

	// Token: 0x0400256F RID: 9583
	[SerializeField]
	public SpriteRenderer puffs;
}
