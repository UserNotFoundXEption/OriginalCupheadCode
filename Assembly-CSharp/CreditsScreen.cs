using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B2 RID: 178
public class CreditsScreen : AbstractMonoBehaviour
{
	// Token: 0x06000849 RID: 2121 RVA: 0x00007F32 File Offset: 0x00006132
	public void Start()
	{
		this.Init(false);
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00075D14 File Offset: 0x00073F14
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.verticalLayoutGroup = this.content.GetComponent<VerticalLayoutGroup>();
		base.StartCoroutine(this.credits_cr());
		base.StartCoroutine(this.skip_cr());
		base.StartCoroutine(this.fastForward_cr());
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00075D68 File Offset: 0x00073F68
	public IEnumerator credits_cr()
	{
		float wait = this.introDuration;
		while (wait > 0f)
		{
			wait -= CupheadTime.Delta * this.timeMultiplier;
			yield return null;
		}
		float accumulator = 0f;
		while (this.content.anchoredPosition.y < this.verticalLayoutGroup.preferredHeight - base.rectTransform.sizeDelta.y)
		{
			accumulator += CupheadTime.Delta * this.timeMultiplier;
			while (accumulator > 0.0416666679f)
			{
				accumulator -= 0.0416666679f;
				this.content.anchoredPosition = new Vector2(0f, this.content.anchoredPosition.y + this.scrollSpeed * 0.0416666679f);
			}
			yield return null;
		}
		this.doneScrolling = true;
		wait = this.outroDuration;
		while (wait > 0f)
		{
			wait -= CupheadTime.Delta * this.timeMultiplier;
			yield return null;
		}
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		yield break;
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00075D84 File Offset: 0x00073F84
	public IEnumerator fastForward_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetAnyButtonHeld() && !this.input.GetButtonDown(CupheadButton.Pause) && !this.doneScrolling)
			{
				if (this.timeMultiplier == 1f)
				{
					this.timeMultiplier = 8f;
					AudioManager.ChangeBGMPitch(8f, 0.125f);
				}
			}
			else if (this.timeMultiplier > 1f)
			{
				this.timeMultiplier = 1f;
				AudioManager.ChangeBGMPitch(1f, 0.125f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00075DA0 File Offset: 0x00073FA0
	public IEnumerator skip_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				PlayerManager.ResetPlayers();
				SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00007F3B File Offset: 0x0000613B
	public void LateUpdate()
	{
		if (CupheadMapCamera.Current == null)
		{
			return;
		}
		base.transform.position = CupheadMapCamera.Current.transform.position;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00007F68 File Offset: 0x00006168
	public bool GetButtonDown(CupheadButton button)
	{
		return this.input.GetButtonDown(button);
	}

	// Token: 0x04000654 RID: 1620
	public static bool goodEnding = true;

	// Token: 0x04000655 RID: 1621
	[SerializeField]
	public RectTransform content;

	// Token: 0x04000656 RID: 1622
	public VerticalLayoutGroup verticalLayoutGroup;

	// Token: 0x04000657 RID: 1623
	[SerializeField]
	public float introDuration;

	// Token: 0x04000658 RID: 1624
	[SerializeField]
	public float scrollSpeed;

	// Token: 0x04000659 RID: 1625
	[SerializeField]
	public float outroDuration;

	// Token: 0x0400065A RID: 1626
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x0400065B RID: 1627
	public bool doneScrolling;

	// Token: 0x0400065C RID: 1628
	public float timeMultiplier = 1f;
}
