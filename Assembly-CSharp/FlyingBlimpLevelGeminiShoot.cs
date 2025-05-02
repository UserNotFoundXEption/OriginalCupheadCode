using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000244 RID: 580
public class FlyingBlimpLevelGeminiShoot : AbstractCollidableObject
{
	// Token: 0x06001A9B RID: 6811 RVA: 0x000A922C File Offset: 0x000A742C
	public void Init(LevelProperties.FlyingBlimp.Gemini properties, Vector2 pos)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.smallRadius = base.GetComponent<CircleCollider2D>().radius;
		float num = (float)Random.Range(0, 2);
		this.pointingUp = (num == 0f);
		if (this.pointingUp)
		{
			this.projectileRoot = this.projectileRootUp;
		}
		else
		{
			this.projectileRoot = this.projectileRootDown;
		}
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x000A92B8 File Offset: 0x000A74B8
	public IEnumerator rotate_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		AudioManager.Play("level_flying_blimp_wheel_start");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetBool("Attack", true);
		this.smallFXSpawning = true;
		base.StartCoroutine(this.spawn_small_fx_cr());
		AudioManager.PlayLoop("level_flying_blimp_gemini_sphere_attack");
		float pct = 0f;
		float startRotation = (float)((!Rand.Bool()) ? -360 : 360);
		while (pct <= 1f)
		{
			base.transform.SetEulerAngles(null, null, new float?(startRotation * pct));
			pct += CupheadTime.FixedDelta * this.properties.rotationSpeed;
			this.ShootBullet();
			yield return wait;
		}
		base.transform.SetEulerAngles(null, null, new float?((float)((startRotation != 360f) ? 360 : -360)));
		this.smallFXSpawning = false;
		base.animator.SetBool("Attack", false);
		base.animator.SetTrigger("Leave");
		AudioManager.Stop("level_flying_blimp_gemini_sphere_attack");
		AudioManager.Play("level_flying_blimp_wheel_end");
		yield break;
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000A92D4 File Offset: 0x000A74D4
	public void ShootBullet()
	{
		float num = this.projectileRoot.position.x - base.transform.position.x;
		float num2 = this.projectileRoot.position.y - base.transform.position.y;
		float rotation = Mathf.Atan2(num2, num) * 57.29578f;
		if (this.delayTime < this.properties.bulletDelay)
		{
			this.delayTime += 1f;
		}
		else
		{
			this.projectilePrefab.Create(this.projectileRoot.position, rotation, this.properties.bulletSpeed);
			this.delayTime = 0f;
		}
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x000A93A4 File Offset: 0x000A75A4
	public IEnumerator spawn_small_fx_cr()
	{
		while (this.smallFXSpawning)
		{
			GameObject small = Object.Instantiate<GameObject>(this.smallFX);
			Vector3 scale = new Vector3(1f, 1f, 1f);
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
			small.transform.SetScale(new float?(scale.x), new float?(scale.y), new float?(1f));
			small.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
			small.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(0, 3);
			small.transform.position = this.GetRandomPoint();
			base.StartCoroutine(this.delete_small_fx(small));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x000A93C0 File Offset: 0x000A75C0
	public Vector2 GetRandomPoint()
	{
		Vector2 vector = base.transform.position;
		Vector2 vector2;
		vector2..ctor((float)Random.Range(-1, 1), (float)Random.Range(-1, 1));
		Vector2 vector3 = vector2.normalized * (this.smallRadius * Random.value) * 2f;
		return vector + vector3;
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x000A9420 File Offset: 0x000A7620
	public IEnumerator delete_small_fx(GameObject smallFX)
	{
		yield return smallFX.GetComponent<Animator>().WaitForAnimationToEnd(this, "SmallFX", false, true);
		Object.Destroy(smallFX);
		yield break;
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00016A5D File Offset: 0x00014C5D
	public void Die()
	{
		AudioManager.Play("level_flying_blimp_gemini_sphere_leave");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001560 RID: 5472
	public LevelProperties.FlyingBlimp.Gemini properties;

	// Token: 0x04001561 RID: 5473
	[SerializeField]
	public GameObject smallFX;

	// Token: 0x04001562 RID: 5474
	[SerializeField]
	public Transform projectileRootUp;

	// Token: 0x04001563 RID: 5475
	[SerializeField]
	public Transform projectileRootDown;

	// Token: 0x04001564 RID: 5476
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x04001565 RID: 5477
	public float smallRadius;

	// Token: 0x04001566 RID: 5478
	public Transform projectileRoot;

	// Token: 0x04001567 RID: 5479
	public Vector3 target;

	// Token: 0x04001568 RID: 5480
	public Quaternion startRotation;

	// Token: 0x04001569 RID: 5481
	public float rotationTime;

	// Token: 0x0400156A RID: 5482
	public float delayTime;

	// Token: 0x0400156B RID: 5483
	public bool pointingUp;

	// Token: 0x0400156C RID: 5484
	public bool smallFXSpawning;

	// Token: 0x0400156D RID: 5485
	public bool halfWay;
}
