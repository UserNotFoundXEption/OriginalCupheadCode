using System;
using System.Collections;

// Token: 0x0200055E RID: 1374
public class PlanePlayerAudioController : AbstractPlanePlayerComponent
{
	// Token: 0x06003990 RID: 14736 RVA: 0x0002ED68 File Offset: 0x0002CF68
	public override void OnAwake()
	{
		base.OnAwake();
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x0002ED70 File Offset: 0x0002CF70
	public void Start()
	{
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		AudioManager.PlayLoop("player_plane_engine");
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x0002ED98 File Offset: 0x0002CF98
	public void LevelInit()
	{
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x0002ED9A File Offset: 0x0002CF9A
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (info.damage > 0f)
		{
			AudioManager.Play("player_plane_hit");
			if (base.player.stats.Health > 0)
			{
				base.StartCoroutine(this.change_pitch_cr());
			}
		}
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x0010CC7C File Offset: 0x0010AE7C
	public IEnumerator change_pitch_cr()
	{
		if (base.player.stats.Health == 1)
		{
			AudioManager.Play("player_damage_crack_level4");
			this.emitAudioFromObject.Add("player_damage_crack_level4");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.3f, 0.4f);
		}
		else if (base.player.stats.Health == 2)
		{
			AudioManager.Play("player_damage_crack_level3");
			this.emitAudioFromObject.Add("player_damage_crack_level3");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.35f, 0.4f);
		}
		else if (base.player.stats.Health == 3)
		{
			AudioManager.Play("player_damage_crack_level2");
			this.emitAudioFromObject.Add("player_damage_crack_level2");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.4f, 0.4f);
		}
		else
		{
			AudioManager.Play("player_damage_crack_level1");
			this.emitAudioFromObject.Add("player_damage_crack_level1");
			AudioManager.ChangeSFXPitch("player_plane_engine", 0.5f, 0.4f);
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.ChangeSFXPitch("player_plane_engine", 1f, 1f);
		yield return null;
		yield break;
	}
}
