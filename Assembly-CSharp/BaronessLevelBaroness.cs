using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class BaronessLevelBaroness : AbstractCollidableObject
{
	// Token: 0x06000F4B RID: 3915 RVA: 0x0000CF3B File Offset: 0x0000B13B
	public override void Awake()
	{
		base.Awake();
		this.isEasyFinal = false;
		this.damageReceiver = this.shootPoint.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0008D3E8 File Offset: 0x0008B5E8
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.isEasyFinal)
		{
			this.properties.DealDamage(info.damage);
			if (this.properties.CurrentHealth <= 0f && this.isEasyFinal)
			{
				this.isEasyFinal = false;
			}
		}
		else if (this.health < 0f && !this.shotEnough)
		{
			this.shotEnough = true;
		}
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x0000CF72 File Offset: 0x0000B172
	public void Update()
	{
		if (this.shotEnough)
		{
			this.health = this.maxHealth;
		}
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x0000CF8B File Offset: 0x0000B18B
	public void getProperties(LevelProperties.Baroness properties, float health, BaronessLevelCastle parent)
	{
		this.properties = properties;
		this.maxHealth = health;
		this.parent = parent;
		health = this.maxHealth;
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x0000CFAA File Offset: 0x0000B1AA
	public void ShootCounter()
	{
		this.FireProjectileBunch();
		this.shootCounter++;
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
	public void PopUpCounter()
	{
		this.popUpCounter++;
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x0000CFD0 File Offset: 0x0000B1D0
	public void TransformCounter()
	{
		this.transformCounter++;
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x0008D474 File Offset: 0x0008B674
	public void FireProjectileBunch()
	{
		AudioManager.Play("level_baroness_gun_fire");
		AbstractPlayerController next = PlayerManager.GetNext();
		float num = next.transform.position.x - base.transform.position.x;
		float num2 = next.transform.position.y - base.transform.position.y;
		float pointAt = Mathf.Atan2(num2, num) * 57.29578f;
		BaronessLevelBaronessProjectileBunch baronessLevelBaronessProjectileBunch = Object.Instantiate<BaronessLevelBaronessProjectileBunch>(this.baronessProjectileBunch);
		baronessLevelBaronessProjectileBunch.Init(this.baronessProjectileShootPoint.position, (float)this.properties.CurrentState.baronessVonBonbon.projectileSpeed, pointAt, this.properties.CurrentState.baronessVonBonbon, this.parent);
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x0008D548 File Offset: 0x0008B748
	public void FireFinalProjectile()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		BaronessLevelFollowingProjectile baronessLevelFollowingProjectile = Object.Instantiate<BaronessLevelFollowingProjectile>(this.baronessFollowProjectile);
		baronessLevelFollowingProjectile.Init(this.baronessTossPoint.position, next.transform.position, this.properties.CurrentState.baronessVonBonbon, next, this.parent);
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
	public void SoundVoiceAngry()
	{
		AudioManager.Play("level_baroness_voice_angry");
		this.emitAudioFromObject.Add("level_baroness_voice_angry");
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x0000CFFC File Offset: 0x0000B1FC
	public void SoundVoiceEffort()
	{
		AudioManager.Play("level_baroness_voice_effort");
		this.emitAudioFromObject.Add("level_baroness_voice_effort");
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x0000D018 File Offset: 0x0000B218
	public void SoundVoiceCastleyank()
	{
		AudioManager.Play("level_baroness_voice_castleyank");
		this.emitAudioFromObject.Add("level_baroness_voice_castleyank");
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x0000D034 File Offset: 0x0000B234
	public void SoundVoiceIntroA()
	{
		AudioManager.Play("level_baroness_voice_intro_a");
		this.emitAudioFromObject.Add("level_baroness_voice_intro_a");
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x0000D050 File Offset: 0x0000B250
	public void SoundVoiceIntroB()
	{
		AudioManager.Play("level_baroness_voice_intro_b");
		this.emitAudioFromObject.Add("level_baroness_voice_intro_b");
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x0000D06C File Offset: 0x0000B26C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.baronessProjectileBunch = null;
		this.baronessFollowProjectile = null;
	}

	// Token: 0x04000C7C RID: 3196
	[SerializeField]
	public Transform baronessTossPoint;

	// Token: 0x04000C7D RID: 3197
	[SerializeField]
	public Transform baronessProjectileShootPoint;

	// Token: 0x04000C7E RID: 3198
	[SerializeField]
	public BaronessLevelBaronessProjectileBunch baronessProjectileBunch;

	// Token: 0x04000C7F RID: 3199
	[SerializeField]
	public BaronessLevelFollowingProjectile baronessFollowProjectile;

	// Token: 0x04000C80 RID: 3200
	[SerializeField]
	public Transform shootPoint;

	// Token: 0x04000C81 RID: 3201
	public LevelProperties.Baroness properties;

	// Token: 0x04000C82 RID: 3202
	public BaronessLevelCastle parent;

	// Token: 0x04000C83 RID: 3203
	public bool isEasyFinal;

	// Token: 0x04000C84 RID: 3204
	public int shootCounter;

	// Token: 0x04000C85 RID: 3205
	public int popUpCounter;

	// Token: 0x04000C86 RID: 3206
	public int transformCounter;

	// Token: 0x04000C87 RID: 3207
	public bool shotEnough;

	// Token: 0x04000C88 RID: 3208
	public float health;

	// Token: 0x04000C89 RID: 3209
	public float maxHealth;

	// Token: 0x04000C8A RID: 3210
	public DamageReceiver damageReceiver;
}
