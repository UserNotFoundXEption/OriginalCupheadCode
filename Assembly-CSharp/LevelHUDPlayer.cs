using System;
using System.Collections;
using RektTransform;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200010A RID: 266
public class LevelHUDPlayer : AbstractPausableComponent
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0000AE1C File Offset: 0x0000901C
	// (set) Token: 0x06000C6E RID: 3182 RVA: 0x0000AE24 File Offset: 0x00009024
	public AbstractPlayerController player { get; set; }

	// Token: 0x06000C6F RID: 3183 RVA: 0x0008344C File Offset: 0x0008164C
	public void Init(AbstractPlayerController player, bool startAtOneHealth = false)
	{
		this.player = player;
		if (player.id == PlayerId.PlayerTwo)
		{
			this.SetupPlayerTwo();
		}
		player.stats.OnHealthChangedEvent += this.OnHealthChanged;
		player.stats.OnSuperChangedEvent += this.OnSuperChanged;
		player.stats.OnWeaponChangedEvent += this.OnWeaponChanged;
		this.health.Init(this);
		if (startAtOneHealth)
		{
			this.health.OnHealthChanged(1);
		}
		this.super.Init(this);
		if (player as PlanePlayerController != null)
		{
			this.weaponSwitchNotification.gameObject.SetActive(PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).MustNotifySwitchSHMUPWeapon && !Level.IsTowerOfPowerMain);
		}
		else
		{
			this.weaponSwitchNotification.gameObject.SetActive(PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).MustNotifySwitchRegularWeapon && !Level.IsTowerOfPowerMain);
		}
		this.weaponSwitchNotification.alpha = 1f;
		this.weaponSwitchTransform = this.weaponSwitchNotification.GetComponent<RectTransform>();
		this.weaponSwitchStartPosition = this.weaponSwitchTransform.anchoredPosition;
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x000835A0 File Offset: 0x000817A0
	public void SetupPlayerTwo()
	{
		base.gameObject.name = "Mugman";
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x *= -1f;
		base.rectTransform.SetAnchors(new RektTransform.MinMax(new Vector2(1f, 0f), new Vector2(1f, 0f)));
		base.rectTransform.pivot = new Vector2(1f, 0f);
		base.transform.localPosition = localPosition;
		localPosition = this.health.rectTransform.localPosition;
		localPosition.x *= -1f;
		this.health.rectTransform.localPosition = localPosition;
		this.weaponRoot.localPosition = localPosition;
		localPosition = this.super.rectTransform.localPosition;
		localPosition.x *= -1f;
		this.super.rectTransform.SetScale(new float?(-1f), null, null);
		this.super.rectTransform.localPosition = localPosition;
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0000AE2D File Offset: 0x0000902D
	public void OnHealthChanged(int health, PlayerId playerId)
	{
		this.health.OnHealthChanged(health);
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0000AE3B File Offset: 0x0000903B
	public void OnSuperChanged(float super, PlayerId playerId, bool playEffect)
	{
		this.super.OnSuperChanged(super);
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x000836D4 File Offset: 0x000818D4
	public void OnWeaponChanged(Weapon weapon)
	{
		this.weaponIconPrefab.Create(this.weaponRoot, weapon);
		if (this.weaponSwitchNotification.gameObject.activeSelf)
		{
			bool flag = PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchSHMUPWeapon || PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchRegularWeapon;
			if (this.player as PlanePlayerController != null)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchSHMUPWeapon = false;
			}
			else
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchRegularWeapon = false;
			}
			if (flag)
			{
				PlayerData.SaveCurrentFile();
			}
			base.StartCoroutine(this.FadeOutSwitchNotification(0.4f));
		}
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x000837C8 File Offset: 0x000819C8
	public void Update()
	{
		if (this.weaponSwitchNotification.gameObject.activeSelf)
		{
			this.weaponSwitchTransform.anchoredPosition = this.weaponSwitchStartPosition + Vector2.up * Mathf.Sin(Time.time * this.weaponSwitchWobbleSpeed) * this.weaponSwitchWobbleScale;
		}
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0000AE49 File Offset: 0x00009049
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.health = null;
		this.super = null;
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x00083828 File Offset: 0x00081A28
	public IEnumerator FadeOutSwitchNotification(float overTime)
	{
		if (overTime > 0f)
		{
			float startAlpha = this.weaponSwitchNotification.alpha;
			float timeSpent = 0f;
			while (timeSpent < overTime)
			{
				this.weaponSwitchNotification.alpha = Mathf.Lerp(startAlpha, 0f, timeSpent / overTime);
				timeSpent += Time.deltaTime;
				yield return null;
			}
			this.weaponSwitchNotification.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x040009DF RID: 2527
	[SerializeField]
	public LevelHUDPlayerHealth health;

	// Token: 0x040009E0 RID: 2528
	[SerializeField]
	public LevelHUDPlayerSuper super;

	// Token: 0x040009E1 RID: 2529
	[Space(10f)]
	[SerializeField]
	public RectTransform weaponRoot;

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	[FormerlySerializedAs("weaponPrefab")]
	public LevelHUDWeapon weaponIconPrefab;

	// Token: 0x040009E3 RID: 2531
	[SerializeField]
	public CanvasGroup weaponSwitchNotification;

	// Token: 0x040009E4 RID: 2532
	public float weaponSwitchWobbleSpeed = 1f;

	// Token: 0x040009E5 RID: 2533
	public float weaponSwitchWobbleScale = 1f;

	// Token: 0x040009E7 RID: 2535
	public RectTransform weaponSwitchTransform;

	// Token: 0x040009E8 RID: 2536
	public Vector2 weaponSwitchStartPosition;
}
