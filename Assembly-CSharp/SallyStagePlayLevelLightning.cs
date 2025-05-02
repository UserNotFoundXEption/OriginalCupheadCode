using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035F RID: 863
public class SallyStagePlayLevelLightning : AbstractProjectile
{
	// Token: 0x06002611 RID: 9745 RVA: 0x000C820C File Offset: 0x000C640C
	public SallyStagePlayLevelLightning Create(Vector2 pos, float rotation, float speed, bool lightningLast)
	{
		SallyStagePlayLevelLightning sallyStagePlayLevelLightning = base.Create(pos, rotation) as SallyStagePlayLevelLightning;
		sallyStagePlayLevelLightning.speed = speed;
		sallyStagePlayLevelLightning.rotation = rotation;
		sallyStagePlayLevelLightning.lightningLast = lightningLast;
		return sallyStagePlayLevelLightning;
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x000C8240 File Offset: 0x000C6440
	public override void Start()
	{
		base.Start();
		this.sprite.SetEulerAngles(null, null, new float?(0f));
		base.animator.Play(Random.Range(0, 3).ToStringInvariant());
		base.StartCoroutine(this.move_cr());
		AudioManager.PlayLoop("sally_sally_lightning_move_loop");
		this.emitAudioFromObject.Add("sally_sally_lightning_move_loop");
		AudioManager.Play("sally_thunder");
		this.sprite.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x0001FF6C File Offset: 0x0001E16C
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002614 RID: 9748 RVA: 0x0001FF8A File Offset: 0x0001E18A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002615 RID: 9749 RVA: 0x000C82E0 File Offset: 0x000C64E0
	public IEnumerator move_cr()
	{
		this.velocity = base.transform.right;
		for (;;)
		{
			base.transform.position += this.velocity * this.speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002616 RID: 9750 RVA: 0x000C82FC File Offset: 0x000C64FC
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter && !this.goingBackUp)
		{
			Vector3 position = base.transform.position;
			Vector3 vector;
			vector..ctor(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.goingBackUp = true;
			this.collisionPoint = vector - position;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x06002617 RID: 9751 RVA: 0x000C8380 File Offset: 0x000C6580
	public IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		base.transform.SetEulerAngles(null, null, new float?(-this.rotation));
		this.sprite.SetEulerAngles(null, null, new float?(0f));
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return new WaitForEndOfFrame();
		AudioManager.Play("sally_thunder_impact");
		while (base.transform.position.y < (float)(Level.Current.Ceiling + 100))
		{
			yield return null;
		}
		if (this.lightningLast)
		{
			AudioManager.Stop("sally_sally_lightning_move_loop");
			AudioManager.Play("sally_thunder_end");
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002618 RID: 9752 RVA: 0x0001FFA8 File Offset: 0x0001E1A8
	public override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001F86 RID: 8070
	[SerializeField]
	public Transform sprite;

	// Token: 0x04001F87 RID: 8071
	public Vector3 velocity;

	// Token: 0x04001F88 RID: 8072
	public Vector3 collisionPoint;

	// Token: 0x04001F89 RID: 8073
	public float speed;

	// Token: 0x04001F8A RID: 8074
	public float rotation;

	// Token: 0x04001F8B RID: 8075
	public bool lightningLast;

	// Token: 0x04001F8C RID: 8076
	public bool goingBackUp;
}
