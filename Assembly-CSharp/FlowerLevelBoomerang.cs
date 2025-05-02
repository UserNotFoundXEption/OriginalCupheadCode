using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class FlowerLevelBoomerang : BasicProjectile
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06001886 RID: 6278 RVA: 0x00014F24 File Offset: 0x00013124
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x000A3EB8 File Offset: 0x000A20B8
	public void OnBoomerangStart(float delay)
	{
		this.BoomerangNumberSFX++;
		if (this.BoomerangNumberSFX == 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_1", 1f, 1f);
			AudioManager.PlayLoop("flower_boomerang_1");
			this.emitAudioFromObject.Add("flower_boomerang_1");
		}
		else if (this.BoomerangNumberSFX != 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_2", 1f, 1f);
			AudioManager.PlayLoop("flower_boomerang_2");
			this.emitAudioFromObject.Add("flower_boomerang_2");
		}
		this.offScreenDelay = delay;
		this.returnXPosition = (float)(Level.Current.Left - 100);
		this.endXPosition = (float)(Level.Current.Right + 500);
		base.StartCoroutine(this.boomerangStart_cr());
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x000A3F8C File Offset: 0x000A218C
	public IEnumerator boomerangStart_cr()
	{
		base.transform.GetChild(0).transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		while (base.transform.position.x > this.returnXPosition)
		{
			yield return null;
		}
		this.move = false;
		yield return CupheadTime.WaitForSeconds(this, this.offScreenDelay);
		this.OnBoomerangReturn();
		yield break;
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x000A3FA8 File Offset: 0x000A21A8
	public void OnBoomerangReturn()
	{
		this.Speed = -this.Speed;
		this.move = true;
		base.transform.position = new Vector3(base.transform.position.x, (float)(Level.Current.Ground + Level.Current.Height / 6), 0f);
		base.StartCoroutine(this.boomerangReturn_cr());
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000A4018 File Offset: 0x000A2218
	public IEnumerator boomerangReturn_cr()
	{
		base.transform.GetChild(0).transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
		while (base.transform.position.x < this.endXPosition)
		{
			yield return null;
		}
		if (this.BoomerangNumberSFX == 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_1", 0f, 3f);
			AudioManager.FadeSFXVolume("flower_boomerang_2", 0f, 3f);
		}
		else if (this.BoomerangNumberSFX != 1)
		{
			AudioManager.FadeSFXVolume("flower_boomerang_1", 0f, 3f);
			AudioManager.FadeSFXVolume("flower_boomerang_2", 0f, 3f);
		}
		this.BoomerangNumberSFX--;
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x00014F27 File Offset: 0x00013127
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x00014F50 File Offset: 0x00013150
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x00014F58 File Offset: 0x00013158
	public override void OnLevelEnd()
	{
		AudioManager.Stop("flower_boomerang_1");
		AudioManager.Stop("flower_boomerang_2");
		base.OnLevelEnd();
	}

	// Token: 0x040013E4 RID: 5092
	public float returnXPosition;

	// Token: 0x040013E5 RID: 5093
	public float endXPosition;

	// Token: 0x040013E6 RID: 5094
	public float offScreenDelay;

	// Token: 0x040013E7 RID: 5095
	public int BoomerangNumberSFX;
}
