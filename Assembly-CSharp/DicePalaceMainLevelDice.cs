using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class DicePalaceMainLevelDice : ParrySwitch
{
	// Token: 0x06001740 RID: 5952 RVA: 0x000A1268 File Offset: 0x0009F468
	public void Init(Vector2 pos, LevelProperties.DicePalaceMain.Dice properties, Transform pivotPoint)
	{
		base.transform.position = pos;
		this.pivotPoint = pivotPoint;
		this.properties = properties;
		base.GetComponent<Collider2D>().enabled = false;
		this.waitingToRoll = true;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x00013CCE File Offset: 0x00011ECE
	public void StartRoll()
	{
		base.animator.SetTrigger("StartRoll");
		this.waitingToRoll = true;
		base.animator.SetBool("Reverse", Rand.Bool());
		base.GetComponent<Collider2D>().enabled = true;
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x00013D08 File Offset: 0x00011F08
	public void RollOne()
	{
		this.roll = DicePalaceMainLevelDice.Roll.One;
		this.PostRoll();
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x00013D17 File Offset: 0x00011F17
	public void RollTwo()
	{
		this.roll = DicePalaceMainLevelDice.Roll.Two;
		this.PostRoll();
	}

	// Token: 0x06001744 RID: 5956 RVA: 0x00013D26 File Offset: 0x00011F26
	public void RollThree()
	{
		this.roll = DicePalaceMainLevelDice.Roll.Three;
		this.PostRoll();
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x00013D35 File Offset: 0x00011F35
	public void PostRoll()
	{
		this.waitingToRoll = false;
		DicePalaceMainLevelGameInfo.TURN_COUNTER++;
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x000A12B4 File Offset: 0x0009F4B4
	public IEnumerator move_cr()
	{
		float loopSize = 20f;
		float speed = this.properties.movementSpeed;
		float angle = 0f;
		for (;;)
		{
			Vector3 pivotOffset = Vector3.left * 2f * loopSize;
			angle += speed * CupheadTime.Delta;
			if (angle > 6.28318548f)
			{
				this.reverse = !this.reverse;
				angle -= 6.28318548f;
			}
			if (angle < 0f)
			{
				angle += 6.28318548f;
			}
			float value;
			if (this.reverse)
			{
				base.transform.position = this.pivotPoint.position + pivotOffset;
				value = 1f;
			}
			else
			{
				base.transform.position = this.pivotPoint.position;
				value = -1f;
			}
			Vector3 handleRotationX = new Vector3(Mathf.Cos(angle) * value * loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(angle) * loopSize, 0f);
			base.transform.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001747 RID: 5959 RVA: 0x00013D4A File Offset: 0x00011F4A
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.animator.SetTrigger("Hit");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x040012E7 RID: 4839
	public DicePalaceMainLevelDice.Roll roll;

	// Token: 0x040012E8 RID: 4840
	public bool waitingToRoll;

	// Token: 0x040012E9 RID: 4841
	public LevelProperties.DicePalaceMain.Dice properties;

	// Token: 0x040012EA RID: 4842
	public Transform pivotPoint;

	// Token: 0x040012EB RID: 4843
	public bool reverse;

	// Token: 0x02000BBB RID: 3003
	public enum Roll
	{
		// Token: 0x04005593 RID: 21907
		One,
		// Token: 0x04005594 RID: 21908
		Two,
		// Token: 0x04005595 RID: 21909
		Three
	}
}
