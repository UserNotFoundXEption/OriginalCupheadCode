using System;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class ChessQueenLevelCannonball : BasicProjectile
{
	// Token: 0x060012EC RID: 4844 RVA: 0x0000FED9 File Offset: 0x0000E0D9
	public override void Awake()
	{
		base.Awake();
		this.DamagesType.Player = false;
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x0009652C File Offset: 0x0009472C
	public override AbstractProjectile Create(Vector2 position, float rotation)
	{
		ChessQueenLevelCannonball chessQueenLevelCannonball = this.Create(position) as ChessQueenLevelCannonball;
		chessQueenLevelCannonball.vel = MathUtils.AngleToDirection(rotation);
		return chessQueenLevelCannonball;
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x00096558 File Offset: 0x00094758
	public override void Move()
	{
		if (this.hit)
		{
			return;
		}
		base.transform.position += this.vel * this.Speed * CupheadTime.FixedDelta;
		this.frameTimer += CupheadTime.FixedDelta;
		if (this.frameTimer >= 0.0416666679f)
		{
			float num = Mathf.Lerp(1f, this.minScale, Mathf.InverseLerp(-150f, 440f, base.transform.position.y));
			this.sprite.transform.localScale = new Vector3(num, num);
			this.frameTimer -= 0.0416666679f;
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x00096620 File Offset: 0x00094820
	public void HitQueen()
	{
		this.hit = true;
		this.sprite.transform.localScale = new Vector3(1f, 1f);
		this.sprite.flipX = Rand.Bool();
		this.sprite.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		base.animator.Play("Explode");
		base.animator.Update(0f);
	}

	// Token: 0x04000F3E RID: 3902
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x04000F3F RID: 3903
	public Vector3 vel;

	// Token: 0x04000F40 RID: 3904
	[SerializeField]
	public float minScale = 0.75f;

	// Token: 0x04000F41 RID: 3905
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x04000F42 RID: 3906
	public float frameTimer;

	// Token: 0x04000F43 RID: 3907
	public bool hit;
}
