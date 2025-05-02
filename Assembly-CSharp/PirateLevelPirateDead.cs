using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class PirateLevelPirateDead : AbstractMonoBehaviour
{
	// Token: 0x060021F9 RID: 8697 RVA: 0x0001D126 File Offset: 0x0001B326
	public override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x0001D13A File Offset: 0x0001B33A
	public void Go(float delay, float speed)
	{
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.go_cr(delay, speed));
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x0001D157 File Offset: 0x0001B357
	public void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060021FC RID: 8700 RVA: 0x000BBD9C File Offset: 0x000B9F9C
	public IEnumerator go_cr(float delay, float time)
	{
		float startY = base.transform.position.y;
		bool splash = false;
		yield return CupheadTime.WaitForSeconds(this, delay);
		float t = 0f;
		while (t < time)
		{
			float y = EaseUtils.Ease(EaseUtils.EaseType.linear, startY, -250f, t / time);
			base.transform.SetLocalPosition(null, new float?(y), null);
			if (!splash && base.transform.position.y <= -25f)
			{
				splash = true;
				this.splashPrefab.Create(base.transform.position + new Vector3(0f, 20f, 0f));
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(null, new float?(-250f), null);
		this.End();
		yield break;
	}

	// Token: 0x060021FD RID: 8701 RVA: 0x0001D16A File Offset: 0x0001B36A
	public void OnDestroy()
	{
		this.splashPrefab = null;
	}

	// Token: 0x04001BF7 RID: 7159
	public const float END_Y = -250f;

	// Token: 0x04001BF8 RID: 7160
	public const float SPLASH_Y = -25f;

	// Token: 0x04001BF9 RID: 7161
	[SerializeField]
	public Effect splashPrefab;
}
