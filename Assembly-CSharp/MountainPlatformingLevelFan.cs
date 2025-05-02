using System;
using System.Collections;

// Token: 0x02000442 RID: 1090
public class MountainPlatformingLevelFan : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002ECC RID: 11980 RVA: 0x00027011 File Offset: 0x00025211
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x000E0450 File Offset: 0x000DE650
	public float GetSpeed()
	{
		if (base.transform.localScale.x == 1f)
		{
			this.speed = -base.Properties.fanVelocity;
		}
		else
		{
			this.speed = base.Properties.fanVelocity;
		}
		return this.speed;
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x00027026 File Offset: 0x00025226
	public override void OnStart()
	{
		base.StartCoroutine(this.fan_cr());
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x000E04A8 File Offset: 0x000DE6A8
	public IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x00027035 File Offset: 0x00025235
	public void FanOn()
	{
		this.PlayLionRoarSFX();
		this.fanOn = true;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x00027044 File Offset: 0x00025244
	public void FanOff()
	{
		this.fanOn = false;
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x000E04C4 File Offset: 0x000DE6C4
	public IEnumerator fan_cr()
	{
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			base.animator.SetBool("IsFan", true);
			yield return null;
			yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
			base.animator.SetBool("WindOn", true);
			yield return null;
			float t = 0f;
			float time = base.Properties.fanWaitTime.RandomFloat();
			while (t < time)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			base.animator.SetBool("IsFan", false);
			base.animator.SetBool("WindOn", false);
			yield return null;
			yield return base.animator.WaitForAnimationToEnd(this, "Roar_End", false, true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x0002704D File Offset: 0x0002524D
	public void PlayLionRoarSFX()
	{
		AudioManager.Play("castle_rock_lion_roar");
		this.emitAudioFromObject.Add("castle_rock_lion_roar");
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x000E04E0 File Offset: 0x000DE6E0
	public override void Die()
	{
		AudioManager.Play("castle_rock_lion_death");
		this.emitAudioFromObject.Add("castle_rock_lion_death");
		base.animator.SetBool("WindOn", false);
		this.speed = 0f;
		this.fanOn = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("Death");
		base.Dead = true;
	}

	// Token: 0x040026DB RID: 9947
	public float speed;

	// Token: 0x040026DC RID: 9948
	public float offset = 50f;

	// Token: 0x040026DD RID: 9949
	public bool fanOn;
}
