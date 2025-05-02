using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000406 RID: 1030
public class CircusPlatformingLevelClouds : AbstractPausableComponent
{
	// Token: 0x06002CFA RID: 11514 RVA: 0x000258EF File Offset: 0x00023AEF
	public void Start()
	{
		base.StartCoroutine(this.change_y_axis());
	}

	// Token: 0x06002CFB RID: 11515 RVA: 0x000DBAD0 File Offset: 0x000D9CD0
	public IEnumerator change_y_axis()
	{
		float[] cloudStartPositionsX = new float[this.cloudPieces.Length];
		float[] cloudStartSpeedX = new float[this.cloudPieces.Length];
		for (int j = 0; j < this.cloudPieces.Length; j++)
		{
			cloudStartPositionsX[j] = this.cloudPieces[j].cloud.position.y;
			foreach (ScrollingSprite scrollingSprite in this.cloudPieces[j].cloud.GetComponentsInChildren<ScrollingSprite>())
			{
				cloudStartSpeedX[j] = scrollingSprite.speed;
			}
		}
		for (;;)
		{
			for (int i = 0; i < this.cloudPieces.Length; i++)
			{
				this.cloudPieces[i].cloud.SetPosition(null, new float?(Mathf.Lerp(cloudStartPositionsX[i], this.cloudPieces[i].cloudEndY, this.RelativePosition(this.cloudPieces[i].cameraRelativePosX))), null);
				if (CupheadLevelCamera.Current.transform.position != this.lastPosition)
				{
					if (this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>())
					{
						this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>().enabled = false;
					}
					foreach (ScrollingSprite scrollingSprite2 in this.cloudPieces[i].cloud.GetComponentsInChildren<ScrollingSprite>())
					{
						if (CupheadLevelCamera.Current.transform.position.x < this.lastPosition.x)
						{
							if (scrollingSprite2.speed > cloudStartSpeedX[i])
							{
								scrollingSprite2.speed -= this.incrementAmount;
							}
						}
						else if (scrollingSprite2.speed < cloudStartSpeedX[i] * this.cloudPieces[i].speedMultiplyAmount)
						{
							scrollingSprite2.speed += this.incrementAmount;
						}
					}
					this.cloudPieces[i].UpdateCurrentRelativePos(this.RelativePosition(this.cloudPieces[i].cameraRelativePosX));
					this.lastPosition = CupheadLevelCamera.Current.transform.position;
					yield return null;
				}
				else
				{
					if (this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>())
					{
						this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>().enabled = true;
						this.cloudPieces[i].cloud.GetComponent<PlatformingLevelParallax>().UpdateBasePosition();
					}
					foreach (ScrollingSprite scrollingSprite3 in this.cloudPieces[i].cloud.GetComponentsInChildren<ScrollingSprite>())
					{
						if (scrollingSprite3.speed > cloudStartSpeedX[i])
						{
							scrollingSprite3.speed -= this.incrementAmount;
						}
						else
						{
							scrollingSprite3.speed = cloudStartSpeedX[i];
						}
					}
					yield return null;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x000DBAEC File Offset: 0x000D9CEC
	public float RelativePosition(float relativePosX)
	{
		float num = relativePosX - (float)Level.Current.Left;
		float num2 = CupheadLevelCamera.Current.transform.position.x - (float)Level.Current.Left;
		return num2 / num;
	}

	// Token: 0x04002539 RID: 9529
	[SerializeField]
	public CircusPlatformingLevelClouds.CloudPiece[] cloudPieces;

	// Token: 0x0400253A RID: 9530
	public Vector3 lastPosition;

	// Token: 0x0400253B RID: 9531
	[SerializeField]
	public float incrementAmount = 2f;

	// Token: 0x02001051 RID: 4177
	[Serializable]
	public class CloudPiece
	{
		// Token: 0x0600788F RID: 30863 RVA: 0x00051C03 File Offset: 0x0004FE03
		public void UpdateCurrentRelativePos(float pos)
		{
			this.currentRelativePosX = pos;
		}

		// Token: 0x06007890 RID: 30864 RVA: 0x00051C0C File Offset: 0x0004FE0C
		public float CurrentRelativePosX()
		{
			return this.currentRelativePosX;
		}

		// Token: 0x04007442 RID: 29762
		public Transform cloud;

		// Token: 0x04007443 RID: 29763
		public float cloudEndY;

		// Token: 0x04007444 RID: 29764
		public float cameraRelativePosX;

		// Token: 0x04007445 RID: 29765
		public float speedMultiplyAmount;

		// Token: 0x04007446 RID: 29766
		public float currentRelativePosX;
	}
}
