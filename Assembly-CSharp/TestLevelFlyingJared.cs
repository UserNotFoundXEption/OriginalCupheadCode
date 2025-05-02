using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class TestLevelFlyingJared : LevelProperties.Test.Entity
{
	// Token: 0x06000D80 RID: 3456 RVA: 0x000875B0 File Offset: 0x000857B0
	public override void LevelInit(LevelProperties.Test properties)
	{
		base.LevelInit(properties);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
		AudioManager.PlayLoop("test_sound");
		this.emitAudioFromObject.Add("test_sound");
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x0000B8B6 File Offset: 0x00009AB6
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		base.GetComponent<AudioWarble>().HandleWarble();
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x0000B8D4 File Offset: 0x00009AD4
	public void OnLevelStart()
	{
		base.StartCoroutine(this.moveX_cr());
		base.StartCoroutine(this.moveY_cr());
		base.StartCoroutine(this.scale_cr());
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x00087618 File Offset: 0x00085818
	public float GetHealthTimeX()
	{
		float i = 1f - base.properties.CurrentHealth / base.properties.TotalHealth;
		return base.properties.CurrentState.moving.timeX.GetFloatAt(i);
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x00087660 File Offset: 0x00085860
	public float GetHealthTimeY()
	{
		float i = 1f - base.properties.CurrentHealth / base.properties.TotalHealth;
		return base.properties.CurrentState.moving.timeY.GetFloatAt(i);
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x000876A8 File Offset: 0x000858A8
	public float GetHealthTimeScale()
	{
		float i = 1f - base.properties.CurrentHealth / base.properties.TotalHealth;
		return base.properties.CurrentState.moving.timeScale.GetFloatAt(i);
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x000876F0 File Offset: 0x000858F0
	public IEnumerator moveX_cr()
	{
		float start = base.transform.position.x;
		float end = -start;
		for (;;)
		{
			this.childSprite.transform.SetScale(new float?(1f), null, null);
			yield return base.TweenLocalPositionX(start, end, this.GetHealthTimeX(), EaseUtils.EaseType.easeInOutSine);
			this.childSprite.transform.SetScale(new float?(-1f), null, null);
			yield return base.TweenLocalPositionX(end, start, this.GetHealthTimeX(), EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0008770C File Offset: 0x0008590C
	public IEnumerator moveY_cr()
	{
		float start = base.transform.position.y;
		float end = start - 100f;
		for (;;)
		{
			yield return base.TweenLocalPositionY(start, end, this.GetHealthTimeY(), EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionY(end, start, this.GetHealthTimeY(), EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x00087728 File Offset: 0x00085928
	public IEnumerator scale_cr()
	{
		Vector3 start = new Vector3(1f, 1f, 1f);
		Vector3 end = new Vector3(2f, 2f, 2f);
		for (;;)
		{
			yield return base.TweenScale(start, end, this.GetHealthTimeScale(), EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenScale(end, start, this.GetHealthTimeScale(), EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x04000A8C RID: 2700
	[SerializeField]
	public Transform childSprite;

	// Token: 0x04000A8D RID: 2701
	public DamageReceiver damageReceiver;

	// Token: 0x04000A8E RID: 2702
	[SerializeField]
	public AudioSource audioClip;
}
