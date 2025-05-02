using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x020000E8 RID: 232
public class AchievementToastManager : AbstractMonoBehaviour
{
	// Token: 0x06000AFB RID: 2811 RVA: 0x00009D3E File Offset: 0x00007F3E
	public void OnEnable()
	{
		LocalAchievementsManager.AchievementUnlockedEvent += this.UnlockAchievement;
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00009D51 File Offset: 0x00007F51
	public void OnDisable()
	{
		LocalAchievementsManager.AchievementUnlockedEvent -= this.UnlockAchievement;
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0007CEAC File Offset: 0x0007B0AC
	public void Start()
	{
		this.uiCamera = Object.Instantiate<GameObject>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		Camera component = this.uiCamera.GetComponent<Camera>();
		component.cullingMask = 65536;
		component.depth = (float)AchievementToastManager.CameraDepth;
		Canvas componentInChildren = base.GetComponentInChildren<Canvas>(true);
		componentInChildren.worldCamera = component;
		componentInChildren.sortingLayerName = SpriteLayer.AchievementToast.ToString();
		this.uiCamera.SetActive(false);
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x00009D64 File Offset: 0x00007F64
	public void UnlockAchievement(LocalAchievementsManager.Achievement achievement)
	{
		if (this.currentAnimation != null)
		{
			this.queuedAchievements.Add(achievement);
			return;
		}
		this.currentAnimation = base.StartCoroutine(this.showUnlock(achievement));
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0007CF44 File Offset: 0x0007B144
	public IEnumerator showUnlock(LocalAchievementsManager.Achievement achievement)
	{
		AudioManager.Play("achievement_unlocked");
		string achievementName = achievement.ToString();
		string titleKey = "Achievement" + achievementName + "Toast";
		this.titleLocalization.ApplyTranslation(Localization.Find(titleKey), null);
		string spriteName = achievementName + "_toast";
		Sprite sprite = this.getAtlas(achievement).GetSprite(spriteName);
		this.icon.sprite = sprite;
		this.toastTransform.position = AchievementToastManager.InitialPosition;
		this.visual.SetActive(true);
		this.uiCamera.SetActive(true);
		Vector2 displacement = AchievementToastManager.FinalPosition - AchievementToastManager.InitialPosition;
		float elapsed = 0f;
		while (elapsed < AchievementToastManager.AnimationDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			float factor = this.easeOutBack(elapsed, 0f, 1f, AchievementToastManager.AnimationDuration);
			this.toastTransform.localPosition = AchievementToastManager.InitialPosition + factor * displacement;
			yield return null;
		}
		this.toastTransform.localPosition = AchievementToastManager.FinalPosition;
		yield return new AchievementToastManager.WaitForSecondsRealtime(AchievementToastManager.HoldDuration);
		elapsed = 0f;
		while (elapsed < AchievementToastManager.AnimationDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			float factor2 = this.easeInBack(elapsed, 1f, -1f, AchievementToastManager.AnimationDuration);
			this.toastTransform.localPosition = AchievementToastManager.InitialPosition + factor2 * displacement;
			yield return null;
		}
		if (this.queuedAchievements.Count > 0)
		{
			LocalAchievementsManager.Achievement achievement2 = this.queuedAchievements[0];
			this.queuedAchievements.RemoveAt(0);
			this.currentAnimation = base.StartCoroutine(this.showUnlock(achievement2));
		}
		else
		{
			this.currentAnimation = null;
			this.visual.SetActive(false);
			this.uiCamera.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0007CF68 File Offset: 0x0007B168
	public float easeOutBack(float t, float initial, float change, float duration)
	{
		float num = 1.70158f;
		return change * ((t = t / duration - 1f) * t * ((num + 1f) * t + num) + 1f) + initial;
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0007CFA0 File Offset: 0x0007B1A0
	public float easeInBack(float t, float initial, float change, float duration)
	{
		float num = 1.70158f;
		float num2;
		t = (num2 = t / duration);
		return change * num2 * t * ((num + 1f) * t - num) + initial;
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x00009D91 File Offset: 0x00007F91
	public SpriteAtlas getAtlas(LocalAchievementsManager.Achievement achievement)
	{
		if (Array.IndexOf<LocalAchievementsManager.Achievement>(LocalAchievementsManager.DLCAchievements, achievement) >= 0)
		{
			return this.dlcAtlas;
		}
		return this.defaultAtlas;
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00009DB1 File Offset: 0x00007FB1
	public SpriteAtlas defaultAtlas
	{
		get
		{
			if (!this._defaultAtlasCached)
			{
				this._defaultAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements");
				this._defaultAtlasCached = true;
			}
			return this._defaultAtlas;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00009DDB File Offset: 0x00007FDB
	public SpriteAtlas dlcAtlas
	{
		get
		{
			if (!this._dlcAtlasCached && DLCManager.DLCEnabled())
			{
				this._dlcAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements_DLC");
				this._dlcAtlasCached = true;
			}
			return this._dlcAtlas;
		}
	}

	// Token: 0x0400088E RID: 2190
	public static readonly int CameraDepth = 91;

	// Token: 0x0400088F RID: 2191
	public static readonly Vector2 InitialPosition = new Vector2(0f, -460f);

	// Token: 0x04000890 RID: 2192
	public static readonly Vector2 FinalPosition = new Vector2(0f, -280f);

	// Token: 0x04000891 RID: 2193
	public static readonly float AnimationDuration = 0.34f;

	// Token: 0x04000892 RID: 2194
	public static readonly float HoldDuration = 2f;

	// Token: 0x04000893 RID: 2195
	[SerializeField]
	public GameObject uiCameraPrefab;

	// Token: 0x04000894 RID: 2196
	[SerializeField]
	public GameObject visual;

	// Token: 0x04000895 RID: 2197
	[SerializeField]
	public RectTransform toastTransform;

	// Token: 0x04000896 RID: 2198
	[SerializeField]
	public LocalizationHelper titleLocalization;

	// Token: 0x04000897 RID: 2199
	[SerializeField]
	public Image icon;

	// Token: 0x04000898 RID: 2200
	public List<LocalAchievementsManager.Achievement> queuedAchievements = new List<LocalAchievementsManager.Achievement>();

	// Token: 0x04000899 RID: 2201
	public Coroutine currentAnimation;

	// Token: 0x0400089A RID: 2202
	public GameObject uiCamera;

	// Token: 0x0400089B RID: 2203
	public bool _defaultAtlasCached;

	// Token: 0x0400089C RID: 2204
	public SpriteAtlas _defaultAtlas;

	// Token: 0x0400089D RID: 2205
	public bool _dlcAtlasCached;

	// Token: 0x0400089E RID: 2206
	public SpriteAtlas _dlcAtlas;

	// Token: 0x02000961 RID: 2401
	public class WaitForSecondsRealtime : CustomYieldInstruction
	{
		// Token: 0x060054DD RID: 21725 RVA: 0x0004037C File Offset: 0x0003E57C
		public WaitForSecondsRealtime(float time)
		{
			this.waitTime = time;
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x060054DE RID: 21726 RVA: 0x00040396 File Offset: 0x0003E596
		// (set) Token: 0x060054DF RID: 21727 RVA: 0x0004039E File Offset: 0x0003E59E
		public float waitTime { get; set; }

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060054E0 RID: 21728 RVA: 0x001C489C File Offset: 0x001C2A9C
		public override bool keepWaiting
		{
			get
			{
				if (this.m_WaitUntilTime < 0f)
				{
					this.m_WaitUntilTime = Time.realtimeSinceStartup + this.waitTime;
				}
				bool flag = Time.realtimeSinceStartup < this.m_WaitUntilTime;
				if (!flag)
				{
					this.m_WaitUntilTime = -1f;
				}
				return flag;
			}
		}

		// Token: 0x0400465B RID: 18011
		public float m_WaitUntilTime = -1f;
	}
}
