using System;

// Token: 0x02000108 RID: 264
public class AbstractLevelHUDComponent : AbstractMonoBehaviour
{
	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0000AD3C File Offset: 0x00008F3C
	// (set) Token: 0x06000C5C RID: 3164 RVA: 0x0000AD44 File Offset: 0x00008F44
	public LevelHUDPlayer _hud { get; set; }

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0000AD4D File Offset: 0x00008F4D
	public AbstractPlayerController _player
	{
		get
		{
			return this._hud.player;
		}
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x0000AD5A File Offset: 0x00008F5A
	public override void Awake()
	{
		base.Awake();
		this.ignoreGlobalTime = true;
		this.timeLayer = CupheadTime.Layer.UI;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0000AD70 File Offset: 0x00008F70
	public void Start()
	{
		if (this._parentToHudCanvas)
		{
			base.transform.SetParent(LevelHUD.Current.Canvas.transform, false);
		}
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0000AD98 File Offset: 0x00008F98
	public virtual void Init(LevelHUDPlayer hud)
	{
		this._hud = hud;
	}

	// Token: 0x040009D9 RID: 2521
	public bool _parentToHudCanvas;
}
