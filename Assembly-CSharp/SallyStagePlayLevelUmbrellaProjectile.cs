using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class SallyStagePlayLevelUmbrellaProjectile : AbstractProjectile
{
	// Token: 0x06002686 RID: 9862 RVA: 0x0002055A File Offset: 0x0001E75A
	public override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x000C9364 File Offset: 0x000C7564
	public void InitProjectile(LevelProperties.SallyStagePlay properties, int direction)
	{
		this.properties = properties;
		this.active = false;
		this.direction = direction;
		base.transform.SetScale(new float?((float)(-(float)direction)), null, null);
		this.currentVelocity = Vector3.down * properties.CurrentState.umbrella.objectSpeed;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.check_bounds_cr());
	}

	// Token: 0x06002688 RID: 9864 RVA: 0x000C93E8 File Offset: 0x000C75E8
	public IEnumerator move_cr()
	{
		bool isFalling = false;
		for (;;)
		{
			if (this.active)
			{
				for (int i = 0; i < 2; i++)
				{
					AbstractPlayerController next = PlayerManager.GetNext();
					if (base.transform.position.x >= next.center.x - 10f && base.transform.position.x <= next.center.x + 10f)
					{
						if (!isFalling)
						{
							base.animator.SetTrigger("OnFall");
							isFalling = true;
						}
						this.currentVelocity = Vector3.down * this.properties.CurrentState.umbrella.objectDropSpeed;
					}
				}
			}
			base.transform.position += this.currentVelocity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002689 RID: 9865 RVA: 0x000C9404 File Offset: 0x000C7604
	public IEnumerator check_bounds_cr()
	{
		float offset = 50f;
		bool goingVertically = false;
		bool goingUp = false;
		for (;;)
		{
			if (base.transform.position.y >= 360f - offset && goingVertically)
			{
				base.transform.position = new Vector3(base.transform.position.x, 360f - offset, 0f);
				this.currentVelocity = Vector3.left * this.properties.CurrentState.umbrella.objectSpeed * (float)this.direction;
				this.active = true;
				goingVertically = false;
			}
			else if (base.transform.position.y <= -360f + offset && goingVertically)
			{
				this.currentVelocity = Vector3.right * this.properties.CurrentState.umbrella.objectSpeed * (float)this.direction;
				goingVertically = false;
			}
			else if ((base.transform.position.x <= -630f && !goingVertically) || (base.transform.position.x >= 630f && !goingVertically))
			{
				if (!goingUp)
				{
					base.animator.SetTrigger("OnClimb");
					goingUp = true;
				}
				if (!this.dropped)
				{
					base.GetComponent<SpriteRenderer>().material = this.change;
					this.dropped = true;
				}
				this.currentVelocity = Vector3.up * this.properties.CurrentState.umbrella.objectSpeed;
				goingVertically = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600268A RID: 9866 RVA: 0x0002056D File Offset: 0x0001E76D
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x0600268B RID: 9867 RVA: 0x000C9420 File Offset: 0x000C7620
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.active)
		{
			this.Die();
		}
		else if (!this.dropped)
		{
			base.GetComponent<SpriteRenderer>().material = this.change;
			base.animator.SetTrigger("OnDrop");
			this.dropped = true;
		}
		if (phase == CollisionPhase.Enter)
		{
			this.currentVelocity = Vector3.right * this.properties.CurrentState.umbrella.objectSpeed * (float)this.direction;
		}
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x0600268C RID: 9868 RVA: 0x000C94B8 File Offset: 0x000C76B8
	public override void Die()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		foreach (SpriteDeathParts spriteDeathParts in this.sprites)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
		base.Die();
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x00020591 File Offset: 0x0001E791
	public void Kill()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x0002059E File Offset: 0x0001E79E
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.sprites = null;
	}

	// Token: 0x04001FCA RID: 8138
	public bool active;

	// Token: 0x04001FCB RID: 8139
	public bool dropped;

	// Token: 0x04001FCC RID: 8140
	public int direction;

	// Token: 0x04001FCD RID: 8141
	public Vector3 currentVelocity;

	// Token: 0x04001FCE RID: 8142
	public LevelProperties.SallyStagePlay properties;

	// Token: 0x04001FCF RID: 8143
	[SerializeField]
	public Material change;

	// Token: 0x04001FD0 RID: 8144
	[SerializeField]
	public SpriteDeathParts[] sprites;
}
