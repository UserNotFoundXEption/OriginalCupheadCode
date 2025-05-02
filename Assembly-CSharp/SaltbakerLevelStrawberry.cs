using System;
using UnityEngine;

// Token: 0x0200037F RID: 895
public class SaltbakerLevelStrawberry : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x17000324 RID: 804
	// (get) Token: 0x0600279F RID: 10143 RVA: 0x00021425 File Offset: 0x0001F625
	public override Vector3 Direction
	{
		get
		{
			return -base.transform.up;
		}
	}

	// Token: 0x060027A0 RID: 10144 RVA: 0x000CC4C0 File Offset: 0x000CA6C0
	public BasicProjectile Create(Vector2 position, float rotation, float speed, int anim)
	{
		SaltbakerLevelStrawberry saltbakerLevelStrawberry = (SaltbakerLevelStrawberry)base.Create(position, rotation, speed);
		saltbakerLevelStrawberry.anim.Play(anim.ToString());
		return saltbakerLevelStrawberry;
	}

	// Token: 0x060027A1 RID: 10145 RVA: 0x000CC4F8 File Offset: 0x000CA6F8
	public override void Move()
	{
		if (!this.coll.enabled)
		{
			return;
		}
		base.Move();
		if (base.transform.position.y - 40f < (float)Level.Current.Ground)
		{
			this.shadow.enabled = false;
			this.Die();
		}
		else
		{
			base.HandleShadow(40f, 0f);
		}
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x00021437 File Offset: 0x0001F637
	public override void Die()
	{
		this.coll.enabled = false;
		this.createSparks = false;
		base.transform.eulerAngles = Vector3.zero;
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x0002146C File Offset: 0x0001F66C
	public void OnDeathAnimationEnd()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040020D8 RID: 8408
	public const float OFFSET = 40f;

	// Token: 0x040020D9 RID: 8409
	[SerializeField]
	public Animator anim;

	// Token: 0x040020DA RID: 8410
	[SerializeField]
	public Collider2D coll;
}
