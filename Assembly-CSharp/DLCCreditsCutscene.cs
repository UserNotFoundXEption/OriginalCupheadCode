using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B8 RID: 184
public class DLCCreditsCutscene : Cutscene
{
	// Token: 0x06000880 RID: 2176 RVA: 0x00008281 File Offset: 0x00006481
	public override void Start()
	{
		base.Start();
		CutsceneGUI.Current.pause.pauseAllowed = false;
		this.input = new CupheadInput.AnyPlayerInput(false);
		base.StartCoroutine(this.credits_cr());
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x000760CC File Offset: 0x000742CC
	public void Update()
	{
		if (this.canSkip)
		{
			if (this.input.GetButtonDown(CupheadButton.Pause))
			{
				this.canSkip = false;
				this.StopAllCoroutines();
				this.goToNext();
				return;
			}
			if (this.input.GetAnyButtonHeld() && !this.input.GetButtonDown(CupheadButton.Pause))
			{
				if (this.multiplier == 1f)
				{
					this.multiplier = 8f;
					AudioManager.ChangeBGMPitch(8f, 0.125f);
				}
			}
			else if (this.multiplier > 1f)
			{
				this.multiplier = 1f;
				AudioManager.ChangeBGMPitch(1f, 0.125f);
			}
		}
		else if (this.multiplier > 1f)
		{
			this.multiplier = 1f;
			AudioManager.ChangeBGMPitch(1f, 0.125f);
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x000761B4 File Offset: 0x000743B4
	public IEnumerator credits_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		AudioManager.PlayBGM();
		this.canSkip = true;
		float preferredHeight = this.contentTransform.GetComponent<VerticalLayoutGroup>().preferredHeight;
		float speed = preferredHeight / this.scrollDuration;
		float elapsedTime = 0f;
		float accumulator = 0f;
		while (elapsedTime < this.scrollDuration)
		{
			yield return null;
			for (accumulator += CupheadTime.Delta * this.multiplier; accumulator > 0.0416666679f; accumulator -= 0.0416666679f)
			{
				elapsedTime += 0.0416666679f;
			}
			Vector2 position = this.contentTransform.anchoredPosition;
			position.y = Mathf.Lerp(0f, preferredHeight - 720f, elapsedTime / this.scrollDuration);
			this.contentTransform.anchoredPosition = position;
		}
		this.canSkip = false;
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.goToNext();
		yield break;
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x000082B2 File Offset: 0x000064B2
	public void goToNext()
	{
		PlayerManager.ResetPlayers();
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x0400067A RID: 1658
	[SerializeField]
	public float scrollDuration;

	// Token: 0x0400067B RID: 1659
	[SerializeField]
	public RectTransform contentTransform;

	// Token: 0x0400067C RID: 1660
	[SerializeField]
	public float memphisFontSize;

	// Token: 0x0400067D RID: 1661
	[SerializeField]
	public float vogueBoldFontSize;

	// Token: 0x0400067E RID: 1662
	[SerializeField]
	public float vogueExtraBoldFontSize;

	// Token: 0x0400067F RID: 1663
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000680 RID: 1664
	public bool canSkip;

	// Token: 0x04000681 RID: 1665
	public float multiplier = 1f;
}
