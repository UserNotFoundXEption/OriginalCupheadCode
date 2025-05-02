using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004CA RID: 1226
public class MapEquipUICursor : AbstractMonoBehaviour
{
	// Token: 0x060032CF RID: 13007 RVA: 0x0002A20E File Offset: 0x0002840E
	public virtual void SetPosition(Vector3 position)
	{
		base.transform.position = position;
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x0002A21C File Offset: 0x0002841C
	public virtual void SelectIcon(bool onSame)
	{
		if (onSame)
		{
			base.animator.Play("Select_V2", 1);
		}
		else
		{
			base.animator.Play("Select", 1);
		}
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x0002A24B File Offset: 0x0002844B
	public virtual void OnLocked()
	{
		base.animator.Play("Locked", 1);
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x0002A25E File Offset: 0x0002845E
	public virtual void Hide()
	{
		this.image.enabled = false;
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x0002A26C File Offset: 0x0002846C
	public virtual void Show()
	{
		this.image.enabled = true;
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x0002A27A File Offset: 0x0002847A
	public void HideSelectionCursor()
	{
		this.selectionCursor.enabled = false;
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x0002A288 File Offset: 0x00028488
	public void ShowSelectionCursor()
	{
		this.selectionCursor.enabled = true;
	}

	// Token: 0x040029B4 RID: 10676
	[SerializeField]
	public Image selectionCursor;

	// Token: 0x040029B5 RID: 10677
	[SerializeField]
	public Image image;

	// Token: 0x040029B6 RID: 10678
	public int index;
}
