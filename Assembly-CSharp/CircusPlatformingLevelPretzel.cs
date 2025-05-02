using System;
using UnityEngine;

// Token: 0x0200040E RID: 1038
public class CircusPlatformingLevelPretzel : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002D44 RID: 11588 RVA: 0x00025CA6 File Offset: 0x00023EA6
	public override void OnStart()
	{
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x00025CA8 File Offset: 0x00023EA8
	public void SetPath(Transform[] path)
	{
		this.path = path;
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x00025CB1 File Offset: 0x00023EB1
	public void SetStartPosition(int index)
	{
		this.nextPoint = index;
		base.transform.position = this.path[this.nextPoint].position;
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x000DC514 File Offset: 0x000DA714
	public void Jump()
	{
		if (this.goingLeft)
		{
			this.nextPoint--;
		}
		else
		{
			this.nextPoint++;
		}
		if (this.nextPoint < 0 || (this.path != null && this.nextPoint >= this.path.Length))
		{
			this.Die();
		}
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x000DC580 File Offset: 0x000DA780
	public void Land()
	{
		base.animator.SetTrigger("Salt");
		if (this.nextPoint < this.path.Length)
		{
			base.transform.position = this.path[this.nextPoint].position;
		}
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x00025CD7 File Offset: 0x00023ED7
	public void JumpSFX()
	{
		AudioManager.Play("circus_pretzel_jump");
		this.emitAudioFromObject.Add("circus_pretzel_jump");
	}

	// Token: 0x06002D4A RID: 11594 RVA: 0x000DC5D0 File Offset: 0x000DA7D0
	public override void Die()
	{
		AudioManager.Stop("circus_pretzel_jump");
		AudioManager.Play("circus_generic_death_big");
		this.emitAudioFromObject.Add("circus_generic_death_big");
		base.Die();
		Object.Destroy(base.transform.parent.gameObject);
	}

	// Token: 0x04002580 RID: 9600
	public const string SaltParameterName = "Salt";

	// Token: 0x04002581 RID: 9601
	public bool goingLeft;

	// Token: 0x04002582 RID: 9602
	[SerializeField]
	public float jumpMultiplierX;

	// Token: 0x04002583 RID: 9603
	[SerializeField]
	public float jumpMultiplierY;

	// Token: 0x04002584 RID: 9604
	[SerializeField]
	public float inverseJumpMultiplierY;

	// Token: 0x04002585 RID: 9605
	[SerializeField]
	public Transform transformDustA;

	// Token: 0x04002586 RID: 9606
	[SerializeField]
	public Transform transformDustB;

	// Token: 0x04002587 RID: 9607
	public Transform[] path;

	// Token: 0x04002588 RID: 9608
	public int nextPoint;
}
