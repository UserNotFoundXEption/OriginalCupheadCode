using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class DragonLevelPotion : AbstractProjectile
{
	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06001866 RID: 6246 RVA: 0x00014D60 File Offset: 0x00012F60
	// (set) Token: 0x06001867 RID: 6247 RVA: 0x00014D68 File Offset: 0x00012F68
	public DragonLevelPotion.State state { get; set; }

	// Token: 0x06001868 RID: 6248 RVA: 0x000A3C3C File Offset: 0x000A1E3C
	public void Init(Vector2 pos, float hp, float rotation, LevelProperties.Dragon.Potions properties)
	{
		base.transform.position = pos;
		this.hp = hp;
		this.properties = properties;
		base.transform.SetScale(new float?(properties.potionScale), new float?(properties.potionScale), new float?(properties.potionScale));
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		this.state = DragonLevelPotion.State.Alive;
		this.moveRoutine = base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x00014D71 File Offset: 0x00012F71
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x00014D9C File Offset: 0x00012F9C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x00014DBA File Offset: 0x00012FBA
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x000A3CD8 File Offset: 0x000A1ED8
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != DragonLevelPotion.State.Dead)
		{
			this.state = DragonLevelPotion.State.Dead;
			base.StopCoroutine(this.moveRoutine);
			base.StartCoroutine(this.handle_die_cr());
		}
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x000A3D34 File Offset: 0x000A1F34
	public IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.potionSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x000A3D50 File Offset: 0x000A1F50
	public IEnumerator handle_die_cr()
	{
		DragonLevelPotion.PotionType potionType = this.type;
		if (potionType != DragonLevelPotion.PotionType.Horizontal)
		{
			if (potionType != DragonLevelPotion.PotionType.Vertical)
			{
				if (potionType == DragonLevelPotion.PotionType.Both)
				{
					this.SpawnProjectile(Vector3.right);
					this.SpawnProjectile(-Vector3.right);
					this.SpawnProjectile(Vector3.up);
					this.SpawnProjectile(-Vector3.up);
				}
			}
			else
			{
				this.SpawnProjectile(Vector3.up);
				this.SpawnProjectile(-Vector3.up);
			}
		}
		else
		{
			this.SpawnProjectile(Vector3.right);
			this.SpawnProjectile(-Vector3.right);
		}
		base.animator.SetTrigger("Explode");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x000A3D6C File Offset: 0x000A1F6C
	public void SpawnProjectile(Vector3 direction)
	{
		float rotation = MathUtils.DirectionToAngle(direction);
		this.bulletPrefab.Create(base.transform.position, rotation, this.properties.spitBulletSpeed).transform.SetScale(new float?(this.properties.explosionBulletScale), new float?(this.properties.explosionBulletScale), new float?(this.properties.explosionBulletScale));
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x00014DD8 File Offset: 0x00012FD8
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
	}

	// Token: 0x040013CF RID: 5071
	public const string ExplodeTrigger = "Explode";

	// Token: 0x040013D0 RID: 5072
	[SerializeField]
	public BasicProjectile bulletPrefab;

	// Token: 0x040013D1 RID: 5073
	public DragonLevelPotion.PotionType type;

	// Token: 0x040013D3 RID: 5075
	public LevelProperties.Dragon.Potions properties;

	// Token: 0x040013D4 RID: 5076
	public DamageReceiver damageReceiver;

	// Token: 0x040013D5 RID: 5077
	public float hp;

	// Token: 0x040013D6 RID: 5078
	public Coroutine moveRoutine;

	// Token: 0x02000C04 RID: 3076
	public enum PotionType
	{
		// Token: 0x04005771 RID: 22385
		Horizontal,
		// Token: 0x04005772 RID: 22386
		Vertical,
		// Token: 0x04005773 RID: 22387
		Both
	}

	// Token: 0x02000C05 RID: 3077
	public enum State
	{
		// Token: 0x04005775 RID: 22389
		Alive,
		// Token: 0x04005776 RID: 22390
		Dead
	}
}
