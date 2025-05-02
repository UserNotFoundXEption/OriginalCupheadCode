using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000361 RID: 865
public class SallyStagePlayLevelProjectile : AbstractCollidableObject
{
	// Token: 0x06002633 RID: 9779 RVA: 0x000200F6 File Offset: 0x0001E2F6
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x000C8660 File Offset: 0x000C6860
	public void Update()
	{
		this.sprite.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x000C86AC File Offset: 0x000C68AC
	public void Init(Vector2 pos, float rotation, LevelProperties.SallyStagePlay.Projectile properties)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.speed = properties.projectileSpeed;
		base.transform.SetEulerAngles(null, null, new float?(rotation));
		base.StartCoroutine(this.move_cr());
		AudioManager.Play("sally_fan_shoot");
		this.emitAudioFromObject.Add("sally_fan_shoot");
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x000C8728 File Offset: 0x000C6928
	public IEnumerator move_cr()
	{
		AudioManager.PlayLoop("sally_fan_shoot_loop");
		this.emitAudioFromObject.Add("sally_fan_shoot_loop");
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnLand");
		AudioManager.Play("sally_fan_stick");
		this.emitAudioFromObject.Add("sally_fan_stick");
		AudioManager.Stop("sally_fan_shoot_loop");
		yield return CupheadTime.WaitForSeconds(this, this.properties.groundDuration);
		base.animator.SetTrigger("OnDeath");
		yield break;
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x00020109 File Offset: 0x0001E309
	public void Die()
	{
		this.StopAllCoroutines();
		AudioManager.Play("sally_fan_dissappear");
		this.emitAudioFromObject.Add("sally_fan_dissappear");
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x00020136 File Offset: 0x0001E336
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x04001F95 RID: 8085
	[SerializeField]
	public Transform sprite;

	// Token: 0x04001F96 RID: 8086
	public float speed;

	// Token: 0x04001F97 RID: 8087
	public DamageDealer damageDealer;

	// Token: 0x04001F98 RID: 8088
	public LevelProperties.SallyStagePlay.Projectile properties;
}
