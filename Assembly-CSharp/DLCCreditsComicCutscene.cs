using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class DLCCreditsComicCutscene : Cutscene
{
	// Token: 0x0600087A RID: 2170 RVA: 0x00008210 File Offset: 0x00006410
	public override void Start()
	{
		base.Start();
		CutsceneGUI.Current.pause.pauseAllowed = false;
		this.input = new CupheadInput.AnyPlayerInput(false);
		base.StartCoroutine(this.credits_cr());
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00075FC8 File Offset: 0x000741C8
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

	// Token: 0x0600087C RID: 2172 RVA: 0x000760B0 File Offset: 0x000742B0
	public IEnumerator credits_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.canSkip = true;
		AudioManager.PlayBGM();
		float distance = this.panels.GetLast<SpriteRenderer>().transform.position.x - this.panels[0].transform.position.x - DLCCreditsComicCutscene.EndingAdjustment;
		float elapsedTime = 0f;
		while (elapsedTime < DLCCreditsComicCutscene.ScrollDuration)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta * this.multiplier;
			Vector3 position = this.parentTransform.position;
			position.x = Mathf.Lerp(0f, -distance, elapsedTime / DLCCreditsComicCutscene.ScrollDuration);
			this.parentTransform.position = position;
		}
		yield return CupheadTime.WaitForSeconds(this, 5f);
		this.canSkip = false;
		this.goToNext();
		yield break;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00008241 File Offset: 0x00006441
	public void goToNext()
	{
		SceneLoader.LoadScene(Scenes.scene_cutscene_dlc_credits, SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x04000672 RID: 1650
	public static readonly float AdjustmentAmount = -1f;

	// Token: 0x04000673 RID: 1651
	public static readonly float EndingAdjustment = 15f;

	// Token: 0x04000674 RID: 1652
	public static readonly float ScrollDuration = 90.8f;

	// Token: 0x04000675 RID: 1653
	[SerializeField]
	public Transform parentTransform;

	// Token: 0x04000676 RID: 1654
	[SerializeField]
	public SpriteRenderer[] panels;

	// Token: 0x04000677 RID: 1655
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04000678 RID: 1656
	public bool canSkip;

	// Token: 0x04000679 RID: 1657
	public float multiplier = 1f;
}
