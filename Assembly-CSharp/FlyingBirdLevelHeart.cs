using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class FlyingBirdLevelHeart : AbstractCollidableObject
{
	// Token: 0x060019CD RID: 6605 RVA: 0x000A70E0 File Offset: 0x000A52E0
	public void InitHeart(LevelProperties.FlyingBird properties)
	{
		this.properties = properties;
		this.mainShootIndex = Random.Range(0, properties.CurrentState.heart.shootString.Length);
		this.projectileMainIndex = Random.Range(0, properties.CurrentState.heart.numOfProjectiles.Length);
		this.thisAnimator = base.GetComponent<Animator>();
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000A713C File Offset: 0x000A533C
	public void StartHeartAttack()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		this.faceRight = (next.transform.position.x > base.transform.position.x);
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].flipX = this.faceRight;
		}
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.accend_cr());
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000A71C4 File Offset: 0x000A53C4
	public IEnumerator accend_cr()
	{
		float start = base.transform.position.y;
		this.FireSpreadshot();
		while (base.transform.position.y < start + this.properties.CurrentState.heart.heartHeight)
		{
			base.transform.position += Vector3.up * this.properties.CurrentState.heart.movementSpeed * CupheadTime.Delta;
			if (base.transform.localScale.x < 1f)
			{
				base.transform.localScale += Vector3.one * 1.75f * CupheadTime.Delta;
			}
			else
			{
				base.transform.localScale = Vector3.one * 1f;
			}
			yield return null;
			if (this.properties.CurrentHealth <= 0f)
			{
				break;
			}
		}
		while (base.transform.position.y > start)
		{
			base.transform.position += Vector3.down * this.properties.CurrentState.heart.movementSpeed * CupheadTime.Delta;
			if (base.transform.position.y < start + 100f)
			{
				if (base.transform.localScale.x > 0.5f)
				{
					base.transform.localScale -= Vector3.one * 1.75f * CupheadTime.Delta;
				}
				else
				{
					base.transform.localScale = Vector3.one * 0.5f;
				}
			}
			yield return null;
			if (this.properties.CurrentHealth <= 0f)
			{
				break;
			}
		}
		base.transform.SetPosition(null, new float?(start), null);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x00016016 File Offset: 0x00014216
	public void FireSpreadshot()
	{
		base.StartCoroutine(this.spreadShot_cr());
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x00016025 File Offset: 0x00014225
	public void SpawnFX()
	{
		this.puffFX.Create(base.transform.position);
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000A71E0 File Offset: 0x000A53E0
	public IEnumerator spreadShot_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		string[] shootString = this.properties.CurrentState.heart.shootString[this.mainShootIndex].Split(new char[]
		{
			','
		});
		int shootIndex = Random.Range(0, shootString.Length);
		for (int i = 0; i < this.properties.CurrentState.heart.shotCount; i++)
		{
			string[] projectileString = this.properties.CurrentState.heart.numOfProjectiles[this.projectileMainIndex].Split(new char[]
			{
				','
			});
			int projectiles = 0;
			float projectileDelay = 0f;
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			Parser.IntTryParse(projectileString[this.projectileSubIndex], out projectiles);
			Parser.FloatTryParse(shootString[shootIndex], out projectileDelay);
			yield return CupheadTime.WaitForSeconds(this, projectileDelay);
			this.thisAnimator.SetTrigger("Attack");
			yield return CupheadTime.WaitForSeconds(this, 0.125f);
			float directionX = player.transform.position.x - base.transform.position.x;
			float directionY = player.transform.position.y - base.transform.position.y;
			AudioManager.Play("level_flyingbird_stretcher_regurgitate_projectile");
			this.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_projectile");
			for (int j = 0; j < projectiles; j++)
			{
				float num = this.properties.CurrentState.heart.spreadAngle.GetFloatAt((float)j / ((float)projectiles - 1f));
				float num2 = this.properties.CurrentState.heart.spreadAngle.max / 2f;
				num -= num2;
				float num3 = Mathf.Atan2(directionY, directionX) * 57.29578f;
				if (this.faceRight && (num3 < -90f || num3 > 90f))
				{
					num3 = 180f - num3;
				}
				else if (!this.faceRight && num3 > -90f && num3 < 90f)
				{
					num3 = -180f - num3;
				}
				Vector3 vector;
				vector..ctor(72f, 0f, 0f);
				vector *= (float)((!this.faceRight) ? 1 : -1);
				this.projectilePrefab.Create(base.transform.position - vector, num3 + num, this.properties.CurrentState.heart.projectileSpeed);
				shootIndex = (shootIndex + 1) % shootString.Length;
			}
			if (shootIndex < shootString.Length - 1)
			{
				shootIndex++;
			}
			else
			{
				this.mainShootIndex = (this.mainShootIndex + 1) % this.properties.CurrentState.heart.shootString.Length;
				shootIndex = 0;
			}
			if (this.projectileSubIndex < projectileString.Length - 1)
			{
				this.projectileSubIndex++;
			}
			else
			{
				this.projectileMainIndex = (this.projectileMainIndex + 1) % this.properties.CurrentState.heart.numOfProjectiles.Length;
				this.projectileSubIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040014AF RID: 5295
	public const float ProjectileOffsetX = 72f;

	// Token: 0x040014B0 RID: 5296
	public const float ScaleRate = 1.75f;

	// Token: 0x040014B1 RID: 5297
	public const float ScaleStartPosition = 100f;

	// Token: 0x040014B2 RID: 5298
	public const float InitialScale = 0.5f;

	// Token: 0x040014B3 RID: 5299
	public const float TargetScale = 1f;

	// Token: 0x040014B4 RID: 5300
	[SerializeField]
	public Effect puffFX;

	// Token: 0x040014B5 RID: 5301
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x040014B6 RID: 5302
	[SerializeField]
	public SpriteRenderer[] renderers;

	// Token: 0x040014B7 RID: 5303
	public int mainShootIndex;

	// Token: 0x040014B8 RID: 5304
	public int projectileMainIndex;

	// Token: 0x040014B9 RID: 5305
	public int projectileSubIndex;

	// Token: 0x040014BA RID: 5306
	public Animator thisAnimator;

	// Token: 0x040014BB RID: 5307
	public LevelProperties.FlyingBird properties;

	// Token: 0x040014BC RID: 5308
	public bool faceRight;
}
