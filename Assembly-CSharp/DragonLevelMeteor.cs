using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class DragonLevelMeteor : AbstractProjectile
{
	// Token: 0x0600184F RID: 6223 RVA: 0x000A3988 File Offset: 0x000A1B88
	public DragonLevelMeteor Create(Vector2 pos, DragonLevelMeteor.Properties properties)
	{
		DragonLevelMeteor dragonLevelMeteor = base.Create() as DragonLevelMeteor;
		dragonLevelMeteor.properties = properties;
		dragonLevelMeteor.transform.position = pos;
		return dragonLevelMeteor;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000A39BC File Offset: 0x000A1BBC
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.smoke_cr());
		base.StartCoroutine(this.moveX_cr());
		if (this.properties.state != DragonLevelMeteor.State.Forward)
		{
			base.StartCoroutine(this.moveY_cr());
		}
		base.StartCoroutine(this.rotate_cr());
		AudioManager.PlayLoop("level_dragon_left_dragon_meteor_a_loop");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_a_loop");
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x00014C88 File Offset: 0x00012E88
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000A3A30 File Offset: 0x000A1C30
	public IEnumerator rotate_cr()
	{
		Vector2 lastPos = base.transform.position;
		for (;;)
		{
			base.transform.LookAt2D(lastPos);
			lastPos = base.transform.position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x000A3A4C File Offset: 0x000A1C4C
	public IEnumerator smoke_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.smokePrefab.Create(base.transform.position).transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(base.transform.eulerAngles.z + Random.Range(-45f, 45f)));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x000A3A68 File Offset: 0x000A1C68
	public IEnumerator moveX_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > -840f)
		{
			base.transform.AddPosition(-this.properties.speedX * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x000A3A84 File Offset: 0x000A1C84
	public IEnumerator moveY_cr()
	{
		int state = (int)this.properties.state;
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(start.x - this.properties.speedX / 2f, 300f * (float)state);
		yield return base.TweenPositionY(start.y, end.y, this.properties.timeY / 2f, EaseUtils.EaseType.easeOutSine);
		for (;;)
		{
			state *= -1;
			start = base.transform.position;
			end = new Vector2(start.x - this.properties.speedX, 300f * (float)state);
			yield return base.TweenPositionY(start.y, end.y, this.properties.timeY, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x00014CA6 File Offset: 0x00012EA6
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		AudioManager.Stop("level_dragon_left_dragon_meteor_a_loop");
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x00014CBE File Offset: 0x00012EBE
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.smokePrefab = null;
	}

	// Token: 0x040013C4 RID: 5060
	public DragonLevelMeteor.Properties properties;

	// Token: 0x040013C5 RID: 5061
	[SerializeField]
	public Effect smokePrefab;

	// Token: 0x02000BFC RID: 3068
	public enum State
	{
		// Token: 0x04005744 RID: 22340
		Up = 1,
		// Token: 0x04005745 RID: 22341
		Down = -1,
		// Token: 0x04005746 RID: 22342
		Both,
		// Token: 0x04005747 RID: 22343
		Forward = 10
	}

	// Token: 0x02000BFD RID: 3069
	public class Properties
	{
		// Token: 0x06006236 RID: 25142 RVA: 0x00046C15 File Offset: 0x00044E15
		public Properties(float timeY, float speedX, DragonLevelMeteor.State state)
		{
			this.timeY = timeY;
			this.speedX = speedX;
			this.state = state;
		}

		// Token: 0x04005748 RID: 22344
		public float timeY;

		// Token: 0x04005749 RID: 22345
		public float speedX;

		// Token: 0x0400574A RID: 22346
		public DragonLevelMeteor.State state;
	}
}
