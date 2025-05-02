using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000298 RID: 664
public abstract class AbstractFrogsLevelSlotBullet : AbstractPausableComponent
{
	// Token: 0x06001DF7 RID: 7671 RVA: 0x000194ED File Offset: 0x000176ED
	public AbstractFrogsLevelSlotBullet()
	{
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x000194F5 File Offset: 0x000176F5
	public virtual float Y
	{
		get
		{
			return (float)(Level.Current.Ground + 50);
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x00019505 File Offset: 0x00017705
	public virtual float Y_Time
	{
		get
		{
			return 0.45f;
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06001DFA RID: 7674 RVA: 0x0001950C File Offset: 0x0001770C
	public virtual EaseUtils.EaseType Y_Ease
	{
		get
		{
			return EaseUtils.EaseType.easeOutBounce;
		}
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x000B22AC File Offset: 0x000B04AC
	public AbstractFrogsLevelSlotBullet Create(Vector2 pos, float speed)
	{
		AbstractFrogsLevelSlotBullet abstractFrogsLevelSlotBullet = this.InstantiatePrefab<AbstractFrogsLevelSlotBullet>();
		abstractFrogsLevelSlotBullet.transform.SetPosition(new float?(pos.x), new float?(pos.y), null);
		abstractFrogsLevelSlotBullet.speed = speed;
		return abstractFrogsLevelSlotBullet;
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x000B22F4 File Offset: 0x000B04F4
	public virtual void Start()
	{
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
		GameObject gameObject = new GameObject("Damage Shit!");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.ResetLocalTransforms();
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.size = new Vector2(240f, 40f);
		boxCollider2D.isTrigger = true;
		CollisionChild collisionChild = gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.DealDamage;
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.y_cr());
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x00019510 File Offset: 0x00017710
	public void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x0001951D File Offset: 0x0001771D
	public void DealDamage(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x0001952C File Offset: 0x0001772C
	public virtual void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x000B23AC File Offset: 0x000B05AC
	public IEnumerator x_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (base.transform.position.x < -1280f)
			{
				this.End();
			}
			base.transform.AddPosition(-this.speed * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x000B23C8 File Offset: 0x000B05C8
	public IEnumerator y_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float start = base.transform.position.y;
		float t = 0f;
		while (t < this.Y_Time)
		{
			float val = t / this.Y_Time;
			float y = EaseUtils.Ease(this.Y_Ease, start, this.Y, val);
			base.transform.SetPosition(null, new float?(y), null);
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x04001897 RID: 6295
	public DamageDealer damageDealer;

	// Token: 0x04001898 RID: 6296
	public float speed;
}
