using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class LevelHorizontalBounce : AbstractCollidableObject
{
	// Token: 0x06000D62 RID: 3426 RVA: 0x0000B737 File Offset: 0x00009937
	public void Start()
	{
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, (!this.onLeft) ? (-this.fanForce) : this.fanForce);
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x000871D0 File Offset: 0x000853D0
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		LevelPlayerMotor component = hit.GetComponent<LevelPlayerMotor>();
		if (phase != CollisionPhase.Exit)
		{
			this.FanOn(component);
		}
		else
		{
			this.FanOff(hit.GetComponent<LevelPlayerMotor>());
		}
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x0000B762 File Offset: 0x00009962
	public void FanOn(LevelPlayerMotor player)
	{
		player.AddForce(this.scrollForce);
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x0000B770 File Offset: 0x00009970
	public void FanOff(LevelPlayerMotor player)
	{
		player.RemoveForce(this.scrollForce);
	}

	// Token: 0x04000A74 RID: 2676
	[SerializeField]
	public bool onLeft;

	// Token: 0x04000A75 RID: 2677
	[SerializeField]
	public float fanForce = 1f;

	// Token: 0x04000A76 RID: 2678
	public LevelPlayerMotor.VelocityManager.Force scrollForce;
}
