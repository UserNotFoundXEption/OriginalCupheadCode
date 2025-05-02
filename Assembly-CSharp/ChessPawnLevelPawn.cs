using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018A RID: 394
public class ChessPawnLevelPawn : AbstractProjectile
{
	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060012B9 RID: 4793 RVA: 0x0000FCBA File Offset: 0x0000DEBA
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060012BA RID: 4794 RVA: 0x0000FCC1 File Offset: 0x0000DEC1
	// (set) Token: 0x060012BB RID: 4795 RVA: 0x0000FCC9 File Offset: 0x0000DEC9
	public float speed { get; set; }

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060012BC RID: 4796 RVA: 0x0000FCD2 File Offset: 0x0000DED2
	// (set) Token: 0x060012BD RID: 4797 RVA: 0x0000FCDA File Offset: 0x0000DEDA
	public bool inUse { get; set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060012BE RID: 4798 RVA: 0x0000FCE3 File Offset: 0x0000DEE3
	// (set) Token: 0x060012BF RID: 4799 RVA: 0x0000FCEB File Offset: 0x0000DEEB
	public int currentIndex { get; set; }

	// Token: 0x060012C0 RID: 4800 RVA: 0x00095C6C File Offset: 0x00093E6C
	public ChessPawnLevelPawn Init(ChessPawnLevel level)
	{
		ChessPawnLevelPawn chessPawnLevelPawn = Object.Instantiate<ChessPawnLevelPawn>(this, Camera.main.transform.position + Vector3.up * 2000f, Quaternion.identity);
		chessPawnLevelPawn.level = level;
		return chessPawnLevelPawn;
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.level = null;
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x0000FD03 File Offset: 0x0000DF03
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x0000FD21 File Offset: 0x0000DF21
	public override void Die()
	{
		base.Die();
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x0000FD29 File Offset: 0x0000DF29
	public void SetIndex(int i)
	{
		this.currentIndex = i;
		if (i >= 0)
		{
			this.lastIndex = i;
		}
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0000FD40 File Offset: 0x0000DF40
	public override void OnDieDistance()
	{
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x0000FD42 File Offset: 0x0000DF42
	public override void OnDieLifetime()
	{
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x0000FD44 File Offset: 0x0000DF44
	public override void OnLevelEnd()
	{
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x00095CB0 File Offset: 0x00093EB0
	public override void OnParry(AbstractPlayerController player)
	{
		this.parryCount++;
		if (PlayerManager.BothPlayersActive() && this.parryCount < 2)
		{
			return;
		}
		this.SetParryable(false);
		base.StartCoroutine(this.disable_collision_cr());
		if (this.state == ChessPawnLevelPawn.State.Run)
		{
			base.animator.SetTrigger("Parry");
		}
		this.parriedHead.CreatePart(base.transform.position + Vector3.up * 100f);
		this.headRenderer.enabled = false;
		base.StartCoroutine(this.SFX_KOG_PAWN_PawnParry_cr());
		this.level.TakeDamage();
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x0000FD46 File Offset: 0x0000DF46
	public void StartIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x00095D64 File Offset: 0x00093F64
	public IEnumerator intro_cr()
	{
		this.inUse = true;
		yield return base.StartCoroutine(this.drop_cr(true));
		this.inUse = false;
		yield break;
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x00095D80 File Offset: 0x00093F80
	public IEnumerator drop_cr(bool isIntro)
	{
		AnimationHelper animationHelper = base.GetComponent<AnimationHelper>();
		Vector3 targetPosition = this.level.GetPosition(this.currentIndex);
		base.animator.Play("IntroStart");
		base.animator.SetInteger("Intro", (!isIntro) ? 0 : (this.currentIndex % 2 + 1));
		targetPosition.z = (float)(this.currentIndex % 2) * 0.0001f;
		base.animator.Update(0f);
		animationHelper.Speed = 0f;
		this.bodyRenderer.sortingOrder = 0;
		this.headRenderer.sortingOrder = 1;
		float t = 0f;
		Vector3 dropPosition = targetPosition + ChessPawnLevelPawn.DropPositionOffset;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < 1f)
		{
			base.transform.position = Vector3.Lerp(dropPosition, targetPosition, t);
			t += CupheadTime.FixedDelta * 8f;
			yield return wait;
		}
		animationHelper.Speed = 1f;
		base.transform.position = targetPosition;
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		yield break;
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0000FD55 File Offset: 0x0000DF55
	public void Attack(float warningTime, float horiztonalMovement, float dropSpeed, float runDelay, float runSpeed, float returnSpeed)
	{
		base.StartCoroutine(this.attack_cr(warningTime, horiztonalMovement, dropSpeed, runDelay, runSpeed, returnSpeed));
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x00095DA4 File Offset: 0x00093FA4
	public IEnumerator attack_cr(float warningTime, float horizontalMovement, float dropSpeed, float runDelay, float runSpeed, float returnDelay)
	{
		this.inUse = true;
		this.initialPosition = base.transform.position;
		YieldInstruction wait = new WaitForFixedUpdate();
		base.animator.SetTrigger("JumpWarning");
		yield return CupheadTime.WaitForSeconds(this, warningTime);
		this.currentIndex = -1;
		this.state = ChessPawnLevelPawn.State.Jump;
		this.collider.enabled = true;
		base.animator.SetTrigger("Jump");
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		if (horizontalMovement != 0f)
		{
			base.transform.SetScale(new float?(-Mathf.Sign(horizontalMovement)), null, null);
		}
		Coroutine horizontalMovementCoroutine = base.StartCoroutine(this.horizontalMovement_cr(horizontalMovement, dropSpeed));
		while (!this.beginFall)
		{
			yield return null;
		}
		this.beginFall = false;
		float t = 0f;
		while (t < 1f)
		{
			Vector3 position = base.transform.position;
			position.y = this.initialPosition.y - 650f + 650f * Mathf.Sin(1.57079637f + t * 3.14159274f / 2f);
			if (position.y < base.transform.position.y)
			{
				this.bodyRenderer.sortingOrder = 20;
				this.headRenderer.sortingOrder = 21;
			}
			base.transform.position = position;
			t += CupheadTime.FixedDelta * dropSpeed;
			yield return wait;
		}
		base.StopCoroutine(horizontalMovementCoroutine);
		base.transform.position = new Vector3(base.transform.position.x, this.initialPosition.y - 650f);
		float testDir = Mathf.Sign(PlayerManager.GetNext().transform.position.x - base.transform.position.x);
		bool quickLand = this.level.ClearToRun(testDir, base.transform.position);
		if (runDelay == 0f && quickLand)
		{
			base.animator.SetInteger("Land", 1);
			base.transform.SetScale(new float?(testDir), null, null);
		}
		else
		{
			base.animator.SetInteger("Land", 2);
			float delay = runDelay - 0.625f;
			yield return CupheadTime.WaitForSeconds(this, runDelay);
			while (!this.level.ClearToRun(testDir, base.transform.position))
			{
				yield return wait;
				testDir = Mathf.Sign(PlayerManager.GetNext().transform.position.x - base.transform.position.x);
			}
			base.animator.SetInteger("Land", 3);
			base.transform.SetScale(new float?(testDir), null, null);
			yield return base.animator.WaitForAnimationToStart(this, "LandLongToRun", false);
		}
		this.state = ChessPawnLevelPawn.State.Run;
		this.speed = runSpeed * testDir;
		while (Mathf.Abs(base.transform.position.x - Camera.main.transform.position.x) < 850f)
		{
			base.transform.position += this.speed * CupheadTime.FixedDelta * Vector3.right;
			yield return wait;
		}
		this.speed = 0f;
		this.state = ChessPawnLevelPawn.State.Idle;
		base.animator.SetInteger("Land", 0);
		this.collider.enabled = false;
		this.currentIndex = this.level.GetReturnIndex();
		yield return base.StartCoroutine(this.drop_cr(false));
		this.inUse = false;
		yield break;
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x00095DE4 File Offset: 0x00093FE4
	public IEnumerator horizontalMovement_cr(float horizontalMovement, float dropSpeed)
	{
		float duration = 1f / dropSpeed;
		duration += 0.458333343f;
		float horizontalSpeed = horizontalMovement / duration;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			Vector3 position = base.transform.position;
			position.x += CupheadTime.FixedDelta * horizontalSpeed;
			base.transform.position = position;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x00095E10 File Offset: 0x00094010
	public void Death()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
		if (this.headRenderer.enabled)
		{
			this.parriedHead.CreatePart(base.transform.position + Vector3.up * 100f);
			this.collider.enabled = false;
		}
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x00095E78 File Offset: 0x00094078
	public IEnumerator death_cr()
	{
		this.collider.enabled = false;
		base.transform.SetScale(new float?(1f), null, null);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, this.initialPosition.x);
		base.GetComponent<AnimationHelper>().Speed = 1f;
		base.animator.Play("DeathTwitch", 0, (float)this.lastIndex * 0.125f);
		Effect smoke = this.deathSmokeEffect.Create(base.transform.position);
		base.StartCoroutine(this.move_smoke_cr(smoke));
		float delay = (float)(this.lastIndex % 4 * 2 + this.lastIndex / 2) * this.deathTwitchDelayFixed + Random.Range(this.deathTwitchDelayRange.minimum, this.deathTwitchDelayRange.maximum);
		yield return CupheadTime.WaitForSeconds(this, delay);
		base.animator.Play("DeathAngel", 0, Random.Range(0f, 1f));
		smoke.animator.Play("Explode");
		this.deathBody.CreatePart(base.transform.position);
		for (;;)
		{
			Vector3 position = base.transform.position;
			position.y += this.deathFloatUpSpeed * CupheadTime.Delta;
			base.transform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x00095E94 File Offset: 0x00094094
	public IEnumerator move_smoke_cr(Effect smoke)
	{
		SpriteRenderer smokeRenderer = smoke.GetComponent<SpriteRenderer>();
		while (smoke != null)
		{
			smoke.transform.position = base.transform.position + MathUtils.AngleToDirection((float)Random.Range(0, 360)) * 50f;
			while (smokeRenderer != null && smokeRenderer.sprite != null)
			{
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x0000FD6D File Offset: 0x0000DF6D
	public void animationEvent_BeginFall()
	{
		this.beginFall = true;
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x00095EB8 File Offset: 0x000940B8
	public IEnumerator disable_collision_cr()
	{
		this.collider.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.noHeadCollider.enabled = true;
		yield break;
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0000FD76 File Offset: 0x0000DF76
	public void AnimationEvent_SFX_KOG_PAWN_PawnLand()
	{
		AudioManager.Play("sfx_dlc_kog_pawn_land");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_land");
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x0000FD92 File Offset: 0x0000DF92
	public void AnimationEvent_SFX_KOG_PAWN_PawnJumpDown()
	{
		AudioManager.Play("sfx_dlc_kog_pawn_jumpdown");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_jumpdown");
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x0000FDAE File Offset: 0x0000DFAE
	public void AnimationEvent_SFX_KOG_PAWN_PawnParryHit()
	{
		AudioManager.Play(string.Empty);
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parryhit");
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x00095ED4 File Offset: 0x000940D4
	public IEnumerator SFX_KOG_PAWN_PawnParry_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		AudioManager.Play("sfx_dlc_kog_pawn_parryhit");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parryhit");
		yield return CupheadTime.WaitForSeconds(this, 0.15f);
		AudioManager.Play("sfx_dlc_kog_pawn_parrywoodbreak");
		this.emitAudioFromObject.Add("sfx_dlc_kog_pawn_parrywoodbreak");
		yield break;
	}

	// Token: 0x04000F07 RID: 3847
	public const float FALL_DISTANCE = 650f;

	// Token: 0x04000F08 RID: 3848
	public static readonly Vector3 DropPositionOffset = new Vector3(0f, 100f);

	// Token: 0x04000F09 RID: 3849
	[SerializeField]
	public Collider2D collider;

	// Token: 0x04000F0A RID: 3850
	[SerializeField]
	public SpriteRenderer bodyRenderer;

	// Token: 0x04000F0B RID: 3851
	[SerializeField]
	public SpriteRenderer headRenderer;

	// Token: 0x04000F0C RID: 3852
	[SerializeField]
	public SpriteDeathParts parriedHead;

	// Token: 0x04000F0D RID: 3853
	[SerializeField]
	public float deathTwitchDelayFixed;

	// Token: 0x04000F0E RID: 3854
	[SerializeField]
	public Rangef deathTwitchDelayRange;

	// Token: 0x04000F0F RID: 3855
	[SerializeField]
	public float deathFloatUpSpeed;

	// Token: 0x04000F10 RID: 3856
	[SerializeField]
	public Effect deathSmokeEffect;

	// Token: 0x04000F11 RID: 3857
	[SerializeField]
	public SpriteDeathParts deathBody;

	// Token: 0x04000F12 RID: 3858
	[SerializeField]
	public BoxCollider2D noHeadCollider;

	// Token: 0x04000F13 RID: 3859
	public ChessPawnLevel level;

	// Token: 0x04000F14 RID: 3860
	public ChessPawnLevelPawn.State state;

	// Token: 0x04000F15 RID: 3861
	public Vector3 initialPosition;

	// Token: 0x04000F16 RID: 3862
	public bool beginFall;

	// Token: 0x04000F17 RID: 3863
	public int parryCount;

	// Token: 0x04000F18 RID: 3864
	public int lastIndex;

	// Token: 0x02000ACA RID: 2762
	public enum State
	{
		// Token: 0x04004F2E RID: 20270
		Idle,
		// Token: 0x04004F2F RID: 20271
		Jump,
		// Token: 0x04004F30 RID: 20272
		Run
	}
}
