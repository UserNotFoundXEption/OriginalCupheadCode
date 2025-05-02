using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class FlowerLevelCloudBomb : AbstractCollidableObject
{
	// Token: 0x060018A1 RID: 6305 RVA: 0x00015026 File Offset: 0x00013226
	public void OnCloudBombStart(Vector3 target, float s, float delay)
	{
		this.playerPos = target;
		base.transform.LookAt2D(this.playerPos);
		this.speed = s;
		this.detonationDelay = delay;
		this.hasDetonated = false;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x00015060 File Offset: 0x00013260
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000A43D4 File Offset: 0x000A25D4
	public void FixedUpdate()
	{
		if (!this.hasDetonated)
		{
			if ((this.playerPos - base.transform.position).magnitude > (base.transform.right * (this.speed * CupheadTime.FixedDelta)).magnitude)
			{
				base.transform.position += base.transform.right * (this.speed * CupheadTime.FixedDelta);
			}
			else
			{
				this.hasDetonated = true;
				base.StartCoroutine(this.explode_cr());
				base.transform.position += this.playerPos - base.transform.position;
			}
		}
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x00015078 File Offset: 0x00013278
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x000A44AC File Offset: 0x000A26AC
	public IEnumerator explode_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.detonationDelay);
		base.animator.SetTrigger("Explode");
		BoxCollider2D collider = base.GetComponent<BoxCollider2D>();
		collider.size = base.GetComponent<SpriteRenderer>().bounds.size;
		yield break;
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x000150A1 File Offset: 0x000132A1
	public void Die()
	{
		AudioManager.Play("flower_minion_simple_deathpop_high");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_high");
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040013F1 RID: 5105
	public bool hasDetonated;

	// Token: 0x040013F2 RID: 5106
	public float speed;

	// Token: 0x040013F3 RID: 5107
	public float detonationDelay;

	// Token: 0x040013F4 RID: 5108
	public Vector3 playerPos;

	// Token: 0x040013F5 RID: 5109
	public DamageDealer damageDealer;
}
