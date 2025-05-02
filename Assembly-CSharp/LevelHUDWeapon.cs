using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class LevelHUDWeapon : AbstractMonoBehaviour
{
	// Token: 0x06000C89 RID: 3209 RVA: 0x00083E50 File Offset: 0x00082050
	public LevelHUDWeapon Create(Transform parent, Weapon weapon)
	{
		LevelHUDWeapon levelHUDWeapon = this.InstantiatePrefab<LevelHUDWeapon>();
		levelHUDWeapon.transform.SetParent(parent, false);
		levelHUDWeapon.SetIcon(weapon);
		return levelHUDWeapon;
	}

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06000C8A RID: 3210 RVA: 0x00083E7C File Offset: 0x0008207C
	// (remove) Token: 0x06000C8B RID: 3211 RVA: 0x00083EB0 File Offset: 0x000820B0
	public static event Action OnAwakeEvent;

	// Token: 0x06000C8C RID: 3212 RVA: 0x00083EE4 File Offset: 0x000820E4
	public override void Awake()
	{
		base.Awake();
		this.startY = -80f;
		this.endY = -10f;
		base.transform.ResetLocalTransforms();
		base.transform.SetLocalPosition(new float?(0f), new float?(this.startY), new float?(0f));
		this.inCoroutine = base.StartCoroutine(this.go_cr());
		if (LevelHUDWeapon.OnAwakeEvent != null)
		{
			LevelHUDWeapon.OnAwakeEvent();
		}
		LevelHUDWeapon.OnAwakeEvent = null;
		LevelHUDWeapon.OnAwakeEvent += this.Out;
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x0000AF6D File Offset: 0x0000916D
	public void OnDestroy()
	{
		LevelHUDWeapon.OnAwakeEvent -= this.Out;
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0000AF80 File Offset: 0x00009180
	public void Out()
	{
		if (!this.ending)
		{
			if (this.inCoroutine != null)
			{
				base.StopCoroutine(this.inCoroutine);
			}
			base.StartCoroutine(this.out_cr());
		}
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x0000AFB1 File Offset: 0x000091B1
	public void SetIcon(Weapon weapon)
	{
		base.animator.Play(weapon.ToString());
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00083F80 File Offset: 0x00082180
	public IEnumerator go_cr()
	{
		yield return base.TweenLocalPositionY(this.startY, this.endY, 0.2f, EaseUtils.EaseType.easeOutSine);
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.ending = true;
		yield return base.TweenLocalPositionY(this.endY, this.startY, 0.2f, EaseUtils.EaseType.easeInSine);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x00083F9C File Offset: 0x0008219C
	public IEnumerator out_cr()
	{
		this.ending = true;
		yield return base.TweenLocalPositionY(this.endY, this.startY, 0.05f, EaseUtils.EaseType.easeInSine);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040009FA RID: 2554
	public const float TIME_IN = 0.2f;

	// Token: 0x040009FB RID: 2555
	public const float TIME_DELAY = 2f;

	// Token: 0x040009FC RID: 2556
	public const float TIME_OUT = 0.2f;

	// Token: 0x040009FD RID: 2557
	public const float TIME_OUT_FAST = 0.05f;

	// Token: 0x040009FE RID: 2558
	public bool ending;

	// Token: 0x040009FF RID: 2559
	public Coroutine inCoroutine;

	// Token: 0x04000A00 RID: 2560
	public float startY;

	// Token: 0x04000A01 RID: 2561
	public float endY;
}
