using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004FE RID: 1278
public class ArcadePlayerParryController : AbstractArcadePlayerComponent
{
	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06003542 RID: 13634 RVA: 0x0002BB7A File Offset: 0x00029D7A
	public ArcadePlayerParryController.ParryState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x06003543 RID: 13635 RVA: 0x000F99DC File Offset: 0x000F7BDC
	// (remove) Token: 0x06003544 RID: 13636 RVA: 0x000F9A14 File Offset: 0x000F7C14
	public event Action OnParryStartEvent;

	// Token: 0x1400007F RID: 127
	// (add) Token: 0x06003545 RID: 13637 RVA: 0x000F9A4C File Offset: 0x000F7C4C
	// (remove) Token: 0x06003546 RID: 13638 RVA: 0x000F9A84 File Offset: 0x000F7C84
	public event Action OnParryEndEvent;

	// Token: 0x06003547 RID: 13639 RVA: 0x0002BB82 File Offset: 0x00029D82
	public void Start()
	{
		base.player.motor.OnParryEvent += this.StartParry;
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x0002BBA0 File Offset: 0x00029DA0
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		this.state = ArcadePlayerParryController.ParryState.Ready;
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x0002BBAF File Offset: 0x00029DAF
	public void StartParry()
	{
		this.state = ArcadePlayerParryController.ParryState.Parrying;
		if (this.OnParryStartEvent != null)
		{
			this.OnParryStartEvent();
		}
		base.StartCoroutine(this.parry_cr());
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x000F9ABC File Offset: 0x000F7CBC
	public IEnumerator parry_cr()
	{
		this.effect.Create(base.player);
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.state = ArcadePlayerParryController.ParryState.Ready;
		if (this.OnParryEndEvent != null)
		{
			this.OnParryEndEvent();
		}
		yield break;
	}

	// Token: 0x04002B71 RID: 11121
	public const float DURATION = 0.2f;

	// Token: 0x04002B72 RID: 11122
	public ArcadePlayerParryController.ParryState state;

	// Token: 0x04002B73 RID: 11123
	[SerializeField]
	public ArcadePlayerParryEffect effect;

	// Token: 0x0200116B RID: 4459
	public enum ParryState
	{
		// Token: 0x04007A62 RID: 31330
		Init,
		// Token: 0x04007A63 RID: 31331
		Ready,
		// Token: 0x04007A64 RID: 31332
		Parrying
	}
}
