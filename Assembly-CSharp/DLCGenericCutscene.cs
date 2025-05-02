using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BC RID: 188
public class DLCGenericCutscene : Cutscene
{
	// Token: 0x060008C0 RID: 2240 RVA: 0x000085FF File Offset: 0x000067FF
	public override void Start()
	{
		base.Start();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.skip_cr());
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x000764E8 File Offset: 0x000746E8
	public virtual void Update()
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

	// Token: 0x060008C2 RID: 2242 RVA: 0x0000863D File Offset: 0x0000683D
	public virtual void OnScreenAdvance(int which)
	{
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0000863F File Offset: 0x0000683F
	public virtual void OnContinue()
	{
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00008641 File Offset: 0x00006841
	public virtual void OnScreenSkip()
	{
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00076554 File Offset: 0x00074754
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.mainDelay);
		this.curScreen = 0;
		while (this.curScreen < this.screens.Length)
		{
			this.screens[this.curScreen].gameObject.SetActive(true);
			int target = Animator.StringToHash(this.screens[this.curScreen].GetLayerName(0) + ".End");
			while (this.screens[this.curScreen].GetCurrentAnimatorStateInfo(0).fullPathHash != target)
			{
				yield return null;
				if (this.arrowVisible)
				{
					while ((this.input.GetButtonDown(CupheadButton.Pause) || !this.input.GetAnyButtonDown()) && !this.fastForwardActive)
					{
						yield return null;
					}
					this.curPathHash = this.screens[this.curScreen].GetCurrentAnimatorStateInfo(0).fullPathHash;
					this.screens[this.curScreen].SetTrigger("Continue");
					this.OnContinue();
					this.text[this.textCounter].SetActive(false);
					this.arrowVisible = false;
				}
				else if (this.allowScreenSkip && this.input.GetAnyButtonDown())
				{
					this.OnScreenSkip();
				}
			}
			this.OnScreenAdvance(this.curScreen);
			if (this.curScreen < this.screens.Length - 1)
			{
				this.screens[this.curScreen].gameObject.SetActive(false);
			}
			this.arrowVisible = false;
			this.curScreen++;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.Skip();
		yield break;
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00076570 File Offset: 0x00074770
	public void IrisIn()
	{
		this.allowScreenSkip = false;
		Animator component = this.fader.GetComponent<Animator>();
		Color color = this.fader.color;
		color.a = 1f;
		this.fader.color = color;
		component.SetTrigger("Iris_In");
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x000765C0 File Offset: 0x000747C0
	public virtual void IrisOut()
	{
		Animator component = this.fader.GetComponent<Animator>();
		Color color = this.fader.color;
		color.a = 1f;
		this.fader.color = color;
		component.SetTrigger("Iris_Out");
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00008643 File Offset: 0x00006843
	public void ShowText()
	{
		this.textCounter++;
		this.text[this.textCounter].SetActive(true);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00076608 File Offset: 0x00074808
	public void ShowArrow()
	{
		if (this.curPathHash != this.screens[this.curScreen].GetCurrentAnimatorStateInfo(0).fullPathHash)
		{
			this.arrowVisible = true;
		}
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00076644 File Offset: 0x00074844
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

	// Token: 0x060008CB RID: 2251 RVA: 0x00076660 File Offset: 0x00074860
	public DLCGenericCutscene.TrappedChar DetectCharacter()
	{
		if (PlayerManager.Multiplayer)
		{
			if (!PlayerManager.playerWasChalice[0] && !PlayerManager.playerWasChalice[1])
			{
				return DLCGenericCutscene.TrappedChar.Chalice;
			}
			if (PlayerManager.playerWasChalice[0])
			{
				return (!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Cuphead : DLCGenericCutscene.TrappedChar.Mugman;
			}
			return (!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Mugman : DLCGenericCutscene.TrappedChar.Cuphead;
		}
		else
		{
			if (PlayerManager.playerWasChalice[0])
			{
				return (!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Cuphead : DLCGenericCutscene.TrappedChar.Mugman;
			}
			return (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm != Charm.charm_chalice) ? DLCGenericCutscene.TrappedChar.Chalice : ((!PlayerManager.player1IsMugman) ? DLCGenericCutscene.TrappedChar.Mugman : DLCGenericCutscene.TrappedChar.Cuphead);
		}
	}

	// Token: 0x04000693 RID: 1683
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000694 RID: 1684
	[SerializeField]
	public float cursorToVisableTime = 1.25f;

	// Token: 0x04000695 RID: 1685
	[SerializeField]
	public float mainDelay = 0.25f;

	// Token: 0x04000696 RID: 1686
	[SerializeField]
	public Image arrow;

	// Token: 0x04000697 RID: 1687
	[SerializeField]
	public GameObject[] text;

	// Token: 0x04000698 RID: 1688
	[SerializeField]
	public Animator[] screens;

	// Token: 0x04000699 RID: 1689
	public int activeScreen;

	// Token: 0x0400069A RID: 1690
	public bool allowScreenSkip;

	// Token: 0x0400069B RID: 1691
	public float arrowTransparency;

	// Token: 0x0400069C RID: 1692
	public bool arrowVisible;

	// Token: 0x0400069D RID: 1693
	public int textCounter = -1;

	// Token: 0x0400069E RID: 1694
	public int curPathHash;

	// Token: 0x0400069F RID: 1695
	public int curScreen;

	// Token: 0x040006A0 RID: 1696
	public bool fastForwardActive;

	// Token: 0x040006A1 RID: 1697
	public Image fader;

	// Token: 0x02000918 RID: 2328
	public enum TrappedChar
	{
		// Token: 0x040044F3 RID: 17651
		None = -1,
		// Token: 0x040044F4 RID: 17652
		Cuphead,
		// Token: 0x040044F5 RID: 17653
		Mugman,
		// Token: 0x040044F6 RID: 17654
		Chalice
	}
}
