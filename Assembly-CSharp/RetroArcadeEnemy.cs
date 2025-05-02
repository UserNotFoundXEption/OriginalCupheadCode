using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030F RID: 783
public abstract class RetroArcadeEnemy : AbstractProjectile
{
	// Token: 0x0600228F RID: 8847 RVA: 0x0001D64F File Offset: 0x0001B84F
	public RetroArcadeEnemy()
	{
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06002290 RID: 8848 RVA: 0x0001D657 File Offset: 0x0001B857
	// (set) Token: 0x06002291 RID: 8849 RVA: 0x0001D65F File Offset: 0x0001B85F
	public bool IsDead { get; set; }

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06002292 RID: 8850 RVA: 0x0001D668 File Offset: 0x0001B868
	// (set) Token: 0x06002293 RID: 8851 RVA: 0x0001D670 File Offset: 0x0001B870
	public float PointsWorth { get; set; }

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06002294 RID: 8852 RVA: 0x0001D679 File Offset: 0x0001B879
	// (set) Token: 0x06002295 RID: 8853 RVA: 0x0001D681 File Offset: 0x0001B881
	public float PointsBonus { get; set; }

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06002296 RID: 8854 RVA: 0x0001D68A File Offset: 0x0001B88A
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x0001D691 File Offset: 0x0001B891
	public override void Awake()
	{
		base.Awake();
		if (base.GetComponent<DamageReceiver>() != null)
		{
			base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		}
		this.IsDead = false;
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x0001D6C9 File Offset: 0x0001B8C9
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x0001D6E7 File Offset: 0x0001B8E7
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000BDF88 File Offset: 0x000BC188
	public virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		RetroArcadeLevel.TOTAL_POINTS += this.PointsWorth;
		this.hp -= info.damage;
		if (this.hp < 0f && !this.IsDead)
		{
			this.Dead();
		}
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x0001D705 File Offset: 0x0001B905
	public void MoveY(float moveAmount, float moveSpeed)
	{
		if (this.moveCoroutine != null)
		{
			base.StopCoroutine(this.moveCoroutine);
		}
		this.movingY = true;
		this.moveCoroutine = base.StartCoroutine(this.moveY_cr(moveAmount, Mathf.Abs(moveAmount) / moveSpeed));
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000BDFDC File Offset: 0x000BC1DC
	public IEnumerator moveY_cr(float moveAmount, float time)
	{
		float t = 0f;
		float startY = base.transform.position.y;
		float endY = startY + moveAmount;
		while (t < time)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startY, endY, t / time)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(endY), null);
		this.movingY = false;
		yield break;
	}

	// Token: 0x0600229D RID: 8861 RVA: 0x000BE008 File Offset: 0x000BC208
	public virtual void Dead()
	{
		if (this.type != RetroArcadeEnemy.Type.IsBoss)
		{
			this.CheckForColorBonus();
		}
		Collider2D component = base.GetComponent<Collider2D>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.IsDead = true;
		base.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.25f);
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000BE06C File Offset: 0x000BC26C
	public void CheckForColorBonus()
	{
		if (this.type == RetroArcadeEnemy.LAST_TYPE)
		{
			RetroArcadeEnemy.COUNTER++;
			if (RetroArcadeEnemy.COUNTER >= 3)
			{
				this.GiveBonus();
				RetroArcadeEnemy.COUNTER = 0;
				RetroArcadeEnemy.LAST_TYPE = RetroArcadeEnemy.Type.None;
			}
			else
			{
				RetroArcadeEnemy.LAST_TYPE = this.type;
			}
		}
		else
		{
			RetroArcadeEnemy.COUNTER = 1;
			RetroArcadeEnemy.LAST_TYPE = this.type;
		}
	}

	// Token: 0x0600229F RID: 8863 RVA: 0x0001D740 File Offset: 0x0001B940
	public virtual void GiveBonus()
	{
		RetroArcadeLevel.TOTAL_POINTS += this.PointsBonus;
	}

	// Token: 0x04001C8F RID: 7311
	[SerializeField]
	public RetroArcadeEnemy.Type type;

	// Token: 0x04001C90 RID: 7312
	public static int COUNTER;

	// Token: 0x04001C91 RID: 7313
	public static RetroArcadeEnemy.Type LAST_TYPE;

	// Token: 0x04001C92 RID: 7314
	public Coroutine moveCoroutine;

	// Token: 0x04001C96 RID: 7318
	public float pointsBonusAccuracy;

	// Token: 0x04001C97 RID: 7319
	public bool inComboChain;

	// Token: 0x04001C98 RID: 7320
	public float hp;

	// Token: 0x04001C99 RID: 7321
	public bool movingY;

	// Token: 0x02000E3D RID: 3645
	public enum Type
	{
		// Token: 0x040066E6 RID: 26342
		A,
		// Token: 0x040066E7 RID: 26343
		B,
		// Token: 0x040066E8 RID: 26344
		C,
		// Token: 0x040066E9 RID: 26345
		None,
		// Token: 0x040066EA RID: 26346
		IsBoss
	}
}
