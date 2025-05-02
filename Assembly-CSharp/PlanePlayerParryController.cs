using System;
using UnityEngine;

// Token: 0x02000564 RID: 1380
public class PlanePlayerParryController : AbstractPlanePlayerComponent, IParryAttack
{
	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x060039D4 RID: 14804 RVA: 0x0002F1A7 File Offset: 0x0002D3A7
	// (set) Token: 0x060039D5 RID: 14805 RVA: 0x0002F1AF File Offset: 0x0002D3AF
	public PlanePlayerParryController.ParryState State
	{
		get
		{
			return this.state;
		}
		set
		{
			this.state = value;
		}
	}

	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x060039D6 RID: 14806 RVA: 0x0010D86C File Offset: 0x0010BA6C
	// (remove) Token: 0x060039D7 RID: 14807 RVA: 0x0010D8A4 File Offset: 0x0010BAA4
	public event Action OnParryStartEvent;

	// Token: 0x140000A3 RID: 163
	// (add) Token: 0x060039D8 RID: 14808 RVA: 0x0010D8DC File Offset: 0x0010BADC
	// (remove) Token: 0x060039D9 RID: 14809 RVA: 0x0010D914 File Offset: 0x0010BB14
	public event Action OnParrySuccessEvent;

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x060039DA RID: 14810 RVA: 0x0002F1B8 File Offset: 0x0002D3B8
	// (set) Token: 0x060039DB RID: 14811 RVA: 0x0002F1C0 File Offset: 0x0002D3C0
	public bool AttackParryUsed { get; set; }

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x060039DC RID: 14812 RVA: 0x0002F1C9 File Offset: 0x0002D3C9
	// (set) Token: 0x060039DD RID: 14813 RVA: 0x0002F1D1 File Offset: 0x0002D3D1
	public bool HasHitEnemy { get; set; }

	// Token: 0x060039DE RID: 14814 RVA: 0x0002F1DA File Offset: 0x0002D3DA
	public void Start()
	{
		base.player.OnReviveEvent += this.OnRevive;
		base.player.stats.OnStoned += this.OnStoned;
	}

	// Token: 0x060039DF RID: 14815 RVA: 0x0010D94C File Offset: 0x0010BB4C
	public void FixedUpdate()
	{
		PlanePlayerParryController.ParryState parryState = this.state;
		if (parryState != PlanePlayerParryController.ParryState.Ready)
		{
			if (parryState == PlanePlayerParryController.ParryState.Cooldown)
			{
				this.UpdateCooldown();
			}
		}
		else
		{
			this.UpdateReady();
		}
	}

	// Token: 0x060039E0 RID: 14816 RVA: 0x0010D990 File Offset: 0x0010BB90
	public void UpdateReady()
	{
		if (base.player.Shrunk || base.player.WeaponBusy || base.player.stats.StoneTime > 0f)
		{
			return;
		}
		if (this.state != PlanePlayerParryController.ParryState.Ready)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(2) || base.player.motor.HasBufferedInput(PlanePlayerMotor.BufferedInput.Jump))
		{
			base.player.motor.ClearBufferedInput();
			this.StartParry();
		}
	}

	// Token: 0x060039E1 RID: 14817 RVA: 0x0002F20F File Offset: 0x0002D40F
	public void UpdateCooldown()
	{
		this.timeSinceParry += CupheadTime.FixedDelta;
		if (this.timeSinceParry > 0.3f)
		{
			this.state = PlanePlayerParryController.ParryState.Ready;
			this.AttackParryUsed = false;
		}
	}

	// Token: 0x060039E2 RID: 14818 RVA: 0x0002F241 File Offset: 0x0002D441
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		this.state = PlanePlayerParryController.ParryState.Ready;
	}

	// Token: 0x060039E3 RID: 14819 RVA: 0x0010DA2C File Offset: 0x0010BC2C
	public void StartParry()
	{
		if (this.state != PlanePlayerParryController.ParryState.Ready)
		{
			return;
		}
		this.state = PlanePlayerParryController.ParryState.Parrying;
		if (this.OnParryStartEvent != null)
		{
			this.OnParryStartEvent();
		}
		this.effectInstance = (this.effect.Create(base.player) as PlanePlayerParryEffect);
		if (base.player.stats.isChalice)
		{
			this.effectInstance.GetComponent<CircleCollider2D>().radius *= 1.3f;
		}
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x0010DAB0 File Offset: 0x0010BCB0
	public void OnParrySuccess()
	{
		if (this.OnParrySuccessEvent != null)
		{
			this.OnParrySuccessEvent();
		}
		this.state = PlanePlayerParryController.ParryState.Cooldown;
		this.timeSinceParry = 0f;
		if (this.effectInstance != null)
		{
			Object.Destroy(this.effectInstance.gameObject);
		}
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x0002F250 File Offset: 0x0002D450
	public void OnParryEnd()
	{
		this.state = PlanePlayerParryController.ParryState.Cooldown;
		this.timeSinceParry = 0f;
		this.HasHitEnemy = false;
		if (this.effectInstance != null)
		{
			Object.Destroy(this.effectInstance.gameObject);
		}
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x0002F28C File Offset: 0x0002D48C
	public void OnRevive(Vector3 pos)
	{
		this.state = PlanePlayerParryController.ParryState.Ready;
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x0002F295 File Offset: 0x0002D495
	public void OnStoned()
	{
		this.state = PlanePlayerParryController.ParryState.Ready;
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x0002F29E File Offset: 0x0002D49E
	public void SoundPlaneParry()
	{
		AudioManager.Play("player_plane_parry");
		this.emitAudioFromObject.Add("player_plane_parry");
	}

	// Token: 0x04002E70 RID: 11888
	public const float COOLDOWN_DURATION = 0.3f;

	// Token: 0x04002E71 RID: 11889
	public const float CHALICE_PARRY_SIZE_MODIFIER = 1.3f;

	// Token: 0x04002E72 RID: 11890
	public PlanePlayerParryController.ParryState state;

	// Token: 0x04002E73 RID: 11891
	[SerializeField]
	public PlanePlayerParryEffect effect;

	// Token: 0x04002E76 RID: 11894
	public PlanePlayerParryEffect effectInstance;

	// Token: 0x04002E77 RID: 11895
	public float timeSinceParry;

	// Token: 0x020011E2 RID: 4578
	public enum ParryState
	{
		// Token: 0x04007CBB RID: 31931
		Init,
		// Token: 0x04007CBC RID: 31932
		Ready,
		// Token: 0x04007CBD RID: 31933
		Cooldown,
		// Token: 0x04007CBE RID: 31934
		Parrying,
		// Token: 0x04007CBF RID: 31935
		Disabled
	}
}
