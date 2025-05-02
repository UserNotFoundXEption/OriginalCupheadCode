using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E6 RID: 230
public class AchievementIcon : MonoBehaviour
{
	// Token: 0x06000AE3 RID: 2787 RVA: 0x00009C72 File Offset: 0x00007E72
	public void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00009C80 File Offset: 0x00007E80
	public void SetIcon(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x04000866 RID: 2150
	public Image image;
}
