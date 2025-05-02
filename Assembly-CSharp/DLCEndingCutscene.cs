using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class DLCEndingCutscene : DLCGenericCutscene
{
	// Token: 0x0600089E RID: 2206 RVA: 0x000761D0 File Offset: 0x000743D0
	public override void Start()
	{
		base.Start();
		SceneLoader.OnLoaderCompleteEvent += this.StartMusic;
		if (this.trappedChar == DLCGenericCutscene.TrappedChar.None)
		{
			this.trappedChar = base.DetectCharacter();
		}
		this.shot1Animator.SetBool("NeedToSwap", this.trappedChar != DLCGenericCutscene.TrappedChar.Chalice);
		this.rightCuphead.SetActive(false);
		this.ghostBodyChalice.SetActive(this.trappedChar == DLCGenericCutscene.TrappedChar.Chalice);
		this.ghostBodyCHMM.SetActive(this.trappedChar != DLCGenericCutscene.TrappedChar.Chalice);
		DLCGenericCutscene.TrappedChar trappedChar = this.trappedChar;
		if (trappedChar != DLCGenericCutscene.TrappedChar.Chalice)
		{
			if (trappedChar != DLCGenericCutscene.TrappedChar.Mugman)
			{
				if (trappedChar == DLCGenericCutscene.TrappedChar.Cuphead)
				{
					this.leftCuphead.SetActive(false);
					this.rightMugman.SetActive(false);
					this.rightCuphead.SetActive(false);
					this.trappedChalice.SetActive(false);
					this.trappedMugman.SetActive(false);
					this.text[0] = this.altText[1];
				}
			}
			else
			{
				this.leftMugman.SetActive(false);
				this.rightMugman.SetActive(false);
				this.trappedChalice.SetActive(false);
				this.trappedCuphead.SetActive(false);
				this.text[0] = this.altText[0];
			}
		}
		else
		{
			this.leftMugman.SetActive(false);
			this.rightChalice.SetActive(false);
			this.trappedMugman.SetActive(false);
			this.trappedCuphead.SetActive(false);
		}
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00008412 File Offset: 0x00006612
	public void StartMusic()
	{
		base.StartCoroutine(this.handle_music_cr());
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0007634C File Offset: 0x0007454C
	public IEnumerator handle_music_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.StartBGMAlternate(0);
		yield return base.StartCoroutine(this.hold_for_music_advance_and_loop(5.5f, string.Empty));
		AudioManager.StartBGMAlternate(1);
		yield return base.StartCoroutine(this.hold_for_music_advance_and_loop(3f, string.Empty));
		AudioManager.StartBGMAlternate(2);
		yield break;
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00076368 File Offset: 0x00074568
	public IEnumerator hold_for_music_advance_and_loop(float time, string loopName)
	{
		float t = 0f;
		this.advanceMusic = false;
		while (t < time && !this.advanceMusic)
		{
			t += Time.deltaTime;
			yield return null;
		}
		if (!this.advanceMusic)
		{
			AudioManager.PlayLoop(loopName);
		}
		while (!this.advanceMusic)
		{
			yield return null;
		}
		AudioManager.Stop(loopName);
		yield break;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00076394 File Offset: 0x00074594
	public IEnumerator crossfade_final_music_cr()
	{
		AudioManager.FadeBGMVolume(0f, 1.5f, true);
		AudioManager.FadeSFXVolume("mus_dlc_ending_4", 0.0001f, 0.0001f);
		yield return null;
		AudioManager.Play("mus_dlc_ending_4");
		AudioManager.FadeSFXVolume("mus_dlc_ending_4", 0.4f, 1.5f);
		yield break;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x00008421 File Offset: 0x00006621
	public void AdvanceMusic()
	{
		this.advanceMusic = true;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x000763A8 File Offset: 0x000745A8
	public void SwapChars()
	{
		this.rightChalice.SetActive(false);
		this.trappedChalice.SetActive(true);
		this.ghostBodyChalice.SetActive(true);
		this.ghostBodyCHMM.SetActive(false);
		DLCGenericCutscene.TrappedChar trappedChar = this.trappedChar;
		if (trappedChar != DLCGenericCutscene.TrappedChar.Mugman)
		{
			if (trappedChar == DLCGenericCutscene.TrappedChar.Cuphead)
			{
				this.rightCuphead.SetActive(true);
				this.trappedCuphead.SetActive(false);
			}
		}
		else
		{
			this.rightMugman.SetActive(true);
			this.trappedMugman.SetActive(false);
		}
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0000842A File Offset: 0x0000662A
	public override void IrisOut()
	{
		SceneLoader.LoadScene(Scenes.scene_cutscene_dlc_credits_comic, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.None, null);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00008437 File Offset: 0x00006637
	public void StartShake()
	{
		this.screenShakeAmt = 4f;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00008444 File Offset: 0x00006644
	public void StopShake()
	{
		this.screenShakeAmt = 0f;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00076438 File Offset: 0x00074638
	public void LateUpdate()
	{
		float num = Random.Range(-this.screenShakeAmt, this.screenShakeAmt);
		float num2 = Random.Range(-this.screenShakeAmt, this.screenShakeAmt);
		this.screens[this.curScreen].transform.localPosition = new Vector3(num, num2, 0f);
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00076490 File Offset: 0x00074690
	public override void OnScreenAdvance(int which)
	{
		if (which == 3)
		{
			base.StartCoroutine(this.crossfade_final_music_cr());
			GameObject gameObject = GameObject.Find("Fader");
			if (gameObject != null)
			{
				Animator component = gameObject.GetComponent<Animator>();
				if (component != null)
				{
					component.Play("Transparent");
				}
			}
		}
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00008451 File Offset: 0x00006651
	public override void OnDestroy()
	{
		SceneLoader.OnLoaderCompleteEvent -= this.StartMusic;
		base.OnDestroy();
	}

	// Token: 0x04000683 RID: 1667
	[SerializeField]
	public DLCGenericCutscene.TrappedChar trappedChar;

	// Token: 0x04000684 RID: 1668
	[SerializeField]
	public GameObject leftCuphead;

	// Token: 0x04000685 RID: 1669
	[SerializeField]
	public GameObject leftMugman;

	// Token: 0x04000686 RID: 1670
	[SerializeField]
	public GameObject rightMugman;

	// Token: 0x04000687 RID: 1671
	[SerializeField]
	public GameObject rightChalice;

	// Token: 0x04000688 RID: 1672
	[SerializeField]
	public GameObject rightCuphead;

	// Token: 0x04000689 RID: 1673
	[SerializeField]
	public GameObject trappedChalice;

	// Token: 0x0400068A RID: 1674
	[SerializeField]
	public GameObject trappedMugman;

	// Token: 0x0400068B RID: 1675
	[SerializeField]
	public GameObject trappedCuphead;

	// Token: 0x0400068C RID: 1676
	[SerializeField]
	public GameObject ghostBodyChalice;

	// Token: 0x0400068D RID: 1677
	[SerializeField]
	public GameObject ghostBodyCHMM;

	// Token: 0x0400068E RID: 1678
	[SerializeField]
	public Animator shot1Animator;

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	public GameObject[] altText;

	// Token: 0x04000690 RID: 1680
	public float screenShakeAmt;

	// Token: 0x04000691 RID: 1681
	public bool advanceMusic;

	// Token: 0x04000692 RID: 1682
	[SerializeField]
	[Range(-1f, 3f)]
	public int fastForward = -1;
}
