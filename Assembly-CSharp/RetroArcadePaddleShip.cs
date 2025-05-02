using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class RetroArcadePaddleShip : RetroArcadeEnemy
{
	// Token: 0x0600226C RID: 8812 RVA: 0x0001D55D File Offset: 0x0001B75D
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x000BD5FC File Offset: 0x000BB7FC
	public void StartPaddleShip()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.paddleShip;
		this.ySpeed = this.p.ySpeed.RandomFloat();
		base.PointsBonus = this.p.pointsBonus;
		base.PointsWorth = this.p.pointsGained;
		base.transform.SetPosition(new float?(Random.Range(-300f, 300f)), new float?(350f), null);
		this.moveDir = new Trilean2((!Rand.Bool()) ? 1 : -1, -1);
		this.hp = this.p.hp;
		base.StartCoroutine(this.moveY_cr());
		base.StartCoroutine(this.moveX_cr());
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000BD6E0 File Offset: 0x000BB8E0
	public IEnumerator moveY_cr()
	{
		while (base.transform.position.y > (float)Level.Current.Ceiling - 80f)
		{
			yield return new WaitForFixedUpdate();
			base.transform.AddPosition(0f, -this.ySpeed * CupheadTime.FixedDelta, 0f);
		}
		base.transform.SetPosition(null, new float?((float)Level.Current.Ceiling - 80f), null);
		while (this.paddle.position.y > (float)Level.Current.Ground + 20f)
		{
			yield return new WaitForFixedUpdate();
			this.paddle.AddPosition(0f, -this.ySpeed * CupheadTime.FixedDelta, 0f);
		}
		this.paddle.SetPosition(null, new float?((float)Level.Current.Ground + 20f), null);
		for (;;)
		{
			yield return new WaitForFixedUpdate();
			if ((this.moveDir.y > 0 && base.transform.position.y > (float)Level.Current.Ceiling - 80f) || (this.moveDir.y < 0 && base.transform.position.y < (float)Level.Current.Ground + 80f))
			{
				this.moveDir.y = this.moveDir.y * -1;
			}
			base.transform.AddPosition(0f, this.moveDir.y * this.ySpeed * CupheadTime.FixedDelta, 0f);
			this.paddle.SetPosition(null, new float?((float)Level.Current.Ground + 20f), null);
		}
		yield break;
	}

	// Token: 0x0600226F RID: 8815 RVA: 0x000BD6FC File Offset: 0x000BB8FC
	public IEnumerator moveX_cr()
	{
		for (;;)
		{
			yield return new WaitForFixedUpdate();
			if ((this.moveDir.x > 0 && base.transform.position.x > 300f) || (this.moveDir.x < 0 && base.transform.position.x < -300f))
			{
				this.moveDir.x = this.moveDir.x * -1;
			}
			base.transform.AddPosition(this.moveDir.x * this.p.xSpeed * CupheadTime.FixedDelta, 0f, 0f);
		}
		yield break;
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x000BD718 File Offset: 0x000BB918
	public override void Dead()
	{
		this.StopAllCoroutines();
		foreach (Collider2D collider2D in base.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.IsDead = true;
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.color = new Color(0f, 0f, 0f, 0.25f);
		}
		this.properties.DealDamageToNextNamedState();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x000BD7B8 File Offset: 0x000BB9B8
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(350f - ((float)Level.Current.Ground + 20f), 500f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001C6B RID: 7275
	public const float OFFSCREEN_Y = 350f;

	// Token: 0x04001C6C RID: 7276
	public const float TURNAROUND_X = 300f;

	// Token: 0x04001C6D RID: 7277
	public const float PADDLE_PADDING = 20f;

	// Token: 0x04001C6E RID: 7278
	public const float SHIP_PADDING = 80f;

	// Token: 0x04001C6F RID: 7279
	public const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x04001C70 RID: 7280
	[SerializeField]
	public Transform paddle;

	// Token: 0x04001C71 RID: 7281
	public LevelProperties.RetroArcade properties;

	// Token: 0x04001C72 RID: 7282
	public LevelProperties.RetroArcade.PaddleShip p;

	// Token: 0x04001C73 RID: 7283
	public float ySpeed;

	// Token: 0x04001C74 RID: 7284
	public Trilean2 moveDir;
}
