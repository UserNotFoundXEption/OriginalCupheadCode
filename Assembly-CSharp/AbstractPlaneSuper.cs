using System;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public abstract class AbstractPlaneSuper : AbstractCollidableObject
{
	// Token: 0x06003A16 RID: 14870 RVA: 0x0002F44D File Offset: 0x0002D64D
	public AbstractPlaneSuper()
	{
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06003A17 RID: 14871 RVA: 0x0002F45C File Offset: 0x0002D65C
	public PlanePlayerWeaponManager.States.Super State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06003A18 RID: 14872 RVA: 0x0002F464 File Offset: 0x0002D664
	public override bool allowCollisionPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x0002F467 File Offset: 0x0002D667
	public override void Awake()
	{
		base.tag = "PlayerProjectile";
		base.Awake();
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x0002F47A File Offset: 0x0002D67A
	public virtual void Start()
	{
		this.animHelper = base.GetComponent<AnimationHelper>();
		base.transform.position = this.player.transform.position;
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x0002F4A3 File Offset: 0x0002D6A3
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x0002F4BB File Offset: 0x0002D6BB
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x0010E59C File Offset: 0x0010C79C
	public AbstractPlaneSuper Create(PlanePlayerController player)
	{
		AbstractPlaneSuper abstractPlaneSuper = this.InstantiatePrefab<AbstractPlaneSuper>();
		abstractPlaneSuper.player = player;
		if (player.stats.isChalice)
		{
			abstractPlaneSuper.spriteRenderer = this.chalice;
			abstractPlaneSuper.chalice.gameObject.SetActive(true);
			if (abstractPlaneSuper.cuphead)
			{
				abstractPlaneSuper.cuphead.gameObject.SetActive(false);
			}
			if (abstractPlaneSuper.mugman)
			{
				abstractPlaneSuper.mugman.gameObject.SetActive(false);
			}
		}
		else
		{
			PlayerId id = player.id;
			if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
			{
				abstractPlaneSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman);
				abstractPlaneSuper.cuphead.gameObject.SetActive(!PlayerManager.player1IsMugman);
				abstractPlaneSuper.mugman.gameObject.SetActive(PlayerManager.player1IsMugman);
			}
			else
			{
				abstractPlaneSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead);
				abstractPlaneSuper.cuphead.gameObject.SetActive(PlayerManager.player1IsMugman);
				abstractPlaneSuper.mugman.gameObject.SetActive(!PlayerManager.player1IsMugman);
			}
		}
		abstractPlaneSuper.StartSuper();
		return abstractPlaneSuper;
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x0010E6F4 File Offset: 0x0010C8F4
	public virtual void StartSuper()
	{
		this.animHelper = base.GetComponent<AnimationHelper>();
		this.animHelper.IgnoreGlobal = true;
		PauseManager.Pause();
		this.player.PauseAll();
		this.player.SetSpriteVisible(false);
		AudioManager.SnapshotTransition(new string[]
		{
			"Super",
			"Unpaused",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.1f);
		AudioManager.ChangeBGMPitch(1.3f, 1.5f);
		AudioManager.Play("player_super_beam_start");
		base.transform.SetScale(new float?(this.player.transform.localScale.x), new float?(this.player.transform.localScale.y), new float?(1f));
		base.transform.position = this.player.transform.position;
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x0002F4DD File Offset: 0x0002D6DD
	public virtual void Fire()
	{
		this.state = PlanePlayerWeaponManager.States.Super.Ending;
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x0010E804 File Offset: 0x0010CA04
	public void SnapshotAudio()
	{
		string[] array = new string[2];
		array[0] = "Super";
		if (SettingsData.Data.vintageAudioEnabled)
		{
			array[1] = "Unpaused_1920s";
		}
		else
		{
			array[1] = "Unpaused";
		}
		AudioManager.SnapshotTransition(array, new float[]
		{
			0f,
			1f
		}, 4f);
		AudioManager.ChangeBGMPitch(1f, 4f);
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x0002F4E6 File Offset: 0x0002D6E6
	public virtual void StartCountdown()
	{
		this.SnapshotAudio();
		PauseManager.Unpause();
		this.player.UnpauseAll(false);
		this.player.SetSpriteVisible(true);
		this.animHelper.IgnoreGlobal = false;
		this.state = PlanePlayerWeaponManager.States.Super.Countdown;
	}

	// Token: 0x04002E8B RID: 11915
	public PlanePlayerWeaponManager.States.Super state = PlanePlayerWeaponManager.States.Super.Intro;

	// Token: 0x04002E8C RID: 11916
	[SerializeField]
	[Header("Player Sprites")]
	public SpriteRenderer cuphead;

	// Token: 0x04002E8D RID: 11917
	[SerializeField]
	public SpriteRenderer mugman;

	// Token: 0x04002E8E RID: 11918
	[SerializeField]
	public SpriteRenderer chalice;

	// Token: 0x04002E8F RID: 11919
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002E90 RID: 11920
	public PlanePlayerController player;

	// Token: 0x04002E91 RID: 11921
	public DamageDealer damageDealer;

	// Token: 0x04002E92 RID: 11922
	public AnimationHelper animHelper;
}
