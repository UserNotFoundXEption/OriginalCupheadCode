using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000398 RID: 920
public class SnowCultLevelSnowball : AbstractProjectile
{
	// Token: 0x0600289B RID: 10395 RVA: 0x000CEF00 File Offset: 0x000CD100
	public virtual SnowCultLevelSnowball Init(Vector3 pos, float gravity, float verticalVelocity, float horizontalVelocity, LevelProperties.SnowCult.Snowball properties, SnowCultLevelYeti main, bool makeSound)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.properties = properties;
		this.gravity = gravity;
		this.velocity.x = -horizontalVelocity;
		this.velocity.y = verticalVelocity;
		base.transform.localScale = new Vector3(Mathf.Sign(horizontalVelocity), 1f);
		this.hitGround = false;
		this.main = main;
		this.makeSound = makeSound;
		base.animator.Play("Spin", 0, main.GetIceCubeStartFrame() * 0.0625f);
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x000CEFB0 File Offset: 0x000CD1B0
	public virtual SnowCultLevelSnowball InitOriginal(Vector3 pos, float gravity, float speed, float angle, LevelProperties.SnowCult.Snowball properties, SnowCultLevelYeti main)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.speed = speed;
		base.transform.localScale = new Vector3(Mathf.Sign(angle - 90f), 1f);
		this.gravity = gravity;
		this.angle = MathUtils.AngleToDirection(angle);
		this.properties = properties;
		this.hitGround = false;
		this.main = main;
		this.makeSound = true;
		base.animator.Play("Spin", 0, main.GetIceCubeStartFrame() * 0.0625f);
		base.StartCoroutine(this.move_from_yeti_cr());
		return this;
	}

	// Token: 0x0600289D RID: 10397 RVA: 0x00022204 File Offset: 0x00020404
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x00022222 File Offset: 0x00020422
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		this.SFX_SNOWCULT_IceCubeImpact();
		this.hitGround = true;
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x000CF064 File Offset: 0x000CD264
	public void TriggerGlare()
	{
		if (this.glareCounter == 0)
		{
			this.glares[0].enabled = false;
			this.glares[1].enabled = false;
		}
		this.glareCounter++;
		if (this.glareCounter == 3)
		{
			this.glares[0].enabled = true;
			this.glares[1].enabled = true;
			this.glareCounter = 0;
		}
	}

	// Token: 0x060028A0 RID: 10400 RVA: 0x000CF0D8 File Offset: 0x000CD2D8
	public IEnumerator move_from_yeti_cr()
	{
		float accumulativeGravity = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.hitGround)
		{
			base.transform.position += this.angle * this.speed * CupheadTime.FixedDelta - new Vector3(0f, accumulativeGravity * CupheadTime.FixedDelta, 0f);
			accumulativeGravity += this.gravity * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.newProjectiles();
		this.Recycle<SnowCultLevelSnowball>();
		yield return null;
		yield break;
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x000CF0F4 File Offset: 0x000CD2F4
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!this.hitGround)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.newProjectiles();
		this.Recycle<SnowCultLevelSnowball>();
		yield return null;
		yield break;
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x000CF110 File Offset: 0x000CD310
	public void newProjectiles()
	{
		SnowCultLevelSnowballExplosion snowCultLevelSnowballExplosion = this.snowballExplosion.Spawn<SnowCultLevelSnowballExplosion>();
		snowCultLevelSnowballExplosion.Init(base.transform.position, this.size, this.main);
		float num = Time.realtimeSinceStartup % 0.0001f;
		if (this.size == SnowCultLevelSnowball.Size.Large)
		{
			SnowCultLevelSnowball snowCultLevelSnowball = this.mediumSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball.Init(base.transform.position + Vector3.back * num, this.properties.mediumGravity, this.properties.mediumVelocityY, this.properties.mediumVelocityX, this.properties, this.main, false);
			SnowCultLevelSnowball snowCultLevelSnowball2 = this.mediumSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball2.Init(base.transform.position + Vector3.forward * num, this.properties.mediumGravity, this.properties.mediumVelocityY, -this.properties.mediumVelocityX, this.properties, this.main, true);
		}
		else if (this.size == SnowCultLevelSnowball.Size.Medium)
		{
			SnowCultLevelSnowball snowCultLevelSnowball3 = this.smallSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball3.Init(base.transform.position + Vector3.back * num, this.properties.smallGravity, this.properties.smallVelocityY, this.properties.smallVelocityX, this.properties, this.main, true);
			SnowCultLevelSnowball snowCultLevelSnowball4 = this.smallSnowballPrefab.Spawn<SnowCultLevelSnowball>();
			snowCultLevelSnowball4.Init(base.transform.position + Vector3.forward * num, this.properties.smallGravity, this.properties.smallVelocityY, -this.properties.smallVelocityX, this.properties, this.main, false);
		}
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x000CF2E8 File Offset: 0x000CD4E8
	public void SFX_SNOWCULT_IceCubeImpact()
	{
		if (!this.makeSound)
		{
			return;
		}
		string str = "_large";
		if (this.size == SnowCultLevelSnowball.Size.Medium)
		{
			str = "_medium";
		}
		if (this.size == SnowCultLevelSnowball.Size.Small)
		{
			str = "_small";
		}
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_impact" + str);
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_fridge_icecube_impact" + str);
	}

	// Token: 0x040021DB RID: 8667
	[SerializeField]
	public SnowCultLevelSnowball smallSnowballPrefab;

	// Token: 0x040021DC RID: 8668
	[SerializeField]
	public SnowCultLevelSnowball mediumSnowballPrefab;

	// Token: 0x040021DD RID: 8669
	[SerializeField]
	public SnowCultLevelSnowballExplosion snowballExplosion;

	// Token: 0x040021DE RID: 8670
	public SnowCultLevelSnowball.Size size;

	// Token: 0x040021DF RID: 8671
	public LevelProperties.SnowCult.Snowball properties;

	// Token: 0x040021E0 RID: 8672
	public Vector3 velocity;

	// Token: 0x040021E1 RID: 8673
	public float gravity;

	// Token: 0x040021E2 RID: 8674
	public float speed;

	// Token: 0x040021E3 RID: 8675
	public bool hitGround;

	// Token: 0x040021E4 RID: 8676
	public bool makeSound;

	// Token: 0x040021E5 RID: 8677
	public Vector3 angle;

	// Token: 0x040021E6 RID: 8678
	[SerializeField]
	public SpriteRenderer[] glares;

	// Token: 0x040021E7 RID: 8679
	public int glareCounter = 1;

	// Token: 0x040021E8 RID: 8680
	public SnowCultLevelYeti main;

	// Token: 0x02000F76 RID: 3958
	public enum Size
	{
		// Token: 0x04006F7E RID: 28542
		Small,
		// Token: 0x04006F7F RID: 28543
		Medium,
		// Token: 0x04006F80 RID: 28544
		Large
	}
}
