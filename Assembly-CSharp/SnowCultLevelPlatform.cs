using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class SnowCultLevelPlatform : LevelPlatform
{
	// Token: 0x06002878 RID: 10360 RVA: 0x00021FBF File Offset: 0x000201BF
	public void SetID(int value)
	{
		base.animator.SetInteger("ID", value % 5);
	}

	// Token: 0x06002879 RID: 10361 RVA: 0x000CE760 File Offset: 0x000CC960
	public void StartRotate(float angle, Vector3 pivotPoint, float loopSizeX, float loopSizeY, float speed, float pathOffset, bool isClockwise)
	{
		this.angle = angle;
		this.pivotPos = pivotPoint;
		this.speed = speed;
		this.loopSizeX = loopSizeX;
		this.loopSizeY = loopSizeY;
		this.isClockwise = isClockwise;
		base.StartCoroutine(this.move_rotating_platforms_cr());
		base.StartCoroutine(this.sheen_cr());
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x000CE7B4 File Offset: 0x000CC9B4
	public IEnumerator move_rotating_platforms_cr()
	{
		Vector3 handleRotationX = Vector3.zero;
		if (this.angle == 0f || this.angle == 180f)
		{
			base.transform.parent.position = this.pivotPos + MathUtils.AngleToDirection(this.angle) * this.loopSizeX;
		}
		else
		{
			base.transform.parent.position = this.pivotPos + MathUtils.AngleToDirection(this.angle) * this.loopSizeY;
		}
		this.angle *= 0.0174532924f;
		for (;;)
		{
			this.angle += this.speed * CupheadTime.FixedDelta;
			if (this.isClockwise)
			{
				handleRotationX = new Vector3(Mathf.Sin(this.angle) * this.loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.angle) * this.loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSizeY, 0f);
			base.transform.parent.position = this.pivotPos;
			base.transform.parent.position += handleRotationX + handleRotationY;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x000CE7D0 File Offset: 0x000CC9D0
	public IEnumerator sheen_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(this.sheenTimeMin, this.sheenTimeMax));
			base.animator.SetTrigger("Sheen");
			while (!base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Sheen"))
			{
				yield return null;
			}
			while (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Sheen"))
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x000CE7EC File Offset: 0x000CC9EC
	public void FixedUpdate()
	{
		this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (this.player1 != null)
		{
			this.p1IsColliding = (this.player1.transform.parent == base.transform);
			this.player1.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p1IsColliding = false;
		}
		if (this.player2 != null)
		{
			this.p2IsColliding = (this.player2.transform.parent == base.transform);
			this.player2.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p2IsColliding = false;
		}
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, (!this.p1IsColliding && !this.p2IsColliding) ? 0f : this.downDist), this.bounceSpeed * CupheadTime.FixedDelta);
	}

	// Token: 0x040021A8 RID: 8616
	[SerializeField]
	public float downDist = -30f;

	// Token: 0x040021A9 RID: 8617
	[SerializeField]
	public float bounceSpeed = 20f;

	// Token: 0x040021AA RID: 8618
	public Vector3 pivotPos;

	// Token: 0x040021AB RID: 8619
	public bool isClockwise;

	// Token: 0x040021AC RID: 8620
	public float angle;

	// Token: 0x040021AD RID: 8621
	public float speed;

	// Token: 0x040021AE RID: 8622
	public float loopSizeX;

	// Token: 0x040021AF RID: 8623
	public float loopSizeY;

	// Token: 0x040021B0 RID: 8624
	public AbstractPlayerController player1;

	// Token: 0x040021B1 RID: 8625
	public AbstractPlayerController player2;

	// Token: 0x040021B2 RID: 8626
	public bool p1IsColliding;

	// Token: 0x040021B3 RID: 8627
	public bool p2IsColliding;

	// Token: 0x040021B4 RID: 8628
	[SerializeField]
	public float sheenTimeMin = 1f;

	// Token: 0x040021B5 RID: 8629
	[SerializeField]
	public float sheenTimeMax = 5f;
}
