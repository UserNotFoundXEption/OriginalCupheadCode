using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000369 RID: 873
public class SallyStagePlayLevelWindowProjectile : AbstractProjectile
{
	// Token: 0x060026A9 RID: 9897 RVA: 0x000C97E4 File Offset: 0x000C79E4
	public SallyStagePlayLevelWindowProjectile Create(Vector2 pos, float rotation, float speed, SallyStagePlayLevel parent)
	{
		SallyStagePlayLevelWindowProjectile sallyStagePlayLevelWindowProjectile = base.Create() as SallyStagePlayLevelWindowProjectile;
		sallyStagePlayLevelWindowProjectile.transform.position = pos;
		sallyStagePlayLevelWindowProjectile.rotation = rotation;
		sallyStagePlayLevelWindowProjectile.speed = speed;
		return sallyStagePlayLevelWindowProjectile;
	}

	// Token: 0x060026AA RID: 9898 RVA: 0x0002078F File Offset: 0x0001E98F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060026AB RID: 9899 RVA: 0x000207AD File Offset: 0x0001E9AD
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060026AC RID: 9900 RVA: 0x000C9820 File Offset: 0x000C7A20
	public IEnumerator move_cr()
	{
		this.move = true;
		Vector3 dir = MathUtils.AngleToDirection(this.rotation);
		while (this.move)
		{
			base.transform.position += dir * this.speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060026AD RID: 9901 RVA: 0x000C983C File Offset: 0x000C7A3C
	public override void Start()
	{
		base.Start();
		if (this.child != null)
		{
			this.child.transform.SetEulerAngles(null, null, new float?(0f));
			this.child.OnPlayerCollision += this.OnCollisionPlayer;
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.on_ground_hit_cr());
	}

	// Token: 0x060026AE RID: 9902 RVA: 0x000207CB File Offset: 0x0001E9CB
	public void OnPhase3()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060026AF RID: 9903 RVA: 0x000207D8 File Offset: 0x0001E9D8
	public override void Die()
	{
		base.Die();
	}

	// Token: 0x060026B0 RID: 9904 RVA: 0x000C98C0 File Offset: 0x000C7AC0
	public IEnumerator on_ground_hit_cr()
	{
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			yield return null;
		}
		this.move = false;
		if (this.isBaby)
		{
			base.animator.SetTrigger("OnSmash");
			AudioManager.Play("sally_bottle_smash");
			this.emitAudioFromObject.Add("sally_bottle_smash");
		}
		else
		{
			base.animator.Play("Death");
		}
		yield return null;
		yield break;
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x000207E0 File Offset: 0x0001E9E0
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04001FE3 RID: 8163
	[SerializeField]
	public bool isBaby;

	// Token: 0x04001FE4 RID: 8164
	[SerializeField]
	public CollisionChild child;

	// Token: 0x04001FE5 RID: 8165
	public float speed;

	// Token: 0x04001FE6 RID: 8166
	public float rotation;

	// Token: 0x04001FE7 RID: 8167
	public bool move;
}
