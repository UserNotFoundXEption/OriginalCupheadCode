using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000096 RID: 150
public class AudioManagerComponent : AbstractMonoBehaviour
{
	// Token: 0x06000720 RID: 1824 RVA: 0x00071554 File Offset: 0x0006F754
	public override void Awake()
	{
		base.Awake();
		this.SetChannels();
		this.dict = new Dictionary<string, AudioManagerComponent.SoundGroup>();
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Init(true);
			this.dict[soundGroup.key.ToLowerIfNecessary()] = soundGroup;
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup2 in this.bgmPlaylist)
		{
			soundGroup2.Init(true);
			this.dict[soundGroup2.key.ToLowerIfNecessary()] = soundGroup2;
		}
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmAlternates)
		{
			source.Init(true);
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmSources)
		{
			source2.Init(true);
		}
		this.AddEvents();
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x000070AC File Offset: 0x000052AC
	public void OnDestroy()
	{
		this.RemoveEvents();
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x000716E4 File Offset: 0x0006F8E4
	public void OnValidate()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			if (string.IsNullOrEmpty(soundGroup.key))
			{
				soundGroup.key = soundGroup.trigger.ToString();
			}
			soundGroup.key = soundGroup.key.ToLower();
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup2 in this.bgmPlaylist)
		{
			if (string.IsNullOrEmpty(soundGroup2.key))
			{
				soundGroup2.key = soundGroup2.trigger.ToString();
			}
			soundGroup2.key = soundGroup2.key.ToLower();
		}
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x000717F0 File Offset: 0x0006F9F0
	public void AddEvents()
	{
		AudioManager.OnPlayBGMEvent += this.StartBGM;
		AudioManager.OnPlayBGMPlaylistEvent += this.StartBGMPlaylist;
		AudioManager.OnSnapshotEvent += this.SnapshotTransition;
		AudioManager.OnCheckEvent.Add(new Predicate<string>(this.OnIsPlaying));
		AudioManager.OnPlayEvent += this.OnPlay;
		AudioManager.OnPlayLoopEvent += this.OnPlayLoop;
		AudioManager.OnStopEvent += this.OnStop;
		AudioManager.OnPauseEvent += this.OnPause;
		AudioManager.OnUnpauseEvent += this.OnUnpause;
		AudioManager.OnFollowObject += this.OnFollowOject;
		AudioManager.OnPanEvent += this.OnPan;
		AudioManager.OnStopAllEvent += this.OnStopAll;
		AudioManager.OnStopBGMEvent += this.OnStopBGM;
		AudioManager.OnPauseAllSFXEvent += this.OnPauseAllSFX;
		AudioManager.OnUnpauseAllSFXEvent += this.OnUnpauseAllSFX;
		AudioManager.OnBGMSlowdown += this.OnBGMSlowdown;
		AudioManager.OnSFXSlowDown += this.OnSFXSlowDown;
		AudioManager.OnSFXFadeVolume += this.OnSFXVolume;
		AudioManager.OnSFXFadeVolumeLinear += this.OnSFXVolumeLinear;
		AudioManager.OnBGMPitchWarble += this.OnBGMWarblePitch;
		AudioManager.OnAttenuation += this.OnAttenuation;
		AudioManager.OnPlayManualBGM += this.PlayManualBGMTrack;
		AudioManager.OnStopManualBGMTrackEvent += this.StopManualBGMTrack;
		AudioManager.OnBGMFadeVolume += this.OnBGMVolumeFade;
		AudioManager.OnStartBGMAlternate += this.StartBGMAlternate;
		if (this.autoplayBGM)
		{
			SceneLoader.OnLoaderCompleteEvent += this.StartBGM;
		}
		if (this.autoplayBGMPlaylist)
		{
			SceneLoader.OnLoaderCompleteEvent += this.StartBGMPlaylist;
		}
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x000719E4 File Offset: 0x0006FBE4
	public void RemoveEvents()
	{
		AudioManager.OnPlayBGMEvent -= this.StartBGM;
		AudioManager.OnPlayBGMPlaylistEvent -= this.StartBGMPlaylist;
		AudioManager.OnSnapshotEvent -= this.SnapshotTransition;
		AudioManager.OnCheckEvent.Remove(new Predicate<string>(this.OnIsPlaying));
		AudioManager.OnPlayEvent -= this.OnPlay;
		AudioManager.OnPlayLoopEvent -= this.OnPlayLoop;
		AudioManager.OnStopEvent -= this.OnStop;
		AudioManager.OnPauseEvent -= this.OnPause;
		AudioManager.OnUnpauseEvent -= this.OnUnpause;
		AudioManager.OnFollowObject -= this.OnFollowOject;
		AudioManager.OnPanEvent -= this.OnPan;
		AudioManager.OnStopAllEvent -= this.OnStopAll;
		AudioManager.OnStopBGMEvent -= this.OnStopBGM;
		AudioManager.OnPauseAllSFXEvent -= this.OnPauseAllSFX;
		AudioManager.OnUnpauseAllSFXEvent -= this.OnUnpauseAllSFX;
		AudioManager.OnBGMSlowdown -= this.OnBGMSlowdown;
		AudioManager.OnSFXSlowDown -= this.OnSFXSlowDown;
		AudioManager.OnSFXFadeVolume -= this.OnSFXVolume;
		AudioManager.OnSFXFadeVolumeLinear -= this.OnSFXVolumeLinear;
		AudioManager.OnBGMPitchWarble -= this.OnBGMWarblePitch;
		AudioManager.OnAttenuation -= this.OnAttenuation;
		AudioManager.OnPlayManualBGM -= this.PlayManualBGMTrack;
		AudioManager.OnStopManualBGMTrackEvent -= this.StopManualBGMTrack;
		AudioManager.OnBGMFadeVolume -= this.OnBGMVolumeFade;
		AudioManager.OnStartBGMAlternate -= this.StartBGMAlternate;
		if (this.autoplayBGM)
		{
			SceneLoader.OnLoaderCompleteEvent -= this.StartBGM;
		}
		if (this.autoplayBGMPlaylist)
		{
			SceneLoader.OnLoaderCompleteEvent -= this.StartBGMPlaylist;
		}
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00071BD8 File Offset: 0x0006FDD8
	public void Update()
	{
		for (int i = 0; i < this.sounds.Count; i++)
		{
			AudioManagerComponent.SoundGroup soundGroup = this.sounds[i];
			if (soundGroup.emissionTransform != null)
			{
				soundGroup.FollowObject(soundGroup.emissionTransform.position);
			}
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00071C30 File Offset: 0x0006FE30
	public void StartBGM()
	{
		this.StopBGM();
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			if (source.noLoop)
			{
				source.Play();
			}
			else
			{
				source.PlayLooped();
			}
		}
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00071CA8 File Offset: 0x0006FEA8
	public void StartBGMAlternate(int index)
	{
		this.StopBGM();
		if (this.bgmAlternates.Count > index && this.bgmAlternates[index] != null)
		{
			if (this.bgmAlternates[index].noLoop)
			{
				this.bgmAlternates[index].Play();
			}
			else
			{
				this.bgmAlternates[index].PlayLooped();
			}
		}
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00071D1C File Offset: 0x0006FF1C
	public void StopBGM()
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			source.Stop();
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmAlternates)
		{
			source2.Stop();
		}
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x000070B4 File Offset: 0x000052B4
	public void OnLevelStart()
	{
		this.StartBGM();
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x000070BC File Offset: 0x000052BC
	public void OnStopBGM()
	{
		this.StopBGM();
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00071DC4 File Offset: 0x0006FFC4
	public void OnBGMSlowdown(float end, float time)
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			base.StartCoroutine(source.change_pitch_cr(end, time));
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmAlternates)
		{
			base.StartCoroutine(source2.change_pitch_cr(end, time));
		}
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].CheckIfPlaying())
			{
				base.StartCoroutine(this.bgmPlaylist[i].change_pitch_sfx(end, time));
			}
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00071ED0 File Offset: 0x000700D0
	public void OnBGMVolumeFade(float end, float time, bool onFadeout)
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			if ((source.isPlaying() && onFadeout) || (!onFadeout && source.isFadedOut))
			{
				base.StartCoroutine(source.change_volume_cr(end, time, onFadeout));
			}
		}
		foreach (AudioManagerComponent.SoundGroup.Source source2 in this.bgmAlternates)
		{
			if ((source2.isPlaying() && onFadeout) || (!onFadeout && source2.isFadedOut))
			{
				base.StartCoroutine(source2.change_volume_cr(end, time, onFadeout));
			}
		}
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if ((this.bgmPlaylist[i].CheckIfPlaying() && onFadeout) || (!onFadeout && this.bgmPlaylist[i].isFadedOut))
			{
				base.StartCoroutine(this.bgmPlaylist[i].change_volume_cr(end, time, onFadeout));
			}
		}
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00072044 File Offset: 0x00070244
	public void OnBGMWarblePitch(int warbles, float[] minValue, float[] maxValue, float[] warbleTime, float[] playTime)
	{
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			base.StartCoroutine(source.warble_pitch_cr(warbles, minValue, maxValue, warbleTime, playTime));
		}
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].CheckIfPlaying())
			{
				base.StartCoroutine(this.bgmPlaylist[i].warble_pitch_cr(warbles, minValue, maxValue, warbleTime, playTime));
			}
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00072100 File Offset: 0x00070300
	public void PlayManualBGMTrack(bool loopPlayListAfter)
	{
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].activatedManually)
			{
				if (loopPlayListAfter)
				{
					this.bgmPlaylist[i].Play();
					base.StartCoroutine(this.handle_cr(this.bgmPlaylist[i].ClipLength()));
				}
				else
				{
					this.bgmPlaylist[i].PlayLoop();
				}
			}
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0007218C File Offset: 0x0007038C
	public IEnumerator handle_cr(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);
		this.StartBGMPlaylist();
		yield return null;
		yield break;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x000721B0 File Offset: 0x000703B0
	public void StopManualBGMTrack()
	{
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (this.bgmPlaylist[i].activatedManually)
			{
				this.bgmPlaylist[i].Stop();
			}
		}
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x00072200 File Offset: 0x00070400
	public void StartBGMPlaylist()
	{
		bool flag = true;
		for (int i = 0; i < this.bgmPlaylist.Count; i++)
		{
			if (!this.bgmPlaylist[i].activatedManually)
			{
				flag = false;
			}
		}
		if (flag)
		{
			return;
		}
		this.StopBGM();
		if (this.bgmPlaylist.Count > 0)
		{
			PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(SceneLoader.CurrentLevel);
			levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) % this.bgmPlaylist.Count;
			base.StartCoroutine(this.play_track_cr());
		}
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00072298 File Offset: 0x00070498
	public IEnumerator play_track_cr()
	{
		PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(SceneLoader.CurrentLevel);
		for (;;)
		{
			while (this.bgmPlaylist[levelData.bgmPlayListCurrent].activatedManually)
			{
				levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) % this.bgmPlaylist.Count;
				yield return new WaitForFixedUpdate();
			}
			this.bgmPlaylist[levelData.bgmPlayListCurrent].Play();
			yield return new WaitForSeconds(this.bgmPlaylist[levelData.bgmPlayListCurrent].ClipLength());
			levelData.bgmPlayListCurrent = (levelData.bgmPlayListCurrent + 1) % this.bgmPlaylist.Count;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x000070C4 File Offset: 0x000052C4
	public void OnPlay(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			if (AudioManagerComponent.ShowAudioVariations || AudioManagerComponent.ShowAudioPlaying)
			{
			}
			this.dict[key].Play();
		}
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x000070FC File Offset: 0x000052FC
	public void OnPlayLoop(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			if (AudioManagerComponent.ShowAudioVariations || AudioManagerComponent.ShowAudioPlaying)
			{
			}
			this.dict[key].PlayLoop();
		}
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x00007134 File Offset: 0x00005334
	public void OnStop(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Stop();
		}
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00007158 File Offset: 0x00005358
	public void OnPause(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Pause();
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x0000717C File Offset: 0x0000537C
	public void OnUnpause(string key)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Unpause();
		}
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x000722B4 File Offset: 0x000704B4
	public void OnPauseAllSFX()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Pause();
		}
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00072310 File Offset: 0x00070510
	public void OnUnpauseAllSFX()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Unpause();
		}
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0007236C File Offset: 0x0007056C
	public void OnFollowOject(string key, Transform transform)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].emissionTransform = transform;
			this.dict[key].FollowObject(this.dict[key].emissionTransform.position);
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x000071A0 File Offset: 0x000053A0
	public bool OnIsPlaying(string key)
	{
		return this.dict.ContainsKey(key) && this.dict[key].CheckIfPlaying();
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x000071C6 File Offset: 0x000053C6
	public void OnAttenuation(string key, bool attenuating, float endVolume)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].OnAttenuate(attenuating, endVolume);
		}
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x000071EC File Offset: 0x000053EC
	public void OnPan(string key, float value)
	{
		if (this.dict.ContainsKey(key))
		{
			this.dict[key].Pan(value);
		}
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x000723C4 File Offset: 0x000705C4
	public void OnStopAll()
	{
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.Stop();
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00007211 File Offset: 0x00005411
	public void OnSFXSlowDown(string key, float end, float time)
	{
		if (this.dict.ContainsKey(key))
		{
			base.StartCoroutine(this.dict[key].change_pitch_sfx(end, time));
		}
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0000723E File Offset: 0x0000543E
	public void OnSFXVolume(string key, float start, float end, float time)
	{
		if (this.dict.ContainsKey(key))
		{
			base.StartCoroutine(this.dict[key].change_volume_sfx(start, end, time, false));
		}
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0000726E File Offset: 0x0000546E
	public void OnSFXVolumeLinear(string key, float start, float end, float time)
	{
		if (this.dict.ContainsKey(key))
		{
			base.StartCoroutine(this.dict[key].change_volume_sfx(start, end, time, true));
		}
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00072420 File Offset: 0x00070620
	public void SnapshotTransition(string[] snapshotNames, float[] weights, float time)
	{
		AudioManagerMixer.Groups groups = AudioManagerMixer.GetGroups();
		List<AudioMixerGroup> list = new List<AudioMixerGroup>();
		List<AudioMixerSnapshot> list2 = new List<AudioMixerSnapshot>();
		list.Add(groups.master);
		list.Add(groups.bgm_Options);
		list.Add(groups.sfx_Options);
		list.Add(groups.master_Options);
		list.Add(groups.sfx);
		list.Add(groups.levelSfx);
		list.Add(groups.ambience);
		list.Add(groups.creatures);
		list.Add(groups.announcer);
		list.Add(groups.super);
		list.Add(groups.bgm);
		list.Add(groups.levelBgm);
		list.Add(groups.musicSting);
		list.Add(groups.noise);
		list.Add(groups.noiseConstant);
		list.Add(groups.noiseShortterm);
		list.Add(groups.noise1920s);
		for (int i = 0; i < weights.Length; i++)
		{
			if (list[i].audioMixer.FindSnapshot(snapshotNames[i]) != null)
			{
				list2.Add(list[0].audioMixer.FindSnapshot(snapshotNames[i]));
			}
			else
			{
				Debug.LogError("Snapshot string is invalid", null);
			}
		}
		foreach (AudioMixerGroup audioMixerGroup in list)
		{
			audioMixerGroup.audioMixer.TransitionToSnapshots(list2.ToArray(), weights, time);
		}
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x000725C4 File Offset: 0x000707C4
	public void SetChannels()
	{
		AudioManagerMixer.Groups groups = AudioManagerMixer.GetGroups();
		AudioManager.Channel channel = this.channel;
		AudioMixerGroup audioMixerGroup;
		AudioMixerGroup mixerGroup;
		if (channel == AudioManager.Channel.Default || channel != AudioManager.Channel.Level)
		{
			audioMixerGroup = groups.bgm;
			mixerGroup = groups.sfx;
			AudioMixerGroup noiseConstant = groups.noiseConstant;
			AudioMixerGroup noiseShortterm = groups.noiseShortterm;
		}
		else
		{
			audioMixerGroup = groups.levelBgm;
			mixerGroup = groups.levelSfx;
		}
		foreach (AudioManagerComponent.SoundGroup.Source source in this.bgmSources)
		{
			if (source.audio.outputAudioMixerGroup == null)
			{
				source.audio.outputAudioMixerGroup = audioMixerGroup;
			}
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup in this.sounds)
		{
			soundGroup.SetMixerGroup(mixerGroup);
		}
		foreach (AudioManagerComponent.SoundGroup soundGroup2 in this.bgmPlaylist)
		{
			soundGroup2.SetMixerGroup(audioMixerGroup);
		}
	}

	// Token: 0x0400058A RID: 1418
	[SerializeField]
	public AudioManager.Channel channel;

	// Token: 0x0400058B RID: 1419
	[SerializeField]
	public List<AudioManagerComponent.SoundGroup.Source> bgmSources;

	// Token: 0x0400058C RID: 1420
	[SerializeField]
	public List<AudioManagerComponent.SoundGroup.Source> bgmAlternates;

	// Token: 0x0400058D RID: 1421
	[SerializeField]
	public List<AudioManagerComponent.SoundGroup> sounds = new List<AudioManagerComponent.SoundGroup>();

	// Token: 0x0400058E RID: 1422
	[SerializeField]
	public List<AudioManagerComponent.SoundGroup> bgmPlaylist = new List<AudioManagerComponent.SoundGroup>();

	// Token: 0x0400058F RID: 1423
	[SerializeField]
	public bool autoplayBGM = true;

	// Token: 0x04000590 RID: 1424
	[SerializeField]
	public bool autoplayBGMPlaylist = true;

	// Token: 0x04000591 RID: 1425
	[SerializeField]
	public float[] minValue;

	// Token: 0x04000592 RID: 1426
	public Dictionary<string, AudioManagerComponent.SoundGroup> dict;

	// Token: 0x04000593 RID: 1427
	public static bool ShowAudioPlaying;

	// Token: 0x04000594 RID: 1428
	public static bool ShowAudioVariations;

	// Token: 0x020008E8 RID: 2280
	[Serializable]
	public class SoundGroup
	{
		// Token: 0x060052C7 RID: 21191 RVA: 0x001BCA3C File Offset: 0x001BAC3C
		public void Init(bool initializeDeferrals = false)
		{
			this.key = this.key.ToLowerIfNecessary();
			for (int i = 0; i < this.sources.Count; i++)
			{
				if (this.sources[i].audio == null)
				{
					this.sources.RemoveAt(i);
					i--;
				}
			}
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Init(initializeDeferrals);
			}
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x001BCAF4 File Offset: 0x001BACF4
		public void SetMixerGroup(AudioMixerGroup group)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				if (source.audio != null && source.audio.outputAudioMixerGroup == null)
				{
					source.audio.outputAudioMixerGroup = group;
				}
			}
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x001BCB7C File Offset: 0x001BAD7C
		public void SetVolume(float v)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.SetVolume(v);
			}
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x001BCBD8 File Offset: 0x001BADD8
		public void Play()
		{
			AudioManagerComponent.SoundGroup.Source source = this.GetSource();
			if (this.sources.Count > 1)
			{
				foreach (AudioManagerComponent.SoundGroup.Source source2 in this.sources)
				{
					if (!source.wasJustPlayed)
					{
						break;
					}
					source = this.GetSource();
				}
			}
			source.wasJustPlayed = true;
			source.Play();
			foreach (AudioManagerComponent.SoundGroup.Source source3 in this.sources)
			{
				if (source3 != source)
				{
					source3.wasJustPlayed = false;
				}
			}
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x0003F20B File Offset: 0x0003D40B
		public void PlayLoop()
		{
			this.GetSource().PlayLooped();
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x001BCCC4 File Offset: 0x001BAEC4
		public void Pan(float pan)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Pan(pan);
			}
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x001BCD20 File Offset: 0x001BAF20
		public void FollowObject(Vector3 position)
		{
			for (int i = 0; i < this.sources.Count; i++)
			{
				AudioManagerComponent.SoundGroup.Source source = this.sources[i];
				source.FollowObject(position);
			}
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x001BCD60 File Offset: 0x001BAF60
		public bool CheckIfPlaying()
		{
			this.isPlaying = false;
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.isPlaying();
				if (source.isPlaying())
				{
					this.isPlaying = true;
				}
			}
			return this.isPlaying;
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x001BCDDC File Offset: 0x001BAFDC
		public float ClipLength()
		{
			float result = 0f;
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				result = source.ClipLength();
			}
			return result;
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x001BCE40 File Offset: 0x001BB040
		public void OnAttenuate(bool attentuating, float endVolume)
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.OnAttenuate(attentuating, endVolume);
			}
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x001BCEA0 File Offset: 0x001BB0A0
		public IEnumerator warble_pitch_cr(int warbles, float[] minValue, float[] maxValue, float[] incrementAmount, float[] playTime)
		{
			bool isDecreasing = Rand.Bool();
			float t = 0f;
			float startPitch = 1f;
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				if (s != null && s.audio.clip != null)
				{
					for (int i = 0; i < warbles; i++)
					{
						while (t < playTime[i])
						{
							t += CupheadTime.Delta;
							if (isDecreasing)
							{
								if (s.audio.pitch > minValue[i])
								{
									s.audio.pitch -= incrementAmount[i];
								}
								else
								{
									isDecreasing = false;
								}
							}
							else if (s.audio.pitch < maxValue[i])
							{
								s.audio.pitch += incrementAmount[i];
							}
							else
							{
								isDecreasing = true;
							}
							yield return null;
						}
						t = 0f;
						yield return null;
					}
					s.audio.pitch = startPitch;
				}
			}
			yield break;
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x001BCEE0 File Offset: 0x001BB0E0
		public IEnumerator change_pitch_sfx(float end, float time)
		{
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				float t = 0f;
				if (s != null && s.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						s.audio.pitch = Mathf.Lerp(s.audio.pitch, end, val);
						t += Time.deltaTime;
						yield return null;
					}
					s.audio.pitch = end;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x001BCF0C File Offset: 0x001BB10C
		public IEnumerator change_volume_sfx(float start, float end, float time, bool linear)
		{
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				float t = 0f;
				if (s != null && s.audio.clip != null)
				{
					float initialVolume = (start < 0f) ? s.audio.volume : start;
					if (!linear && start >= 0f)
					{
						s.audio.volume = initialVolume;
					}
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						if (linear)
						{
							s.audio.volume = Mathf.Lerp(initialVolume, end, val);
						}
						else
						{
							s.audio.volume = Mathf.Lerp(s.audio.volume, end, val);
						}
						t += Time.deltaTime;
						yield return null;
					}
					s.audio.volume = end;
					if (end == 0f)
					{
						s.audio.Stop();
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x001BCF44 File Offset: 0x001BB144
		public IEnumerator change_volume_cr(float endVolume, float time, bool onFadeOut)
		{
			foreach (AudioManagerComponent.SoundGroup.Source s in this.sources)
			{
				float t = 0f;
				float startVol = (!onFadeOut) ? 0f : s.audio.volume;
				float endVol = (!onFadeOut) ? s.audio.volume : endVolume;
				if (!onFadeOut)
				{
					s.audio.Play();
					this.isFadedOut = false;
				}
				else
				{
					this.isFadedOut = true;
				}
				if (s.audio != null && s.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						s.audio.volume = Mathf.Lerp(startVol, endVol, val);
						t += Time.deltaTime;
						yield return null;
					}
					if (onFadeOut)
					{
						s.audio.Stop();
						s.audio.volume = s.originalVolume;
						this.isFadedOut = true;
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x001BCF74 File Offset: 0x001BB174
		public void Stop()
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Stop();
			}
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x001BCFD0 File Offset: 0x001BB1D0
		public void Pause()
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.Pause();
			}
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001BD02C File Offset: 0x001BB22C
		public void Unpause()
		{
			foreach (AudioManagerComponent.SoundGroup.Source source in this.sources)
			{
				source.UnPause();
			}
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x0003F218 File Offset: 0x0003D418
		public AudioManagerComponent.SoundGroup.Source GetSource()
		{
			return this.sources[Random.Range(0, this.sources.Count)];
		}

		// Token: 0x040043B8 RID: 17336
		[SerializeField]
		public List<AudioManagerComponent.SoundGroup.Source> sources = new List<AudioManagerComponent.SoundGroup.Source>
		{
			new AudioManagerComponent.SoundGroup.Source()
		};

		// Token: 0x040043B9 RID: 17337
		public Sfx trigger;

		// Token: 0x040043BA RID: 17338
		public string key;

		// Token: 0x040043BB RID: 17339
		public bool isPlaying;

		// Token: 0x040043BC RID: 17340
		public Transform emissionTransform;

		// Token: 0x040043BD RID: 17341
		public bool activatedManually;

		// Token: 0x040043BE RID: 17342
		public bool isFadedOut;

		// Token: 0x040043BF RID: 17343
		public float volume;

		// Token: 0x020015CA RID: 5578
		[Serializable]
		public class Source
		{
			// Token: 0x0600871E RID: 34590 RVA: 0x002A40B0 File Offset: 0x002A22B0
			public void Init(bool initializeDeferrals)
			{
				if (initializeDeferrals)
				{
					DeferredAudioSource component = this.audio.GetComponent<DeferredAudioSource>();
					if (component != null)
					{
						component.Initialize();
					}
				}
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.ignoreListenerPause = true;
					this.originalVolume = this.audio.volume;
				}
			}

			// Token: 0x0600871F RID: 34591 RVA: 0x0005B734 File Offset: 0x00059934
			public void SetVolume(float v)
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.volume = v * this.originalVolume;
				}
			}

			// Token: 0x06008720 RID: 34592 RVA: 0x002A4128 File Offset: 0x002A2328
			public void Play()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.PlayOneShot(this.audio.clip);
					if (!AudioManagerComponent.ShowAudioPlaying || AudioManagerComponent.ShowAudioVariations)
					{
					}
				}
			}

			// Token: 0x06008721 RID: 34593 RVA: 0x002A4188 File Offset: 0x002A2388
			public void PlayLooped()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.loop = true;
					this.audio.Play();
					if (!AudioManagerComponent.ShowAudioPlaying || AudioManagerComponent.ShowAudioVariations)
					{
					}
				}
			}

			// Token: 0x06008722 RID: 34594 RVA: 0x002A41E8 File Offset: 0x002A23E8
			public IEnumerator change_pitch_cr(float end, float time)
			{
				float t = 0f;
				if (this.audio != null && this.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						this.audio.pitch = Mathf.Lerp(this.audio.pitch, end, val);
						t += Time.deltaTime;
						yield return null;
					}
					this.audio.pitch = end;
				}
				yield break;
			}

			// Token: 0x06008723 RID: 34595 RVA: 0x002A4214 File Offset: 0x002A2414
			public IEnumerator change_volume_cr(float endVolume, float time, bool onFadeOut)
			{
				float t = 0f;
				float startVol = (!onFadeOut) ? 0f : this.audio.volume;
				float endVol = (!onFadeOut) ? this.audio.volume : endVolume;
				if (!onFadeOut)
				{
					this.audio.Play();
					this.isFadedOut = false;
				}
				else
				{
					this.isFadedOut = true;
				}
				if (this.audio != null && this.audio.clip != null)
				{
					while (t < time)
					{
						float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
						this.audio.volume = Mathf.Lerp(startVol, endVol, val);
						t += Time.deltaTime;
						yield return null;
					}
					if (onFadeOut)
					{
						this.audio.Stop();
						this.audio.volume = this.originalVolume;
						this.isFadedOut = true;
					}
				}
				yield return null;
				yield break;
			}

			// Token: 0x06008724 RID: 34596 RVA: 0x002A4244 File Offset: 0x002A2444
			public IEnumerator warble_pitch_cr(int warbles, float[] minValue, float[] maxValue, float[] incrementAmount, float[] playTime)
			{
				bool isDecreasing = Rand.Bool();
				float t = 0f;
				float startPitch = 1f;
				if (this.audio != null && this.audio.clip != null)
				{
					for (int i = 0; i < warbles; i++)
					{
						while (t < playTime[i])
						{
							t += CupheadTime.Delta;
							if (isDecreasing)
							{
								if (this.audio.pitch > minValue[i])
								{
									this.audio.pitch -= incrementAmount[i];
								}
								else
								{
									isDecreasing = false;
								}
							}
							else if (this.audio.pitch < maxValue[i])
							{
								this.audio.pitch += incrementAmount[i];
							}
							else
							{
								isDecreasing = true;
							}
							yield return null;
						}
						t = 0f;
						yield return null;
					}
					this.audio.pitch = startPitch;
				}
				yield break;
			}

			// Token: 0x06008725 RID: 34597 RVA: 0x0005B770 File Offset: 0x00059970
			public void Stop()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.loop = false;
					this.audio.Stop();
				}
			}

			// Token: 0x06008726 RID: 34598 RVA: 0x0005B7B0 File Offset: 0x000599B0
			public void Pause()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.Pause();
				}
			}

			// Token: 0x06008727 RID: 34599 RVA: 0x0005B7E4 File Offset: 0x000599E4
			public void UnPause()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.UnPause();
				}
			}

			// Token: 0x06008728 RID: 34600 RVA: 0x0005B818 File Offset: 0x00059A18
			public void Pan(float pan)
			{
				if (this.audio != null && this.audio.clip != null)
				{
					this.audio.panStereo = pan;
				}
			}

			// Token: 0x06008729 RID: 34601 RVA: 0x002A4284 File Offset: 0x002A2484
			public float ClipLength()
			{
				if (this.audio != null && this.audio.clip != null)
				{
					return this.audio.clip.length;
				}
				Debug.LogError("Clip is null", null);
				return 0f;
			}

			// Token: 0x0600872A RID: 34602 RVA: 0x0005B84D File Offset: 0x00059A4D
			public void FollowObject(Vector3 position)
			{
				if (this.audio != null)
				{
					this.audio.transform.position = position;
				}
			}

			// Token: 0x0600872B RID: 34603 RVA: 0x0005B871 File Offset: 0x00059A71
			public bool isPlaying()
			{
				return this.audio != null && this.audio.clip != null && this.audio.isPlaying;
			}

			// Token: 0x0600872C RID: 34604 RVA: 0x002A42DC File Offset: 0x002A24DC
			public void OnAttenuate(bool attenuating, float volumeChange)
			{
				if (this.audio != null && this.audio.clip != null)
				{
					if (attenuating)
					{
						this.audio.volume = volumeChange;
					}
					else
					{
						this.audio.volume = this.originalVolume;
					}
				}
			}

			// Token: 0x04009162 RID: 37218
			[SerializeField]
			public AudioSource audio;

			// Token: 0x04009163 RID: 37219
			public float originalVolume;

			// Token: 0x04009164 RID: 37220
			public bool wasJustPlayed;

			// Token: 0x04009165 RID: 37221
			public bool isFadedOut;

			// Token: 0x04009166 RID: 37222
			public bool noLoop;
		}
	}
}
