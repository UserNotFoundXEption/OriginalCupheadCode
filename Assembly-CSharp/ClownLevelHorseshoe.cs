using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class ClownLevelHorseshoe : AbstractProjectile
{
	// Token: 0x06001419 RID: 5145 RVA: 0x000997B4 File Offset: 0x000979B4
	public void Init(Vector2 pos, float velocityX, float velocityY, bool onRight, float durationBeforeDrop, LevelProperties.Clown.Horse properties, ClownLevelClownHorse.HorseType horseType)
	{
		base.transform.position = pos;
		this.velocityX = velocityX;
		this.velocityY = velocityY;
		this.properties = properties;
		this.onRight = onRight;
		this.durationBeforeDrop = durationBeforeDrop;
		if (horseType != ClownLevelClownHorse.HorseType.Wave)
		{
			if (horseType == ClownLevelClownHorse.HorseType.Drop)
			{
				base.StartCoroutine(this.move_to_drop_point_cr());
				this.selectedSparkle = this.yellowSparkle;
				base.animator.SetInteger("type", 0);
			}
		}
		else
		{
			base.StartCoroutine(this.wave_cr());
			if (base.CanParry)
			{
				base.animator.SetInteger("type", 2);
				this.selectedSparkle = this.pinkSparkle;
			}
			else
			{
				base.animator.SetInteger("type", 1);
				this.selectedSparkle = this.greenSparkle;
			}
		}
		base.StartCoroutine(this.spawn_sparkle_cr());
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x00010E8D File Offset: 0x0000F08D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x00010EAB File Offset: 0x0000F0AB
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x000998AC File Offset: 0x00097AAC
	public IEnumerator wave_cr()
	{
		float angle = 0f;
		float speed = 0f;
		float loopSize = 0f;
		Vector3 moveX = base.transform.position;
		float edge = (!this.onRight) ? 690f : -690f;
		speed = ((!this.onRight) ? this.velocityX : (-this.velocityX));
		while ((!this.onRight) ? (base.transform.position.x < edge) : (base.transform.position.x > edge))
		{
			if (this.velocityY < 0f)
			{
				loopSize = -this.properties.WaveBulletAmount;
			}
			else
			{
				loopSize = this.properties.WaveBulletAmount;
			}
			angle += this.velocityY * CupheadTime.Delta;
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle + this.properties.WaveBulletAmount) * CupheadTime.Delta * 60f * loopSize / 2f);
			moveX = base.transform.right * speed * CupheadTime.Delta;
			base.transform.position += moveX + moveY;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000998C8 File Offset: 0x00097AC8
	public IEnumerator move_to_drop_point_cr()
	{
		Vector3 pos = base.transform.position;
		if (this.onRight)
		{
			float leavePos = -740f;
			while (base.transform.position.x > leavePos)
			{
				base.transform.AddPosition(-this.velocityX * CupheadTime.Delta, 0f, 0f);
				yield return null;
			}
			pos.x = -740f;
		}
		else
		{
			float leavePos = 740f;
			while (base.transform.position.x < leavePos)
			{
				base.transform.AddPosition(this.velocityX * CupheadTime.Delta, 0f, 0f);
				yield return null;
			}
			pos.x = 740f;
		}
		pos.y = 260f;
		base.transform.position = pos;
		float dropPos = this.onRight ? (640f - this.velocityY) : (-640f + this.velocityY);
		base.animator.SetTrigger("onTop");
		yield return CupheadTime.WaitForSeconds(this, this.properties.DropBulletDelay);
		while (base.transform.position.x != dropPos)
		{
			pos.x = Mathf.MoveTowards(base.transform.position.x, dropPos, this.velocityX * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		this.isSparkling = false;
		yield return CupheadTime.WaitForSeconds(this, this.durationBeforeDrop);
		this.isSparkling = true;
		base.animator.SetTrigger("down");
		AudioManager.Play("clown_horseshoe_drop");
		this.emitAudioFromObject.Add("clown_horseshoe_drop");
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			pos.y -= this.properties.DropBulletSpeedDown * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		AudioManager.Play("clown_horseshoe_land");
		this.emitAudioFromObject.Add("clown_horseshoe_land");
		base.animator.SetTrigger("dead");
		this.deathPoof.Create(base.transform.position);
		yield return null;
		yield break;
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x000998E4 File Offset: 0x00097AE4
	public IEnumerator drop_cr()
	{
		Vector3 pos = base.transform.position;
		yield return null;
		yield break;
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x00099900 File Offset: 0x00097B00
	public IEnumerator simple_cr()
	{
		float speed = 0f;
		float edge = (float)((!this.onRight) ? 640 : -640);
		speed = ((!this.onRight) ? this.velocityX : (-this.velocityX));
		while (base.transform.position.x != edge)
		{
			base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x0009991C File Offset: 0x00097B1C
	public IEnumerator spawn_sparkle_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			if (this.isSparkling)
			{
				this.selectedSparkle.Create(base.transform.position);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x00010EC9 File Offset: 0x0000F0C9
	public override void Die()
	{
		this.StopAllCoroutines();
		base.transform.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x00010EE8 File Offset: 0x0000F0E8
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.greenSparkle = null;
		this.yellowSparkle = null;
		this.pinkSparkle = null;
		this.deathPoof = null;
	}

	// Token: 0x0400105A RID: 4186
	[SerializeField]
	public Effect greenSparkle;

	// Token: 0x0400105B RID: 4187
	[SerializeField]
	public Effect pinkSparkle;

	// Token: 0x0400105C RID: 4188
	[SerializeField]
	public Effect yellowSparkle;

	// Token: 0x0400105D RID: 4189
	[SerializeField]
	public Effect deathPoof;

	// Token: 0x0400105E RID: 4190
	public Effect selectedSparkle;

	// Token: 0x0400105F RID: 4191
	public LevelProperties.Clown.Horse properties;

	// Token: 0x04001060 RID: 4192
	public float velocityX;

	// Token: 0x04001061 RID: 4193
	public float velocityY;

	// Token: 0x04001062 RID: 4194
	public bool onRight;

	// Token: 0x04001063 RID: 4195
	public float durationBeforeDrop;

	// Token: 0x04001064 RID: 4196
	public bool isSparkling = true;
}
