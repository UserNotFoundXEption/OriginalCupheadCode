using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class CutsceneGUI : AbstractMonoBehaviour
{
	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x0000885B File Offset: 0x00006A5B
	// (set) Token: 0x06000902 RID: 2306 RVA: 0x00008862 File Offset: 0x00006A62
	public static CutsceneGUI Current { get; set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000903 RID: 2307 RVA: 0x0000886A File Offset: 0x00006A6A
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00008872 File Offset: 0x00006A72
	public override void Awake()
	{
		base.Awake();
		CutsceneGUI.Current = this;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0007709C File Offset: 0x0007529C
	public void Start()
	{
		this.uiCamera = Object.Instantiate<CupheadUICamera>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		this.canvas.worldCamera = this.uiCamera.camera;
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00008880 File Offset: 0x00006A80
	public void OnDestroy()
	{
		if (CutsceneGUI.Current == this)
		{
			CutsceneGUI.Current = null;
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00008898 File Offset: 0x00006A98
	public void CutseneInit()
	{
		this.pause.Init(false);
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x000770F8 File Offset: 0x000752F8
	public virtual void CutsceneSnapshot()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Cutscene.ToString(), 0.15f);
	}

	// Token: 0x040006D4 RID: 1748
	public const string PATH = "UI/Cutscene_UI";

	// Token: 0x040006D6 RID: 1750
	[SerializeField]
	public Canvas canvas;

	// Token: 0x040006D7 RID: 1751
	[SerializeField]
	public CutscenePauseGUI pause;

	// Token: 0x040006D8 RID: 1752
	[Space(10f)]
	[SerializeField]
	public CupheadUICamera uiCameraPrefab;

	// Token: 0x040006D9 RID: 1753
	public CupheadUICamera uiCamera;
}
