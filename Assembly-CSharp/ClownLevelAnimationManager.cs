using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class ClownLevelAnimationManager : AbstractPausableComponent
{
	// Token: 0x0600136C RID: 4972 RVA: 0x000104D3 File Offset: 0x0000E6D3
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x00097D7C File Offset: 0x00095F7C
	public void Start()
	{
		this.headSprite = this.headSprite.GetComponent<Animator>();
		this.pivotPoint.position = this.balloonSprite.position;
		base.StartCoroutine(this.head_cr());
		base.StartCoroutine(this.balloon_loop_cr());
		foreach (Animator ani in this.twelveFpsAnimations)
		{
			base.StartCoroutine(this.manual_fps_animation_cr(ani, 0.0833333358f));
		}
		foreach (Animator ani2 in this.twentyFourFpsAnimations)
		{
			base.StartCoroutine(this.manual_fps_animation_cr(ani2, 0.0416666679f));
		}
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x00097E38 File Offset: 0x00096038
	public IEnumerator head_cr()
	{
		for (;;)
		{
			float getSeconds = Random.Range(3f, 8f);
			this.headSprite.SetTrigger("Continue");
			yield return CupheadTime.WaitForSeconds(this, getSeconds);
		}
		yield break;
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x00097E54 File Offset: 0x00096054
	public IEnumerator balloon_loop_cr()
	{
		float loopSize = 20f;
		float speed = 1f;
		float angle = 0f;
		for (;;)
		{
			Vector3 pivotOffset = Vector3.left * 2f * loopSize;
			angle += speed * CupheadTime.Delta;
			if (angle > 6.28318548f)
			{
				this.invert = !this.invert;
				angle -= 6.28318548f;
			}
			if (angle < 0f)
			{
				angle += 6.28318548f;
			}
			float value;
			if (this.invert)
			{
				this.balloonSprite.position = this.pivotPoint.position + pivotOffset;
				value = 1f;
			}
			else
			{
				this.balloonSprite.position = this.pivotPoint.position;
				value = -1f;
			}
			Vector3 handleRotationX = new Vector3(Mathf.Cos(angle) * value * loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(angle) * loopSize, 0f);
			this.balloonSprite.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x00097E70 File Offset: 0x00096070
	public IEnumerator manual_fps_animation_cr(Animator ani, float fps)
	{
		float frameTime = 0f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > fps)
			{
				frameTime -= fps;
				ani.enabled = true;
				ani.Update(fps);
				ani.enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000FBD RID: 4029
	[SerializeField]
	public Animator headSprite;

	// Token: 0x04000FBE RID: 4030
	[SerializeField]
	public Transform balloonSprite;

	// Token: 0x04000FBF RID: 4031
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04000FC0 RID: 4032
	[SerializeField]
	public Animator[] twelveFpsAnimations;

	// Token: 0x04000FC1 RID: 4033
	[SerializeField]
	public Animator[] twentyFourFpsAnimations;

	// Token: 0x04000FC2 RID: 4034
	public bool invert;
}
