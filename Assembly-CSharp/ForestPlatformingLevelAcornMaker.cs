using System;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class ForestPlatformingLevelAcornMaker : PlatformingLevelShootingEnemy
{
	// Token: 0x06002BF0 RID: 11248 RVA: 0x000D8BF0 File Offset: 0x000D6DF0
	public override void Shoot()
	{
		ForestPlatformingLevelAcorn.Direction direction;
		if (this._target.transform.position.x < base.transform.position.x)
		{
			direction = ForestPlatformingLevelAcorn.Direction.Left;
		}
		else
		{
			direction = ForestPlatformingLevelAcorn.Direction.Right;
		}
		this.acornPrefab.Spawn(this, this.spawnRoot.transform.position, direction, true);
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x000D8C5C File Offset: 0x000D6E5C
	public override void Die()
	{
		if (!this.isDying)
		{
			if (this.killAcorns != null)
			{
				this.killAcorns();
			}
			base.animator.SetTrigger("Death");
			Collider2D[] componentsInChildren = base.GetComponentsInChildren<Collider2D>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			this.isDying = true;
			this.explosion.Create(this.gruntRoot.transform.position);
			this.gruntSprite.enabled = false;
		}
		else
		{
			base.Die();
		}
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x000D8CF8 File Offset: 0x000D6EF8
	public void PlayGruntSFX()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("level_acorn_maker_grunt");
			this.emitAudioFromObject.Add("level_acorn_maker_grunt");
		}
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x000D8D50 File Offset: 0x000D6F50
	public void PlayIdleSFX()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("level_acorn_maker_idle");
			this.emitAudioFromObject.Add("level_acorn_maker_idle");
		}
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x00024D18 File Offset: 0x00022F18
	public void PlayDeathSFX()
	{
		AudioManager.Play("level_acorn_maker_death");
		this.emitAudioFromObject.Add("level_acorn_maker_death");
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x00024D34 File Offset: 0x00022F34
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.acornPrefab = null;
		this.explosion = null;
	}

	// Token: 0x0400245D RID: 9309
	public const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x0400245E RID: 9310
	[SerializeField]
	public Effect explosion;

	// Token: 0x0400245F RID: 9311
	[SerializeField]
	public Transform gruntRoot;

	// Token: 0x04002460 RID: 9312
	[SerializeField]
	public SpriteRenderer gruntSprite;

	// Token: 0x04002461 RID: 9313
	[SerializeField]
	public ForestPlatformingLevelAcorn acornPrefab;

	// Token: 0x04002462 RID: 9314
	[SerializeField]
	public Transform spawnRoot;

	// Token: 0x04002463 RID: 9315
	public bool isDying;

	// Token: 0x04002464 RID: 9316
	public Action killAcorns;
}
