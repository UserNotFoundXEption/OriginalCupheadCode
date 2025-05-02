using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002CB RID: 715
public class MouseLevelFallingObject : AbstractProjectile
{
	// Token: 0x06001FD7 RID: 8151 RVA: 0x000B6BD4 File Offset: 0x000B4DD4
	public MouseLevelFallingObject Create(float xPos, LevelProperties.Mouse.Claw properties)
	{
		Vector2 vector;
		vector..ctor(-600f, 50f);
		MouseLevelFallingObject mouseLevelFallingObject = this.InstantiatePrefab<MouseLevelFallingObject>();
		mouseLevelFallingObject.GetComponent<Animator>().SetInteger("Pick", Random.Range(0, 3));
		mouseLevelFallingObject.speed = properties.objectStartingFallSpeed;
		mouseLevelFallingObject.gravity = properties.objectGravity;
		mouseLevelFallingObject.transform.SetPosition(new float?(xPos + vector.x), new float?((float)Level.Current.Ceiling + vector.y), null);
		mouseLevelFallingObject.StartCoroutine(mouseLevelFallingObject.move_cr());
		return mouseLevelFallingObject;
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x0001AEA5 File Offset: 0x000190A5
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x0001AEC3 File Offset: 0x000190C3
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x000B6C70 File Offset: 0x000B4E70
	public IEnumerator move_cr()
	{
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			this.speed += this.gravity * CupheadTime.FixedDelta;
			base.transform.AddPosition(0f, -this.speed * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.explosionSmall.Create(base.transform.position);
		base.animator.SetTrigger("Death");
		AudioManager.Play("level_mouse_debris_smash");
		this.emitAudioFromObject.Add("level_mouse_debris_smash");
		yield break;
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x0001AEE1 File Offset: 0x000190E1
	public void DestroyWood()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x0001AEEE File Offset: 0x000190EE
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionSmall = null;
	}

	// Token: 0x040019F0 RID: 6640
	[SerializeField]
	public Effect explosionSmall;

	// Token: 0x040019F1 RID: 6641
	public float gravity;

	// Token: 0x040019F2 RID: 6642
	public float speed;
}
