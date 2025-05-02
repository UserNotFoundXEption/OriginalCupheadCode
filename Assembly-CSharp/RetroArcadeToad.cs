using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class RetroArcadeToad : RetroArcadeEnemy
{
	// Token: 0x060022EE RID: 8942 RVA: 0x000BF23C File Offset: 0x000BD43C
	public RetroArcadeToad Create(RetroArcadeToadManager parent, LevelProperties.RetroArcade.Toad properties, bool onLeft)
	{
		RetroArcadeToad retroArcadeToad = this.InstantiatePrefab<RetroArcadeToad>();
		retroArcadeToad.transform.SetPosition(new float?((!onLeft) ? 330f : -330f), new float?(200f), null);
		retroArcadeToad.properties = properties;
		retroArcadeToad.parent = parent;
		retroArcadeToad.hp = properties.hp;
		retroArcadeToad.onLeft = onLeft;
		return retroArcadeToad;
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x0001DA4E File Offset: 0x0001BC4E
	public override void Start()
	{
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000BF2AC File Offset: 0x000BD4AC
	public IEnumerator jump_cr()
	{
		float speedY = this.properties.jumpVerticalSpeedRange.RandomFloat();
		float speedX = this.properties.jumpHorizontalSpeedRange.RandomFloat();
		float velocityX = speedX;
		float velocityY = speedY;
		float ground = (float)Level.Current.Ground + 50f;
		bool jumping = false;
		bool goingUp = false;
		this.gravity = this.properties.jumpGravity;
		while (base.transform.position.y > ground)
		{
			velocityY -= this.gravity * CupheadTime.Delta;
			base.transform.AddPosition(0f, velocityY * CupheadTime.Delta, 0f);
			yield return null;
		}
		Vector3 pos = base.transform.position;
		pos.y = ground;
		base.transform.position = pos;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.jumpDelay.RandomFloat());
			velocityY = speedY;
			velocityX = ((!this.onLeft) ? (-speedX) : speedX);
			jumping = true;
			goingUp = true;
			while (jumping)
			{
				velocityY -= this.gravity * CupheadTime.Delta;
				base.transform.AddPosition(velocityX * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0f);
				if (velocityY < 0f && goingUp)
				{
					goingUp = false;
				}
				if (velocityY < 0f && jumping && base.transform.position.y <= ground)
				{
					jumping = false;
					pos = base.transform.position;
					pos.y = ground;
					base.transform.position = pos;
				}
				if ((base.transform.position.x < -330f && !this.onLeft) || (base.transform.position.x > 330f && this.onLeft))
				{
					if (this.onLeft)
					{
						base.transform.SetPosition(new float?(330f), null, null);
						this.onLeft = false;
					}
					else
					{
						base.transform.SetPosition(new float?(-330f), null, null);
						this.onLeft = true;
					}
					velocityX = ((!this.onLeft) ? (-speedX) : speedX);
				}
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x0001DA5D File Offset: 0x0001BC5D
	public override void Dead()
	{
		base.Dead();
		this.parent.OnToadDie();
	}

	// Token: 0x04001CF2 RID: 7410
	public const float TOAD_MAX_X_POS = 330f;

	// Token: 0x04001CF3 RID: 7411
	public const float OFFSET_Y = 50f;

	// Token: 0x04001CF4 RID: 7412
	public const float OFFSCREEN_Y = 200f;

	// Token: 0x04001CF5 RID: 7413
	public const float BASE_Y = 250f;

	// Token: 0x04001CF6 RID: 7414
	public const float MOVE_Y_SPEED = 500f;

	// Token: 0x04001CF7 RID: 7415
	public LevelProperties.RetroArcade.Toad properties;

	// Token: 0x04001CF8 RID: 7416
	public RetroArcadeToadManager parent;

	// Token: 0x04001CF9 RID: 7417
	public float gravity;

	// Token: 0x04001CFA RID: 7418
	public bool onLeft;
}
