using System;
using UnityEngine;

// Token: 0x0200052A RID: 1322
public class PlayerSuperChaliceIIISpear : AbstractProjectile
{
	// Token: 0x060037BD RID: 14269 RVA: 0x0002D7B4 File Offset: 0x0002B9B4
	public override void OnDieLifetime()
	{
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x0002D7B6 File Offset: 0x0002B9B6
	public override void Start()
	{
		this._countParryTowardsScore = false;
		this.basePos = base.transform.position;
	}

	// Token: 0x060037BF RID: 14271 RVA: 0x0002D7D0 File Offset: 0x0002B9D0
	public void DetachFromSuper(LevelPlayerController p)
	{
		this.sourcePlayer = p;
		this.sourcePlayer.weaponManager.OnSuperStart += this.Die;
		base.transform.parent = null;
	}

	// Token: 0x060037C0 RID: 14272 RVA: 0x0002D802 File Offset: 0x0002BA02
	public override void OnParry(AbstractPlayerController player)
	{
		AudioManager.Play("player_super_chalice_barrage_spearparry");
		base.OnParry(player);
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x0002D815 File Offset: 0x0002BA15
	public override void OnParryDie()
	{
		this.Die();
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x0002D81D File Offset: 0x0002BA1D
	public override void Die()
	{
		this.coll.enabled = false;
		base.animator.Play("Die");
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x0002D83B File Offset: 0x0002BA3B
	public override void OnDestroy()
	{
		if (this.sourcePlayer != null)
		{
			this.sourcePlayer.weaponManager.OnSuperStart -= this.Die;
		}
		base.OnDestroy();
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x0002D871 File Offset: 0x0002BA71
	public override void FixedUpdate()
	{
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x00104DC0 File Offset: 0x00102FC0
	public override void Update()
	{
		this.floatT += CupheadTime.Delta * this.floatSpeed;
		base.transform.position = new Vector3(this.basePos.x, this.basePos.y + Mathf.Sin(this.floatT) * this.floatAmplitude);
		this.timer += CupheadTime.Delta;
		if (this.timer > 10f)
		{
			this.Die();
		}
		if (base.transform.parent == null && this.sourcePlayer == null)
		{
			this.Die();
		}
	}

	// Token: 0x04002CEC RID: 11500
	public const float EXPIRE_TIME = 10f;

	// Token: 0x04002CED RID: 11501
	[SerializeField]
	public BoxCollider2D coll;

	// Token: 0x04002CEE RID: 11502
	[SerializeField]
	public float floatAmplitude = 20f;

	// Token: 0x04002CEF RID: 11503
	[SerializeField]
	public float floatT;

	// Token: 0x04002CF0 RID: 11504
	[SerializeField]
	public float floatSpeed = 1f;

	// Token: 0x04002CF1 RID: 11505
	public Vector3 basePos;

	// Token: 0x04002CF2 RID: 11506
	public float timer;

	// Token: 0x04002CF3 RID: 11507
	public LevelPlayerController sourcePlayer;
}
