using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x02000659 RID: 1625
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x0600449C RID: 17564 RVA: 0x00036784 File Offset: 0x00034984
	public static SteamManager Instance
	{
		get
		{
			return SteamManager.s_instance ?? new GameObject("SteamManager").AddComponent<SteamManager>();
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x0600449D RID: 17565 RVA: 0x000367A1 File Offset: 0x000349A1
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x0600449E RID: 17566 RVA: 0x000367AD File Offset: 0x000349AD
	public static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
	}

	// Token: 0x0600449F RID: 17567 RVA: 0x0013D3D8 File Offset: 0x0013B5D8
	public void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)268910u))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInialized = true;
	}

	// Token: 0x060044A0 RID: 17568 RVA: 0x0013D4C0 File Offset: 0x0013B6C0
	public void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x000367AF File Offset: 0x000349AF
	public void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x000367D9 File Offset: 0x000349D9
	public void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04003528 RID: 13608
	public static SteamManager s_instance;

	// Token: 0x04003529 RID: 13609
	public static bool s_EverInialized;

	// Token: 0x0400352A RID: 13610
	public bool m_bInitialized;

	// Token: 0x0400352B RID: 13611
	public SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
