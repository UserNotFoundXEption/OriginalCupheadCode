using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public abstract class AbstractMapPlayerComponent : AbstractPausableComponent
{
	// Token: 0x060031BA RID: 12730 RVA: 0x0002956D File Offset: 0x0002776D
	public AbstractMapPlayerComponent()
	{
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x060031BB RID: 12731 RVA: 0x00029575 File Offset: 0x00027775
	// (set) Token: 0x060031BC RID: 12732 RVA: 0x0002957D File Offset: 0x0002777D
	public MapPlayerController player { get; set; }

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x060031BD RID: 12733 RVA: 0x00029586 File Offset: 0x00027786
	// (set) Token: 0x060031BE RID: 12734 RVA: 0x0002958E File Offset: 0x0002778E
	public PlayerInput input { get; set; }

	// Token: 0x060031BF RID: 12735 RVA: 0x00029597 File Offset: 0x00027797
	public override void Awake()
	{
		base.Awake();
		this.player = base.GetComponent<MapPlayerController>();
		this.input = base.GetComponent<PlayerInput>();
		this.RegisterEvents();
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x000295BD File Offset: 0x000277BD
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.UnregisterEvents();
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x000EAFFC File Offset: 0x000E91FC
	public void RegisterEvents()
	{
		this.player.LadderEnterEvent += this.OnLadderEnter;
		this.player.LadderEnterCompleteEvent += this.OnLadderEnterComplete;
		this.player.LadderExitEvent += this.OnLadderExit;
		this.player.LadderExitCompleteEvent += this.OnLadderExitComplete;
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x000EB06C File Offset: 0x000E926C
	public void UnregisterEvents()
	{
		this.player.LadderEnterEvent -= this.OnLadderEnter;
		this.player.LadderEnterCompleteEvent -= this.OnLadderEnterComplete;
		this.player.LadderExitEvent -= this.OnLadderExit;
		this.player.LadderExitCompleteEvent -= this.OnLadderExitComplete;
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x000295CB File Offset: 0x000277CB
	public virtual void OnLadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x000295CD File Offset: 0x000277CD
	public virtual void OnLadderExit(Vector2 point, Vector2 exit, MapLadder.Location location)
	{
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x000295CF File Offset: 0x000277CF
	public virtual void OnLadderEnterComplete()
	{
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x000295D1 File Offset: 0x000277D1
	public virtual void OnLadderExitComplete()
	{
	}
}
