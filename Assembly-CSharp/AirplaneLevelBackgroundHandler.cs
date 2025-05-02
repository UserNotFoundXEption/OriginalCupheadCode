using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class AirplaneLevelBackgroundHandler : MonoBehaviour
{
	// Token: 0x06000D98 RID: 3480 RVA: 0x0000B9C2 File Offset: 0x00009BC2
	public void Start()
	{
		this.hillsFrameIndex = this.hillsSprites.Length - 1;
		base.StartCoroutine(this.main_loop_cr());
		base.StartCoroutine(this.cloud_loop_cr());
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x0008784C File Offset: 0x00085A4C
	public IEnumerator cloud_loop_cr()
	{
		bool[] useAlternate = new bool[8];
		int[] lastCloud = new int[]
		{
			-1,
			-1,
			-1
		};
		for (int i = 0; i < 4; i++)
		{
			int num = Random.Range(0, 8);
			if (num != 3 && num != lastCloud[0] && num != lastCloud[1] && num != lastCloud[2])
			{
				this.cloudRenderers[i].flipX = (num >= 4);
				this.cloudAnimators[i].Play((num % 4 * 2 + ((!useAlternate[num]) ? 1 : 0)).ToString(), 0, Random.Range(0f, 1f));
				useAlternate[num] = !useAlternate[num];
				lastCloud[2] = lastCloud[1];
				lastCloud[1] = lastCloud[0];
				lastCloud[0] = num;
			}
		}
		for (;;)
		{
			int delay = Random.Range(this.CLOUD_DELAY_FRAMES_MIN, this.CLOUD_DELAY_FRAMES_MAX);
			int t = 0;
			while (t < delay)
			{
				if (!CupheadTime.IsPaused())
				{
					t++;
				}
				yield return null;
			}
			int myRenderer = -1;
			for (int j = 0; j < this.cloudRenderers.Length; j++)
			{
				if (this.cloudRenderers[j].sprite == null)
				{
					myRenderer = j;
					break;
				}
			}
			if (myRenderer == -1)
			{
				yield return null;
			}
			else
			{
				int num2 = Random.Range(0, 8);
				if (num2 == 3 && (float)Random.Range(0, 1) < 0.75f)
				{
					num2 += Random.Range(1, 3) * ((!MathUtils.RandomBool()) ? 1 : -1);
				}
				if (num2 != lastCloud[0] && num2 != lastCloud[1] && num2 != lastCloud[2])
				{
					this.cloudRenderers[myRenderer].flipX = (num2 >= 4);
					if (num2 == 3)
					{
						this.cloudRenderers[myRenderer].flipX = MathUtils.RandomBool();
					}
					this.cloudAnimators[myRenderer].Play((num2 % 4 * 2 + ((!useAlternate[num2]) ? 1 : 0)).ToString(), 0, 0f);
					useAlternate[num2] = !useAlternate[num2];
					lastCloud[2] = lastCloud[1];
					lastCloud[1] = lastCloud[0];
					lastCloud[0] = num2;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x00087868 File Offset: 0x00085A68
	public IEnumerator play_object_cr(int objectNum, int myIndex)
	{
		this.groundControllerCoroutineCurrentObject.Add(objectNum);
		for (;;)
		{
			while (this.groundControllerCoroutineCurrentObject[myIndex] == -1)
			{
				yield return null;
			}
			objectNum = this.groundControllerCoroutineCurrentObject[myIndex];
			while (this.hillsFrameIndex < this.objects[objectNum].startFrame)
			{
				yield return null;
			}
			int myRenderer = -1;
			for (int i = 0; i < this.spriteRenderers.Length; i++)
			{
				if (this.spriteRenderers[i].sprite == null)
				{
					myRenderer = i;
					break;
				}
			}
			if (myRenderer != -1)
			{
				int frameCounter = 0;
				int curHillsFrameIndex = this.hillsFrameIndex;
				while (frameCounter < this.objects[objectNum].duration)
				{
					this.spriteRenderers[myRenderer].sprite = this.objectSprites[this.objects[objectNum].spriteIndex + frameCounter];
					this.spriteRenderers[myRenderer].sortingOrder = -100 - frameCounter * 2 + this.objects[objectNum].layerOffset;
					while (curHillsFrameIndex == this.hillsFrameIndex)
					{
						yield return null;
					}
					curHillsFrameIndex = this.hillsFrameIndex;
					frameCounter++;
				}
				this.spriteRenderers[myRenderer].sprite = null;
			}
			this.groundControllerCoroutineCurrentObject[myIndex] = -1;
		}
		yield break;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x00087894 File Offset: 0x00085A94
	public IEnumerator main_loop_cr()
	{
		bool[] startObject = new bool[this.objects.Length];
		for (;;)
		{
			this.hillsFrameIndex = (this.hillsFrameIndex + 1) % this.hillsSprites.Length;
			if (this.hillsFrameIndex == 0)
			{
				this.densityWavePosition += this.densityWaveRate;
				for (int i = 0; i < this.objects.Length; i++)
				{
					startObject[i] = (Random.Range(0f, 1f) < 0.4f + Mathf.Sin(this.densityWavePosition) * 0.2f);
				}
				if (startObject[3] && startObject[5])
				{
					startObject[(!MathUtils.RandomBool()) ? 5 : 3] = false;
				}
				for (int j = 0; j < this.objects.Length; j++)
				{
					if (startObject[j])
					{
						int num = -1;
						for (int k = 0; k < this.groundControllerCoroutine.Count; k++)
						{
							if (this.groundControllerCoroutineCurrentObject[k] == -1)
							{
								num = k;
								break;
							}
						}
						if (num > -1)
						{
							this.groundControllerCoroutineCurrentObject[num] = j;
						}
						else
						{
							this.groundControllerCoroutine.Add(base.StartCoroutine(this.play_object_cr(j, this.groundControllerCoroutine.Count)));
						}
					}
				}
			}
			if (this.prepopulateCounter >= 48)
			{
				yield return new WaitForEndOfFrame();
			}
			this.hillsRenderer.sprite = this.hillsSprites[this.hillsFrameIndex];
			this.bgFillSprite.color = this.bgColor[this.hillsFrameIndex];
			if (this.prepopulateCounter >= 48)
			{
				yield return CupheadTime.WaitForSeconds(this, 1f / this.frameRate);
			}
			else
			{
				yield return null;
			}
			this.prepopulateCounter++;
		}
		yield break;
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x000878B0 File Offset: 0x00085AB0
	public void Update()
	{
		this.distantHillsTimer += CupheadTime.Delta;
		float num = this.distantHillsTimer % (this.distantHillsLoopTime * (float)this.distantHillsRenderers.Length) / (this.distantHillsLoopTime * (float)this.distantHillsRenderers.Length);
		for (int i = 0; i < this.distantHillsRenderers.Length; i++)
		{
			float num2 = EaseUtils.EaseOutCubic(this.distantHillsMaxScale, this.distantHillsMinScale, (num + (float)i * (1f / (float)this.distantHillsRenderers.Length)) % 1f);
			this.distantHillsRenderers[i].transform.localScale = new Vector3(num2, num2);
			this.distantHillsRenderers[i].sortingOrder = -490 - (int)((num * (float)this.distantHillsRenderers.Length + (float)i) % (float)this.distantHillsRenderers.Length);
			this.distantHillsRenderers[i].color = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(this.distantHillsFadeStartScale, this.distantHillsMinScale, num2));
		}
	}

	// Token: 0x04000A99 RID: 2713
	public int CLOUD_DELAY_FRAMES_MIN = 5;

	// Token: 0x04000A9A RID: 2714
	public int CLOUD_DELAY_FRAMES_MAX = 10;

	// Token: 0x04000A9B RID: 2715
	[SerializeField]
	public float frameRate = 30f;

	// Token: 0x04000A9C RID: 2716
	[SerializeField]
	public Color[] bgColor;

	// Token: 0x04000A9D RID: 2717
	[SerializeField]
	public SpriteRenderer bgFillSprite;

	// Token: 0x04000A9E RID: 2718
	[SerializeField]
	public Sprite[] hillsSprites;

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	public SpriteRenderer hillsRenderer;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	public AirplaneLevelBackgroundHandler.bgObject[] objects;

	// Token: 0x04000AA1 RID: 2721
	[SerializeField]
	public SpriteRenderer[] spriteRenderers;

	// Token: 0x04000AA2 RID: 2722
	[SerializeField]
	public Sprite[] objectSprites;

	// Token: 0x04000AA3 RID: 2723
	[SerializeField]
	public SpriteRenderer[] cloudRenderers;

	// Token: 0x04000AA4 RID: 2724
	[SerializeField]
	public Animator[] cloudAnimators;

	// Token: 0x04000AA5 RID: 2725
	[SerializeField]
	public SpriteRenderer[] distantHillsRenderers;

	// Token: 0x04000AA6 RID: 2726
	public float distantHillsTimer;

	// Token: 0x04000AA7 RID: 2727
	[SerializeField]
	public float distantHillsLoopTime = 40f;

	// Token: 0x04000AA8 RID: 2728
	[SerializeField]
	public float distantHillsMaxScale = 3f;

	// Token: 0x04000AA9 RID: 2729
	[SerializeField]
	public float distantHillsMinScale = 0.04f;

	// Token: 0x04000AAA RID: 2730
	[SerializeField]
	public float distantHillsFadeStartScale = 0.2f;

	// Token: 0x04000AAB RID: 2731
	public List<Coroutine> groundControllerCoroutine = new List<Coroutine>();

	// Token: 0x04000AAC RID: 2732
	public List<int> groundControllerCoroutineCurrentObject = new List<int>();

	// Token: 0x04000AAD RID: 2733
	public int hillsFrameIndex;

	// Token: 0x04000AAE RID: 2734
	public int prepopulateCounter;

	// Token: 0x04000AAF RID: 2735
	public float densityWavePosition;

	// Token: 0x04000AB0 RID: 2736
	public float densityWaveRate = 0.5f;

	// Token: 0x020009A7 RID: 2471
	[Serializable]
	public struct bgObject
	{
		// Token: 0x040047D8 RID: 18392
		public string nameForSanity;

		// Token: 0x040047D9 RID: 18393
		public int startFrame;

		// Token: 0x040047DA RID: 18394
		public int duration;

		// Token: 0x040047DB RID: 18395
		public int spriteIndex;

		// Token: 0x040047DC RID: 18396
		public int layerOffset;
	}
}
