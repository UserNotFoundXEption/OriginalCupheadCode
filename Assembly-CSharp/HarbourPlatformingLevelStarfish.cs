using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000434 RID: 1076
public class HarbourPlatformingLevelStarfish : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002E65 RID: 11877 RVA: 0x000DF7E4 File Offset: 0x000DD9E4
	public void Init(float rotation, float speedX, float speedY, float loopSize, string type)
	{
		base.transform.SetEulerAngles(null, null, new float?(rotation - 90f));
		this.figureEightSpeed = speedX;
		this.movementSpeed = speedY;
		this.loopSize = loopSize;
		this.type = type;
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x00026B34 File Offset: 0x00024D34
	public override void OnStart()
	{
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x000DF838 File Offset: 0x000DDA38
	public override void Start()
	{
		base.Start();
		this.pivotOffset = Vector3.up * 2f * this.loopSize;
		this.pivotPoint = new GameObject("PivotPoint");
		this.pivotPoint.transform.position = base.transform.position;
		if (this.type == "A")
		{
			this._canParry = true;
			this.bubbles = this.pinkBubbles;
		}
		else
		{
			this.bubbles = this.normalBubbles;
		}
		base.GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(this.type);
		base.StartCoroutine(this.spawn_bubbles_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.figure_eight_cr());
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x000DF908 File Offset: 0x000DDB08
	public IEnumerator move_cr()
	{
		for (;;)
		{
			this.pivotPoint.transform.position += base.transform.up * this.movementSpeed * CupheadTime.FixedDelta;
			if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax + 200f)
			{
				break;
			}
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x000DF924 File Offset: 0x000DDB24
	public IEnumerator figure_eight_cr()
	{
		bool invert = false;
		for (;;)
		{
			this.angle += this.figureEightSpeed * CupheadTime.Delta;
			if (this.angle > 6.28318548f)
			{
				invert = !invert;
				this.angle -= 6.28318548f;
			}
			if (this.angle < 0f)
			{
				this.angle += 6.28318548f;
			}
			float value;
			if (invert)
			{
				base.transform.position = this.pivotPoint.transform.position + this.pivotOffset;
				value = -1f;
			}
			else
			{
				base.transform.position = this.pivotPoint.transform.position;
				value = 1f;
			}
			Vector3 handleRotation = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, Mathf.Cos(this.angle) * value * this.loopSize, 0f);
			base.transform.position += handleRotation;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x000DF940 File Offset: 0x000DDB40
	public IEnumerator spawn_bubbles_cr()
	{
		string bubbleTypes = "A,B,C,D";
		for (;;)
		{
			string bubbleType = bubbleTypes.Split(new char[]
			{
				','
			})[Random.Range(0, bubbleTypes.Split(new char[]
			{
				','
			}).Length)];
			this.bubbles.Create(base.transform.position).GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(bubbleType);
			yield return CupheadTime.WaitForSeconds(this, 1f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x00026B36 File Offset: 0x00024D36
	public override void Die()
	{
		AudioManager.Play("harbour_star_death");
		this.emitAudioFromObject.Add("harbour_star_death");
		base.Die();
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x00026B58 File Offset: 0x00024D58
	public override void OnDestroy()
	{
		base.OnDestroy();
		Object.Destroy(this.pivotPoint.gameObject);
	}

	// Token: 0x04002679 RID: 9849
	[SerializeField]
	public Effect normalBubbles;

	// Token: 0x0400267A RID: 9850
	[SerializeField]
	public Effect pinkBubbles;

	// Token: 0x0400267B RID: 9851
	public GameObject pivotPoint;

	// Token: 0x0400267C RID: 9852
	public float angle;

	// Token: 0x0400267D RID: 9853
	public float figureEightSpeed;

	// Token: 0x0400267E RID: 9854
	public float movementSpeed;

	// Token: 0x0400267F RID: 9855
	public float loopSize;

	// Token: 0x04002680 RID: 9856
	public string type;

	// Token: 0x04002681 RID: 9857
	public Vector3 pivotOffset;

	// Token: 0x04002682 RID: 9858
	public Effect bubbles;
}
