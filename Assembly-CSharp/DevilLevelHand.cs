using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class DevilLevelHand : AbstractCollidableObject
{
	// Token: 0x0600148C RID: 5260 RVA: 0x0009A214 File Offset: 0x00098414
	public override void Awake()
	{
		base.Awake();
		this.isDead = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.demonSprite.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.demonSprite.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x00011679 File Offset: 0x0000F879
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x00011691 File Offset: 0x0000F891
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.isInvincible)
		{
			return;
		}
		this.hp -= info.damage;
		if (this.hp < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x000116C8 File Offset: 0x0000F8C8
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x0009A274 File Offset: 0x00098474
	public void StartPattern(LevelProperties.Devil.Hands properties)
	{
		this.properties = properties;
		this.pinkStringIndex = Random.Range(0, properties.pinkString.Length);
		this.maxHp = properties.HP;
		this.hp = this.maxHp;
		this.state = DevilLevelHand.State.Idle;
		this.startPos = new Vector2(base.transform.position.x, properties.yRange.max);
		base.transform.position = this.startPos;
		this.handLocalStartPos = this.handSprite.transform.localPosition;
		this.demonLocalStartPos = this.demonSprite.transform.localPosition;
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x000116F1 File Offset: 0x0000F8F1
	public void SpawnIn()
	{
		base.StartCoroutine(this.move_in_cr());
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x0009A344 File Offset: 0x00098544
	public IEnumerator move_in_cr()
	{
		this.startAtTop = true;
		this.despawned = false;
		base.transform.position = this.startPos;
		this.handSprite.transform.localPosition = this.handLocalStartPos;
		this.demonSprite.transform.localPosition = this.demonLocalStartPos;
		base.animator.Play("Off", 1);
		base.animator.Play("Hand_Loop");
		float xPos = 547f;
		Vector3 start = new Vector3(this.startPos.x, this.properties.yRange.max);
		Vector3 end = new Vector3((!this.onLeft) ? xPos : (-xPos), this.properties.yRange.max);
		float t = 0f;
		float time = 1f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(start, end, t / time);
			yield return wait;
		}
		this.isInvincible = false;
		base.StartCoroutine(this.hand_move_up_cr());
		yield break;
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x0009A360 File Offset: 0x00098560
	public IEnumerator move_cr()
	{
		float moveTime = (this.properties.yRange.max - this.properties.yRange.min) / this.properties.speed;
		float t = 0f;
		float startY = this.demonSprite.transform.position.y;
		float endY = this.properties.yRange.min;
		for (;;)
		{
			while (!this.isSliding)
			{
				yield return null;
			}
			startY = this.demonSprite.transform.position.y;
			endY = this.properties.yRange.min;
			this.startAtTop = false;
			while (this.isSliding)
			{
				t = 0f;
				while (t < moveTime && this.isSliding)
				{
					this.demonSprite.transform.SetPosition(null, new float?(Mathf.Lerp(startY, endY, t / moveTime)), null);
					t += CupheadTime.FixedDelta;
					yield return new WaitForFixedUpdate();
				}
				startY = ((!this.startAtTop) ? this.properties.yRange.min : this.properties.yRange.max);
				endY = ((!this.startAtTop) ? this.properties.yRange.max : this.properties.yRange.min);
				this.startAtTop = !this.startAtTop;
				yield return new WaitForFixedUpdate();
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x0009A37C File Offset: 0x0009857C
	public void Fire()
	{
		if (this.properties.pinkString[this.pinkStringIndex] == 'P')
		{
			BasicProjectile basicProjectile = this.bulletPinkPrefab.Create(this.bulletRoot.position, this.shootAngle, this.properties.bulletSpeed);
			basicProjectile.transform.SetScale(new float?((float)((!this.onLeft) ? 1 : 1)), new float?((float)((!this.onLeft) ? -1 : 1)), null);
		}
		else
		{
			BasicProjectile basicProjectile2 = this.bulletPrefab.Create(this.bulletRoot.position, this.shootAngle, this.properties.bulletSpeed);
			basicProjectile2.transform.SetScale(new float?((float)((!this.onLeft) ? 1 : 1)), new float?((float)((!this.onLeft) ? -1 : 1)), null);
		}
		this.pinkStringIndex = (this.pinkStringIndex + 1) % this.properties.pinkString.Length;
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x00011700 File Offset: 0x0000F900
	public void Die()
	{
		this.isSliding = false;
		this.isInvincible = true;
		base.StartCoroutine(this.demon_move_down_cr());
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x0009A4AC File Offset: 0x000986AC
	public IEnumerator hand_move_up_cr()
	{
		base.animator.SetTrigger("OnRelease");
		yield return base.animator.WaitForAnimationToEnd(this, "Hand_Release_Start", false, true);
		this.isSliding = true;
		float t = 0f;
		float time = 0.5f;
		float start = this.handSprite.transform.position.y;
		float end = 860f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			this.handSprite.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, t / time)), null);
			yield return wait;
		}
		yield return wait;
		yield break;
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x0009A4C8 File Offset: 0x000986C8
	public IEnumerator demon_move_down_cr()
	{
		base.animator.SetTrigger("OnDeath");
		float t = 0f;
		float time = 1f;
		float start = this.demonSprite.transform.position.y;
		float end = -860f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			this.demonSprite.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, t / time)), null);
			yield return wait;
		}
		yield return wait;
		if (this.isDead)
		{
			Object.Destroy(base.gameObject);
		}
		this.despawned = true;
		this.hp = this.maxHp;
		yield break;
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x0001171D File Offset: 0x0000F91D
	public void SFXAttack()
	{
		AudioManager.Play("fat_bat_attack");
		this.emitAudioFromObject.Add("fat_bat_attack");
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x00011739 File Offset: 0x0000F939
	public void SFXDeath()
	{
		AudioManager.Play("fat_bat_die");
		this.emitAudioFromObject.Add("fat_bat_die");
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x00011755 File Offset: 0x0000F955
	public void SFXHandRelease()
	{
		AudioManager.Play("p3_hand_release_start");
		this.emitAudioFromObject.Add("p3_hand_release_start");
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x00011771 File Offset: 0x0000F971
	public void SFXFatSpawn()
	{
		AudioManager.Play("fat_bat_spawn");
		this.emitAudioFromObject.Add("fat_bat_spawn");
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x0001178D File Offset: 0x0000F98D
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
		this.bulletPinkPrefab = null;
	}

	// Token: 0x040010CE RID: 4302
	public DevilLevelHand.State state;

	// Token: 0x040010CF RID: 4303
	public bool despawned;

	// Token: 0x040010D0 RID: 4304
	public bool isDead;

	// Token: 0x040010D1 RID: 4305
	[SerializeField]
	public bool onLeft;

	// Token: 0x040010D2 RID: 4306
	[SerializeField]
	public float shootAngle;

	// Token: 0x040010D3 RID: 4307
	[SerializeField]
	public Transform bulletRoot;

	// Token: 0x040010D4 RID: 4308
	[SerializeField]
	public BasicProjectile bulletPrefab;

	// Token: 0x040010D5 RID: 4309
	[SerializeField]
	public BasicProjectile bulletPinkPrefab;

	// Token: 0x040010D6 RID: 4310
	[Header("Sprites")]
	[SerializeField]
	public SpriteRenderer demonSprite;

	// Token: 0x040010D7 RID: 4311
	public Vector3 demonLocalStartPos;

	// Token: 0x040010D8 RID: 4312
	[SerializeField]
	public SpriteRenderer handSprite;

	// Token: 0x040010D9 RID: 4313
	public Vector3 handLocalStartPos;

	// Token: 0x040010DA RID: 4314
	public LevelProperties.Devil.Hands properties;

	// Token: 0x040010DB RID: 4315
	public DamageReceiver damageReceiver;

	// Token: 0x040010DC RID: 4316
	public DamageDealer damageDealer;

	// Token: 0x040010DD RID: 4317
	public Vector3 startPos;

	// Token: 0x040010DE RID: 4318
	public float hp;

	// Token: 0x040010DF RID: 4319
	public float maxHp;

	// Token: 0x040010E0 RID: 4320
	public bool isInvincible = true;

	// Token: 0x040010E1 RID: 4321
	public bool isSliding;

	// Token: 0x040010E2 RID: 4322
	public bool startAtTop = true;

	// Token: 0x040010E3 RID: 4323
	public int pinkStringIndex;

	// Token: 0x02000B33 RID: 2867
	public enum State
	{
		// Token: 0x0400521C RID: 21020
		Uninitialized,
		// Token: 0x0400521D RID: 21021
		Idle
	}
}
