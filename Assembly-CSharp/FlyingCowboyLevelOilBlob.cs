using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000258 RID: 600
public class FlyingCowboyLevelOilBlob : AbstractProjectile
{
	// Token: 0x06001BB4 RID: 7092 RVA: 0x000AC5A4 File Offset: 0x000AA7A4
	public FlyingCowboyLevelOilBlob Create(Vector3 position, float finalYPosition, float snakeSpawnX, LevelProperties.FlyingCowboy.SnakeAttack properties, bool playSplatSFX)
	{
		FlyingCowboyLevelOilBlob flyingCowboyLevelOilBlob = base.Create(position) as FlyingCowboyLevelOilBlob;
		flyingCowboyLevelOilBlob.initialYPosition = position.y;
		flyingCowboyLevelOilBlob.finalYPosition = finalYPosition;
		float num = flyingCowboyLevelOilBlob.finalYPosition - flyingCowboyLevelOilBlob.initialYPosition;
		float num2 = Mathf.Abs(num);
		if (num2 >= FlyingCowboyLevelOilBlob.BlobCHeight)
		{
			flyingCowboyLevelOilBlob.animator.Play((!Rand.Bool()) ? "F" : "C");
			flyingCowboyLevelOilBlob.finalYPosition -= Mathf.Sign(num) * FlyingCowboyLevelOilBlob.BlobCHeight;
		}
		else if (num2 >= FlyingCowboyLevelOilBlob.BlobBHeight)
		{
			flyingCowboyLevelOilBlob.animator.Play("B");
			flyingCowboyLevelOilBlob.finalYPosition -= Mathf.Sign(num) * FlyingCowboyLevelOilBlob.BlobBHeight;
		}
		else
		{
			flyingCowboyLevelOilBlob.animator.Play("A");
		}
		if (flyingCowboyLevelOilBlob.finalYPosition < flyingCowboyLevelOilBlob.initialYPosition)
		{
			Vector3 localScale = flyingCowboyLevelOilBlob.transform.localScale;
			localScale.y *= -1f;
			flyingCowboyLevelOilBlob.transform.localScale = localScale;
		}
		flyingCowboyLevelOilBlob.StartCoroutine(flyingCowboyLevelOilBlob.snakeSpawn_cr(snakeSpawnX, finalYPosition, properties, playSplatSFX));
		return flyingCowboyLevelOilBlob;
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x000177F7 File Offset: 0x000159F7
	public override void Awake()
	{
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x000AC6D4 File Offset: 0x000AA8D4
	public void LateUpdate()
	{
		if (this.spriteRenderer.sprite != this.previousSprite)
		{
			this.previousSprite = this.spriteRenderer.sprite;
			this.frameCounter++;
		}
		Vector3 position = base.transform.position;
		position.y = Mathf.Lerp(this.initialYPosition, this.finalYPosition, (float)this.frameCounter / 28f);
		base.transform.position = position;
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x00017805 File Offset: 0x00015A05
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x000AC758 File Offset: 0x000AA958
	public IEnumerator snakeSpawn_cr(float snakeSpawnX, float snakeSpawnY, LevelProperties.FlyingCowboy.SnakeAttack properties, bool playSplatSFX)
	{
		yield return null;
		yield return null;
		yield return base.animator.WaitForNormalizedTime(this, 1f, null, 0, false, false, true);
		BasicProjectile snake = this.snakePrefab.Create(new Vector2(snakeSpawnX + FlyingCowboyLevelOilBlob.SnakeSpawnOffsetX, snakeSpawnY), 0f, -properties.snakeSpeed);
		snake.animator.Play(0, 0, Random.Range(0f, 1f));
		this.splatEffect.Create(new Vector2(640f, snakeSpawnY));
		if (playSplatSFX)
		{
			AudioManager.Play("sfx_DLC_Cowgirl_P1_LiquidSplat");
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400167E RID: 5758
	public static readonly float BlobBHeight = 79f;

	// Token: 0x0400167F RID: 5759
	public static readonly float BlobCHeight = 293f;

	// Token: 0x04001680 RID: 5760
	public static readonly float SnakeSpawnOffsetX = 130f;

	// Token: 0x04001681 RID: 5761
	[SerializeField]
	public BasicProjectile snakePrefab;

	// Token: 0x04001682 RID: 5762
	[SerializeField]
	public Effect splatEffect;

	// Token: 0x04001683 RID: 5763
	public SpriteRenderer spriteRenderer;

	// Token: 0x04001684 RID: 5764
	public Sprite previousSprite;

	// Token: 0x04001685 RID: 5765
	public float initialYPosition;

	// Token: 0x04001686 RID: 5766
	public float finalYPosition;

	// Token: 0x04001687 RID: 5767
	public int frameCounter;
}
