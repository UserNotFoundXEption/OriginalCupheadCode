using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001ED RID: 493
public class DicePalaceFlyingHorseLevelPresent : AbstractProjectile
{
	// Token: 0x060016C5 RID: 5829 RVA: 0x0001361A File Offset: 0x0001181A
	public void Init(Vector3 startPos, Vector3 targetPos, LevelProperties.DicePalaceFlyingHorse.GiftBombs properties)
	{
		base.transform.position = startPos;
		this.targetPos = targetPos;
		this.properties = properties;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x00013643 File Offset: 0x00011843
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060016C7 RID: 5831 RVA: 0x0001366C File Offset: 0x0001186C
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x0009FBAC File Offset: 0x0009DDAC
	public IEnumerator move_cr()
	{
		while (base.transform.position != this.targetPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPos, this.properties.initialSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.explosionTime);
		string[] spreadCountPattern = this.properties.spreadCount.Split(new char[]
		{
			','
		});
		float angle = 0f;
		int parryIndex = Random.Range(0, spreadCountPattern.Length);
		for (int i = 0; i < spreadCountPattern.Length; i++)
		{
			Parser.FloatTryParse(spreadCountPattern[i], out angle);
			this.SpawnBullet(angle, parryIndex == i);
		}
		yield return null;
		this.Die();
		yield break;
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x0009FBC8 File Offset: 0x0009DDC8
	public void SpawnBullet(float angle, bool parryable)
	{
		AudioManager.Play("projectile_explo");
		this.emitAudioFromObject.Add("projectile_explo");
		BasicProjectile basicProjectile = this.bullet.Create(base.transform.position, angle, this.properties.explosionSpeed);
		basicProjectile.SetParryable(parryable);
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x0001368A File Offset: 0x0001188A
	public override void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x04001281 RID: 4737
	[SerializeField]
	public BasicProjectile bullet;

	// Token: 0x04001282 RID: 4738
	public LevelProperties.DicePalaceFlyingHorse.GiftBombs properties;

	// Token: 0x04001283 RID: 4739
	public Vector3 targetPos;
}
