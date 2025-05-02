using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class ForestPlatformingLevelChomper : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002C04 RID: 11268 RVA: 0x000D8FF8 File Offset: 0x000D71F8
	public override void OnStart()
	{
		this.startY = base.transform.position.y;
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x00024DFF File Offset: 0x00022FFF
	public void StartAttacking()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x000D9020 File Offset: 0x000D7220
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.initialDelay.RandomFloat());
		float timeToApex = this.speed / this.gravityUp;
		float upAnimTime = timeToApex - 0.333333f;
		float normalizedExtraTime = upAnimTime / 0.333333f % 1f;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				AudioManager.Play("level_chomper_up");
				this.emitAudioFromObject.Add("level_chomper_up");
			}
			base.animator.Play("Up", 0, 1f - normalizedExtraTime);
			base.StartCoroutine(this.move_cr());
			yield return CupheadTime.WaitForSeconds(this, upAnimTime - 0.1666665f);
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				AudioManager.Play("level_chomper_bite");
				this.emitAudioFromObject.Add("level_chomper_bite");
			}
			base.animator.SetTrigger("Bite");
			while (this.moving)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.mainDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x000D903C File Offset: 0x000D723C
	public IEnumerator move_cr()
	{
		float timeToApex = this.speed / this.gravityUp;
		float t = 0f;
		this.moving = true;
		while (t < timeToApex)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(null, new float?(this.startY + this.speed * t - 0.5f * this.gravityUp * t * t), null);
			yield return new WaitForFixedUpdate();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		float apexY = base.transform.position.y;
		t = 0f;
		while (t < timeToApex)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(null, new float?(apexY - 0.5f * this.gravityDown * t * t), null);
			yield return new WaitForFixedUpdate();
		}
		this.moving = false;
		yield break;
	}

	// Token: 0x0400246F RID: 9327
	[SerializeField]
	public float speed = 1000f;

	// Token: 0x04002470 RID: 9328
	[SerializeField]
	public float gravityUp = 1600f;

	// Token: 0x04002471 RID: 9329
	[SerializeField]
	public float gravityDown = 2400f;

	// Token: 0x04002472 RID: 9330
	[SerializeField]
	public MinMax initialDelay = new MinMax(0f, 0.5f);

	// Token: 0x04002473 RID: 9331
	[SerializeField]
	public MinMax mainDelay = new MinMax(1f, 3f);

	// Token: 0x04002474 RID: 9332
	public const float UP_ANIM_LENGTH = 0.333333f;

	// Token: 0x04002475 RID: 9333
	public const float BITE_ANIM_TIME_TO_APEX = 0.333333f;

	// Token: 0x04002476 RID: 9334
	public const float FREEZE_TIME = 0.0416666679f;

	// Token: 0x04002477 RID: 9335
	public const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x04002478 RID: 9336
	public float startY;

	// Token: 0x04002479 RID: 9337
	public bool moving;
}
