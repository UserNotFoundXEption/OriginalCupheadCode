using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class DragonLevelFireMarcher : AbstractCollidableObject
{
	// Token: 0x06001826 RID: 6182 RVA: 0x00014A98 File Offset: 0x00012C98
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		if (this.canJump)
		{
			base.animator.Play("Idle", 0, Random.Range(0f, 1f));
		}
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x00014AD6 File Offset: 0x00012CD6
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x00014AEE File Offset: 0x00012CEE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x000A2FEC File Offset: 0x000A11EC
	public DragonLevelFireMarcher Create(Transform root, LevelProperties.Dragon.FireMarchers properties)
	{
		DragonLevelFireMarcher dragonLevelFireMarcher = this.InstantiatePrefab<DragonLevelFireMarcher>();
		dragonLevelFireMarcher.transform.parent = root;
		dragonLevelFireMarcher.transform.ResetLocalPosition();
		dragonLevelFireMarcher.properties = properties;
		dragonLevelFireMarcher.StartCoroutine(dragonLevelFireMarcher.move_cr());
		return dragonLevelFireMarcher;
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x000A302C File Offset: 0x000A122C
	public IEnumerator move_cr()
	{
		float initialYOffset = Mathf.Sin(0.9773844f) * 5f;
		float timeSlowed = 0f;
		while (base.transform.position.x < (float)(Level.Current.Right + 100))
		{
			float speed = this.properties.moveSpeed;
			if (this.slowing)
			{
				timeSlowed += CupheadTime.Delta;
				if (timeSlowed > 0.25f)
				{
					yield break;
				}
				speed = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, speed, 0f, timeSlowed / 0.25f);
			}
			float x = base.transform.localPosition.x + speed * CupheadTime.Delta;
			float y = Mathf.Sin((70f + base.transform.localPosition.x) * 2f * 3.14159274f / 450f) * 5f - initialYOffset;
			y += Mathf.Min(1f, x / 300f) * -23f;
			if (x < this.squeezeDistance)
			{
				base.transform.SetScale(new float?(x / this.squeezeDistance), null, null);
			}
			else
			{
				base.transform.SetScale(new float?(1f), null, null);
			}
			base.transform.SetLocalPosition(new float?(x), new float?(y), null);
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x000A3048 File Offset: 0x000A1248
	public bool CanJump()
	{
		if (!this.canJump || this.wantsToJump)
		{
			return false;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(0);
		float num = base.transform.localPosition.x + (1f - currentAnimatorStateInfo.normalizedTime % 1f) * currentAnimatorStateInfo.length * this.properties.moveSpeed;
		return num > this.properties.jumpX.min && num < this.properties.jumpX.max;
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x00014B17 File Offset: 0x00012D17
	public void StartJump(AbstractPlayerController targetPlayer)
	{
		this.targetPlayer = targetPlayer;
		this.wantsToJump = true;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x000A30E4 File Offset: 0x000A12E4
	public IEnumerator jump_cr()
	{
		base.animator.SetTrigger("StartJump");
		yield return base.animator.WaitForAnimationToStart(this, "Crouch_Start", false);
		AudioManager.Play("level_dragon_fire_marcher_b_couch_start");
		this.emitAudioFromObject.Add("level_dragon_fire_marcher_b_couch_start");
		this.slowing = true;
		yield return base.animator.WaitForAnimationToStart(this, "Crouch_Loop", false);
		Vector2 targetPos = this.targetPlayer.center;
		if (targetPos.x < base.transform.position.x)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		float bestDistance = float.MaxValue;
		Vector2 bestLaunchVelocity = Vector2.zero;
		Vector2 relativeTargetPos = targetPos - base.transform.position;
		relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x);
		for (float num = 0f; num < 1f; num += 0.01f)
		{
			float floatAt = this.properties.jumpAngle.GetFloatAt(num);
			float floatAt2 = this.properties.jumpSpeed.GetFloatAt(num);
			Vector2 vector = MathUtils.AngleToDirection(floatAt) * floatAt2;
			float num2 = relativeTargetPos.x / vector.x;
			float num3 = vector.y * num2 - 0.5f * this.properties.gravity * num2 * num2;
			float num4 = Mathf.Abs(relativeTargetPos.y - num3);
			if (num4 < bestDistance)
			{
				bestDistance = num4;
				bestLaunchVelocity = vector;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.crouchTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Jump_Start", false);
		AudioManager.Play("level_dragon_fire_marcher_b_jump_start");
		this.emitAudioFromObject.Add("level_dragon_fire_marcher_b_jump_start");
		Vector2 velocity = bestLaunchVelocity;
		velocity.x *= base.transform.localScale.x;
		float t = 0f;
		Vector2 initialPos = base.transform.localPosition;
		while (base.transform.position.y > -400f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetLocalPosition(new float?(initialPos.x + t * velocity.x), new float?(initialPos.y + t * velocity.y - 0.5f * this.properties.gravity * t * t), null);
			yield return new WaitForFixedUpdate();
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400138C RID: 5004
	public const float sinOffset = 70f;

	// Token: 0x0400138D RID: 5005
	public const float sinPeriod = 450f;

	// Token: 0x0400138E RID: 5006
	public const float sinHeight = 5f;

	// Token: 0x0400138F RID: 5007
	public const float linearOffset = -23f;

	// Token: 0x04001390 RID: 5008
	public const float linearOffsetDistance = 300f;

	// Token: 0x04001391 RID: 5009
	public const float minJumpX = 50f;

	// Token: 0x04001392 RID: 5010
	public const float maxJumpX = 590f;

	// Token: 0x04001393 RID: 5011
	public DamageDealer damageDealer;

	// Token: 0x04001394 RID: 5012
	public LevelProperties.Dragon.FireMarchers properties;

	// Token: 0x04001395 RID: 5013
	[SerializeField]
	public float squeezeDistance;

	// Token: 0x04001396 RID: 5014
	[SerializeField]
	public bool canJump;

	// Token: 0x04001397 RID: 5015
	public bool wantsToJump;

	// Token: 0x04001398 RID: 5016
	public bool slowing;

	// Token: 0x04001399 RID: 5017
	public AbstractPlayerController targetPlayer;
}
