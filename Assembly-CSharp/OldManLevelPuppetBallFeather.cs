using System;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class OldManLevelPuppetBallFeather : Effect
{
	// Token: 0x060020D3 RID: 8403 RVA: 0x000B8D24 File Offset: 0x000B6F24
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		OldManLevelPuppetBallFeather oldManLevelPuppetBallFeather = base.Create(position, scale) as OldManLevelPuppetBallFeather;
		oldManLevelPuppetBallFeather.vel = MathUtils.AngleToDirection((float)Random.Range(0, 360)) * (Random.Range(this.startSpeedMin, this.startSpeedMax) + ((Random.Range(0f, 6f) >= 1f) ? 0f : this.startSpeedMax));
		oldManLevelPuppetBallFeather.rotateDir = (float)MathUtils.PlusOrMinus();
		oldManLevelPuppetBallFeather.fallFactor = Random.Range(oldManLevelPuppetBallFeather.fallFactorMin, oldManLevelPuppetBallFeather.fallFactorMax);
		oldManLevelPuppetBallFeather.fallVel.y = Random.Range(0f, 0.5f);
		oldManLevelPuppetBallFeather.anim.speed = Random.Range(0.5f, 0.75f);
		return oldManLevelPuppetBallFeather;
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x000B8DF4 File Offset: 0x000B6FF4
	public void PhysicsUpdate()
	{
		if (CupheadTime.FixedDelta == 0f)
		{
			return;
		}
		base.transform.position += this.vel + this.fallVel;
		this.vel *= this.slowFactor;
		float magnitude = this.vel.magnitude;
		base.transform.Rotate(new Vector3(0f, 0f, this.rotateDir * Mathf.InverseLerp(0f, this.startSpeedMax, magnitude)) * 100f);
		if (magnitude < 1f)
		{
			this.fallVel.y = this.fallVel.y - this.fallFactor;
			base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(base.transform.eulerAngles.z, 0f, 0.5f));
		}
		if (base.transform.position.y < -560f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x0001BF4E File Offset: 0x0001A14E
	public void FixedUpdate()
	{
		this.skipFrame = !this.skipFrame;
		if (this.skipFrame)
		{
			return;
		}
		this.PhysicsUpdate();
		this.PhysicsUpdate();
	}

	// Token: 0x04001AEF RID: 6895
	[SerializeField]
	public Animator anim;

	// Token: 0x04001AF0 RID: 6896
	[SerializeField]
	public Vector3 vel;

	// Token: 0x04001AF1 RID: 6897
	public Vector3 fallVel;

	// Token: 0x04001AF2 RID: 6898
	[SerializeField]
	public float startSpeedMin = 10f;

	// Token: 0x04001AF3 RID: 6899
	[SerializeField]
	public float startSpeedMax = 20f;

	// Token: 0x04001AF4 RID: 6900
	[SerializeField]
	public float slowFactor = 0.95f;

	// Token: 0x04001AF5 RID: 6901
	[SerializeField]
	public float fallFactorMin = 0.1f;

	// Token: 0x04001AF6 RID: 6902
	[SerializeField]
	public float fallFactorMax = 0.2f;

	// Token: 0x04001AF7 RID: 6903
	public float fallFactor;

	// Token: 0x04001AF8 RID: 6904
	public float rotateDir;

	// Token: 0x04001AF9 RID: 6905
	public bool skipFrame;
}
