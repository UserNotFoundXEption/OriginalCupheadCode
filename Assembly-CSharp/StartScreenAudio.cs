using System;
using Rewired;
using UnityEngine;

// Token: 0x020004DF RID: 1247
public class StartScreenAudio : AbstractMonoBehaviour
{
	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x060033A7 RID: 13223 RVA: 0x0002AC09 File Offset: 0x00028E09
	public static StartScreenAudio Instance
	{
		get
		{
			return StartScreenAudio.startScreenAudio;
		}
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x0002AC10 File Offset: 0x00028E10
	public void Start()
	{
		this.blockInput = CreditsScreen.goodEnding;
		this.players = new Player[]
		{
			PlayerManager.GetPlayerInput(PlayerId.PlayerOne),
			PlayerManager.GetPlayerInput(PlayerId.PlayerTwo)
		};
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x000F59E0 File Offset: 0x000F3BE0
	public void Update()
	{
		if (this.blockInput)
		{
			return;
		}
		if (this.codeIndex < this.code.Length)
		{
			foreach (Player player in this.players)
			{
				if (player.GetAnyButtonDown())
				{
					if (player.GetButtonDown((int)this.code[this.codeIndex]))
					{
						this.codeIndex++;
					}
					else if (!player.GetButtonDown((int)this.code[this.codeIndex]))
					{
						this.codeIndex = 0;
					}
				}
			}
		}
		else
		{
			if (this.bgmAlt2.clip == null)
			{
				this.bgmAlt2.GetComponent<DeferredAudioSource>().Initialize();
			}
			AudioManager.StopBGM();
			this.bgmAlt2.Play();
			this.blockInput = true;
		}
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x0002AC3B File Offset: 0x00028E3B
	public override void Awake()
	{
		base.Awake();
		StartScreenAudio.startScreenAudio = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x04002AE5 RID: 10981
	[SerializeField]
	public AudioSource bgmAlt2;

	// Token: 0x04002AE6 RID: 10982
	public static StartScreenAudio startScreenAudio;

	// Token: 0x04002AE7 RID: 10983
	public CupheadButton[] code = new CupheadButton[]
	{
		CupheadButton.MenuUp,
		CupheadButton.MenuUp,
		CupheadButton.MenuDown,
		CupheadButton.MenuDown,
		CupheadButton.MenuLeft,
		CupheadButton.MenuRight,
		CupheadButton.MenuLeft,
		CupheadButton.MenuRight,
		CupheadButton.Cancel,
		CupheadButton.Accept
	};

	// Token: 0x04002AE8 RID: 10984
	public int codeIndex;

	// Token: 0x04002AE9 RID: 10985
	public Player[] players;

	// Token: 0x04002AEA RID: 10986
	public bool blockInput;
}
