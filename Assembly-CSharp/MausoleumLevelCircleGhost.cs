using System;
using UnityEngine;

// Token: 0x020002BC RID: 700
public class MausoleumLevelCircleGhost : MausoleumLevelGhostBase
{
	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06001F1D RID: 7965 RVA: 0x0001A3A8 File Offset: 0x000185A8
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x000B5780 File Offset: 0x000B3980
	public virtual AbstractCollidableObject Create(Vector2 position, Vector2 urnPosition, float rotation, float speed, float rotationSpeed)
	{
		MausoleumLevelCircleGhost mausoleumLevelCircleGhost = base.Create(position, rotation, speed) as MausoleumLevelCircleGhost;
		mausoleumLevelCircleGhost.rotationSpeed = rotationSpeed;
		mausoleumLevelCircleGhost.rotationBase = new GameObject("CircleGhostBase").transform;
		mausoleumLevelCircleGhost.rotationBase.position = urnPosition;
		mausoleumLevelCircleGhost.rotation = rotation;
		mausoleumLevelCircleGhost.transform.parent = mausoleumLevelCircleGhost.rotationBase;
		return mausoleumLevelCircleGhost;
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x000B57E4 File Offset: 0x000B39E4
	public override void Start()
	{
		base.Start();
		bool flag = Rand.Bool();
		this.setDirection = (float)((!flag) ? -360 : 360);
		base.GetComponent<SpriteRenderer>().flipY = flag;
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x000B5828 File Offset: 0x000B3A28
	public override void Move()
	{
		base.transform.localPosition += MathUtils.AngleToDirection(this.rotation) * this.Speed * CupheadTime.FixedDelta;
		this.rotationBase.AddEulerAngles(0f, 0f, this.rotationSpeed * this.setDirection * CupheadTime.FixedDelta);
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x0001A3AB File Offset: 0x000185AB
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		AudioManager.Play("mausoleum_circle_ghost_death");
		this.emitAudioFromObject.Add("mausoleum_circle_ghost_death");
		Object.Destroy(this.rotationBase.gameObject);
	}

	// Token: 0x0400196C RID: 6508
	public float rotationSpeed;

	// Token: 0x0400196D RID: 6509
	public float rotation;

	// Token: 0x0400196E RID: 6510
	public float setDirection;

	// Token: 0x0400196F RID: 6511
	public Transform rotationBase;
}
