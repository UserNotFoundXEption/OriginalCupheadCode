using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000515 RID: 1301
public class LevelPlayerParryController : AbstractLevelPlayerComponent, IParryAttack
{
	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x060036EE RID: 14062 RVA: 0x0002CE75 File Offset: 0x0002B075
	public LevelPlayerParryController.ParryState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x14000093 RID: 147
	// (add) Token: 0x060036EF RID: 14063 RVA: 0x001017D4 File Offset: 0x000FF9D4
	// (remove) Token: 0x060036F0 RID: 14064 RVA: 0x0010180C File Offset: 0x000FFA0C
	public event Action OnParryStartEvent;

	// Token: 0x14000094 RID: 148
	// (add) Token: 0x060036F1 RID: 14065 RVA: 0x00101844 File Offset: 0x000FFA44
	// (remove) Token: 0x060036F2 RID: 14066 RVA: 0x0010187C File Offset: 0x000FFA7C
	public event Action OnParryEndEvent;

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x060036F3 RID: 14067 RVA: 0x0002CE7D File Offset: 0x0002B07D
	// (set) Token: 0x060036F4 RID: 14068 RVA: 0x0002CE85 File Offset: 0x0002B085
	public bool AttackParryUsed { get; set; }

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x060036F5 RID: 14069 RVA: 0x0002CE8E File Offset: 0x0002B08E
	// (set) Token: 0x060036F6 RID: 14070 RVA: 0x0002CE96 File Offset: 0x0002B096
	public bool HasHitEnemy { get; set; }

	// Token: 0x060036F7 RID: 14071 RVA: 0x0002CE9F File Offset: 0x0002B09F
	public void Start()
	{
		base.player.motor.OnParryEvent += this.StartParry;
		base.player.motor.OnGroundedEvent += this.OnGround;
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x0002CED9 File Offset: 0x0002B0D9
	public void OnGround()
	{
		this.AttackParryUsed = false;
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x0002CEE2 File Offset: 0x0002B0E2
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		this.state = LevelPlayerParryController.ParryState.Ready;
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x0002CEF1 File Offset: 0x0002B0F1
	public void StartParry()
	{
		this.state = LevelPlayerParryController.ParryState.Parrying;
		if (this.OnParryStartEvent != null)
		{
			this.OnParryStartEvent();
		}
		base.StartCoroutine(this.parry_cr());
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x001018B4 File Offset: 0x000FFAB4
	public IEnumerator parry_cr()
	{
		this.effect.Create(base.player);
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.state = LevelPlayerParryController.ParryState.Ready;
		if (this.OnParryEndEvent != null)
		{
			this.OnParryEndEvent();
		}
		this.HasHitEnemy = false;
		yield break;
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x0002CF1D File Offset: 0x0002B11D
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.effect = null;
	}

	// Token: 0x04002C58 RID: 11352
	public const float DURATION = 0.2f;

	// Token: 0x04002C59 RID: 11353
	public LevelPlayerParryController.ParryState state;

	// Token: 0x04002C5A RID: 11354
	[SerializeField]
	public LevelPlayerParryEffect effect;

	// Token: 0x0200119E RID: 4510
	public enum ParryState
	{
		// Token: 0x04007B6D RID: 31597
		Init,
		// Token: 0x04007B6E RID: 31598
		Ready,
		// Token: 0x04007B6F RID: 31599
		Parrying
	}
}
