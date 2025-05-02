using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000558 RID: 1368
public class WeaponWideShot : AbstractLevelWeapon
{
	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x0600393A RID: 14650 RVA: 0x0002E955 File Offset: 0x0002CB55
	public override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x0600393B RID: 14651 RVA: 0x0002E958 File Offset: 0x0002CB58
	public override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponWideShot.Basic.rapidFireRate;
		}
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x0002E95F File Offset: 0x0002CB5F
	public void Start()
	{
		this.maxAngle = WeaponProperties.LevelWeaponWideShot.Basic.angleRange.max;
		base.StartCoroutine(this.angle_cr());
		this.isInitialized = true;
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x0002E985 File Offset: 0x0002CB85
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.isInitialized)
		{
			base.StartCoroutine(this.angle_cr());
		}
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x0002E9A5 File Offset: 0x0002CBA5
	public override void BeginBasic()
	{
		base.BeginBasic();
		this.BasicSoundOneShot("player_wide_shot_start", "player_wide_shot_start_p2");
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x0010B5B4 File Offset: 0x001097B4
	public override AbstractProjectile fireBasic()
	{
		this.BasicSoundOneShot("player_wide_shot_shoot", "player_wide_shot_shoot_p2");
		float damage = WeaponProperties.LevelWeaponWideShot.Basic.damage;
		BasicProjectile basicProjectile = null;
		MinMax minMax = new MinMax(0f, this.maxAngle);
		this.animationCycleCount++;
		int num = 0;
		while ((float)num < 3f)
		{
			float floatAt = minMax.GetFloatAt((float)num / 2f);
			float num2 = minMax.max / 2f;
			basicProjectile = ((num != 0) ? (base.fireBasicNoEffect() as BasicProjectile) : (base.fireBasic() as BasicProjectile));
			basicProjectile.Speed = WeaponProperties.LevelWeaponWideShot.Basic.speed;
			basicProjectile.DestroyDistance = WeaponProperties.LevelWeaponWideShot.Basic.distance - 20f * (float)(num + 1);
			basicProjectile.Damage = damage;
			basicProjectile.PlayerId = this.player.id;
			basicProjectile.transform.AddEulerAngles(0f, 0f, floatAt - num2);
			basicProjectile.transform.position += basicProjectile.transform.right * 50f;
			basicProjectile.animator.SetInteger("Variant", (this.animationCycleCount + num) % 3);
			num++;
		}
		return basicProjectile;
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x0010B6E8 File Offset: 0x001098E8
	public override AbstractProjectile fireEx()
	{
		WeaponWideShotExProjectile weaponWideShotExProjectile = base.fireEx() as WeaponWideShotExProjectile;
		weaponWideShotExProjectile.Damage = WeaponProperties.LevelWeaponWideShot.Ex.exDamage;
		weaponWideShotExProjectile.DamageRate = 0f;
		weaponWideShotExProjectile.origin = weaponWideShotExProjectile.transform.position;
		weaponWideShotExProjectile.mainDuration = WeaponProperties.LevelWeaponWideShot.Ex.exDuration;
		weaponWideShotExProjectile.GetComponent<BoxCollider2D>().size = new Vector2(2000f, WeaponProperties.LevelWeaponWideShot.Ex.exHeight);
		weaponWideShotExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponWideShotExProjectile);
		return weaponWideShotExProjectile;
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x0010B770 File Offset: 0x00109970
	public IEnumerator angle_cr()
	{
		float openTimeMax = WeaponProperties.LevelWeaponWideShot.Basic.openingAngleSpeed;
		float closeTimeMax = WeaponProperties.LevelWeaponWideShot.Basic.closingAngleSpeed;
		float t = 0f;
		float val = 0f;
		bool playerLocked = false;
		for (;;)
		{
			if (playerLocked)
			{
				if (val < 1f)
				{
					val = t / closeTimeMax;
					t += CupheadTime.Delta;
				}
				else
				{
					val = 1f;
					t = 1f;
				}
			}
			else if (val > 0f)
			{
				val = t / openTimeMax;
				t -= CupheadTime.Delta;
			}
			else
			{
				val = 0f;
				t = 0f;
			}
			playerLocked = this.player.input.actions.GetButton(6);
			this.maxAngle = WeaponProperties.LevelWeaponWideShot.Basic.angleRange.GetFloatAt(val);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002E08 RID: 11784
	public float maxAngle;

	// Token: 0x04002E09 RID: 11785
	public bool isInitialized;

	// Token: 0x04002E0A RID: 11786
	public int animationCycleCount;
}
