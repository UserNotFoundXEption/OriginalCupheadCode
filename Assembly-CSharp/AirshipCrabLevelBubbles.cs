using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class AirshipCrabLevelBubbles : AbstractProjectile
{
	// Token: 0x06000EEF RID: 3823 RVA: 0x0000CA96 File Offset: 0x0000AC96
	public void Init(Vector2 pos, LevelProperties.AirshipCrab.Bubbles properties, float speed)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.speed = speed;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x0000CAC4 File Offset: 0x0000ACC4
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x0000CAE2 File Offset: 0x0000ACE2
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x0008C81C File Offset: 0x0008AA1C
	public IEnumerator move_cr()
	{
		this.speedY = this.properties.sinWaveStrength;
		float t = Random.Range(0f, 6.28318548f);
		while (base.transform.position.x > -640f)
		{
			Vector3 pos = base.transform.position;
			pos.x = Mathf.MoveTowards(base.transform.position.x, -640f, this.speed * CupheadTime.Delta);
			base.transform.position = new Vector3(pos.x, base.transform.position.y + Mathf.Sin(t) * this.speedY * CupheadTime.Delta * 60f, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0000CAF9 File Offset: 0x0000ACF9
	public override void Die()
	{
		base.Die();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000C39 RID: 3129
	public LevelProperties.AirshipCrab.Bubbles properties;

	// Token: 0x04000C3A RID: 3130
	public float speed;

	// Token: 0x04000C3B RID: 3131
	public float speedY;
}
