using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class DLCIntroCutscene : DLCGenericCutscene
{
	// Token: 0x060008D1 RID: 2257 RVA: 0x000086B7 File Offset: 0x000068B7
	public override void Start()
	{
		base.Start();
		this.allowScreenSkip = true;
		this.BGanim.speed = this.screen4BGScrollSpeed;
		this.screen4ForestScrollSpeed = this.screen4ForestScrollStartSpeed;
		AudioManager.PlayLoop("sfx_dlc_intro_oceanamb_loop");
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x000086ED File Offset: 0x000068ED
	public override void OnScreenSkip()
	{
		base.StartCoroutine(this.skip_title_cr());
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x000767C4 File Offset: 0x000749C4
	public IEnumerator skip_title_cr()
	{
		this.allowScreenSkip = false;
		base.IrisIn();
		yield return CupheadTime.WaitForSeconds(this, 0.9f);
		this.screens[this.curScreen].Play("End");
		yield break;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x000767E0 File Offset: 0x000749E0
	public override void OnScreenAdvance(int which)
	{
		if (which == 0)
		{
			this.canvas.SetActive(true);
			AudioManager.StartBGMAlternate(0);
			AudioManager.Stop("sfx_dlc_intro_oceanamb_loop");
		}
		if (which < this.astralPlanePositions.Length && this.astralPlanePositions[which])
		{
			this.astralPlaneController.position = this.astralPlanePositions[which].position;
			this.astralPlaneController.localScale = this.astralPlanePositions[which].localScale;
		}
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00076860 File Offset: 0x00074A60
	public override void OnContinue()
	{
		this.allowScreenSkip = false;
		if (this.curScreen == 3 && !this.BGanim.GetBool("End"))
		{
			this.BGanim.SetBool("End", true);
			base.StartCoroutine(this.slow_down_bg_cr());
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x000768B4 File Offset: 0x00074AB4
	public IEnumerator slow_down_bg_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float end = this.screen4BGScrollSpeed / 2f;
		while (!this.BGanim.GetCurrentAnimatorStateInfo(0).IsName("End") && !this.BGanim.GetCurrentAnimatorStateInfo(0).IsName("AltEnd"))
		{
			yield return null;
		}
		foreach (GameObject gameObject in this.screen4Characters)
		{
			gameObject.transform.parent = this.screen4ScrollEnd.transform;
		}
		while (this.BGanim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			this.BGanim.speed = EaseUtils.EaseOutSine(this.screen4BGScrollSpeed, end, this.BGanim.GetCurrentAnimatorStateInfo(0).normalizedTime);
			this.screen4ForestScrollSpeed = this.screen4ForestScrollStartSpeed * (this.BGanim.speed / this.screen4BGScrollSpeed);
			foreach (GameObject gameObject2 in this.screen4Characters)
			{
				gameObject2.transform.localPosition += Vector3.right * 6.4f;
			}
			yield return wait;
		}
		this.screen4ForestScrollSpeed = 0f;
		yield return this.screens[this.curScreen].WaitForAnimationToStart(this, "holdforBG", false);
		this.screens[this.curScreen].SetTrigger("Continue");
		while (this.screen4Characters[2].transform.localPosition.x < 1420f)
		{
			foreach (GameObject gameObject3 in this.screen4Characters)
			{
				gameObject3.transform.localPosition += Vector3.right * 6.4f;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x000768D0 File Offset: 0x00074AD0
	public override void Update()
	{
		base.Update();
		if (this.curScreen == 0)
		{
			CupheadCutsceneCamera.Current.SetPosition(this.cameraPos.transform.position);
		}
		else
		{
			CupheadCutsceneCamera.Current.SetPosition(Vector3.zero);
		}
		if (this.curScreen == 3)
		{
			this.screen4Forest.transform.localPosition += Vector3.left * this.screen4ForestScrollSpeed * CupheadTime.Delta;
			this.screen4Clouds.transform.localPosition += Vector3.left * this.screen4ForestScrollSpeed * CupheadTime.Delta * 0.5f;
			this.screen4FG.transform.localPosition += Vector3.left * this.screen4ForestScrollSpeed * CupheadTime.Delta * 1.5f;
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x000769EC File Offset: 0x00074BEC
	public void LateUpdate()
	{
		if (this.screen4Forest.transform.localPosition.x < -2560f)
		{
			this.screen4Forest.transform.localPosition += Vector3.right * 1280f;
		}
		if (this.screen4Clouds.transform.localPosition.x < -2560f)
		{
			this.screen4Clouds.transform.localPosition += Vector3.right * 1280f;
		}
		if (this.screen4FG.transform.localPosition.x < -5156f)
		{
			this.screen4FG.transform.localPosition += Vector3.right * 4767f;
		}
		if (this.screen4ScrollStart.transform.position.x < -1600f)
		{
			this.screen4ScrollStart.enabled = false;
			this.screen4EndLoopBack.enabled = true;
		}
	}

	// Token: 0x040006A5 RID: 1701
	[SerializeField]
	public GameObject canvas;

	// Token: 0x040006A6 RID: 1702
	[SerializeField]
	public GameObject cameraPos;

	// Token: 0x040006A7 RID: 1703
	[SerializeField]
	public Animator BGanim;

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	public Transform astralPlaneController;

	// Token: 0x040006A9 RID: 1705
	[SerializeField]
	public Transform[] astralPlanePositions;

	// Token: 0x040006AA RID: 1706
	[SerializeField]
	public float screen4BGScrollSpeed = 0.1f;

	// Token: 0x040006AB RID: 1707
	[SerializeField]
	public float screen4ForestScrollStartSpeed = 325f;

	// Token: 0x040006AC RID: 1708
	public float screen4ForestScrollSpeed;

	// Token: 0x040006AD RID: 1709
	[SerializeField]
	public GameObject[] screen4Characters;

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	public GameObject screen4Forest;

	// Token: 0x040006AF RID: 1711
	[SerializeField]
	public GameObject screen4Clouds;

	// Token: 0x040006B0 RID: 1712
	[SerializeField]
	public GameObject screen4FG;

	// Token: 0x040006B1 RID: 1713
	[SerializeField]
	public GameObject screen4ScrollEnd;

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	public SpriteRenderer screen4ScrollStart;

	// Token: 0x040006B3 RID: 1715
	[SerializeField]
	public SpriteRenderer screen4EndLoopBack;

	// Token: 0x040006B4 RID: 1716
	[SerializeField]
	[Range(-1f, 7f)]
	public int fastForward = -1;
}
