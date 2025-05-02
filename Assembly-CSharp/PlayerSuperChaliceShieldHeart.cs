using System;
using UnityEngine;

// Token: 0x0200052D RID: 1325
public class PlayerSuperChaliceShieldHeart : MonoBehaviour
{
	// Token: 0x060037CF RID: 14287 RVA: 0x0002D8D6 File Offset: 0x0002BAD6
	public void Destroy()
	{
		this.popped = true;
		this.animator.Play("HeartDie");
		AudioManager.Play("player_super_chalice_shield_end");
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x0002D8F9 File Offset: 0x0002BAF9
	public void HeartDie()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x00104FC4 File Offset: 0x001031C4
	public void FixedUpdate()
	{
		if (!this.popped && this.player != null)
		{
			this.offset = new Vector3(this.hoverWidth * Mathf.Cos(this.hoverTime) / (1f + Mathf.Sin(this.hoverTime) * Mathf.Sin(this.hoverTime)), this.hoverWidth * Mathf.Sin(this.hoverTime) * Mathf.Cos(this.hoverTime) / (1f + Mathf.Sin(this.hoverTime) * Mathf.Sin(this.hoverTime)));
			this.hoverTime += CupheadTime.FixedDelta * 2f;
			this.lerpSpeed = Mathf.Min(this.lerpSpeed + CupheadTime.FixedDelta, 3f);
			base.transform.position = Vector3.Lerp(base.transform.position, this.player.transform.position + this.offset, CupheadTime.FixedDelta * this.lerpSpeed);
			base.transform.localScale = new Vector3(this.player.transform.localScale.x, 1f);
		}
	}

	// Token: 0x04002CFA RID: 11514
	[SerializeField]
	public Animator animator;

	// Token: 0x04002CFB RID: 11515
	public Transform player;

	// Token: 0x04002CFC RID: 11516
	public Vector3 offset;

	// Token: 0x04002CFD RID: 11517
	public float hoverTime = 1.57079637f;

	// Token: 0x04002CFE RID: 11518
	public float hoverWidth = 100f;

	// Token: 0x04002CFF RID: 11519
	public bool popped;

	// Token: 0x04002D00 RID: 11520
	public float lerpSpeed;
}
