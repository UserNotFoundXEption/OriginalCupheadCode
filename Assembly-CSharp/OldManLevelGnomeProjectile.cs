using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DC RID: 732
public class OldManLevelGnomeProjectile : AbstractProjectile
{
	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06002071 RID: 8305 RVA: 0x0001B957 File Offset: 0x00019B57
	// (set) Token: 0x06002072 RID: 8306 RVA: 0x0001B95F File Offset: 0x00019B5F
	public bool IsFlying { get; set; }

	// Token: 0x06002073 RID: 8307 RVA: 0x000B80AC File Offset: 0x000B62AC
	public virtual OldManLevelGnomeProjectile Init(Vector3 position, Vector3 speed, float gravity, bool spawnParryable, bool parryable, OldManLevelStomachPlatform target)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		base.transform.localScale = new Vector3((float)((!MathUtils.RandomBool()) ? 1 : -1), 1f);
		this.speed = speed;
		this.gravity = gravity;
		this.spawnParryable = spawnParryable;
		this.IsFlying = true;
		this.SetParryable(parryable);
		this.target = target;
		this.animHelper = base.GetComponent<AnimationHelper>();
		this.animHelper.Speed = 1f;
		base.animator.Play((!spawnParryable) ? ((!parryable) ? "Chicken" : "ChickenPink") : "Bone");
		base.animator.Update(0f);
		base.GetComponent<Collider2D>().enabled = true;
		this.bouncingOffscreen = false;
		this.triedHit = false;
		this.underwaterSprite.color = Color.white;
		this.playedAnticipationSound = false;
		return this;
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x0001B968 File Offset: 0x00019B68
	public override void OnParry(AbstractPlayerController player)
	{
		this.target.CancelAnticipation();
		base.OnParry(player);
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x0001B97C File Offset: 0x00019B7C
	public override void OnLevelEnd()
	{
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000B81B8 File Offset: 0x000B63B8
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
		if (this.bouncingOffscreen)
		{
			this.speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
		}
		base.transform.Translate(this.speed * CupheadTime.FixedDelta);
		if (this.bouncingOffscreen)
		{
			if (!this.splashed && base.transform.position.y < this.target.main.splashHandler.transform.position.y)
			{
				this.target.main.splashHandler.SplashIn(base.transform.position.x);
				this.speed *= 0.5f;
				this.gravity *= 0.5f;
				this.animHelper.Speed = 0.5f;
				this.splashed = true;
			}
			if (base.transform.position.y < -560f)
			{
				this.Recycle<OldManLevelGnomeProjectile>();
			}
			if (this.splashed)
			{
				this.underwaterSprite.color = new Color(1f, 1f, 1f, (1f - Mathf.InverseLerp(this.target.main.splashHandler.transform.position.y, this.target.main.splashHandler.transform.position.y - 140f, base.transform.position.y)) * 0.5f);
			}
		}
		if (this.spawnParryable && base.transform.position.y < this.target.transform.position.y + 200f && !this.playedAnticipationSound)
		{
			this.SFX_PreBoneHit();
			this.playedAnticipationSound = true;
		}
		if (base.transform.position.y < this.target.transform.position.y + ((!this.spawnParryable) ? 200f : 50f) && !this.triedHit)
		{
			this.HitTarget();
		}
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x0001B97E File Offset: 0x00019B7E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x000B8470 File Offset: 0x000B6670
	public void HitTarget()
	{
		this.triedHit = true;
		if (this.target.isActivated)
		{
			foreach (AbstractPlayerController abstractPlayerController in this.target.GetComponentsInChildren<AbstractPlayerController>())
			{
				if (!(abstractPlayerController == null))
				{
					abstractPlayerController.transform.parent = null;
				}
			}
			this.target.DeactivatePlatform(this.spawnParryable);
			this.IsFlying = false;
			if (this.spawnParryable)
			{
				this.bouncingOffscreen = true;
				this.speed.y = -this.speed.y * this.bounceModifier;
				this.speed.x = this.speed.x * 2f;
				base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
				base.GetComponent<Collider2D>().enabled = false;
			}
			else
			{
				base.StartCoroutine(this.wait_for_eat());
			}
		}
		else
		{
			this.bouncingOffscreen = true;
		}
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x000B8588 File Offset: 0x000B6788
	public IEnumerator wait_for_eat()
	{
		Animator anim = this.target.GetComponent<Animator>();
		yield return anim.WaitForAnimationToStart(this, "Eat", false);
		while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.18965517f)
		{
			yield return null;
		}
		base.GetComponent<Collider2D>().enabled = false;
		this.Recycle<OldManLevelGnomeProjectile>();
		yield break;
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x0001B99C File Offset: 0x00019B9C
	public void SFX_PreBoneHit()
	{
		AudioManager.Play("sfx_dlc_omm_p3_dinobells_prebonehit");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_prebonehit");
	}

	// Token: 0x04001A9C RID: 6812
	public const float OFFSET_TO_HIT_BONE = 50f;

	// Token: 0x04001A9D RID: 6813
	public const float OFFSET_TO_PLAY_ANTICIPATION_SOUND = 200f;

	// Token: 0x04001A9E RID: 6814
	public const float OFFSET_TO_HIT_LEG = 200f;

	// Token: 0x04001AA0 RID: 6816
	public Vector3 speed;

	// Token: 0x04001AA1 RID: 6817
	public float gravity;

	// Token: 0x04001AA2 RID: 6818
	public bool spawnParryable;

	// Token: 0x04001AA3 RID: 6819
	public OldManLevelStomachPlatform target;

	// Token: 0x04001AA4 RID: 6820
	public bool bouncingOffscreen;

	// Token: 0x04001AA5 RID: 6821
	[SerializeField]
	public float bounceModifier = 0.5f;

	// Token: 0x04001AA6 RID: 6822
	[SerializeField]
	public SpriteRenderer underwaterSprite;

	// Token: 0x04001AA7 RID: 6823
	public bool triedHit;

	// Token: 0x04001AA8 RID: 6824
	public bool splashed;

	// Token: 0x04001AA9 RID: 6825
	public AnimationHelper animHelper;

	// Token: 0x04001AAA RID: 6826
	public bool playedAnticipationSound;
}
