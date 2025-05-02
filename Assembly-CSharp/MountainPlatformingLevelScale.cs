using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044F RID: 1103
public class MountainPlatformingLevelScale : AbstractPausableComponent
{
	// Token: 0x06002F35 RID: 12085 RVA: 0x000E1038 File Offset: 0x000DF238
	public void Start()
	{
		this.scaleLeftStart = this.ScaleLeft.transform.position;
		this.scaleRightStart = this.ScaleRight.transform.position;
		base.StartCoroutine(this.check_scale_cr());
	}

	// Token: 0x06002F36 RID: 12086 RVA: 0x000E1088 File Offset: 0x000DF288
	public IEnumerator check_scale_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (this.ScaleRight.steppedOn)
			{
				if (!this.ScaleLeft.steppedOn)
				{
					if (this.ScaleRight.transform.position.y > this.scaleRightStart.y - this.scaleChangeAmount)
					{
						this.ScaleRight.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown);
					}
					if (this.ScaleLeft.transform.position.y < this.scaleLeftStart.y + this.scaleChangeAmount)
					{
						this.ScaleLeft.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
				else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					if (this.ScaleRight.transform.position.y < this.scaleRightStart.y)
					{
						this.ScaleRight.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown);
					}
					else if (this.ScaleLeft.steppedOn)
					{
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
					}
					if (this.ScaleLeft.transform.position.y > this.scaleLeftStart.y)
					{
						this.ScaleLeft.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
			}
			else
			{
				if (this.ScaleRight.transform.position.y < this.scaleRightStart.y)
				{
					this.ScaleRight.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown);
				}
				else if (!this.ScaleLeft.steppedOn)
				{
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
				}
				if (this.ScaleLeft.transform.position.y > this.scaleLeftStart.y)
				{
					this.ScaleLeft.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
				}
			}
			if (this.ScaleLeft.steppedOn)
			{
				if (!this.ScaleRight.steppedOn)
				{
					if (this.ScaleLeft.transform.position.y > this.scaleLeftStart.y - this.scaleChangeAmount)
					{
						this.ScaleLeft.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown);
					}
					if (this.ScaleRight.transform.position.y < this.scaleRightStart.y + this.scaleChangeAmount)
					{
						this.ScaleRight.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
				else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					if (this.ScaleLeft.transform.position.y < this.scaleLeftStart.y)
					{
						this.ScaleLeft.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown);
					}
					else if (this.ScaleRight.steppedOn)
					{
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
					}
					if (this.ScaleRight.transform.position.y > this.scaleRightStart.y)
					{
						this.ScaleRight.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
			}
			else
			{
				if (this.ScaleLeft.transform.position.y < this.scaleLeftStart.y)
				{
					this.ScaleLeft.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown);
				}
				else if (!this.ScaleRight.steppedOn)
				{
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
				}
				if (this.ScaleRight.transform.position.y > this.scaleRightStart.y)
				{
					this.ScaleRight.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
				}
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002F37 RID: 12087 RVA: 0x000E10A4 File Offset: 0x000DF2A4
	public void ChangeState(MountainPlatformingLevelScale.ScaleState state)
	{
		if (this.scaleState != state)
		{
			this.scaleState = state;
			string text = "goingDown";
			string text2 = "goingUp";
			MountainPlatformingLevelScale.ScaleState scaleState = this.scaleState;
			if (scaleState != MountainPlatformingLevelScale.ScaleState.leftDown)
			{
				if (scaleState != MountainPlatformingLevelScale.ScaleState.rightDown)
				{
					if (scaleState == MountainPlatformingLevelScale.ScaleState.still)
					{
						this.ScaleLeft.animator.SetBool(text, false);
						this.ScaleLeft.animator.SetBool(text2, false);
						this.ScaleRight.animator.SetBool(text, false);
						this.ScaleRight.animator.SetBool(text2, false);
						if (this.ScaleLeft.animator.GetCurrentAnimatorStateInfo(0).IsName("Scale_Idle"))
						{
							AudioManager.Stop("castle_scales_tip_up");
							AudioManager.Stop("castle_scales_tip_down");
						}
					}
				}
				else
				{
					this.ScaleLeft.animator.SetBool(text, false);
					this.ScaleLeft.animator.SetBool(text2, true);
					this.ScaleRight.animator.SetBool(text, true);
					this.ScaleRight.animator.SetBool(text2, false);
					this.ScalesUpSound();
					this.ScalesDownSound();
				}
			}
			else
			{
				this.ScaleLeft.animator.SetBool(text, true);
				this.ScaleLeft.animator.SetBool(text2, false);
				this.ScaleRight.animator.SetBool(text, false);
				this.ScaleRight.animator.SetBool(text2, true);
				this.ScalesUpSound();
				this.ScalesDownSound();
			}
		}
	}

	// Token: 0x06002F38 RID: 12088 RVA: 0x00027536 File Offset: 0x00025736
	public void ScalesUpSound()
	{
		if (!AudioManager.CheckIfPlaying("castle_scales_tip_up"))
		{
			AudioManager.Play("castle_scales_tip_up");
			this.emitAudioFromObject.Add("castle_scales_tip_up");
		}
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x00027561 File Offset: 0x00025761
	public void ScalesDownSound()
	{
		if (!AudioManager.CheckIfPlaying("castle_scales_tip_down"))
		{
			AudioManager.Play("castle_scales_tip_down");
			this.emitAudioFromObject.Add("castle_scales_tip_down");
		}
	}

	// Token: 0x04002728 RID: 10024
	[SerializeField]
	public float scaleSpeed;

	// Token: 0x04002729 RID: 10025
	[SerializeField]
	public float scaleChangeAmount;

	// Token: 0x0400272A RID: 10026
	[SerializeField]
	public MountainPlatformingLevelScalePart ScaleLeft;

	// Token: 0x0400272B RID: 10027
	[SerializeField]
	public MountainPlatformingLevelScalePart ScaleRight;

	// Token: 0x0400272C RID: 10028
	public Vector2 scaleLeftStart;

	// Token: 0x0400272D RID: 10029
	public Vector2 scaleRightStart;

	// Token: 0x0400272E RID: 10030
	public MountainPlatformingLevelScale.ScaleState scaleState;

	// Token: 0x020010C5 RID: 4293
	public enum ScaleState
	{
		// Token: 0x040076FE RID: 30462
		rightDown,
		// Token: 0x040076FF RID: 30463
		leftDown,
		// Token: 0x04007700 RID: 30464
		still
	}
}
