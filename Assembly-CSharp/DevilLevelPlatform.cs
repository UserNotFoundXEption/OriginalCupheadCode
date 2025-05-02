using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class DevilLevelPlatform : AbstractPausableComponent
{
	// Token: 0x060014A8 RID: 5288 RVA: 0x0009A598 File Offset: 0x00098798
	public override void Awake()
	{
		base.Awake();
		base.animator.Play("Platform_" + this.type.ToString());
		this.baseY = base.transform.position.y;
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x0001182C File Offset: 0x0000FA2C
	public void Raise(float speed, float height, float delay)
	{
		this.state = DevilLevelPlatform.State.Raising;
		base.StartCoroutine(this.raise_cr(speed, height, delay));
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0009A5EC File Offset: 0x000987EC
	public IEnumerator raise_cr(float speed, float height, float delay)
	{
		float t = 0f;
		float moveTime = height / speed;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.baseY, this.baseY + height, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(this.baseY + height), null);
		yield return CupheadTime.WaitForSeconds(this, delay);
		t = 0f;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.baseY + height, this.baseY, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(this.baseY), null);
		this.state = DevilLevelPlatform.State.Idle;
		yield break;
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x00011845 File Offset: 0x0000FA45
	public void Lower(float speed)
	{
		this.state = DevilLevelPlatform.State.Lowering;
		base.StartCoroutine(this.lower_cr(speed));
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x0009A61C File Offset: 0x0009881C
	public IEnumerator lower_cr(float speed)
	{
		float t = 0f;
		float moveTime = 300f / speed;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, this.baseY, this.baseY - 300f, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		this.Die();
		yield break;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x0009A640 File Offset: 0x00098840
	public void Die()
	{
		foreach (AbstractPlayerController abstractPlayerController in base.GetComponentsInChildren<AbstractPlayerController>())
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.transform.parent = null;
			}
		}
		base.gameObject.SetActive(false);
		this.state = DevilLevelPlatform.State.Dead;
	}

	// Token: 0x040010E8 RID: 4328
	public const float LOWER_DISTANCE = 300f;

	// Token: 0x040010E9 RID: 4329
	public DevilLevelPlatform.PlatformType type;

	// Token: 0x040010EA RID: 4330
	public DevilLevelPlatform.State state;

	// Token: 0x040010EB RID: 4331
	public float baseY;

	// Token: 0x02000B3A RID: 2874
	public enum PlatformType
	{
		// Token: 0x0400524B RID: 21067
		A,
		// Token: 0x0400524C RID: 21068
		B,
		// Token: 0x0400524D RID: 21069
		C,
		// Token: 0x0400524E RID: 21070
		D,
		// Token: 0x0400524F RID: 21071
		E
	}

	// Token: 0x02000B3B RID: 2875
	public enum State
	{
		// Token: 0x04005251 RID: 21073
		Idle,
		// Token: 0x04005252 RID: 21074
		Raising,
		// Token: 0x04005253 RID: 21075
		Lowering,
		// Token: 0x04005254 RID: 21076
		Dead
	}
}
