using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003B7 RID: 951
public class TrainLevelPlatform : LevelPlatform
{
	// Token: 0x06002A20 RID: 10784 RVA: 0x000D34D8 File Offset: 0x000D16D8
	public override void Awake()
	{
		base.Awake();
		this.animHelper = base.GetComponent<AnimationHelper>();
		this.middlePos = base.transform.position.x + 390f;
		this.leftPos = base.transform.position.x;
		this.rightPos = base.transform.position.x + 780f;
		this.position = TrainLevelPlatform.CartPosition.Left;
		this.rightSwitch.OnActivate += this.OnRight;
		this.leftSwitch.OnActivate += this.OnLeft;
		base.StartCoroutine(this.spark_cr());
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x000D3590 File Offset: 0x000D1790
	public void OnLeft()
	{
		if (this.isMoving)
		{
			return;
		}
		AudioManager.Play("train_hand_car_valves_spin");
		this.emitAudioFromObject.Add("train_hand_car_valves_spin");
		this.position = ((this.position != TrainLevelPlatform.CartPosition.Right) ? TrainLevelPlatform.CartPosition.Left : TrainLevelPlatform.CartPosition.Middle);
		this.Move(this.SelectPosition());
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x000D35E8 File Offset: 0x000D17E8
	public void OnRight()
	{
		if (this.isMoving)
		{
			return;
		}
		AudioManager.Play("train_hand_car_valves_spin");
		this.emitAudioFromObject.Add("train_hand_car_valves_spin");
		this.position = ((this.position != TrainLevelPlatform.CartPosition.Left) ? TrainLevelPlatform.CartPosition.Right : TrainLevelPlatform.CartPosition.Middle);
		this.Move(this.SelectPosition());
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x000D3640 File Offset: 0x000D1840
	public float SelectPosition()
	{
		float result = 0f;
		TrainLevelPlatform.CartPosition cartPosition = this.position;
		if (cartPosition != TrainLevelPlatform.CartPosition.Left)
		{
			if (cartPosition != TrainLevelPlatform.CartPosition.Right)
			{
				if (cartPosition == TrainLevelPlatform.CartPosition.Middle)
				{
					result = this.middlePos;
				}
			}
			else
			{
				result = this.rightPos;
			}
		}
		else
		{
			result = this.leftPos;
		}
		return result;
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x0002374D File Offset: 0x0002194D
	public void Move(float x)
	{
		base.StartCoroutine(this.move_cr(x));
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x000D3698 File Offset: 0x000D1898
	public IEnumerator move_cr(float x)
	{
		this.isMoving = true;
		this.rightSwitch.gameObject.SetActive(false);
		this.leftSwitch.gameObject.SetActive(false);
		base.animator.SetTrigger("OnSlap");
		base.animator.SetBool("Spinning", true);
		base.animator.SetBool("Effect", false);
		this.animHelper.Speed = 1f;
		float t = 0f;
		float time = 1.5f;
		float startX = base.transform.position.x;
		base.transform.SetPosition(new float?(startX), null, null);
		yield return null;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = t / time;
			base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutCubic, startX, x, val)), null, null);
			if (val > 0.5f)
			{
				this.animHelper.Speed = 0.5f;
			}
			yield return null;
		}
		base.transform.SetPosition(new float?(x), null, null);
		this.isMoving = false;
		yield break;
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x000D36BC File Offset: 0x000D18BC
	public void FadeIn()
	{
		this.rightSwitch.gameObject.SetActive(true);
		this.leftSwitch.gameObject.SetActive(true);
		this.animHelper.Speed = 1f;
		base.animator.SetTrigger("OnContinue");
		base.animator.SetBool("Effect", true);
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x000D371C File Offset: 0x000D191C
	public IEnumerator spark_cr()
	{
		for (;;)
		{
			while (!this.leftSwitch.isActiveAndEnabled)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.5f, 1f));
			this.sparkEffectPrefab.Create(this.sparkRoots.RandomChoice<Transform>().position);
			yield return CupheadTime.WaitForSeconds(this, 0.333333343f);
		}
		yield break;
	}

	// Token: 0x04002327 RID: 8999
	public const float DISTANCE = 390f;

	// Token: 0x04002328 RID: 9000
	[SerializeField]
	public ParrySwitch rightSwitch;

	// Token: 0x04002329 RID: 9001
	[SerializeField]
	public ParrySwitch leftSwitch;

	// Token: 0x0400232A RID: 9002
	[SerializeField]
	public Transform[] sparkRoots;

	// Token: 0x0400232B RID: 9003
	[SerializeField]
	public Effect sparkEffectPrefab;

	// Token: 0x0400232C RID: 9004
	public TrainLevelPlatform.CartPosition position;

	// Token: 0x0400232D RID: 9005
	public AnimationHelper animHelper;

	// Token: 0x0400232E RID: 9006
	public SpriteRenderer spriteRenderer;

	// Token: 0x0400232F RID: 9007
	public float middlePos;

	// Token: 0x04002330 RID: 9008
	public float leftPos;

	// Token: 0x04002331 RID: 9009
	public float rightPos;

	// Token: 0x04002332 RID: 9010
	public bool isMoving;

	// Token: 0x02000FC1 RID: 4033
	public enum CartPosition
	{
		// Token: 0x04007164 RID: 29028
		Left,
		// Token: 0x04007165 RID: 29029
		Middle,
		// Token: 0x04007166 RID: 29030
		Right
	}
}
