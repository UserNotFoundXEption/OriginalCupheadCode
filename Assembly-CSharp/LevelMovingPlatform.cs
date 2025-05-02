using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class LevelMovingPlatform : LevelPlatform
{
	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0000950B File Offset: 0x0000770B
	public virtual EaseUtils.EaseType Ease
	{
		get
		{
			return EaseUtils.EaseType.linear;
		}
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0007B28C File Offset: 0x0007948C
	public override void Awake()
	{
		base.Awake();
		this.startPos = base.transform.position;
		this.endPos = this.startPos + this.end;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x0007B2DC File Offset: 0x000794DC
	public IEnumerator move_cr()
	{
		for (;;)
		{
			yield return base.StartCoroutine(this.goTo_cr(this.startPos, this.endPos));
			yield return new WaitForSeconds(1f);
			yield return base.StartCoroutine(this.goTo_cr(this.endPos, this.startPos));
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x0007B2F8 File Offset: 0x000794F8
	public IEnumerator goTo_cr(Vector3 start, Vector3 end)
	{
		float t = 0f;
		base.transform.position = start;
		while (t < this.time)
		{
			float val = t / this.time;
			Vector3 pos = base.transform.position;
			pos.x = EaseUtils.Ease(this.Ease, start.x, end.x, val);
			pos.y = EaseUtils.Ease(this.Ease, start.y, end.y, val);
			base.transform.position = pos;
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.position = end;
		yield break;
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x0007B324 File Offset: 0x00079524
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.magenta;
		if (Application.isPlaying)
		{
			Gizmos.DrawLine(this.startPos, this.endPos);
			Gizmos.DrawWireSphere(base.transform.position, 5f);
		}
		else
		{
			Gizmos.DrawLine(base.baseTransform.position, base.baseTransform.position + this.end);
		}
	}

	// Token: 0x04000831 RID: 2097
	[SerializeField]
	public float time;

	// Token: 0x04000832 RID: 2098
	[SerializeField]
	public Vector2 end;

	// Token: 0x04000833 RID: 2099
	public Vector3 startPos;

	// Token: 0x04000834 RID: 2100
	public Vector3 endPos;
}
