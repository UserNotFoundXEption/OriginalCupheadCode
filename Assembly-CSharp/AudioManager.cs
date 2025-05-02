using System;
using System.Collections.Generic;
using GCFreeUtils;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000094 RID: 148
public static class AudioManager
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060006C7 RID: 1735 RVA: 0x000706AC File Offset: 0x0006E8AC
	// (remove) Token: 0x060006C8 RID: 1736 RVA: 0x000706E0 File Offset: 0x0006E8E0
	public static event AudioManager.OnAttenuationHandler OnAttenuation;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060006C9 RID: 1737 RVA: 0x00070714 File Offset: 0x0006E914
	// (remove) Token: 0x060006CA RID: 1738 RVA: 0x00070748 File Offset: 0x0006E948
	public static event AudioManager.OnTransformHandler OnFollowObject;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060006CB RID: 1739 RVA: 0x0007077C File Offset: 0x0006E97C
	// (remove) Token: 0x060006CC RID: 1740 RVA: 0x000707B0 File Offset: 0x0006E9B0
	public static event AudioManager.OnPanEventHandler OnPanEvent;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060006CD RID: 1741 RVA: 0x000707E4 File Offset: 0x0006E9E4
	// (remove) Token: 0x060006CE RID: 1742 RVA: 0x00070818 File Offset: 0x0006EA18
	public static event AudioManager.OnSnapshotHandler OnSnapshotEvent;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x060006CF RID: 1743 RVA: 0x0007084C File Offset: 0x0006EA4C
	// (remove) Token: 0x060006D0 RID: 1744 RVA: 0x00070880 File Offset: 0x0006EA80
	public static event AudioManager.OnChangeBGMHandler OnBGMSlowdown;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x060006D1 RID: 1745 RVA: 0x000708B4 File Offset: 0x0006EAB4
	// (remove) Token: 0x060006D2 RID: 1746 RVA: 0x000708E8 File Offset: 0x0006EAE8
	public static event AudioManager.OnChangeBGMVolumeHandler OnBGMFadeVolume;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x060006D3 RID: 1747 RVA: 0x0007091C File Offset: 0x0006EB1C
	// (remove) Token: 0x060006D4 RID: 1748 RVA: 0x00070950 File Offset: 0x0006EB50
	public static event AudioManager.OnStartBGMAlternateHandler OnStartBGMAlternate;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x060006D5 RID: 1749 RVA: 0x00070984 File Offset: 0x0006EB84
	// (remove) Token: 0x060006D6 RID: 1750 RVA: 0x000709B8 File Offset: 0x0006EBB8
	public static event AudioManager.OnChangeSFXHandler OnSFXSlowDown;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x060006D7 RID: 1751 RVA: 0x000709EC File Offset: 0x0006EBEC
	// (remove) Token: 0x060006D8 RID: 1752 RVA: 0x00070A20 File Offset: 0x0006EC20
	public static event AudioManager.OnChangeSFXStartEndHandler OnSFXFadeVolume;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x060006D9 RID: 1753 RVA: 0x00070A54 File Offset: 0x0006EC54
	// (remove) Token: 0x060006DA RID: 1754 RVA: 0x00070A88 File Offset: 0x0006EC88
	public static event AudioManager.OnWarbleBGMPitchHandler OnBGMPitchWarble;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x060006DB RID: 1755 RVA: 0x00070ABC File Offset: 0x0006ECBC
	// (remove) Token: 0x060006DC RID: 1756 RVA: 0x00070AF0 File Offset: 0x0006ECF0
	public static event AudioManager.OnBGMPlayListManualHandler OnPlayManualBGM;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x060006DD RID: 1757 RVA: 0x00070B24 File Offset: 0x0006ED24
	// (remove) Token: 0x060006DE RID: 1758 RVA: 0x00070B58 File Offset: 0x0006ED58
	public static event AudioManager.OnSfxHandler OnPlayEvent;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x060006DF RID: 1759 RVA: 0x00070B8C File Offset: 0x0006ED8C
	// (remove) Token: 0x060006E0 RID: 1760 RVA: 0x00070BC0 File Offset: 0x0006EDC0
	public static event AudioManager.OnSfxHandler OnPlayLoopEvent;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x060006E1 RID: 1761 RVA: 0x00070BF4 File Offset: 0x0006EDF4
	// (remove) Token: 0x060006E2 RID: 1762 RVA: 0x00070C28 File Offset: 0x0006EE28
	public static event AudioManager.OnSfxHandler OnStopEvent;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x060006E3 RID: 1763 RVA: 0x00070C5C File Offset: 0x0006EE5C
	// (remove) Token: 0x060006E4 RID: 1764 RVA: 0x00070C90 File Offset: 0x0006EE90
	public static event AudioManager.OnSfxHandler OnPauseEvent;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060006E5 RID: 1765 RVA: 0x00070CC4 File Offset: 0x0006EEC4
	// (remove) Token: 0x060006E6 RID: 1766 RVA: 0x00070CF8 File Offset: 0x0006EEF8
	public static event AudioManager.OnSfxHandler OnUnpauseEvent;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060006E7 RID: 1767 RVA: 0x00070D2C File Offset: 0x0006EF2C
	// (remove) Token: 0x060006E8 RID: 1768 RVA: 0x00070D60 File Offset: 0x0006EF60
	public static event Action OnStopAllEvent;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060006E9 RID: 1769 RVA: 0x00070D94 File Offset: 0x0006EF94
	// (remove) Token: 0x060006EA RID: 1770 RVA: 0x00070DC8 File Offset: 0x0006EFC8
	public static event Action OnStopBGMEvent;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x060006EB RID: 1771 RVA: 0x00070DFC File Offset: 0x0006EFFC
	// (remove) Token: 0x060006EC RID: 1772 RVA: 0x00070E30 File Offset: 0x0006F030
	public static event Action OnPlayBGMEvent;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060006ED RID: 1773 RVA: 0x00070E64 File Offset: 0x0006F064
	// (remove) Token: 0x060006EE RID: 1774 RVA: 0x00070E98 File Offset: 0x0006F098
	public static event Action OnPlayBGMPlaylistEvent;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x060006EF RID: 1775 RVA: 0x00070ECC File Offset: 0x0006F0CC
	// (remove) Token: 0x060006F0 RID: 1776 RVA: 0x00070F00 File Offset: 0x0006F100
	public static event Action OnPauseAllSFXEvent;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x060006F1 RID: 1777 RVA: 0x00070F34 File Offset: 0x0006F134
	// (remove) Token: 0x060006F2 RID: 1778 RVA: 0x00070F68 File Offset: 0x0006F168
	public static event Action OnUnpauseAllSFXEvent;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x060006F3 RID: 1779 RVA: 0x00070F9C File Offset: 0x0006F19C
	// (remove) Token: 0x060006F4 RID: 1780 RVA: 0x00070FD0 File Offset: 0x0006F1D0
	public static event Action OnStopManualBGMTrackEvent;

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00006D6B File Offset: 0x00004F6B
	public static AudioMixer mixer
	{
		get
		{
			if (AudioManager._mixer == null)
			{
				AudioManager._mixer = AudioManagerMixer.GetMixer();
			}
			return AudioManager._mixer;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00071004 File Offset: 0x0006F204
	// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00071030 File Offset: 0x0006F230
	public static float sfxOptionsVolume
	{
		get
		{
			float result;
			AudioManager.mixer.GetFloat(AudioManager.Property.Options_SFXVolume.ToString(), ref result);
			return result;
		}
		set
		{
			AudioManager.mixer.SetFloat(AudioManager.Property.Options_SFXVolume.ToString(), value);
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00071058 File Offset: 0x0006F258
	// (set) Token: 0x060006F9 RID: 1785 RVA: 0x00071084 File Offset: 0x0006F284
	public static float bgmOptionsVolume
	{
		get
		{
			float result;
			AudioManager.mixer.GetFloat(AudioManager.Property.Options_BGMVolume.ToString(), ref result);
			return result;
		}
		set
		{
			AudioManager.mixer.SetFloat(AudioManager.Property.Options_BGMVolume.ToString(), value);
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060006FA RID: 1786 RVA: 0x000710AC File Offset: 0x0006F2AC
	// (set) Token: 0x060006FB RID: 1787 RVA: 0x000710D8 File Offset: 0x0006F2D8
	public static float masterVolume
	{
		get
		{
			float result;
			AudioManager.mixer.GetFloat(AudioManager.Property.MasterVolume.ToString(), ref result);
			return result;
		}
		set
		{
			AudioManager.mixer.SetFloat(AudioManager.Property.MasterVolume.ToString(), value);
		}
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00006D8C File Offset: 0x00004F8C
	public static bool CheckIfPlaying(string key)
	{
		AudioManager.checkIfPlaying = false;
		if (AudioManager.OnCheckEvent != null)
		{
			key = key.ToLowerIfNecessary();
			AudioManager.checkIfPlaying = AudioManager.OnCheckEvent.CallAnyTrue(key);
			return AudioManager.checkIfPlaying;
		}
		return false;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00006DBD File Offset: 0x00004FBD
	public static void PlayBGMPlaylistManually(bool goThroughPlaylistAfter)
	{
		if (AudioManager.OnPlayManualBGM != null)
		{
			AudioManager.OnPlayManualBGM(goThroughPlaylistAfter);
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00006DD4 File Offset: 0x00004FD4
	public static void StopBGMPlaylistManually()
	{
		if (AudioManager.OnStopManualBGMTrackEvent != null)
		{
			AudioManager.OnStopManualBGMTrackEvent();
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00006DEA File Offset: 0x00004FEA
	public static void ChangeSFXPitch(string key, float endPitch, float time)
	{
		if (AudioManager.OnSFXSlowDown != null)
		{
			AudioManager.OnSFXSlowDown(key, endPitch, time);
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00006E03 File Offset: 0x00005003
	public static void ChangeBGMPitch(float endPitch, float time)
	{
		if (AudioManager.OnBGMSlowdown != null)
		{
			AudioManager.OnBGMSlowdown(endPitch, time);
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00006E1B File Offset: 0x0000501B
	public static void FadeBGMVolume(float endVolume, float time, bool fadeOut)
	{
		if (AudioManager.OnBGMFadeVolume != null)
		{
			AudioManager.OnBGMFadeVolume(endVolume, time, fadeOut);
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00006E34 File Offset: 0x00005034
	public static void WarbleBGMPitch(int warbles, float[] minValue, float[] maxValue, float[] incrementTime, float[] playTime)
	{
		if (AudioManager.OnBGMPitchWarble != null)
		{
			AudioManager.OnBGMPitchWarble(warbles, minValue, maxValue, incrementTime, playTime);
		}
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00006E50 File Offset: 0x00005050
	public static void StartBGMAlternate(int index)
	{
		if (AudioManager.OnStartBGMAlternate != null)
		{
			AudioManager.OnStartBGMAlternate(index);
		}
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00006E67 File Offset: 0x00005067
	public static void Attenuation(string key, bool attenuation, float endVolume)
	{
		if (AudioManager.OnAttenuation != null)
		{
			AudioManager.OnAttenuation(key, attenuation, endVolume);
		}
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00006E80 File Offset: 0x00005080
	public static void Play(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPlayEvent != null)
		{
			AudioManager.OnPlayEvent(key);
		}
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00006E9F File Offset: 0x0000509F
	public static void Stop(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnStopEvent != null)
		{
			AudioManager.OnStopEvent(key);
		}
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00006EBE File Offset: 0x000050BE
	public static void PlayLoop(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPlayLoopEvent != null)
		{
			AudioManager.OnPlayLoopEvent(key);
		}
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00006EDD File Offset: 0x000050DD
	public static void Pause(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPauseEvent != null)
		{
			AudioManager.OnPauseEvent(key);
		}
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00006EFC File Offset: 0x000050FC
	public static void Unpaused(string key)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnUnpauseEvent != null)
		{
			AudioManager.OnUnpauseEvent(key);
		}
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00006F1B File Offset: 0x0000511B
	public static void Pan(string key, float value)
	{
		key = key.ToLowerIfNecessary();
		if (AudioManager.OnPanEvent != null)
		{
			AudioManager.OnPanEvent(key, value);
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00006F3B File Offset: 0x0000513B
	public static void FadeSFXVolume(string key, float endVolume, float time)
	{
		AudioManager.FadeSFXVolume(key, -1f, endVolume, time);
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00006F4A File Offset: 0x0000514A
	public static void FadeSFXVolume(string key, float startVolume, float endVolume, float time)
	{
		if (AudioManager.OnSFXFadeVolume != null)
		{
			AudioManager.OnSFXFadeVolume(key, startVolume, endVolume, time);
		}
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00006F64 File Offset: 0x00005164
	public static void FadeSFXVolumeLinear(string key, float endVolume, float time)
	{
		AudioManager.FadeSFXVolumeLinear(key, -1f, endVolume, time);
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00006F73 File Offset: 0x00005173
	public static void FadeSFXVolumeLinear(string key, float startVolume, float endVolume, float time)
	{
		if (AudioManager.OnSFXFadeVolumeLinear != null)
		{
			AudioManager.OnSFXFadeVolumeLinear(key, startVolume, endVolume, time);
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00071100 File Offset: 0x0006F300
	public static void FollowObject(IEnumerable<string> keys, Transform transform)
	{
		foreach (string text in keys)
		{
			AudioManager.FollowObject(keys, transform);
		}
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x00006F8D File Offset: 0x0000518D
	public static void FollowObject(string key, Transform transform)
	{
		key.ToLowerIfNecessary();
		if (AudioManager.OnFollowObject != null)
		{
			AudioManager.OnFollowObject(key, transform);
		}
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x00006FAC File Offset: 0x000051AC
	[Obsolete("Use Play(string key) instead")]
	public static void Play(Sfx sfx)
	{
		AudioManager.Play(sfx.ToString());
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00006FC0 File Offset: 0x000051C0
	[Obsolete("Use Stop(string key) instead")]
	public static void Stop(Sfx sfx)
	{
		AudioManager.Stop(sfx.ToString());
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00006FD4 File Offset: 0x000051D4
	public static void StopAll()
	{
		if (AudioManager.OnStopAllEvent != null)
		{
			AudioManager.OnStopAllEvent();
		}
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00006FEA File Offset: 0x000051EA
	public static void StopBGM()
	{
		if (AudioManager.OnStopBGMEvent != null)
		{
			AudioManager.OnStopBGMEvent();
		}
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00007000 File Offset: 0x00005200
	public static void PlayBGM()
	{
		if (AudioManager.OnPlayBGMEvent != null)
		{
			AudioManager.OnPlayBGMEvent();
		}
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00007016 File Offset: 0x00005216
	public static void PlaylistBGM()
	{
		if (AudioManager.OnPlayBGMPlaylistEvent != null)
		{
			AudioManager.OnPlayBGMPlaylistEvent();
		}
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0000702C File Offset: 0x0000522C
	public static void PauseAllSFX()
	{
		if (AudioManager.OnPauseAllSFXEvent != null)
		{
			AudioManager.OnPauseAllSFXEvent();
		}
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x00007042 File Offset: 0x00005242
	public static void UnpauseAllSFX()
	{
		if (AudioManager.OnUnpauseAllSFXEvent != null)
		{
			AudioManager.OnUnpauseAllSFXEvent();
		}
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00007058 File Offset: 0x00005258
	public static void SnapshotTransition(string[] snapshotNames, float[] weights, float time)
	{
		if (AudioManager.OnSnapshotEvent != null)
		{
			AudioManager.OnSnapshotEvent(snapshotNames, weights, time);
		}
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00071154 File Offset: 0x0006F354
	public static void HandleSnapshot(string snapshot, float time)
	{
		string[] array = new string[]
		{
			AudioManager.Snapshots.Cutscene.ToString(),
			AudioManager.Snapshots.FrontEnd.ToString(),
			AudioManager.Snapshots.Unpaused.ToString(),
			AudioManager.Snapshots.Unpaused_Clean.ToString(),
			AudioManager.Snapshots.Unpaused_1920s.ToString(),
			AudioManager.Snapshots.Loadscreen.ToString(),
			AudioManager.Snapshots.Paused.ToString(),
			AudioManager.Snapshots.Super.ToString(),
			AudioManager.Snapshots.SuperStart.ToString(),
			AudioManager.Snapshots.Death.ToString(),
			AudioManager.Snapshots.EquipMenu.ToString(),
			AudioManager.Snapshots.RumRunners_RedBeam.ToString(),
			AudioManager.Snapshots.RumRunners_GreenBeam.ToString(),
			AudioManager.Snapshots.RumRunners_YellowBeam.ToString()
		};
		float[] array2 = new float[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = ((!(array[i] == snapshot)) ? 0f : 1f);
		}
		AudioManager.SnapshotTransition(array, array2, time);
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x000712C8 File Offset: 0x0006F4C8
	public static void SnapshotReset(string sceneName, float time)
	{
		string[] array = new string[]
		{
			AudioManager.Snapshots.Cutscene.ToString(),
			AudioManager.Snapshots.FrontEnd.ToString(),
			AudioManager.Snapshots.Unpaused.ToString(),
			AudioManager.Snapshots.Unpaused_Clean.ToString(),
			AudioManager.Snapshots.Unpaused_1920s.ToString(),
			AudioManager.Snapshots.Loadscreen.ToString(),
			AudioManager.Snapshots.Paused.ToString(),
			AudioManager.Snapshots.Super.ToString(),
			AudioManager.Snapshots.SuperStart.ToString(),
			AudioManager.Snapshots.Death.ToString(),
			AudioManager.Snapshots.EquipMenu.ToString(),
			AudioManager.Snapshots.RumRunners_RedBeam.ToString(),
			AudioManager.Snapshots.RumRunners_GreenBeam.ToString(),
			AudioManager.Snapshots.RumRunners_YellowBeam.ToString()
		};
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (SettingsData.Data.vintageAudioEnabled)
			{
				if (array[i] == AudioManager.Snapshots.Unpaused_1920s.ToString())
				{
					num = i;
				}
			}
			else if (sceneName == Scenes.scene_level_retro_arcade.ToString())
			{
				if (array[i] == AudioManager.Snapshots.Unpaused_Clean.ToString())
				{
					num = i;
				}
			}
			else if (array[i] == AudioManager.Snapshots.Unpaused.ToString())
			{
				num = i;
			}
		}
		float[] array2 = new float[array.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array2[j] = ((j != num) ? 0f : 1f);
		}
		AudioManager.SnapshotTransition(array, array2, time);
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x0600071C RID: 1820 RVA: 0x000714EC File Offset: 0x0006F6EC
	// (remove) Token: 0x0600071D RID: 1821 RVA: 0x00071520 File Offset: 0x0006F720
	public static event AudioManager.OnChangeSFXStartEndHandler OnSFXFadeVolumeLinear;

	// Token: 0x04000504 RID: 1284
	public const float VOLUME_MAX = 0f;

	// Token: 0x04000505 RID: 1285
	public const float VOLUME_MIN = -80f;

	// Token: 0x04000506 RID: 1286
	public static GCFreePredicateList<string> OnCheckEvent = new GCFreePredicateList<string>(10, true);

	// Token: 0x0400051F RID: 1311
	public static AudioMixer _mixer;

	// Token: 0x04000520 RID: 1312
	public static bool checkIfPlaying;

	// Token: 0x020008D8 RID: 2264
	public enum Channel
	{
		// Token: 0x040043A3 RID: 17315
		Default,
		// Token: 0x040043A4 RID: 17316
		Level
	}

	// Token: 0x020008D9 RID: 2265
	public enum Property
	{
		// Token: 0x040043A6 RID: 17318
		MasterVolume,
		// Token: 0x040043A7 RID: 17319
		Options_BGMVolume,
		// Token: 0x040043A8 RID: 17320
		Options_SFXVolume
	}

	// Token: 0x020008DA RID: 2266
	public enum Snapshots
	{
		// Token: 0x040043AA RID: 17322
		Cutscene,
		// Token: 0x040043AB RID: 17323
		FrontEnd,
		// Token: 0x040043AC RID: 17324
		Unpaused,
		// Token: 0x040043AD RID: 17325
		Unpaused_Clean,
		// Token: 0x040043AE RID: 17326
		Unpaused_1920s,
		// Token: 0x040043AF RID: 17327
		Loadscreen,
		// Token: 0x040043B0 RID: 17328
		Paused,
		// Token: 0x040043B1 RID: 17329
		Super,
		// Token: 0x040043B2 RID: 17330
		SuperStart,
		// Token: 0x040043B3 RID: 17331
		Death,
		// Token: 0x040043B4 RID: 17332
		EquipMenu,
		// Token: 0x040043B5 RID: 17333
		RumRunners_RedBeam,
		// Token: 0x040043B6 RID: 17334
		RumRunners_GreenBeam,
		// Token: 0x040043B7 RID: 17335
		RumRunners_YellowBeam
	}

	// Token: 0x020008DB RID: 2267
	// (Invoke) Token: 0x06005293 RID: 21139
	public delegate bool OnCheckIfPlaying(string key);

	// Token: 0x020008DC RID: 2268
	// (Invoke) Token: 0x06005297 RID: 21143
	public delegate void OnSfxHandler(string key);

	// Token: 0x020008DD RID: 2269
	// (Invoke) Token: 0x0600529B RID: 21147
	public delegate void OnTransformHandler(string key, Transform transform);

	// Token: 0x020008DE RID: 2270
	// (Invoke) Token: 0x0600529F RID: 21151
	public delegate void OnAttenuationHandler(string key, bool attenuation, float endVolume);

	// Token: 0x020008DF RID: 2271
	// (Invoke) Token: 0x060052A3 RID: 21155
	public delegate void OnChangeBGMHandler(float end, float time);

	// Token: 0x020008E0 RID: 2272
	// (Invoke) Token: 0x060052A7 RID: 21159
	public delegate void OnChangeBGMVolumeHandler(float end, float time, bool fadeOut);

	// Token: 0x020008E1 RID: 2273
	// (Invoke) Token: 0x060052AB RID: 21163
	public delegate void OnStartBGMAlternateHandler(int index);

	// Token: 0x020008E2 RID: 2274
	// (Invoke) Token: 0x060052AF RID: 21167
	public delegate void OnChangeSFXHandler(string key, float end, float time);

	// Token: 0x020008E3 RID: 2275
	// (Invoke) Token: 0x060052B3 RID: 21171
	public delegate void OnChangeSFXStartEndHandler(string key, float start, float end, float time);

	// Token: 0x020008E4 RID: 2276
	// (Invoke) Token: 0x060052B7 RID: 21175
	public delegate void OnWarbleBGMPitchHandler(int warbles, float[] minValue, float[] maxValue, float[] warbleTime, float[] playTime);

	// Token: 0x020008E5 RID: 2277
	// (Invoke) Token: 0x060052BB RID: 21179
	public delegate void OnSnapshotHandler(string[] names, float[] weight, float time);

	// Token: 0x020008E6 RID: 2278
	// (Invoke) Token: 0x060052BF RID: 21183
	public delegate void OnPanEventHandler(string key, float value);

	// Token: 0x020008E7 RID: 2279
	// (Invoke) Token: 0x060052C3 RID: 21187
	public delegate void OnBGMPlayListManualHandler(bool loopPlayListAfter);
}
