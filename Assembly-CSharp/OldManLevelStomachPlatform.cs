using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class OldManLevelStomachPlatform : LevelPlatform
{
	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x0600214C RID: 8524 RVA: 0x0001C6AF File Offset: 0x0001A8AF
	// (set) Token: 0x0600214D RID: 8525 RVA: 0x0001C6B7 File Offset: 0x0001A8B7
	public bool isActivated { get; set; }

	// Token: 0x0600214E RID: 8526 RVA: 0x0001C6C0 File Offset: 0x0001A8C0
	public void Start()
	{
		this.isActivated = true;
		base.animator.Play("Idle", 0, Random.Range(0f, 1f));
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000BA37C File Offset: 0x000B857C
	public void aniEvent_SpawnParryable()
	{
		this.main.SpawnParryable(base.transform.position + this.tongueOffset);
		this.splashAnimator.Play("OpenSplash");
		this.splashAnimator.Update(0f);
		this.SFX_BellLoop();
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x000BA3D0 File Offset: 0x000B85D0
	public void FlipX()
	{
		foreach (SpriteRenderer spriteRenderer in this.rend)
		{
			spriteRenderer.flipX = true;
		}
		this.boxColl.offset = new Vector2(-this.boxColl.offset.x, this.boxColl.offset.y);
		this.tongueOffset.x = -this.tongueOffset.x;
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x0001C6E9 File Offset: 0x0001A8E9
	public void Anticipation()
	{
		if (this.isActivated)
		{
			this.isTargeted = true;
			base.animator.SetTrigger("Anticipation");
		}
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x000BA454 File Offset: 0x000B8654
	public void CancelAnticipation()
	{
		this.isTargeted = false;
		if (this.isActivated)
		{
			base.animator.Play("Idle");
		}
		else
		{
			this.isActivated = true;
			base.animator.SetBool("IsEating", false);
			base.animator.Play("ReverseEat", 0, 1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			base.animator.Play("Eat_Ripple_End", 1, 0f);
			if (this.bubbleCoroutine != null)
			{
				base.StopCoroutine(this.bubbleCoroutine);
			}
		}
	}

	// Token: 0x06002153 RID: 8531 RVA: 0x0001C70D File Offset: 0x0001A90D
	public Vector3 GetTonguePos()
	{
		return base.transform.position + this.tongueOffset;
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x000BA4F8 File Offset: 0x000B86F8
	public void DeactivatePlatform(bool spawnsParryable)
	{
		this.spawnsParryable = spawnsParryable;
		string text = (!spawnsParryable) ? "IsEating" : "IsBell";
		if (spawnsParryable)
		{
			this.sparkAnimator.Play("Spark");
			this.SFX_BonkHead();
		}
		else
		{
			this.SFX_Chomp();
		}
		base.animator.SetBool(text, true);
		this.isActivated = false;
		this.isTargeted = false;
		if (!spawnsParryable)
		{
			this.bubbleCoroutine = base.StartCoroutine(this.bubble_cr());
		}
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000BA57C File Offset: 0x000B877C
	public IEnumerator bubble_cr()
	{
		float noseTimer = this.noseBubbleRange.RandomFloat();
		float mouthTimer = this.mouthBubbleRange.RandomFloat();
		for (;;)
		{
			noseTimer -= CupheadTime.Delta;
			mouthTimer -= CupheadTime.Delta;
			if (noseTimer <= 0f)
			{
				this.noseBubble.Play("Bubble", 0, 0f);
				this.noseBubbleRend.flipX = Rand.Bool();
				noseTimer += this.noseBubbleRange.RandomFloat();
			}
			if (mouthTimer <= 0f)
			{
				this.mouthBubble.Play("Bubble", 0, 0f);
				this.mouthBubbleRend.flipX = Rand.Bool();
				mouthTimer += this.mouthBubbleRange.RandomFloat();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x0001C725 File Offset: 0x0001A925
	public void ActivatePlatform()
	{
		base.StartCoroutine(this.activate_cr());
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000BA598 File Offset: 0x000B8798
	public IEnumerator activate_cr()
	{
		if (this.bubbleCoroutine != null)
		{
			base.StopCoroutine(this.bubbleCoroutine);
		}
		this.mouthBubble.Play("None");
		this.noseBubble.Play("None");
		if (base.animator.GetBool("IsBell"))
		{
			base.animator.SetBool("IsBell", false);
			base.animator.Play("Bell_End");
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.2f));
			base.animator.SetBool("IsEating", false);
		}
		this.isActivated = true;
		this.spawnsParryable = false;
		yield break;
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x0001C734 File Offset: 0x0001A934
	public void SFX_Chomp()
	{
		AudioManager.Play("sfx_dlc_omm_p3_dino_chomp");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dino_chomp");
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x0001C750 File Offset: 0x0001A950
	public void SFX_BonkHead()
	{
		AudioManager.Play("sfx_dlc_omm_p3_dinobells_bonkhead");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_bonkhead");
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x0001C76C File Offset: 0x0001A96C
	public void SFX_BellLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_omm_p3_dinobells_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_dinobells_loop");
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x0001C788 File Offset: 0x0001A988
	public void AniEvent_SFX_BellLoopEnd()
	{
		AudioManager.Stop("sfx_dlc_omm_p3_dinobells_loop");
	}

	// Token: 0x04001B74 RID: 7028
	[SerializeField]
	public SpriteRenderer[] rend;

	// Token: 0x04001B75 RID: 7029
	[SerializeField]
	public BoxCollider2D boxColl;

	// Token: 0x04001B77 RID: 7031
	public bool isTargeted;

	// Token: 0x04001B78 RID: 7032
	public bool spawnsParryable;

	// Token: 0x04001B79 RID: 7033
	public Animator sparkAnimator;

	// Token: 0x04001B7A RID: 7034
	public Vector3 tongueOffset = new Vector3(20f, 30f);

	// Token: 0x04001B7B RID: 7035
	public OldManLevelGnomeLeader main;

	// Token: 0x04001B7C RID: 7036
	[SerializeField]
	public Animator splashAnimator;

	// Token: 0x04001B7D RID: 7037
	[SerializeField]
	public Animator mouthBubble;

	// Token: 0x04001B7E RID: 7038
	[SerializeField]
	public Animator noseBubble;

	// Token: 0x04001B7F RID: 7039
	[SerializeField]
	public SpriteRenderer mouthBubbleRend;

	// Token: 0x04001B80 RID: 7040
	[SerializeField]
	public SpriteRenderer noseBubbleRend;

	// Token: 0x04001B81 RID: 7041
	[SerializeField]
	public MinMax noseBubbleRange = new MinMax(0.5833333f, 1f);

	// Token: 0x04001B82 RID: 7042
	[SerializeField]
	public MinMax mouthBubbleRange = new MinMax(1f, 1.58333337f);

	// Token: 0x04001B83 RID: 7043
	public Coroutine bubbleCoroutine;
}
