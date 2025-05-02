using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004DD RID: 1245
public class SplashScreenMDHR : AbstractMonoBehaviour
{
	// Token: 0x06003393 RID: 13203 RVA: 0x0002AA78 File Offset: 0x00028C78
	public override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.fader.color = new Color(0f, 0f, 0f, 1f);
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x0002AAB6 File Offset: 0x00028CB6
	public void Start()
	{
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x0002AAC5 File Offset: 0x00028CC5
	public void Update()
	{
		if (this.fading)
		{
			return;
		}
		if (this.input.GetButtonDown(CupheadButton.Accept))
		{
			this.BeginFadeOut();
		}
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x000F588C File Offset: 0x000F3A8C
	public IEnumerator go_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.animator.Play("Logo");
		yield break;
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x0002AAEB File Offset: 0x00028CEB
	public void BeginFadeOut()
	{
		if (this.fading)
		{
			return;
		}
		this.fading = true;
		SceneLoader.properties.transitionEnd = SceneLoader.Transition.Iris;
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Iris;
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x04002AD7 RID: 10967
	public const float WAIT = 3f;

	// Token: 0x04002AD8 RID: 10968
	[SerializeField]
	public SpriteRenderer fader;

	// Token: 0x04002AD9 RID: 10969
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x04002ADA RID: 10970
	public bool fading;
}
