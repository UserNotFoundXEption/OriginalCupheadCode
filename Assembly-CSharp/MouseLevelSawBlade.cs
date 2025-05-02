using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class MouseLevelSawBlade : AbstractCollidableObject
{
	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06002000 RID: 8192 RVA: 0x0001B171 File Offset: 0x00019371
	// (set) Token: 0x06002001 RID: 8193 RVA: 0x0001B179 File Offset: 0x00019379
	public MouseLevelSawBlade.State state { get; set; }

	// Token: 0x06002002 RID: 8194 RVA: 0x0001B182 File Offset: 0x00019382
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x0001B1A1 File Offset: 0x000193A1
	public void Start()
	{
		base.animator.SetFloat("SawID", (float)this.sawId / 5f);
		base.animator.SetFloat("StickID", (float)this.stickId / 7f);
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x0001B1DD File Offset: 0x000193DD
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002005 RID: 8197 RVA: 0x0001B1F5 File Offset: 0x000193F5
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x0001B21E File Offset: 0x0001941E
	public void Begin(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		base.StartCoroutine(this.intro_cr());
		this.attackX = this.attackMinX;
		this.fullAttackX = this.attackMinX;
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x0001B24C File Offset: 0x0001944C
	public void Attack()
	{
		AudioManager.Play("level_mouse_buzzsaw_small");
		if (this.state == MouseLevelSawBlade.State.Idle)
		{
			this.state = MouseLevelSawBlade.State.Warning;
			base.StartCoroutine(this.attack_cr(false));
		}
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x000B7020 File Offset: 0x000B5220
	public void FullAttack()
	{
		if (this.state == MouseLevelSawBlade.State.Warning)
		{
			this.StopAllCoroutines();
		}
		else if (this.state == MouseLevelSawBlade.State.Idle)
		{
			this.state = MouseLevelSawBlade.State.Warning;
		}
		this.fullAttacking = true;
		base.StartCoroutine(this.attack_cr(true));
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x000B706C File Offset: 0x000B526C
	public IEnumerator intro_cr()
	{
		float t = 0f;
		float introTime = Mathf.Abs(this.idleX - this.initX) / this.properties.CurrentState.brokenCanSawBlades.entrySpeed;
		while (t < introTime)
		{
			if (t > introTime * 0.75f)
			{
				base.GetComponent<Collider2D>().enabled = true;
			}
			base.transform.SetLocalPosition(new float?(Mathf.Lerp(this.initX, this.idleX, t / introTime)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.idleX), null, null);
		this.state = MouseLevelSawBlade.State.Idle;
		yield break;
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x000B7088 File Offset: 0x000B5288
	public IEnumerator attack_cr(bool fullAttack)
	{
		LevelProperties.Mouse.BrokenCanSawBlades p = this.properties.CurrentState.brokenCanSawBlades;
		float t = 0f;
		while (t < p.delayBeforeAttack)
		{
			this.progress = t / p.delayBeforeAttack;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.idleX, this.attackMinX, this.progress);
			this.setX(x, fullAttack);
			t += CupheadTime.Delta;
			this.blade.transform.Rotate(Vector3.forward, this.rotateSpeed / 2f * CupheadTime.Delta);
			yield return null;
		}
		base.animator.SetBool("Attacking", true);
		this.state = MouseLevelSawBlade.State.Attack;
		float attackTime = 2f * Mathf.Abs(this.attackMaxX - this.attackMinX) / p.speed;
		t = 0f;
		while (t < attackTime)
		{
			float start = this.attackMinX;
			this.progress = t / attackTime * 2f;
			if (this.progress > 1f)
			{
				start = this.idleX;
				this.progress = 2f - this.progress;
				this.blade.transform.Rotate(Vector3.forward, EaseUtils.EaseInOutSine(0f, this.rotateSpeed, this.progress) * CupheadTime.Delta);
			}
			else
			{
				this.blade.transform.Rotate(Vector3.forward, this.rotateSpeed * CupheadTime.Delta);
			}
			float x2 = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, this.attackMaxX, this.progress);
			this.setX(x2, fullAttack);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.setX(this.idleX, fullAttack);
		if (fullAttack)
		{
			this.fullAttacking = false;
		}
		if (!this.fullAttacking)
		{
			base.animator.SetBool("Attacking", false);
			this.attackX = this.attackMinX;
			this.fullAttackX = this.attackMinX;
			this.state = MouseLevelSawBlade.State.Idle;
		}
		yield break;
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x0001B279 File Offset: 0x00019479
	public void Leave()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x000B70AC File Offset: 0x000B52AC
	public IEnumerator leave_cr()
	{
		float leaveTime = 0f;
		if (this.state == MouseLevelSawBlade.State.Warning)
		{
			this.state = MouseLevelSawBlade.State.Idle;
			base.animator.SetBool("Attacking", false);
		}
		if (this.state == MouseLevelSawBlade.State.Attack)
		{
			leaveTime = Mathf.Abs(base.transform.localPosition.x - this.initX) / this.properties.CurrentState.brokenCanSawBlades.speed;
		}
		else
		{
			leaveTime = 2f;
		}
		float t = 0f;
		float startingX = base.transform.localPosition.x;
		while (t < leaveTime)
		{
			if (t > leaveTime * 0.25f)
			{
				base.GetComponent<Collider2D>().enabled = false;
			}
			base.transform.SetLocalPosition(new float?(Mathf.Lerp(startingX, this.initX, t / leaveTime)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.initX), null, null);
		yield break;
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x000B70C8 File Offset: 0x000B52C8
	public void setX(float x, bool fullAttack)
	{
		if (fullAttack)
		{
			this.fullAttackX = x;
		}
		else
		{
			this.attackX = x;
		}
		if (this.idleX > 0f)
		{
			base.transform.SetLocalPosition(new float?(Mathf.Min(this.attackX, this.fullAttackX)), null, null);
		}
		else
		{
			base.transform.SetLocalPosition(new float?(Mathf.Max(this.attackX, this.fullAttackX)), null, null);
		}
	}

	// Token: 0x04001A03 RID: 6659
	public const string SawParameterName = "SawID";

	// Token: 0x04001A04 RID: 6660
	public const string StickParameterName = "StickID";

	// Token: 0x04001A05 RID: 6661
	public const string AttackParameterName = "Attacking";

	// Token: 0x04001A06 RID: 6662
	public const int SawIdMax = 6;

	// Token: 0x04001A07 RID: 6663
	public const int StickIdMax = 8;

	// Token: 0x04001A09 RID: 6665
	[SerializeField]
	public float initX;

	// Token: 0x04001A0A RID: 6666
	[SerializeField]
	public float idleX;

	// Token: 0x04001A0B RID: 6667
	[SerializeField]
	public float attackMinX;

	// Token: 0x04001A0C RID: 6668
	[SerializeField]
	public float attackMaxX;

	// Token: 0x04001A0D RID: 6669
	[SerializeField]
	public Transform blade;

	// Token: 0x04001A0E RID: 6670
	[SerializeField]
	public float rotateSpeed;

	// Token: 0x04001A0F RID: 6671
	[Range(0f, 5f)]
	[SerializeField]
	public int sawId;

	// Token: 0x04001A10 RID: 6672
	[Range(0f, 7f)]
	[SerializeField]
	public int stickId;

	// Token: 0x04001A11 RID: 6673
	public LevelProperties.Mouse properties;

	// Token: 0x04001A12 RID: 6674
	public bool fullAttacking;

	// Token: 0x04001A13 RID: 6675
	public bool goBackwards;

	// Token: 0x04001A14 RID: 6676
	public float attackX;

	// Token: 0x04001A15 RID: 6677
	public float fullAttackX;

	// Token: 0x04001A16 RID: 6678
	public float progress;

	// Token: 0x04001A17 RID: 6679
	public DamageDealer damageDealer;

	// Token: 0x02000DCC RID: 3532
	public enum State
	{
		// Token: 0x040063BA RID: 25530
		Init,
		// Token: 0x040063BB RID: 25531
		Idle,
		// Token: 0x040063BC RID: 25532
		Warning,
		// Token: 0x040063BD RID: 25533
		Attack
	}
}
