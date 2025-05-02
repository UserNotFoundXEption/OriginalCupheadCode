using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public class PirateLevelBoatProjectile : AbstractProjectile
{
	// Token: 0x0600218F RID: 8591 RVA: 0x000BACD4 File Offset: 0x000B8ED4
	public PirateLevelBoatProjectile Create(Vector2 pos, float speed, float rotationSpeed)
	{
		PirateLevelBoatProjectile pirateLevelBoatProjectile = this.Create() as PirateLevelBoatProjectile;
		pirateLevelBoatProjectile.CollisionDeath.OnlyPlayer();
		pirateLevelBoatProjectile.DamagesType.OnlyPlayer();
		pirateLevelBoatProjectile.Init(pos, speed, rotationSpeed);
		return pirateLevelBoatProjectile;
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x0001CA6A File Offset: 0x0001AC6A
	public void Init(Vector2 pos, float speed, float rotationSpeed)
	{
		base.StartCoroutine(this.bullet_cr(pos, speed, rotationSpeed));
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x0001CA7C File Offset: 0x0001AC7C
	public override void Update()
	{
		base.Update();
		this.child.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x0001CAAD File Offset: 0x0001ACAD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
			this.StopAllCoroutines();
			base.StartCoroutine(this.die_cr());
		}
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x0001CADC File Offset: 0x0001ACDC
	public override void Die()
	{
		this.child.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?((float)Random.Range(0, 360)));
		base.Die();
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x0001CB14 File Offset: 0x0001AD14
	public void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000BAD10 File Offset: 0x000B8F10
	public IEnumerator bullet_cr(Vector2 pos, float speed, float rotationSpeed)
	{
		base.transform.position = pos - this.child.localPosition;
		(base.GetComponent<Collider2D>() as CircleCollider2D).offset = this.child.localPosition;
		for (;;)
		{
			if (base.transform.position.x < -1280f)
			{
				this.End();
			}
			base.transform.AddPosition(-speed * CupheadTime.Delta, 0f, 0f);
			base.transform.AddEulerAngles(0f, 0f, -rotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x000BAD40 File Offset: 0x000B8F40
	public IEnumerator die_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001BA9 RID: 7081
	[SerializeField]
	public Transform child;
}
