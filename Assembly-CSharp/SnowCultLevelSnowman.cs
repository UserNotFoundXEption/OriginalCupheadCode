using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200039A RID: 922
public class SnowCultLevelSnowman : AbstractCollidableObject
{
	// Token: 0x060028A8 RID: 10408 RVA: 0x000CF428 File Offset: 0x000CD628
	public void Init(Vector3 pos, LevelProperties.SnowCult.Snowman properties, bool goingRight)
	{
		base.transform.position = pos;
		this.yPos = base.transform.position.y;
		this.properties = properties;
		this.goingRight = goingRight;
		if (goingRight)
		{
			base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(base.transform.localScale.x), null, null);
		}
	}

	// Token: 0x060028A9 RID: 10409 RVA: 0x000CF4E0 File Offset: 0x000CD6E0
	public void Start()
	{
		this.coll = base.GetComponent<Collider2D>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.Health = this.properties.health;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x00022261 File Offset: 0x00020461
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x00022279 File Offset: 0x00020479
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x00022297 File Offset: 0x00020497
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f && !this.melted)
		{
			this.melted = true;
			this.Melt();
		}
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x000222D4 File Offset: 0x000204D4
	public void Melt()
	{
		base.StartCoroutine(this.melt_cr());
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x000CF548 File Offset: 0x000CD748
	public IEnumerator melt_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Melt");
		yield return base.animator.WaitForAnimationToEnd(this, "Melt", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeUntilUnmelt);
		base.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, this.properties.unmeltLoopTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Unmelt", false, true);
		this.melted = false;
		this.Health = this.properties.health;
		base.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x000CF564 File Offset: 0x000CD764
	public void Turn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x000CF5A8 File Offset: 0x000CD7A8
	public IEnumerator move_cr()
	{
		float sizeX = this.coll.bounds.size.x;
		float left = -640f + sizeX / 2f;
		float right = 640f - sizeX / 2f;
		float t = 0f;
		float time = this.properties.runTime;
		EaseUtils.EaseType ease = EaseUtils.EaseType.linear;
		Vector3 endPos = Vector3.zero;
		endPos = ((!this.goingRight) ? new Vector3(left, this.yPos) : new Vector3(right, this.yPos));
		float speed = Vector3.Distance(new Vector3(right, this.yPos), new Vector3(left, this.yPos)) / time;
		while (base.transform.position != endPos)
		{
			while (this.melted)
			{
				yield return null;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, endPos, speed * CupheadTime.Delta);
			yield return null;
		}
		float start = 0f;
		float end = 0f;
		base.animator.Play("Turn");
		yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		if (this.goingRight)
		{
			start = base.transform.position.x;
			end = left;
		}
		else
		{
			start = base.transform.position.x;
			end = right;
		}
		for (;;)
		{
			t = 0f;
			while (t < time)
			{
				if (!this.melted)
				{
					float value = t / time;
					base.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, value)), null, null);
					t += CupheadTime.Delta;
				}
				yield return null;
			}
			base.animator.Play("Turn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
			base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
			this.goingRight = !this.goingRight;
			if (!this.goingRight)
			{
				start = left;
				end = right;
			}
			else
			{
				start = right;
				end = left;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060028B1 RID: 10417 RVA: 0x000222E3 File Offset: 0x000204E3
	public void Die()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040021EB RID: 8683
	public LevelProperties.SnowCult.Snowman properties;

	// Token: 0x040021EC RID: 8684
	public Collider2D coll;

	// Token: 0x040021ED RID: 8685
	public DamageDealer damageDealer;

	// Token: 0x040021EE RID: 8686
	public DamageReceiver damageReceiver;

	// Token: 0x040021EF RID: 8687
	public bool goingRight;

	// Token: 0x040021F0 RID: 8688
	public bool melted;

	// Token: 0x040021F1 RID: 8689
	public float Health;

	// Token: 0x040021F2 RID: 8690
	public float yPos;
}
