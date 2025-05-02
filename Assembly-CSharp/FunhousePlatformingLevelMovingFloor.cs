using System;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class FunhousePlatformingLevelMovingFloor : AbstractCollidableObject
{
	// Token: 0x06002DB6 RID: 11702 RVA: 0x000DD690 File Offset: 0x000DB890
	public void Start()
	{
		if (base.transform.localScale.x == 1f)
		{
			this.speed = -this.velocity;
		}
		else
		{
			this.speed = this.velocity;
		}
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, this.speed);
	}

	// Token: 0x06002DB7 RID: 11703 RVA: 0x000DD6EC File Offset: 0x000DB8EC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Stay)
		{
			if (hit.GetComponent<LevelPlayerMotor>())
			{
				this.ScrollOn(hit.GetComponent<LevelPlayerMotor>());
			}
			else
			{
				this.ScrollOff(hit.GetComponent<LevelPlayerMotor>());
			}
		}
		else
		{
			this.ScrollOff(hit.GetComponent<LevelPlayerMotor>());
		}
	}

	// Token: 0x06002DB8 RID: 11704 RVA: 0x000261FB File Offset: 0x000243FB
	public void ScrollOn(LevelPlayerMotor player)
	{
		player.AddForce(this.scrollForce);
	}

	// Token: 0x06002DB9 RID: 11705 RVA: 0x00026209 File Offset: 0x00024409
	public void ScrollOff(LevelPlayerMotor player)
	{
		player.RemoveForce(this.scrollForce);
	}

	// Token: 0x040025E1 RID: 9697
	[SerializeField]
	public float velocity;

	// Token: 0x040025E2 RID: 9698
	public float speed;

	// Token: 0x040025E3 RID: 9699
	public LevelPlayerMotor.VelocityManager.Force scrollForce;
}
