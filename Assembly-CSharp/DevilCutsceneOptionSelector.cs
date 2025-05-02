using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B6 RID: 182
public class DevilCutsceneOptionSelector : AbstractMonoBehaviour
{
	// Token: 0x06000874 RID: 2164 RVA: 0x000081B6 File Offset: 0x000063B6
	public void Start()
	{
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.cursor.gameObject.SetActive(false);
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x000081D5 File Offset: 0x000063D5
	public void Show()
	{
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.fadeIn_cr());
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00075F90 File Offset: 0x00074190
	public IEnumerator main_cr()
	{
		this.cursor.gameObject.SetActive(true);
		if (this.cutscene.IsTranslationTextActive())
		{
			this.cursor.transform.position = this.options[this.currentOption].position;
		}
		else
		{
			this.cursor.transform.position = this.bakedOptions[this.currentOption].position;
		}
		for (;;)
		{
			int prevOption = this.currentOption;
			if (this.input.GetButtonDown(CupheadButton.MenuLeft))
			{
				this.currentOption = Mathf.Max(0, this.currentOption - 1);
			}
			if (this.input.GetButtonDown(CupheadButton.MenuRight))
			{
				this.currentOption = Mathf.Min(this.options.Length - 1, this.currentOption + 1);
			}
			if (this.cutscene.IsTranslationTextActive())
			{
				this.cursor.transform.position = this.options[this.currentOption].position;
			}
			else
			{
				this.cursor.transform.position = this.bakedOptions[this.currentOption].position;
			}
			if (this.currentOption > prevOption)
			{
				this.ToggleSFX();
				base.animator.SetTrigger("MoveRight");
			}
			if (this.currentOption < prevOption)
			{
				this.ToggleSFX();
				base.animator.SetTrigger("MoveLeft");
			}
			if (this.input.GetButtonDown(CupheadButton.Accept))
			{
				break;
			}
			yield return null;
		}
		if (this.currentOption == 0)
		{
			this.cutscene.RefuseDevil();
		}
		else
		{
			this.cutscene.JoinDevil();
		}
		this.cursor.gameObject.SetActive(false);
		yield break;
		yield break;
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00075FAC File Offset: 0x000741AC
	public IEnumerator fadeIn_cr()
	{
		float t = 0f;
		while (t < 0.75f)
		{
			this.cursorImage.color = new Color(1f, 1f, 1f, t / 0.75f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.cursorImage.color = new Color(1f, 1f, 1f, 1f);
		yield break;
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x000081F1 File Offset: 0x000063F1
	public void ToggleSFX()
	{
		AudioManager.Play("ui_toggle");
	}

	// Token: 0x0400066B RID: 1643
	public Transform[] bakedOptions;

	// Token: 0x0400066C RID: 1644
	public Transform[] options;

	// Token: 0x0400066D RID: 1645
	public Transform cursor;

	// Token: 0x0400066E RID: 1646
	public DevilCutscene cutscene;

	// Token: 0x0400066F RID: 1647
	public int currentOption;

	// Token: 0x04000670 RID: 1648
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000671 RID: 1649
	public Image cursorImage;
}
