using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class FlyingCowboyLevelSnake : AbstractProjectile
{
	// Token: 0x06001BD7 RID: 7127 RVA: 0x000178F9 File Offset: 0x00015AF9
	public void Move(Vector3 position, float speedX, float speedY, float stopPosX, float gravity, LevelProperties.FlyingCowboy.SnakeAttack properties)
	{
		base.transform.position = position;
		this.properties = properties;
		this.speed = new Vector3(speedX, speedY);
		this.stopPosX = stopPosX;
		this.gravity = gravity;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x00017939 File Offset: 0x00015B39
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x000ACE48 File Offset: 0x000AB048
	public IEnumerator move_cr()
	{
		while (base.transform.position.x < this.stopPosX)
		{
			this.speed += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
			base.transform.Translate(this.speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		BasicProjectile snake = this.snakeLine.Create(base.transform.position, 0f, -this.properties.snakeSpeed);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040016A1 RID: 5793
	[SerializeField]
	public BasicProjectile snakeLine;

	// Token: 0x040016A2 RID: 5794
	public LevelProperties.FlyingCowboy.SnakeAttack properties;

	// Token: 0x040016A3 RID: 5795
	public Vector3 speed;

	// Token: 0x040016A4 RID: 5796
	public float gravity;

	// Token: 0x040016A5 RID: 5797
	public float stopPosX;
}
