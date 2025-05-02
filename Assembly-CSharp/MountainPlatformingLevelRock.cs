using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public class MountainPlatformingLevelRock : AbstractProjectile
{
	// Token: 0x06002F21 RID: 12065 RVA: 0x000E0DC0 File Offset: 0x000DEFC0
	public MountainPlatformingLevelRock Create(Vector2 startPos, Vector2 fallPos, float velocity, float delay)
	{
		MountainPlatformingLevelRock mountainPlatformingLevelRock = base.Create() as MountainPlatformingLevelRock;
		mountainPlatformingLevelRock.transform.position = startPos;
		mountainPlatformingLevelRock.fallPos = fallPos;
		mountainPlatformingLevelRock.velocity = velocity;
		mountainPlatformingLevelRock.delay = delay;
		return mountainPlatformingLevelRock;
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x0002743A File Offset: 0x0002563A
	public override void Awake()
	{
		base.Awake();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetBool("PickedA", Rand.Bool());
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x00027463 File Offset: 0x00025663
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.launch_cr());
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x00027478 File Offset: 0x00025678
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002F25 RID: 12069 RVA: 0x00027496 File Offset: 0x00025696
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002F26 RID: 12070 RVA: 0x000E0E04 File Offset: 0x000DF004
	public IEnumerator launch_cr()
	{
		float x = Random.Range(-500f, 500f);
		while (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 100f)
		{
			base.transform.AddPosition(x * CupheadTime.Delta, 1000f * CupheadTime.Delta, 0f);
			yield return null;
		}
		base.animator.SetTrigger("getBig");
		base.transform.position = this.fallPos;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Projectiles.ToString();
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		for (;;)
		{
			base.transform.AddPosition(0f, -this.velocity * CupheadTime.Delta, 0f);
			this.velocity += 1000f * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002F27 RID: 12071 RVA: 0x000E0E20 File Offset: 0x000DF020
	public override void Die()
	{
		base.Die();
		AudioManager.Play("castle_giant_rock_smash");
		this.emitAudioFromObject.Add("castle_giant_rock_smash");
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.DeathParts();
		CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x000E0E7C File Offset: 0x000DF07C
	public void DeathParts()
	{
		this.explosion.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x04002719 RID: 10009
	[SerializeField]
	public Effect explosion;

	// Token: 0x0400271A RID: 10010
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x0400271B RID: 10011
	public Vector2 fallPos;

	// Token: 0x0400271C RID: 10012
	public float velocity;

	// Token: 0x0400271D RID: 10013
	public const float LAUNCH_VELOCITY_Y = 1000f;

	// Token: 0x0400271E RID: 10014
	public const float LAUNCH_VELOCITY_X = 500f;

	// Token: 0x0400271F RID: 10015
	public const float GRAVITY = 1000f;

	// Token: 0x04002720 RID: 10016
	public float delay;
}
