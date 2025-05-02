using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class RetroArcadeQShip : RetroArcadeEnemy
{
	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06002273 RID: 8819 RVA: 0x0001D56E File Offset: 0x0001B76E
	// (set) Token: 0x06002274 RID: 8820 RVA: 0x0001D576 File Offset: 0x0001B776
	public float TileRotationSpeed { get; set; }

	// Token: 0x06002275 RID: 8821 RVA: 0x0001D57F File Offset: 0x0001B77F
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x000BD7D4 File Offset: 0x000BB9D4
	public void StartQShip()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.qShip;
		this.hp = this.p.hp;
		this.TileRotationSpeed = this.p.tileRotationSpeed.min;
		base.PointsBonus = this.p.pointsGained;
		base.PointsWorth = this.p.pointsBonus;
		this.tiles = new List<RetroArcadeQShipOrbitingTile>();
		for (int i = 0; i < this.p.numSpinningTiles; i++)
		{
			RetroArcadeQShipOrbitingTile item = this.tilePrefab.Create(this, 360f * (float)i / (float)this.p.numSpinningTiles, this.p);
			this.tiles.Add(item);
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.tentacle_cr());
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x000BD8C4 File Offset: 0x000BBAC4
	public override void FixedUpdate()
	{
		this.TileRotationSpeed = this.p.tileRotationSpeed.min * Mathf.Pow(this.p.tileRotationSpeed.max / this.p.tileRotationSpeed.min, 1f - this.hp / this.p.hp);
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x000BD928 File Offset: 0x000BBB28
	public IEnumerator move_cr()
	{
		base.transform.SetPosition(new float?(0f), new float?(350f + this.p.tileRotationDistance), null);
		base.MoveY(this.p.yPos - (350f + this.p.tileRotationDistance), 500f);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		float t = 0f;
		float moveTime = this.p.maxXPos * 2f / this.p.moveSpeed;
		for (;;)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Sin(t * 3.14159274f / moveTime) * this.p.maxXPos), null, null);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002279 RID: 8825 RVA: 0x000BD944 File Offset: 0x000BBB44
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

	// Token: 0x0600227A RID: 8826 RVA: 0x000BD9E4 File Offset: 0x000BBBE4
	public IEnumerator moveOffscreen_cr()
	{
		base.MoveY(350f + this.p.tileRotationDistance - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x000BDA00 File Offset: 0x000BBC00
	public void ShootProjectile()
	{
		this.projectilePrefab.Create(this.projectileRoot.position, -90f - this.p.shotSpreadAngle, this.p.shotSpeed);
		this.projectilePrefab.Create(this.projectileRoot.position, -90f, this.p.shotSpeed);
		this.projectilePrefab.Create(this.projectileRoot.position, -90f + this.p.shotSpreadAngle, this.p.shotSpeed);
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x000BDAAC File Offset: 0x000BBCAC
	public IEnumerator tentacle_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.tentacleSpawnRange.RandomFloat());
			bool left = Rand.Bool();
			base.animator.SetBool((!left) ? "RightTentacle" : "LeftTentacle", true);
			yield return CupheadTime.WaitForSeconds(this, this.p.tentacleWarningDuration);
			base.animator.SetBool((!left) ? "RightTentacle" : "LeftTentacle", false);
			this.tentaclePrefab.Create((!left) ? RetroArcadeQShipTentacle.Direction.Left : RetroArcadeQShipTentacle.Direction.Right, this.p);
		}
		yield break;
	}

	// Token: 0x04001C75 RID: 7285
	public const float OFFSCREEN_Y = 350f;

	// Token: 0x04001C76 RID: 7286
	public const float MOVE_Y_SPEED = 500f;

	// Token: 0x04001C77 RID: 7287
	public LevelProperties.RetroArcade properties;

	// Token: 0x04001C78 RID: 7288
	public LevelProperties.RetroArcade.QShip p;

	// Token: 0x04001C79 RID: 7289
	[SerializeField]
	public RetroArcadeQShipOrbitingTile tilePrefab;

	// Token: 0x04001C7A RID: 7290
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x04001C7B RID: 7291
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001C7C RID: 7292
	[SerializeField]
	public RetroArcadeQShipTentacle tentaclePrefab;

	// Token: 0x04001C7D RID: 7293
	public List<RetroArcadeQShipOrbitingTile> tiles;
}
