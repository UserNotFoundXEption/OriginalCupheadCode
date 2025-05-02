using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000568 RID: 1384
public class PlaneSuperBomb : AbstractPlaneSuper
{
	// Token: 0x06003A23 RID: 14883 RVA: 0x0010E878 File Offset: 0x0010CA78
	public override void StartSuper()
	{
		base.StartSuper();
		this.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.player.stats.OnStoned += this.OnStoned;
		if ((this.player.id == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.player.id == PlayerId.PlayerTwo && PlayerManager.player1IsMugman))
		{
			this.boom.gameObject.SetActive(true);
		}
		else
		{
			this.boomMM.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003A24 RID: 14884 RVA: 0x0010E920 File Offset: 0x0010CB20
	public IEnumerator super_cr()
	{
		float t = 0f;
		this.damageDealer = new DamageDealer(WeaponProperties.PlaneSuperBomb.damage, WeaponProperties.PlaneSuperBomb.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		tracker.Add(this.damageDealer);
		while (t < WeaponProperties.PlaneSuperBomb.countdownTime && !this.earlyExplosion)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.Fire();
		if (this.player != null)
		{
			this.player.PauseAll();
			this.player.SetSpriteVisible(false);
			base.transform.position = this.player.transform.position;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
		base.animator.SetTrigger("Explode");
		AudioManager.Stop("player_plane_bomb_ticktock_loop");
		AudioManager.Play("player_plane_bomb_explosion");
		yield break;
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x0002F526 File Offset: 0x0002D726
	public void OnStoned()
	{
		this.earlyExplosion = true;
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x0002F52F File Offset: 0x0002D72F
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.earlyExplosion = true;
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x0002F538 File Offset: 0x0002D738
	public void EndIntroAnimation()
	{
		this.StartCountdown();
		AudioManager.PlayLoop("player_plane_bomb_ticktock_loop");
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x0002F557 File Offset: 0x0002D757
	public void PlayerReappear()
	{
		if (this.player != null)
		{
			this.player.UnpauseAll(false);
			this.player.SetSpriteVisible(true);
		}
	}

	// Token: 0x06003A29 RID: 14889 RVA: 0x0002F582 File Offset: 0x0002D782
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003A2A RID: 14890 RVA: 0x0002F58F File Offset: 0x0002D78F
	public void StartBoomScale()
	{
		this.boomRoutine = base.StartCoroutine(this.boomScale_cr());
	}

	// Token: 0x06003A2B RID: 14891 RVA: 0x0010E93C File Offset: 0x0010CB3C
	public IEnumerator boomScale_cr()
	{
		float t = 0f;
		float frameTime = 0.0416666679f;
		float scale = 1f;
		for (;;)
		{
			t += CupheadTime.Delta;
			while (t > frameTime)
			{
				t -= frameTime;
				scale *= 1.15f;
				this.boom.SetScale(new float?(scale), new float?(scale), null);
				this.boomMM.SetScale(new float?(scale), new float?(scale), null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003A2C RID: 14892 RVA: 0x0002F5A3 File Offset: 0x0002D7A3
	public void Pause()
	{
		if (this.boomRoutine != null)
		{
			base.StopCoroutine(this.boomRoutine);
		}
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x0010E958 File Offset: 0x0010CB58
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.player != null)
		{
			this.player.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
			this.player.stats.OnStoned -= this.OnStoned;
		}
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x0002F5BC File Offset: 0x0002D7BC
	public void PlaneSuperBombLaughAudio()
	{
		AudioManager.Play("player_plane_bomb_laugh");
	}

	// Token: 0x04002E93 RID: 11923
	public bool earlyExplosion;

	// Token: 0x04002E94 RID: 11924
	public Coroutine boomRoutine;

	// Token: 0x04002E95 RID: 11925
	[SerializeField]
	public Transform boom;

	// Token: 0x04002E96 RID: 11926
	[SerializeField]
	public Transform boomMM;
}
