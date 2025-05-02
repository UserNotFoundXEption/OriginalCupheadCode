using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class SallyStagePlayLevelRose : AbstractProjectile
{
	// Token: 0x0600263A RID: 9786 RVA: 0x000C8744 File Offset: 0x000C6944
	public SallyStagePlayLevelRose Create(Vector2 pos, LevelProperties.SallyStagePlay.Roses properties)
	{
		SallyStagePlayLevelRose sallyStagePlayLevelRose = base.Create(pos) as SallyStagePlayLevelRose;
		sallyStagePlayLevelRose.properties = properties;
		return sallyStagePlayLevelRose;
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x000C8768 File Offset: 0x000C6968
	public override void Start()
	{
		base.Start();
		base.transform.SetScale(new float?((float)((!Rand.Bool()) ? -1 : 1)), null, null);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x0002015C File Offset: 0x0001E35C
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x0002017A File Offset: 0x0001E37A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x000C87BC File Offset: 0x000C69BC
	public IEnumerator move_cr()
	{
		float speed = this.properties.fallSpeed.min;
		while (base.transform.position.y > (float)(Level.Current.Ground + 10))
		{
			base.transform.position += Vector3.down * speed * CupheadTime.Delta;
			if (speed < this.properties.fallSpeed.max)
			{
				speed += this.properties.fallAcceleration;
			}
			yield return null;
		}
		base.animator.SetTrigger("Land");
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.animator.SetBool("IsA", Rand.Bool());
		yield return CupheadTime.WaitForSeconds(this, this.properties.groundDuration);
		base.StartCoroutine(this.despawn_cr());
		yield break;
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x000C87D8 File Offset: 0x000C69D8
	public IEnumerator despawn_cr()
	{
		SpriteRenderer s = base.GetComponentInChildren<SpriteRenderer>(false);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			s.color = new Color(s.color.r, s.color.b, s.color.g, 1f - t / time);
			yield return null;
		}
		base.GetComponent<Collider2D>().enabled = false;
		this.Die();
		yield break;
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x00020198 File Offset: 0x0001E398
	public override void Die()
	{
		this.StopAllCoroutines();
		base.GetComponentInChildren<SpriteRenderer>(false).enabled = false;
		base.Die();
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000C87F4 File Offset: 0x000C69F4
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		this.pinkRose.SetActive(false);
		this.normalRose.SetActive(false);
		if (parryable)
		{
			this.pinkRose.SetActive(true);
		}
		else
		{
			this.normalRose.SetActive(true);
		}
	}

	// Token: 0x04001F99 RID: 8089
	[SerializeField]
	public GameObject normalRose;

	// Token: 0x04001F9A RID: 8090
	[SerializeField]
	public GameObject pinkRose;

	// Token: 0x04001F9B RID: 8091
	public LevelProperties.SallyStagePlay.Roses properties;

	// Token: 0x04001F9C RID: 8092
	public float speed;
}
