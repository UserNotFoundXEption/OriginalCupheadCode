using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class DevilLevelBomb : AbstractProjectile
{
	// Token: 0x060014FE RID: 5374 RVA: 0x0009AF3C File Offset: 0x0009913C
	public DevilLevelBomb Create(Vector2 pos, LevelProperties.Devil.BombEye properties, bool onLeft)
	{
		DevilLevelBomb devilLevelBomb = this.InstantiatePrefab<DevilLevelBomb>();
		devilLevelBomb.properties = properties;
		devilLevelBomb.transform.position = pos;
		devilLevelBomb.startPos = pos;
		devilLevelBomb.flipX = Rand.Bool();
		devilLevelBomb.flipY = Rand.Bool();
		devilLevelBomb.onLeft = onLeft;
		return devilLevelBomb;
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x00011D9E File Offset: 0x0000FF9E
	public override void Start()
	{
		base.Start();
		AudioManager.Play("p3_bomb_appear");
		this.emitAudioFromObject.Add("p3_bomb_appear");
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x0009AF90 File Offset: 0x00099190
	public IEnumerator move_cr()
	{
		float t = 0f;
		float time = 0.5f;
		float end = (!this.onLeft) ? (this.startPos.x - 300f) : (this.startPos.x + 300f);
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(this.startPos.x, end, t / time)), null, null);
			yield return new WaitForFixedUpdate();
		}
		base.StartCoroutine(this.fade_shadow_cr());
		this.endPos = base.transform.position;
		this.comingOut = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x0009AFAC File Offset: 0x000991AC
	public IEnumerator fade_shadow_cr()
	{
		float t = 0f;
		float time = 1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.shadowSprite.color = new Color(1f, 1f, 1f, 1f - t / time);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x0009AFC8 File Offset: 0x000991C8
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead || this.comingOut)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		if (this.t > this.properties.explodeDelay)
		{
			this.Explode();
			this.Die();
			return;
		}
		Vector2 vector = this.endPos;
		vector.x += Mathf.Sin(this.t * this.properties.xSinSpeed * (float)((!this.flipX) ? 1 : -1)) * this.properties.xSinHeight;
		vector.y += Mathf.Sin(this.t * this.properties.ySinSpeed * (float)((!this.flipY) ? 1 : -1)) * this.properties.ySinHeight;
		base.transform.SetPosition(new float?(vector.x), new float?(vector.y), null);
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x00011DCD File Offset: 0x0000FFCD
	public void Explode()
	{
		this.explosionPrefab.Create(base.transform.position);
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x00011DE6 File Offset: 0x0000FFE6
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x00011DF9 File Offset: 0x0000FFF9
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionPrefab = null;
	}

	// Token: 0x04001130 RID: 4400
	[SerializeField]
	public DevilLevelBombExplosion explosionPrefab;

	// Token: 0x04001131 RID: 4401
	[SerializeField]
	public SpriteRenderer shadowSprite;

	// Token: 0x04001132 RID: 4402
	public LevelProperties.Devil.BombEye properties;

	// Token: 0x04001133 RID: 4403
	public Vector2 startPos;

	// Token: 0x04001134 RID: 4404
	public Vector2 endPos;

	// Token: 0x04001135 RID: 4405
	public float t;

	// Token: 0x04001136 RID: 4406
	public bool flipX;

	// Token: 0x04001137 RID: 4407
	public bool flipY;

	// Token: 0x04001138 RID: 4408
	public bool onLeft;

	// Token: 0x04001139 RID: 4409
	public bool comingOut = true;
}
