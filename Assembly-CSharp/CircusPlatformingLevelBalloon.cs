using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003FF RID: 1023
public class CircusPlatformingLevelBalloon : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002CD0 RID: 11472 RVA: 0x000256AA File Offset: 0x000238AA
	public override void OnStart()
	{
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x000256AC File Offset: 0x000238AC
	public void Init(Vector2 pos, float rotation, string spreadCount, string c)
	{
		base.transform.position = pos;
		this.rotation = rotation;
		this.spreadCount = spreadCount;
		this.SetColor(c);
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x000DB154 File Offset: 0x000D9354
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.movement_cr());
		base.StartCoroutine(this.idle_audio_delayer_cr(this.idleSoundSelected, 2f, 4f));
		this.emitAudioFromObject.Add(this.idleSoundSelected);
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x000DB1A4 File Offset: 0x000D93A4
	public void SetColor(string c)
	{
		if (c != null)
		{
			if (!(c == "B"))
			{
				if (!(c == "G"))
				{
					if (c == "P")
					{
						base.animator.Play("PinkIdle");
						this.idleSoundSelected = "circus_balloon_girl_idle";
						this._canParry = true;
					}
				}
				else
				{
					base.animator.Play("GirlIdle");
					this.idleSoundSelected = "circus_balloon_girl_idle";
				}
			}
			else
			{
				base.animator.Play("BoyIdle");
				this.idleSoundSelected = "circus_balloon_boy_idle";
			}
		}
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x000DB254 File Offset: 0x000D9454
	public IEnumerator movement_cr()
	{
		float angle = 0f;
		Vector3 xVelocity = Vector3.zero;
		for (;;)
		{
			angle += base.Properties.flyingFishSinVelocity * CupheadTime.Delta;
			xVelocity = ((this.rotation != 180f) ? base.transform.right : (-base.transform.right));
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * CupheadTime.Delta * 60f * base.Properties.flyingFishSinSize);
			Vector3 moveX = xVelocity * base.Properties.flyingFishVelocity * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				Vector3 position = base.transform.position + moveX + moveY;
				position.z = -position.x;
				base.transform.position = position;
			}
			if (base.transform.position.x < (float)(Level.Current.Left - 150))
			{
				Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x000256D5 File Offset: 0x000238D5
	public override void Die()
	{
		if (base.Health <= 0f)
		{
			AudioManager.Play("circus_balloon_hit");
			this.emitAudioFromObject.Add("circus_balloon_hit");
			base.animator.SetTrigger("Death");
		}
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x000DB270 File Offset: 0x000D9470
	public void ExplodeBalloon()
	{
		string[] array = this.spreadCount.Split(new char[]
		{
			','
		});
		float angle = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			Parser.FloatTryParse(array[i], out angle);
			this.SpawnBullet(angle);
		}
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x00025711 File Offset: 0x00023911
	public void OnEndDeathAnim()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x0002571E File Offset: 0x0002391E
	public void SpawnBullet(float angle)
	{
		this.projectile.Create(base.transform.position, angle, this.bulletSpeed);
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x00025743 File Offset: 0x00023943
	public void SoundBalloonDeathAnim()
	{
		AudioManager.Play("circus_balloon_death");
		this.emitAudioFromObject.Add("circus_balloon_death");
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x0002575F File Offset: 0x0002395F
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this._canParry = false;
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x000DB2C0 File Offset: 0x000D94C0
	public IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this._canParry = true;
		yield return null;
		yield break;
	}

	// Token: 0x04002501 RID: 9473
	public const string Blue = "B";

	// Token: 0x04002502 RID: 9474
	public const string Green = "G";

	// Token: 0x04002503 RID: 9475
	public const string Pink = "P";

	// Token: 0x04002504 RID: 9476
	public const string DeathParameterName = "Death";

	// Token: 0x04002505 RID: 9477
	public const string BoyIdle = "BoyIdle";

	// Token: 0x04002506 RID: 9478
	public const string GirlIdle = "GirlIdle";

	// Token: 0x04002507 RID: 9479
	public const string PinkIdle = "PinkIdle";

	// Token: 0x04002508 RID: 9480
	public const string BoyIdleSound = "circus_balloon_boy_idle";

	// Token: 0x04002509 RID: 9481
	public const string GirlIdleSound = "circus_balloon_girl_idle";

	// Token: 0x0400250A RID: 9482
	[SerializeField]
	public float bulletSpeed;

	// Token: 0x0400250B RID: 9483
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x0400250C RID: 9484
	[SerializeField]
	public float coolDown = 0.4f;

	// Token: 0x0400250D RID: 9485
	public float rotation;

	// Token: 0x0400250E RID: 9486
	public string spreadCount;

	// Token: 0x0400250F RID: 9487
	public string idleSoundSelected;
}
