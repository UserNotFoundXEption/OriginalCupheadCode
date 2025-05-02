using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C1 RID: 193
public class GenericCutscene : Cutscene
{
	// Token: 0x060008DD RID: 2269 RVA: 0x0000871B File Offset: 0x0000691B
	public override void Start()
	{
		base.Start();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00076C54 File Offset: 0x00074E54
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		for (int i = 0; i < this.numScreens; i++)
		{
			if (this.specialCase && i == 3)
			{
				yield return CupheadTime.WaitForSeconds(this, 2.25f);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, 1.25f);
			}
			this.arrowVisible = true;
			while (this.input.GetButtonDown(CupheadButton.Pause) || !this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			if (i < this.numScreens - 1)
			{
				base.animator.SetTrigger("Continue");
			}
			AudioManager.Play("ui_confirm_generic");
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.Skip();
		yield break;
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00076C70 File Offset: 0x00074E70
	public IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				base.Skip();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00076C8C File Offset: 0x00074E8C
	public void Update()
	{
		if (this.arrowVisible)
		{
			this.arrowTransparency = Mathf.Clamp01(this.arrowTransparency + Time.deltaTime / 0.25f);
		}
		else
		{
			this.arrowTransparency = 0f;
		}
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency);
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00008759 File Offset: 0x00006959
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x040006BC RID: 1724
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x040006BD RID: 1725
	[SerializeField]
	public bool specialCase;

	// Token: 0x040006BE RID: 1726
	[SerializeField]
	public Image arrow;

	// Token: 0x040006BF RID: 1727
	[SerializeField]
	public int numScreens;

	// Token: 0x040006C0 RID: 1728
	public float arrowTransparency;

	// Token: 0x040006C1 RID: 1729
	public bool arrowVisible;
}
