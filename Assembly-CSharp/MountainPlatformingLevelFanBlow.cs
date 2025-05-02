using System;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class MountainPlatformingLevelFanBlow : AbstractCollidableObject
{
	// Token: 0x06002ED6 RID: 11990 RVA: 0x00027071 File Offset: 0x00025271
	public void Start()
	{
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.All, this.parent.GetSpeed());
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x000E0548 File Offset: 0x000DE748
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			if (hit.GetComponent<LevelPlayerMotor>())
			{
				if (this.parent.fanOn && !this.parent.Dead)
				{
					this.FanOn(hit.GetComponent<LevelPlayerMotor>());
				}
				else
				{
					this.FanOff(hit.GetComponent<LevelPlayerMotor>());
				}
			}
			else
			{
				this.FanOff(hit.GetComponent<LevelPlayerMotor>());
			}
		}
		else
		{
			this.FanOff(hit.GetComponent<LevelPlayerMotor>());
		}
	}

	// Token: 0x06002ED8 RID: 11992 RVA: 0x0002708A File Offset: 0x0002528A
	public void FanOn(LevelPlayerMotor player)
	{
		player.AddForce(this.scrollForce);
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x00027098 File Offset: 0x00025298
	public void FanOff(LevelPlayerMotor player)
	{
		player.RemoveForce(this.scrollForce);
	}

	// Token: 0x040026DE RID: 9950
	[SerializeField]
	public MountainPlatformingLevelFan parent;

	// Token: 0x040026DF RID: 9951
	public LevelPlayerMotor.VelocityManager.Force scrollForce;
}
