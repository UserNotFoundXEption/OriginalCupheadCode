using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using RektTransform;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x020004BB RID: 1211
public class AbstractMapCardIcon : AbstractMonoBehaviour
{
	// Token: 0x06003256 RID: 12886 RVA: 0x000EC874 File Offset: 0x000EAA74
	public void SetIcons(Weapon weapon, bool isGrey)
	{
		string atlasName = AbstractMapCardIcon.DefaultAtlas;
		if (Array.IndexOf<Weapon>(AbstractMapCardIcon.DLCWeapons, weapon) > -1)
		{
			atlasName = AbstractMapCardIcon.DLCAtlas;
			Color white = Color.white;
			if (isGrey)
			{
				white..ctor(1f, 1f, 1f, 0.5f);
			}
			this.iconImage.color = white;
		}
		this.setIcons(WeaponProperties.GetIconPath(weapon), isGrey, atlasName);
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x00029BF4 File Offset: 0x00027DF4
	public void SetIcons(Super super, bool isGrey)
	{
		this.setIcons(WeaponProperties.GetIconPath(super), isGrey, AbstractMapCardIcon.DefaultAtlas);
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x000EC8E0 File Offset: 0x000EAAE0
	public void SetIcons(Charm charm, bool isGrey)
	{
		string atlasName = AbstractMapCardIcon.DefaultAtlas;
		if (Array.IndexOf<Charm>(AbstractMapCardIcon.DLCCharms, charm) > -1)
		{
			atlasName = AbstractMapCardIcon.DLCAtlas;
		}
		this.setIcons(WeaponProperties.GetIconPath(charm), isGrey, atlasName);
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x00029C08 File Offset: 0x00027E08
	public void SetIconsManual(string iconPath, bool isGrey, bool isDLC = false)
	{
		this.setIcons(iconPath, isGrey, (!isDLC) ? AbstractMapCardIcon.DefaultAtlas : AbstractMapCardIcon.DLCAtlas);
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x000EC918 File Offset: 0x000EAB18
	public void setIcons(string iconPath, bool isGrey, string atlasName)
	{
		SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset(atlasName);
		List<Sprite> list = new List<Sprite>();
		string fileName = Path.GetFileName(iconPath);
		Sprite sprite = this.getSprite(cachedAsset, fileName);
		if (sprite != null)
		{
			list.Add(sprite);
		}
		for (int i = 1; i < 4; i++)
		{
			string arg = "_000";
			string fileName2 = Path.GetFileName(iconPath + arg + i);
			Sprite sprite2 = this.getSprite(cachedAsset, fileName2);
			if (!(sprite2 == null))
			{
				list.Add(sprite2);
			}
		}
		this.normalIcons = list.ToArray();
		list.Clear();
		if (sprite != null)
		{
			list.Add(sprite);
		}
		for (int j = 1; j < 4; j++)
		{
			string arg2 = "_grey_000";
			string fileName3 = Path.GetFileName(iconPath + arg2 + j);
			Sprite sprite3 = this.getSprite(cachedAsset, fileName3);
			if (!(sprite3 == null))
			{
				list.Add(sprite3);
			}
		}
		this.greyIcons = list.ToArray();
		this.icons = ((!isGrey) ? this.normalIcons : this.greyIcons);
		this.StopAllCoroutines();
		if (iconPath != WeaponProperties.GetIconPath(Weapon.None))
		{
			base.StartCoroutine(this.animate_cr());
		}
		else
		{
			this.SetIcon(this.icons[0]);
		}
	}

	// Token: 0x0600325B RID: 12891 RVA: 0x000ECA90 File Offset: 0x000EAC90
	public void SetIcons(string iconPath)
	{
		SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset("Equip_Icons");
		List<Sprite> list = new List<Sprite>();
		string fileName = Path.GetFileName(iconPath);
		Sprite sprite = this.getSprite(cachedAsset, fileName);
		if (sprite != null)
		{
			list.Add(sprite);
		}
		string fileName2 = Path.GetFileName(iconPath);
		Sprite sprite2 = this.getSprite(cachedAsset, fileName2);
		list.Add(sprite2);
		this.icons = list.ToArray();
		this.SetIcon(sprite2);
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x00029C27 File Offset: 0x00027E27
	public virtual void SelectIcon()
	{
		if (base.animator != null)
		{
			base.animator.Play("Select");
		}
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x00029C4A File Offset: 0x00027E4A
	public virtual void UnselectIcon()
	{
		if (base.animator != null)
		{
			base.animator.Play("Unselect");
		}
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x00029C6D File Offset: 0x00027E6D
	public virtual void OnLocked()
	{
		if (base.animator != null)
		{
			base.animator.Play("Locked");
		}
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x000ECB00 File Offset: 0x000EAD00
	public void SetIcon(Sprite sprite)
	{
		if (sprite == null)
		{
			return;
		}
		this.iconImage.sprite = sprite;
		this.iconImage.rectTransform.SetSize(sprite.rect.width, sprite.rect.height);
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x000ECB54 File Offset: 0x000EAD54
	public IEnumerator animate_cr()
	{
		int i = 0;
		WaitForSeconds wait = new WaitForSeconds(0.07f);
		this.SetIcon((this.icons != null && this.icons.Length >= 1) ? this.icons[0] : null);
		for (;;)
		{
			yield return wait;
			if (this.icons == null || this.icons.Length < 1)
			{
				this.SetIcon(null);
			}
			else
			{
				i++;
				if (i > this.icons.Length - 1)
				{
					i = 0;
				}
				this.SetIcon(this.icons[i]);
			}
		}
		yield break;
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x000ECB70 File Offset: 0x000EAD70
	public Sprite getSprite(SpriteAtlas atlas, string spriteName)
	{
		return atlas.GetSprite(spriteName);
	}

	// Token: 0x04002932 RID: 10546
	public const float FRAME_DELAY = 0.07f;

	// Token: 0x04002933 RID: 10547
	public static readonly Weapon[] DLCWeapons = new Weapon[]
	{
		Weapon.level_weapon_crackshot,
		Weapon.level_weapon_upshot,
		Weapon.level_weapon_wide_shot
	};

	// Token: 0x04002934 RID: 10548
	public static readonly Charm[] DLCCharms = new Charm[]
	{
		Charm.charm_chalice,
		Charm.charm_curse,
		Charm.charm_healer
	};

	// Token: 0x04002935 RID: 10549
	public static readonly string DefaultAtlas = "Equip_Icons";

	// Token: 0x04002936 RID: 10550
	public static readonly string DLCAtlas = "Equip_Icons_DLC";

	// Token: 0x04002937 RID: 10551
	[SerializeField]
	public Image iconImage;

	// Token: 0x04002938 RID: 10552
	public Sprite[] icons;

	// Token: 0x04002939 RID: 10553
	public Sprite[] normalIcons;

	// Token: 0x0400293A RID: 10554
	public Sprite[] greyIcons;
}
