using System;
using TMPro;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class MapBasicStartUI : AbstractMapSceneStartUI
{
	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060032D8 RID: 13016 RVA: 0x0002A2A6 File Offset: 0x000284A6
	// (set) Token: 0x060032D9 RID: 13017 RVA: 0x0002A2AD File Offset: 0x000284AD
	public static MapBasicStartUI Current { get; set; }

	// Token: 0x060032DA RID: 13018 RVA: 0x0002A2B5 File Offset: 0x000284B5
	public override void Awake()
	{
		base.Awake();
		MapBasicStartUI.Current = this;
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x0002A2C3 File Offset: 0x000284C3
	public new void OnDestroy()
	{
		if (MapBasicStartUI.Current == this)
		{
			MapBasicStartUI.Current = null;
		}
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x000F01A4 File Offset: 0x000EE3A4
	public void UpdateCursor()
	{
		this.cursor.transform.position = this.enter.transform.position;
		this.cursor.sizeDelta = new Vector2(this.enter.sizeDelta.x + 30f, this.enter.sizeDelta.y + 20f);
	}

	// Token: 0x060032DD RID: 13021 RVA: 0x0002A2DB File Offset: 0x000284DB
	public void Update()
	{
		this.UpdateCursor();
		if (base.CurrentState == AbstractMapSceneStartUI.State.Active)
		{
			this.CheckInput();
		}
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x0002A2F5 File Offset: 0x000284F5
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

	// Token: 0x060032DF RID: 13023 RVA: 0x0002A329 File Offset: 0x00028529
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

	// Token: 0x060032E0 RID: 13024 RVA: 0x000F0214 File Offset: 0x000EE414
	public void InitUI(string level)
	{
		TranslationElement translationElement = Localization.Find(level);
		if (translationElement != null)
		{
			this.Title.GetComponent<LocalizationHelper>().ApplyTranslation(translationElement, null);
			if (Localization.language == Localization.Languages.Japanese)
			{
				this.Title.lineSpacing = 0f;
			}
			else
			{
				this.Title.lineSpacing = 17.46f;
			}
		}
	}

	// Token: 0x040029B8 RID: 10680
	public Animator Animator;

	// Token: 0x040029B9 RID: 10681
	public TMP_Text Title;

	// Token: 0x040029BA RID: 10682
	[SerializeField]
	public RectTransform cursor;

	// Token: 0x040029BB RID: 10683
	[Header("Options")]
	[SerializeField]
	public RectTransform enter;
}
