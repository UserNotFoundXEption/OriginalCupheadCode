using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000097 RID: 151
public class AudioManagerMixer : MonoBehaviour
{
	// Token: 0x06000745 RID: 1861 RVA: 0x000072A6 File Offset: 0x000054A6
	public static void Init()
	{
		if (AudioManagerMixer.Manager == null)
		{
			AudioManagerMixer.Manager = Resources.Load<AudioManagerMixer>("Audio/AudioMixer");
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x000072C7 File Offset: 0x000054C7
	public static AudioMixer GetMixer()
	{
		AudioManagerMixer.Init();
		return AudioManagerMixer.Manager.mixer;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x000072D8 File Offset: 0x000054D8
	public static AudioManagerMixer.Groups GetGroups()
	{
		AudioManagerMixer.Init();
		return AudioManagerMixer.Manager.audioGroups;
	}

	// Token: 0x04000595 RID: 1429
	public const string PATH = "Audio/AudioMixer";

	// Token: 0x04000596 RID: 1430
	public static AudioManagerMixer Manager;

	// Token: 0x04000597 RID: 1431
	[SerializeField]
	public AudioMixer mixer;

	// Token: 0x04000598 RID: 1432
	[SerializeField]
	public AudioManagerMixer.Groups audioGroups;

	// Token: 0x020008EB RID: 2283
	[Serializable]
	public class Groups
	{
		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x0003F29C File Offset: 0x0003D49C
		public AudioMixerGroup master
		{
			get
			{
				return this._master;
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060052E7 RID: 21223 RVA: 0x0003F2A4 File Offset: 0x0003D4A4
		public AudioMixerGroup master_Options
		{
			get
			{
				return this._master_Options;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060052E8 RID: 21224 RVA: 0x0003F2AC File Offset: 0x0003D4AC
		public AudioMixerGroup bgm_Options
		{
			get
			{
				return this._bgm_Options;
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060052E9 RID: 21225 RVA: 0x0003F2B4 File Offset: 0x0003D4B4
		public AudioMixerGroup sfx_Options
		{
			get
			{
				return this._sfx_Options;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060052EA RID: 21226 RVA: 0x0003F2BC File Offset: 0x0003D4BC
		public AudioMixerGroup bgm
		{
			get
			{
				return this._bgm;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060052EB RID: 21227 RVA: 0x0003F2C4 File Offset: 0x0003D4C4
		public AudioMixerGroup levelBgm
		{
			get
			{
				return this._levelBgm;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060052EC RID: 21228 RVA: 0x0003F2CC File Offset: 0x0003D4CC
		public AudioMixerGroup musicSting
		{
			get
			{
				return this._musicSting;
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x0003F2D4 File Offset: 0x0003D4D4
		public AudioMixerGroup sfx
		{
			get
			{
				return this._sfx;
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060052EE RID: 21230 RVA: 0x0003F2DC File Offset: 0x0003D4DC
		public AudioMixerGroup levelSfx
		{
			get
			{
				return this._levelSfx;
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060052EF RID: 21231 RVA: 0x0003F2E4 File Offset: 0x0003D4E4
		public AudioMixerGroup ambience
		{
			get
			{
				return this._ambience;
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060052F0 RID: 21232 RVA: 0x0003F2EC File Offset: 0x0003D4EC
		public AudioMixerGroup creatures
		{
			get
			{
				return this._creatures;
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060052F1 RID: 21233 RVA: 0x0003F2F4 File Offset: 0x0003D4F4
		public AudioMixerGroup announcer
		{
			get
			{
				return this._announcer;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060052F2 RID: 21234 RVA: 0x0003F2FC File Offset: 0x0003D4FC
		public AudioMixerGroup super
		{
			get
			{
				return this._super;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060052F3 RID: 21235 RVA: 0x0003F304 File Offset: 0x0003D504
		public AudioMixerGroup noise
		{
			get
			{
				return this._noise;
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060052F4 RID: 21236 RVA: 0x0003F30C File Offset: 0x0003D50C
		public AudioMixerGroup noiseConstant
		{
			get
			{
				return this._noiseConstant;
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060052F5 RID: 21237 RVA: 0x0003F314 File Offset: 0x0003D514
		public AudioMixerGroup noiseShortterm
		{
			get
			{
				return this._noiseShortterm;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060052F6 RID: 21238 RVA: 0x0003F31C File Offset: 0x0003D51C
		public AudioMixerGroup noise1920s
		{
			get
			{
				return this._noise1920s;
			}
		}

		// Token: 0x040043CA RID: 17354
		[SerializeField]
		public AudioMixerGroup _master;

		// Token: 0x040043CB RID: 17355
		[SerializeField]
		public AudioMixerGroup _master_Options;

		// Token: 0x040043CC RID: 17356
		[SerializeField]
		public AudioMixerGroup _bgm_Options;

		// Token: 0x040043CD RID: 17357
		[SerializeField]
		public AudioMixerGroup _sfx_Options;

		// Token: 0x040043CE RID: 17358
		[Space(10f)]
		[Header("BGM")]
		[SerializeField]
		public AudioMixerGroup _bgm;

		// Token: 0x040043CF RID: 17359
		[SerializeField]
		public AudioMixerGroup _levelBgm;

		// Token: 0x040043D0 RID: 17360
		[SerializeField]
		public AudioMixerGroup _musicSting;

		// Token: 0x040043D1 RID: 17361
		[Space(10f)]
		[Header("SFX")]
		[SerializeField]
		public AudioMixerGroup _sfx;

		// Token: 0x040043D2 RID: 17362
		[SerializeField]
		public AudioMixerGroup _levelSfx;

		// Token: 0x040043D3 RID: 17363
		[SerializeField]
		public AudioMixerGroup _ambience;

		// Token: 0x040043D4 RID: 17364
		[SerializeField]
		public AudioMixerGroup _creatures;

		// Token: 0x040043D5 RID: 17365
		[SerializeField]
		public AudioMixerGroup _announcer;

		// Token: 0x040043D6 RID: 17366
		[SerializeField]
		public AudioMixerGroup _super;

		// Token: 0x040043D7 RID: 17367
		[Space(10f)]
		[Header("Noise")]
		[SerializeField]
		public AudioMixerGroup _noise;

		// Token: 0x040043D8 RID: 17368
		[SerializeField]
		public AudioMixerGroup _noiseConstant;

		// Token: 0x040043D9 RID: 17369
		[SerializeField]
		public AudioMixerGroup _noiseShortterm;

		// Token: 0x040043DA RID: 17370
		[SerializeField]
		public AudioMixerGroup _noise1920s;
	}
}
