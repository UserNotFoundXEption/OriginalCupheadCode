using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000548 RID: 1352
public class WeaponFirecrackerProjectile : BasicProjectile
{
	// Token: 0x060038C7 RID: 14535 RVA: 0x001091CC File Offset: 0x001073CC
	public override void Update()
	{
		base.Update();
		if (this.parent != null && !this.brokeOffFromParent && this.parent.transform.localScale.x != this.player.motor.LookDirection.x)
		{
			base.transform.SetParent(null, true);
			this.brokeOffFromParent = true;
		}
	}

	// Token: 0x060038C8 RID: 14536 RVA: 0x0002E470 File Offset: 0x0002C670
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		this.hitEnemy = true;
		this.animator.Play("Hit");
		this.collider.enabled = false;
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x0010924C File Offset: 0x0010744C
	public void SetupFirecracker(Transform parent, LevelPlayerController player, bool isTypeB)
	{
		base.transform.SetParent(parent, true);
		this.parent = parent;
		this.player = player;
		this.distanceTraveled = 0f;
		if (isTypeB)
		{
			base.StartCoroutine(this.bullet_life_B_cr());
		}
		else
		{
			base.StartCoroutine(this.bullet_life_cr());
		}
	}

	// Token: 0x060038CA RID: 14538 RVA: 0x0002E49D File Offset: 0x0002C69D
	public void StillBullet()
	{
		this.move = false;
		base.StartCoroutine(this.bullet_slice_life_cr());
	}

	// Token: 0x060038CB RID: 14539 RVA: 0x001092A4 File Offset: 0x001074A4
	public IEnumerator bullet_life_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.bulletLife);
		this.move = false;
		this.collider.enabled = true;
		base.transform.SetScale(new float?(this.explosionSize), new float?(this.explosionSize), null);
		yield return CupheadTime.WaitForSeconds(this, this.explosionDuration);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060038CC RID: 14540 RVA: 0x001092C0 File Offset: 0x001074C0
	public IEnumerator bullet_life_B_cr()
	{
		float explodeDistance = this.bulletLife * this.Speed;
		while (this.distanceTraveled < explodeDistance)
		{
			yield return null;
		}
		this.move = false;
		WeaponFirecrackerProjectile slice = Object.Instantiate<WeaponFirecrackerProjectile>(this.projectile);
		Vector3 dir = MathUtils.AngleToDirection(this.explosionAngle);
		slice.transform.position = base.transform.position + dir * this.explosionRadiusSize;
		slice.collider.enabled = true;
		slice.collider.transform.SetScale(new float?(this.explosionSize), new float?(this.explosionSize), null);
		slice.DamageRate = this.DamageRate;
		slice.StillBullet();
		slice.gameObject.name = "FirecrackerExplosion";
		slice.transform.eulerAngles = new Vector3(0f, 0f, this.explosionAngle);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060038CD RID: 14541 RVA: 0x001092DC File Offset: 0x001074DC
	public IEnumerator bullet_slice_life_cr()
	{
		this.hitEnemy = false;
		this.animator.Play("Die");
		yield return CupheadTime.WaitForSeconds(this, this.explosionDuration);
		if (this.hitEnemy)
		{
			SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
			while (sprite.enabled)
			{
				yield return null;
			}
		}
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060038CE RID: 14542 RVA: 0x001092F8 File Offset: 0x001074F8
	public override void Move()
	{
		this.moveVector = base.transform.right * this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		base.transform.position += this.moveVector;
		this.distanceTraveled += this.moveVector.magnitude;
	}

	// Token: 0x04002D9A RID: 11674
	[SerializeField]
	public WeaponFirecrackerProjectile projectile;

	// Token: 0x04002D9B RID: 11675
	public float bulletLife;

	// Token: 0x04002D9C RID: 11676
	public float explosionSize;

	// Token: 0x04002D9D RID: 11677
	public float explosionDuration;

	// Token: 0x04002D9E RID: 11678
	public float explosionRadiusSize;

	// Token: 0x04002D9F RID: 11679
	public float explosionAngle;

	// Token: 0x04002DA0 RID: 11680
	public Transform parent;

	// Token: 0x04002DA1 RID: 11681
	public LevelPlayerController player;

	// Token: 0x04002DA2 RID: 11682
	public float parentScaleX;

	// Token: 0x04002DA3 RID: 11683
	public bool brokeOffFromParent;

	// Token: 0x04002DA4 RID: 11684
	public Vector3 moveVector;

	// Token: 0x04002DA5 RID: 11685
	public float distanceTraveled;

	// Token: 0x04002DA6 RID: 11686
	public Collider2D collider;

	// Token: 0x04002DA7 RID: 11687
	public new Animator animator;

	// Token: 0x04002DA8 RID: 11688
	public bool hitEnemy;
}
