using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004CD RID: 1229
public class MapConfirmStartUI : AbstractMapSceneStartUI
{
	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x060032E2 RID: 13026 RVA: 0x0002A371 File Offset: 0x00028571
	// (set) Token: 0x060032E3 RID: 13027 RVA: 0x0002A378 File Offset: 0x00028578
	public static MapConfirmStartUI Current { get; set; }

	// Token: 0x060032E4 RID: 13028 RVA: 0x0002A380 File Offset: 0x00028580
	public override void Awake()
	{
		base.Awake();
		MapConfirmStartUI.Current = this;
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x000F0274 File Offset: 0x000EE474
	public void UpdateCursor()
	{
		this.cursor.transform.position = this.enter.transform.position;
		this.cursor.sizeDelta = new Vector2(this.enter.sizeDelta.x + 30f, this.enter.sizeDelta.y + 20f);
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x0002A38E File Offset: 0x0002858E
	public new void OnDestroy()
	{
		if (MapConfirmStartUI.Current == this)
		{
			MapConfirmStartUI.Current = null;
		}
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x0002A3A6 File Offset: 0x000285A6
	public void Update()
	{
		this.UpdateCursor();
		if (base.CurrentState == AbstractMapSceneStartUI.State.Active)
		{
			this.CheckInput();
		}
	}

	// Token: 0x060032E8 RID: 13032 RVA: 0x0002A3C0 File Offset: 0x000285C0
	public void CheckInput()
	{
		if (!base.Able)
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.Cancel))
		{
			base.Out();
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			base.LoadLevel();
		}
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x000F02E4 File Offset: 0x000EE4E4
	public void InitUI(string level)
	{
		TranslationElement translationElement = Localization.Find(level);
		if (translationElement != null)
		{
			this.Title.ApplyTranslation(translationElement, null);
		}
		this.EmptyCoin2.enabled = true;
		this.Coin2.enabled = false;
		this.EmptyCoin3.enabled = true;
		this.Coin3.enabled = false;
		this.EmptyCoin4.enabled = true;
		this.Coin4.enabled = false;
		this.EmptyCoin5.enabled = true;
		this.Coin5.enabled = false;
		List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = PlayerData.Data.coinManager.LevelsAndCoins;
		for (int i = 0; i < levelsAndCoins.Count; i++)
		{
			if (levelsAndCoins[i].level.ToString() == level)
			{
				if (levelsAndCoins[i].Coin1Collected)
				{
					this.EmptyCoin1.enabled = false;
					this.Coin1.enabled = true;
				}
				else
				{
					this.EmptyCoin1.enabled = true;
					this.Coin1.enabled = false;
				}
				if (levelsAndCoins[i].Coin2Collected)
				{
					this.EmptyCoin2.enabled = false;
					this.Coin2.enabled = true;
				}
				else
				{
					this.EmptyCoin2.enabled = true;
					this.Coin2.enabled = false;
				}
				if (levelsAndCoins[i].Coin3Collected)
				{
					this.EmptyCoin3.enabled = false;
					this.Coin3.enabled = true;
				}
				else
				{
					this.EmptyCoin3.enabled = true;
					this.Coin3.enabled = false;
				}
				if (levelsAndCoins[i].Coin4Collected)
				{
					this.EmptyCoin4.enabled = false;
					this.Coin4.enabled = true;
				}
				else
				{
					this.EmptyCoin4.enabled = true;
					this.Coin4.enabled = false;
				}
				if (levelsAndCoins[i].Coin5Collected)
				{
					this.EmptyCoin5.enabled = false;
					this.Coin5.enabled = true;
				}
				else
				{
					this.EmptyCoin5.enabled = true;
					this.Coin5.enabled = false;
				}
			}
		}
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x0002A3F4 File Offset: 0x000285F4
	public new void In(MapPlayerController playerController)
	{
		base.In(playerController);
		if (this.Animator != null)
		{
			this.Animator.SetTrigger("ZoomIn");
			AudioManager.Play("world_map_level_menu_open");
		}
		this.InitUI(this.level);
	}

	// Token: 0x040029BD RID: 10685
	public Animator Animator;

	// Token: 0x040029BE RID: 10686
	public LocalizationHelper Title;

	// Token: 0x040029BF RID: 10687
	[Header("Coins")]
	public Image EmptyCoin1;

	// Token: 0x040029C0 RID: 10688
	public Image Coin1;

	// Token: 0x040029C1 RID: 10689
	public Image EmptyCoin2;

	// Token: 0x040029C2 RID: 10690
	public Image Coin2;

	// Token: 0x040029C3 RID: 10691
	public Image EmptyCoin3;

	// Token: 0x040029C4 RID: 10692
	public Image Coin3;

	// Token: 0x040029C5 RID: 10693
	public Image EmptyCoin4;

	// Token: 0x040029C6 RID: 10694
	public Image Coin4;

	// Token: 0x040029C7 RID: 10695
	public Image EmptyCoin5;

	// Token: 0x040029C8 RID: 10696
	public Image Coin5;

	// Token: 0x040029C9 RID: 10697
	[SerializeField]
	public RectTransform cursor;

	// Token: 0x040029CA RID: 10698
	[Header("Options")]
	[SerializeField]
	public RectTransform enter;
}
