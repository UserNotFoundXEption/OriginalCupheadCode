using System;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public abstract class AbstractPlayerSuper : AbstractCollidableObject
{
	// Token: 0x06003784 RID: 14212 RVA: 0x0002D590 File Offset: 0x0002B790
	public AbstractPlayerSuper()
	{
	}

	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06003785 RID: 14213 RVA: 0x00103804 File Offset: 0x00101A04
	// (remove) Token: 0x06003786 RID: 14214 RVA: 0x0010383C File Offset: 0x00101A3C
	public event Action OnEndedEvent;

	// Token: 0x1400009F RID: 159
	// (add) Token: 0x06003787 RID: 14215 RVA: 0x00103874 File Offset: 0x00101A74
	// (remove) Token: 0x06003788 RID: 14216 RVA: 0x001038AC File Offset: 0x00101AAC
	public event Action OnStartedEvent;

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06003789 RID: 14217 RVA: 0x0002D598 File Offset: 0x0002B798
	public override bool allowCollisionPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x0002D59B File Offset: 0x0002B79B
	public override void Awake()
	{
		base.tag = "PlayerProjectile";
		base.Awake();
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x0002D5AE File Offset: 0x0002B7AE
	public virtual void Start()
	{
		this.animHelper = base.GetComponent<AnimationHelper>();
		base.transform.position = this.player.transform.position;
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x0002D5D7 File Offset: 0x0002B7D7
	public virtual void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x0002D5EF File Offset: 0x0002B7EF
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x0600378E RID: 14222 RVA: 0x0002D611 File Offset: 0x0002B811
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.player != null)
		{
			this.player.weaponManager.OnSuperInterrupt -= this.Interrupt;
		}
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x001038E4 File Offset: 0x00101AE4
	public AbstractPlayerSuper Create(LevelPlayerController player)
	{
		AbstractPlayerSuper abstractPlayerSuper = this.InstantiatePrefab<AbstractPlayerSuper>();
		abstractPlayerSuper.player = player;
		PlayerId id = player.id;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			if (!this.isChaliceSuper)
			{
				abstractPlayerSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman);
				abstractPlayerSuper.cuphead.gameObject.SetActive(!PlayerManager.player1IsMugman);
				abstractPlayerSuper.mugman.gameObject.SetActive(PlayerManager.player1IsMugman);
			}
		}
		else if (!this.isChaliceSuper)
		{
			abstractPlayerSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead);
			abstractPlayerSuper.cuphead.gameObject.SetActive(PlayerManager.player1IsMugman);
			abstractPlayerSuper.mugman.gameObject.SetActive(!PlayerManager.player1IsMugman);
		}
		this.interrupted = false;
		player.weaponManager.OnSuperInterrupt += abstractPlayerSuper.Interrupt;
		abstractPlayerSuper.StartSuper();
		return abstractPlayerSuper;
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x0002D647 File Offset: 0x0002B847
	public virtual void Interrupt()
	{
		this.interrupted = true;
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x00103A04 File Offset: 0x00101C04
	public virtual void StartSuper()
	{
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		PauseManager.Pause();
		AudioManager.HandleSnapshot(AudioManager.Snapshots.SuperStart.ToString(), 0.2f);
		AudioManager.ChangeBGMPitch(1.3f, 1.5f);
		base.transform.SetScale(new float?(this.player.transform.localScale.x), new float?(this.player.transform.localScale.y), new float?(1f));
		base.transform.position = this.player.transform.position;
		if (this.OnStartedEvent != null)
		{
			this.OnStartedEvent();
		}
		this.OnStartedEvent = null;
	}

	// Token: 0x06003792 RID: 14226 RVA: 0x00103AD4 File Offset: 0x00101CD4
	public virtual void Fire()
	{
		PauseManager.Unpause();
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Super.ToString(), 0.2f);
		if (this.player == null)
		{
			this.Interrupt();
		}
		else
		{
			this.player.PauseAll();
		}
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = false;
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x00103B34 File Offset: 0x00101D34
	public virtual void EndSuper(bool changePitch = true)
	{
		AudioManager.SnapshotReset(SceneLoader.SceneName, 1f);
		if (changePitch)
		{
			AudioManager.ChangeBGMPitch(1f, 2f);
		}
		if (this.player != null)
		{
			this.player.UnpauseAll(false);
		}
		if (this.OnEndedEvent != null)
		{
			this.OnEndedEvent();
		}
		this.OnEndedEvent = null;
	}

	// Token: 0x04002CA4 RID: 11428
	[SerializeField]
	[Header("Player Sprites")]
	public SpriteRenderer cuphead;

	// Token: 0x04002CA5 RID: 11429
	[SerializeField]
	public SpriteRenderer mugman;

	// Token: 0x04002CA6 RID: 11430
	[SerializeField]
	public bool isChaliceSuper;

	// Token: 0x04002CA7 RID: 11431
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002CA8 RID: 11432
	public LevelPlayerController player;

	// Token: 0x04002CA9 RID: 11433
	public DamageDealer damageDealer;

	// Token: 0x04002CAA RID: 11434
	public AnimationHelper animHelper;

	// Token: 0x04002CAB RID: 11435
	public bool interrupted;
}
