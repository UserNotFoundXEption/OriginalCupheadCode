using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class AirplaneLevelBoomerang : AbstractProjectile
{
	// Token: 0x06000D9E RID: 3486 RVA: 0x000879B8 File Offset: 0x00085BB8
	public AirplaneLevelBoomerang Create(Vector2 pos, float speedF, float easeDF, float speedR, float easeDR, float delay, bool onLeft, int id)
	{
		AirplaneLevelBoomerang airplaneLevelBoomerang = base.Create() as AirplaneLevelBoomerang;
		airplaneLevelBoomerang.transform.position = pos;
		airplaneLevelBoomerang.DamagesType.OnlyPlayer();
		airplaneLevelBoomerang.delay = delay;
		airplaneLevelBoomerang.onLeft = onLeft;
		airplaneLevelBoomerang.speedForward = speedF;
		airplaneLevelBoomerang.easeDistanceForward = easeDF;
		airplaneLevelBoomerang.speedReturn = speedR;
		airplaneLevelBoomerang.easeDistanceReturn = easeDR;
		airplaneLevelBoomerang.id = id;
		return airplaneLevelBoomerang;
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x0000B9F6 File Offset: 0x00009BF6
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x0000BA13 File Offset: 0x00009C13
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x00087A28 File Offset: 0x00085C28
	public override void Start()
	{
		base.Start();
		if (!base.CanParry)
		{
			base.animator.Play((!Rand.Bool()) ? "B" : "A");
		}
		base.StartCoroutine(this.move_cr());
		this.SFX_DOGFIGHT_BoneShot_Loop();
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00087A80 File Offset: 0x00085C80
	public IEnumerator move_cr()
	{
		this.rend.enabled = true;
		float end = (!this.onLeft) ? (-725f + this.easeDistanceForward) : (725f - this.easeDistanceForward);
		bool flipSprite = !this.onLeft;
		YieldInstruction wait = new WaitForFixedUpdate();
		base.GetComponent<SpriteRenderer>().flipX = flipSprite;
		while ((this.onLeft && Mathf.Sign(base.transform.position.x - end) == -1f) || (!this.onLeft && Mathf.Sign(base.transform.position.x - end) == 1f))
		{
			base.transform.position += Vector3.right * this.speedForward * CupheadTime.FixedDelta * (float)((!this.onLeft) ? -1 : 1);
			yield return wait;
		}
		float t = 0f;
		float tMax = this.easeDistanceForward / this.speedForward * 2f;
		float start = end;
		end = ((!this.onLeft) ? -725f : 725f);
		while (t < tMax)
		{
			t += CupheadTime.FixedDelta;
			yield return wait;
			base.transform.position = new Vector3(Mathf.Lerp(start, end, EaseUtils.EaseOutSine(0f, 1f, Mathf.InverseLerp(0f, tMax, t))), base.transform.position.y);
		}
		base.transform.position = new Vector3(end, base.transform.position.y);
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		base.GetComponent<SpriteRenderer>().flipX = !flipSprite;
		t = 0f;
		tMax = this.easeDistanceReturn / this.speedReturn * 2f;
		start = base.transform.position.x;
		end = ((!this.onLeft) ? (-725f + this.easeDistanceReturn) : (725f - this.easeDistanceReturn));
		while (t < tMax)
		{
			t += CupheadTime.FixedDelta;
			yield return wait;
			base.transform.position = new Vector3(Mathf.Lerp(start, end, EaseUtils.EaseInSine(0f, 1f, Mathf.InverseLerp(0f, tMax, t))), base.transform.position.y);
		}
		base.transform.position = new Vector3(end, base.transform.position.y);
		end = ((!this.onLeft) ? 1025f : -1025f);
		while ((this.onLeft && Mathf.Sign(base.transform.position.x - end) == 1f) || (!this.onLeft && Mathf.Sign(base.transform.position.x - end) == -1f))
		{
			base.transform.position += Vector3.right * this.speedReturn * CupheadTime.FixedDelta * (float)((!this.onLeft) ? 1 : -1);
			yield return wait;
		}
		this.SFX_DOGFIGHT_BoneShot_StopLoop();
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x0000BA31 File Offset: 0x00009C31
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.SFX_DOGFIGHT_BoneShot_StopLoop();
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00087A9C File Offset: 0x00085C9C
	public void SFX_DOGFIGHT_BoneShot_Loop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1), 0.12f, 0.1f);
		AudioManager.PlayLoop("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1));
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1));
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x0000BA40 File Offset: 0x00009C40
	public void SFX_DOGFIGHT_BoneShot_StopLoop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_p1_bulldog_boneshot_0" + (this.id + 1));
	}

	// Token: 0x04000AB1 RID: 2737
	public const float xMax = 725f;

	// Token: 0x04000AB2 RID: 2738
	public float delay;

	// Token: 0x04000AB3 RID: 2739
	public bool onLeft;

	// Token: 0x04000AB4 RID: 2740
	public float speedForward;

	// Token: 0x04000AB5 RID: 2741
	public float easeDistanceForward;

	// Token: 0x04000AB6 RID: 2742
	public float speedReturn;

	// Token: 0x04000AB7 RID: 2743
	public float easeDistanceReturn;

	// Token: 0x04000AB8 RID: 2744
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04000AB9 RID: 2745
	public int id;
}
