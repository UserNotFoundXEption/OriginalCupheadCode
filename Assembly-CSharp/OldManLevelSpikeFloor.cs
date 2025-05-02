using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class OldManLevelSpikeFloor : AbstractCollidableObject
{
	// Token: 0x0600211F RID: 8479 RVA: 0x0001C49C File Offset: 0x0001A69C
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponentInChildren<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x0001C4C7 File Offset: 0x0001A6C7
	public override void OnDestroy()
	{
		this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000B9794 File Offset: 0x000B7994
	public void SetID(int i)
	{
		this.id = i;
		base.animator.SetInteger("Variant", i % 4);
		this.gnomeRenderer.flipX = (i % 8 > 3);
		this.tuftRenderer.flipX = this.gnomeRenderer.flipX;
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000B97E4 File Offset: 0x000B79E4
	public string AnimSuffix()
	{
		switch (this.id % 4)
		{
		case 0:
			return "A";
		case 1:
			return "B";
		case 2:
			return "C";
		default:
			return "D";
		}
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000B9828 File Offset: 0x000B7A28
	public string PopStartSuffix()
	{
		switch (this.id % 4)
		{
		case 0:
			return "A_C";
		case 1:
			return "B";
		case 2:
			return "A_C";
		default:
			return "D";
		}
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000B986C File Offset: 0x000B7A6C
	public string PopWarningSuffix()
	{
		switch (this.id % 4)
		{
		case 0:
			return "A_C";
		case 1:
			return "B_D";
		case 2:
			return "A_C";
		default:
			return "B_D";
		}
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000B98B0 File Offset: 0x000B7AB0
	public void Update()
	{
		if (this.spikeState == OldManLevelSpikeFloor.SpikeState.Gnomed)
		{
			return;
		}
		if (this.spikeState != OldManLevelSpikeFloor.SpikeState.Spiked && !this.deathTimeOut && this.MinDistanceToPlayer(base.transform.position) < 50f)
		{
			this.ChangeState(OldManLevelSpikeFloor.SpikeState.Spiked);
		}
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x000B9904 File Offset: 0x000B7B04
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.spikeState != OldManLevelSpikeFloor.SpikeState.Gnomed)
		{
			return;
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle);
			Level.Current.RegisterMinionKilled();
			this.Dead();
		}
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000B9958 File Offset: 0x000B7B58
	public void SetProperties(LevelProperties.OldMan properties)
	{
		this.spikeProperties = properties.CurrentState.spikes;
		this.gnomeProperties = properties.CurrentState.turret;
		this.gnomeShootPatternString = new PatternString(this.gnomeProperties.attackString, true, true);
		this.gnomePinkPatternString = new PatternString(this.gnomeProperties.pinkShotString, true, true);
		this.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle);
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x0001C4EC File Offset: 0x0001A6EC
	public void SpawnGnome()
	{
		this.ChangeState(OldManLevelSpikeFloor.SpikeState.Gnomed);
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000B99C0 File Offset: 0x000B7BC0
	public void ChangeState(OldManLevelSpikeFloor.SpikeState state)
	{
		if (this.exit)
		{
			return;
		}
		if (this.spikeState == OldManLevelSpikeFloor.SpikeState.Idle || state == OldManLevelSpikeFloor.SpikeState.Idle)
		{
			if (this.gnomeCR != null)
			{
				base.StopCoroutine(this.gnomeCR);
			}
			if (this.spikeCR != null)
			{
				base.StopCoroutine(this.spikeCR);
			}
			base.animator.ResetTrigger("OnPimple");
			base.animator.ResetTrigger("OnPop");
			base.animator.ResetTrigger("OnWarning");
			base.animator.SetBool("IsAttacking", false);
			this.spikeState = state;
			if (state != OldManLevelSpikeFloor.SpikeState.Gnomed)
			{
				if (state != OldManLevelSpikeFloor.SpikeState.Idle)
				{
					if (state == OldManLevelSpikeFloor.SpikeState.Spiked)
					{
						this.spikeCR = base.StartCoroutine(this.spike_up_cr());
					}
				}
				else if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_" + this.AnimSuffix()))
				{
					base.StartCoroutine(this.restart_idle_cr());
				}
			}
			else
			{
				this.gnomeCR = base.StartCoroutine(this.gnome_up_cr());
			}
		}
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x000B9AE8 File Offset: 0x000B7CE8
	public IEnumerator restart_idle_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0.5f, 1f));
		base.animator.Play("Restart_Idle_" + this.PopStartSuffix());
		this.deathTimeOut = false;
		yield break;
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x000B9B04 File Offset: 0x000B7D04
	public float MinDistanceToPlayer(Vector3 pos)
	{
		float num = float.MaxValue;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			float num2 = Vector3.SqrMagnitude(pos - player.transform.position);
			if (num2 < num)
			{
				num = num2;
			}
		}
		if (player2 != null)
		{
			float num3 = Vector3.SqrMagnitude(pos - player2.transform.position);
			if (num3 < num)
			{
				num = num3;
			}
		}
		return Mathf.Sqrt(num);
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x000B9B88 File Offset: 0x000B7D88
	public IEnumerator gnome_up_cr()
	{
		this.hp = this.gnomeProperties.hp;
		base.animator.SetTrigger("OnPimple");
		yield return base.animator.WaitForAnimationToEnd(this, "Pop_Start_" + this.PopStartSuffix(), false, true);
		float t = 0f;
		while (t < this.gnomeProperties.appearWarning && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		t = 0f;
		while (t < this.gnomeProperties.spawnSecondaryBuffer && this.MinDistanceToPlayer(base.transform.position) < this.gnomeProperties.spawnDistanceCheck && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnPop");
		yield return null;
		while (this.spikeState == OldManLevelSpikeFloor.SpikeState.Gnomed)
		{
			t = 0f;
			while (t < this.gnomeProperties.shotDelay && !this.exit)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			base.animator.SetBool("IsAttacking", true);
			this.shootAngle = this.gnomeShootPatternString.PopFloat();
			if (this.shootAngle != 0f && ((this.shootAngle <= 180f && this.dontShootLeft) || (this.shootAngle > 180f && this.dontShootRight)))
			{
				this.shootAngle = 360f - this.shootAngle;
			}
			base.animator.SetBool("Diagonal", this.shootAngle != 0f);
			t = 0f;
			while (t < this.gnomeProperties.warningDuration && !this.exit)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
			if (!this.exit)
			{
				base.transform.localScale = new Vector3((float)((!(this.shootAngle > 180f ^ this.gnomeRenderer.flipX)) ? 1 : -1), 1f);
			}
			base.animator.SetBool("IsAttacking", false);
			yield return new WaitForEndOfFrame();
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000B9BA4 File Offset: 0x000B7DA4
	public float MinPlayerDistance()
	{
		float num = float.MaxValue;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null) && levelPlayerController.transform.position.y <= base.transform.position.y + 200f)
			{
				float num2 = Mathf.Abs(base.transform.position.x - levelPlayerController.transform.position.x);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000B9C80 File Offset: 0x000B7E80
	public IEnumerator spike_up_cr()
	{
		if (!((OldManLevel)Level.Current).playedFirstSpikeSound)
		{
			this.SFX_OMM_Gnome_SpikeRaiseFirst();
			((OldManLevel)Level.Current).playedFirstSpikeSound = true;
		}
		base.transform.GetChild(0).gameObject.tag = "EnemyProjectile";
		base.animator.SetTrigger("OnWarning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning_Start_" + this.AnimSuffix(), false, true);
		float t = 0f;
		while (t < this.spikeProperties.warningDuration && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetBool("IsAttacking", true);
		t = 0f;
		while (t < this.spikeProperties.attackDuration && !this.exit)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		while (this.MinPlayerDistance() < 75f && !this.exit)
		{
			yield return null;
		}
		yield return null;
		base.animator.SetBool("IsAttacking", false);
		yield return base.animator.WaitForAnimationToStart(this, "Idle_" + this.AnimSuffix(), false);
		this.ChangeState(OldManLevelSpikeFloor.SpikeState.Idle);
		base.transform.GetChild(0).gameObject.tag = "Enemy";
		yield return null;
		yield break;
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x000B9C9C File Offset: 0x000B7E9C
	public void Dead()
	{
		Vector3 position;
		position..ctor(this.shootRoot.position.x, this.shootRoot.position.y - 100f);
		this.deathPuff.Create(position);
		base.animator.Play("None");
		for (int i = 0; i < this.deathParts.Length; i++)
		{
			if (i != 0 || Random.Range(0, 10) == 0)
			{
				this.deathParts[i].CreatePart(position);
			}
		}
		this.deathTimeOut = true;
		AudioManager.Play("sfx_dlc_omm_gnome_death");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_death");
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x0001C4F5 File Offset: 0x0001A6F5
	public void Exit()
	{
		base.StartCoroutine(this.exit_cr());
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000B9D58 File Offset: 0x000B7F58
	public IEnumerator exit_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 1f));
		this.exit = true;
		base.animator.SetBool("Dead", true);
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000B9D74 File Offset: 0x000B7F74
	public void AniEvent_ShootProjectile()
	{
		BasicProjectile basicProjectile = (this.gnomePinkPatternString.PopLetter() != 'P') ? this.gnomeProjectile : this.gnomePinkProjectile;
		if (this.shootAngle == 0f)
		{
			basicProjectile.Create(this.shootRoot.position, this.shootAngle, this.gnomeProperties.shotSpeed);
			this.shootFXRenderer.transform.eulerAngles = Vector3.zero;
			this.shootFXRenderer.transform.localPosition = Vector3.up * 18f;
		}
		else
		{
			basicProjectile.Create(this.shootRoot.position + Vector3.right * 40f * Mathf.Sign(this.shootAngle - 180f), this.shootAngle, this.gnomeProperties.shotSpeed);
			this.shootFXRenderer.transform.eulerAngles = new Vector3(0f, 0f, 40f * Mathf.Sign(this.shootAngle) * (float)((this.shootAngle <= 180f) ? 1 : -1));
			this.shootFXRenderer.transform.localPosition = new Vector3(30.5f * (float)((!this.gnomeRenderer.flipX) ? 1 : -1), 33f);
		}
		AudioManager.Play("sfx_dlc_omm_gnome_shoot_projectile");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_shoot_projectile");
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x0001C504 File Offset: 0x0001A704
	public void SFX_OMM_Gnome_SpikeRaiseFirst()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_spike_raisefirst");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_raisefirst");
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x0001C520 File Offset: 0x0001A720
	public void AnimationEvent_SFX_OMM_Gnome_SpikeRaise()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_spike_raise");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_raise");
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x0001C53C File Offset: 0x0001A73C
	public void AnimationEvent_SFX_OMM_Gnome_SpikeRetract()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_spike_retract");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_spike_retract");
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x0001C558 File Offset: 0x0001A758
	public void AnimationEvent_SFX_OMM_Gnome_BeardAnticipation()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_beard_anticipation");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_beard_anticipation");
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x0001C574 File Offset: 0x0001A774
	public void AnimationEvent_SFX_OMM_Gnome_BeardPopup()
	{
		AudioManager.Play("sfx_dlc_omm_gnome_beard_popup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_beard_popup");
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x000B9F04 File Offset: 0x000B8104
	public void WORKAROUND_NullifyFields()
	{
		this.deathPuff = null;
		this.deathParts = null;
		this.shootRoot = null;
		this.gnomeProjectile = null;
		this.gnomePinkProjectile = null;
		this.gnomeShootPatternString = null;
		this.gnomePinkPatternString = null;
		this.spikeCR = null;
		this.gnomeCR = null;
		this.gnomeRenderer = null;
		this.tuftRenderer = null;
		this.shootFXRenderer = null;
	}

	// Token: 0x04001B47 RID: 6983
	public const float SPIKE_TRIGGER_RANGE = 50f;

	// Token: 0x04001B48 RID: 6984
	public const float MIN_DISTANCE_TO_STAY_SPIKED = 75f;

	// Token: 0x04001B49 RID: 6985
	[Header("Death FX")]
	[SerializeField]
	public Effect deathPuff;

	// Token: 0x04001B4A RID: 6986
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x04001B4B RID: 6987
	[Header("Prefabs")]
	[SerializeField]
	public Transform shootRoot;

	// Token: 0x04001B4C RID: 6988
	[SerializeField]
	public BasicProjectile gnomeProjectile;

	// Token: 0x04001B4D RID: 6989
	[SerializeField]
	public BasicProjectile gnomePinkProjectile;

	// Token: 0x04001B4E RID: 6990
	public OldManLevelSpikeFloor.SpikeState spikeState;

	// Token: 0x04001B4F RID: 6991
	public LevelProperties.OldMan.Spikes spikeProperties;

	// Token: 0x04001B50 RID: 6992
	public LevelProperties.OldMan.Turret gnomeProperties;

	// Token: 0x04001B51 RID: 6993
	public PatternString gnomeShootPatternString;

	// Token: 0x04001B52 RID: 6994
	public PatternString gnomePinkPatternString;

	// Token: 0x04001B53 RID: 6995
	public float hp;

	// Token: 0x04001B54 RID: 6996
	public DamageReceiver damageReceiver;

	// Token: 0x04001B55 RID: 6997
	public float shootAngle;

	// Token: 0x04001B56 RID: 6998
	public Coroutine spikeCR;

	// Token: 0x04001B57 RID: 6999
	public Coroutine gnomeCR;

	// Token: 0x04001B58 RID: 7000
	[SerializeField]
	public bool dontShootLeft;

	// Token: 0x04001B59 RID: 7001
	[SerializeField]
	public bool dontShootRight;

	// Token: 0x04001B5A RID: 7002
	[SerializeField]
	public SpriteRenderer gnomeRenderer;

	// Token: 0x04001B5B RID: 7003
	[SerializeField]
	public SpriteRenderer tuftRenderer;

	// Token: 0x04001B5C RID: 7004
	[SerializeField]
	public SpriteRenderer shootFXRenderer;

	// Token: 0x04001B5D RID: 7005
	public int id;

	// Token: 0x04001B5E RID: 7006
	public bool exit;

	// Token: 0x04001B5F RID: 7007
	public bool deathTimeOut;

	// Token: 0x02000DFA RID: 3578
	public enum SpikeState
	{
		// Token: 0x04006564 RID: 25956
		Idle,
		// Token: 0x04006565 RID: 25957
		Spiked,
		// Token: 0x04006566 RID: 25958
		Gnomed
	}
}
