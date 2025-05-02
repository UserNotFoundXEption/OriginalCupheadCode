using System;
using Rewired;
using Rewired.UI.ControlMapper;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class Cuphead : AbstractMonoBehaviour
{
	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000828 RID: 2088 RVA: 0x00007DB1 File Offset: 0x00005FB1
	// (set) Token: 0x06000829 RID: 2089 RVA: 0x00007DB8 File Offset: 0x00005FB8
	public static Cuphead Current { get; set; }

	// Token: 0x0600082A RID: 2090 RVA: 0x00075A78 File Offset: 0x00073C78
	public static void Init(bool lightInit = false)
	{
		if (Cuphead.Current == null)
		{
			Object.Instantiate<Cuphead>(Resources.Load<Cuphead>("Core/CupheadCore"));
		}
		else
		{
			if (!Cuphead.didLightInit)
			{
				return;
			}
			Cuphead.didLightInit = false;
		}
		if (lightInit)
		{
			Cuphead.didLightInit = true;
		}
		else
		{
			Cuphead.Current.rewired.gameObject.SetActive(true);
			Cuphead.Current.eventSystem.gameObject.SetActive(true);
			Cuphead.Current.controlMapper.gameObject.SetActive(true);
			PlayerManager.Awake();
			if (!PlatformHelper.PreloadSettingsData)
			{
				OnlineManager.Instance.Init();
			}
			PlmManager.Instance.Init();
			PlayerManager.Init();
			Cuphead.didFullInit = true;
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x0600082B RID: 2091 RVA: 0x00007DC0 File Offset: 0x00005FC0
	public ScoringEditorData ScoringProperties
	{
		get
		{
			return this.scoringProperties;
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x0600082C RID: 2092 RVA: 0x00007DC8 File Offset: 0x00005FC8
	// (set) Token: 0x0600082D RID: 2093 RVA: 0x00007DD0 File Offset: 0x00005FD0
	public AchievementToastManager achievementToastManager { get; set; }

	// Token: 0x0600082E RID: 2094 RVA: 0x00075B40 File Offset: 0x00073D40
	public override void Awake()
	{
		base.Awake();
		if (Cuphead.Current == null)
		{
			Cuphead.Current = this;
			base.gameObject.name = base.gameObject.name.Replace("(Clone)", string.Empty);
			Object.DontDestroyOnLoad(base.gameObject);
			this.noiseHandler = Object.Instantiate<AudioNoiseHandler>(this.noiseHandler);
			this.noiseHandler.transform.SetParent(base.transform);
			bool hasBootedUpGame = SettingsData.Data.hasBootedUpGame;
			if (PlatformHelper.ShowAchievements)
			{
				this.achievementToastManager = Object.Instantiate<AchievementToastManager>(this.achievementToastManagerPrefab);
				this.achievementToastManager.transform.SetParent(base.transform);
			}
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00007DD9 File Offset: 0x00005FD9
	public void OnDestroy()
	{
		if (Cuphead.Current == this)
		{
			Cuphead.Current = null;
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00007DF1 File Offset: 0x00005FF1
	public void Update()
	{
		if (Cuphead.didFullInit)
		{
			PlayerManager.Update();
		}
		Cursor.visible = !Screen.fullScreen;
	}

	// Token: 0x0400063C RID: 1596
	public const string PATH = "Core/CupheadCore";

	// Token: 0x0400063D RID: 1597
	public static bool didLightInit;

	// Token: 0x0400063E RID: 1598
	public static bool didFullInit;

	// Token: 0x04000640 RID: 1600
	[SerializeField]
	public AudioNoiseHandler noiseHandler;

	// Token: 0x04000641 RID: 1601
	[SerializeField]
	public InputManager rewired;

	// Token: 0x04000642 RID: 1602
	public ControlMapper controlMapper;

	// Token: 0x04000643 RID: 1603
	[SerializeField]
	public CupheadEventSystem eventSystem;

	// Token: 0x04000644 RID: 1604
	[SerializeField]
	public CupheadRenderer renderer;

	// Token: 0x04000645 RID: 1605
	[SerializeField]
	public ScoringEditorData scoringProperties;

	// Token: 0x04000646 RID: 1606
	[SerializeField]
	public AchievementToastManager achievementToastManagerPrefab;
}
