using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000528 RID: 1320
public class PlayerSuperChaliceIII : AbstractPlayerSuper
{
	// Token: 0x060037B0 RID: 14256 RVA: 0x00104AA4 File Offset: 0x00102CA4
	public override void StartSuper()
	{
		base.StartSuper();
		if (!this.player.motor.Grounded)
		{
			base.animator.Play("StartAir");
		}
		base.animator.Update(0f);
		this.minionSpawn = new PatternString(this.minionSpawnString, true);
		this.minionSpawnTiming = new PatternString(this.minionSpawnTimingString, true);
		this.direction = base.transform.localScale.x;
		this.zoomFactor = Camera.main.orthographicSize / 360f;
		AudioManager.Play("player_super_chalice_barrage_start");
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x00104B4C File Offset: 0x00102D4C
	public IEnumerator super_cr()
	{
		this.Fire();
		for (;;)
		{
			if (this.player != null)
			{
				if (this.target.position.y < this.player.transform.position.y)
				{
					this.target.position = new Vector3(this.target.position.x, this.player.transform.position.y);
				}
				else
				{
					this.target.position += Vector3.down * this.aimSinkRate * CupheadTime.Delta;
				}
			}
			else
			{
				this.EndSuper(true);
				this.StopAllCoroutines();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x0002D758 File Offset: 0x0002B958
	public void StartMinions()
	{
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x00104B68 File Offset: 0x00102D68
	public IEnumerator shoot_cr()
	{
		this.mainTimer = WeaponProperties.LevelSuperChaliceIII.superDuration;
		yield return null;
		for (int i = 0; i < this.minionCount; i++)
		{
			int spawnDataOffset = this.minionSpawn.GetSubStringIndex();
			int spawnType = this.minionSpawn.PopInt();
			float angle = (this.direction <= 0f) ? 180f : 0f;
			BasicProjectile bullet = this.minionPrefab.Create(new Vector3(CupheadLevelCamera.Current.transform.position.x - 1000f * Mathf.Sign(this.direction), this.target.position.y + this.minionVerticalRange[spawnDataOffset].RandomFloat() * ((!this.linkRangeToZoom) ? 1f : this.zoomFactor)), angle, this.minionSpeed[spawnDataOffset] * ((!this.linkSpeedToZoom) ? 1f : this.zoomFactor));
			bullet.Damage = this.minionDamage[spawnDataOffset] / ((!this.linkDamageToZoom) ? 1f : this.zoomFactor);
			float s = this.minionScaleRange[spawnDataOffset].RandomFloat() * ((!this.linkScaleToZoom) ? 1f : this.zoomFactor);
			bullet.transform.localScale = new Vector3(s, s);
			((PlayerSuperChaliceIIIMinion)bullet).elementIndex = spawnDataOffset;
			((PlayerSuperChaliceIIIMinion)bullet).wave = this.wave;
			SpriteRenderer r = bullet.GetComponent<SpriteRenderer>();
			r.flipY = (this.direction < 0f);
			r.sortingOrder = ((s >= 1f) ? (100 - spawnType) : (-100 - spawnType));
			bullet.transform.position = new Vector3(bullet.transform.position.x, bullet.transform.position.y, (s - 1f) * 0.0001f);
			bullet.GetComponent<Animator>().Play(spawnType.ToString());
			bullet.DamageRate = 0.2f;
			bullet.PlayerId = this.player.id;
			bullet.GetComponent<Collider2D>().isTrigger = true;
			this.minionTypeCount[spawnType] = (this.minionTypeCount[spawnType] + 1) % 3;
			yield return CupheadTime.WaitForSeconds(this, this.minionSpawnTiming.PopFloat() * ((!this.linkSpawnRateToZoom) ? 1f : (this.zoomFactor * this.spawnRateZoomModifier)));
		}
		while (this.spear && !this.spear.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x00104B84 File Offset: 0x00102D84
	public void RespawnChalice()
	{
		this.EndSuper(true);
		this.fxRenderer.sortingLayerName = "Player";
		this.fxRenderer.sortingOrder = -10;
		this.chaliceSprite.sortingLayerName = "Player";
		this.chaliceSprite.sortingOrder = -20;
		this.chaliceRespawned = true;
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x00104BDC File Offset: 0x00102DDC
	public void ActivateSpear()
	{
		if (this.player != null)
		{
			this.spear.DetachFromSuper(this.player);
		}
		else
		{
			this.spear.transform.parent = null;
		}
		this.spear.gameObject.SetActive(true);
	}

	// Token: 0x04002CCA RID: 11466
	[SerializeField]
	public BasicProjectile minionPrefab;

	// Token: 0x04002CCB RID: 11467
	public float mainTimer = 100f;

	// Token: 0x04002CCC RID: 11468
	public float direction;

	// Token: 0x04002CCD RID: 11469
	[SerializeField]
	public int minionCount = 50;

	// Token: 0x04002CCE RID: 11470
	[SerializeField]
	public bool wave = true;

	// Token: 0x04002CCF RID: 11471
	[SerializeField]
	public float aimSinkRate = 100f;

	// Token: 0x04002CD0 RID: 11472
	[SerializeField]
	public SpriteRenderer chaliceSprite;

	// Token: 0x04002CD1 RID: 11473
	[SerializeField]
	public SpriteRenderer fxRenderer;

	// Token: 0x04002CD2 RID: 11474
	[SerializeField]
	public MinMax[] minionVerticalRange;

	// Token: 0x04002CD3 RID: 11475
	[SerializeField]
	public MinMax[] minionScaleRange;

	// Token: 0x04002CD4 RID: 11476
	[SerializeField]
	public float[] minionSpeed;

	// Token: 0x04002CD5 RID: 11477
	[SerializeField]
	public float[] minionDamage;

	// Token: 0x04002CD6 RID: 11478
	[SerializeField]
	public string minionSpawnString;

	// Token: 0x04002CD7 RID: 11479
	public PatternString minionSpawn;

	// Token: 0x04002CD8 RID: 11480
	[SerializeField]
	public string minionSpawnTimingString;

	// Token: 0x04002CD9 RID: 11481
	public PatternString minionSpawnTiming;

	// Token: 0x04002CDA RID: 11482
	[SerializeField]
	public int[] minionTypeCount = new int[6];

	// Token: 0x04002CDB RID: 11483
	[SerializeField]
	public PlayerSuperChaliceIIISpear spear;

	// Token: 0x04002CDC RID: 11484
	[SerializeField]
	public Transform target;

	// Token: 0x04002CDD RID: 11485
	public float zoomFactor;

	// Token: 0x04002CDE RID: 11486
	[SerializeField]
	public bool linkSpeedToZoom = true;

	// Token: 0x04002CDF RID: 11487
	[SerializeField]
	public bool linkScaleToZoom;

	// Token: 0x04002CE0 RID: 11488
	[SerializeField]
	public bool linkRangeToZoom;

	// Token: 0x04002CE1 RID: 11489
	[SerializeField]
	public bool linkDamageToZoom;

	// Token: 0x04002CE2 RID: 11490
	[SerializeField]
	public bool linkSpawnRateToZoom;

	// Token: 0x04002CE3 RID: 11491
	[SerializeField]
	public float spawnRateZoomModifier = 1f;

	// Token: 0x04002CE4 RID: 11492
	public bool chaliceRespawned;
}
