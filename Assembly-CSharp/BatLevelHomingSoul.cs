using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class BatLevelHomingSoul : AbstractCollidableObject
{
	// Token: 0x060010CA RID: 4298 RVA: 0x00091000 File Offset: 0x0008F200
	public void Init(Vector2 pos, AbstractPlayerController player, LevelProperties.Bat.WolfSoul properties)
	{
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
		this.properties = properties;
		this.player = player;
		this.durationString = properties.floatUpDuration.GetRandom<string>().Split(new char[]
		{
			','
		});
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x0000E272 File Offset: 0x0000C472
	public override void Awake()
	{
		base.Awake();
		base.GetComponent<Collider2D>().enabled = false;
		this.isHoming = true;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x00091070 File Offset: 0x0008F270
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.aim == null || this.player == null)
		{
			return;
		}
		if (this.isHoming)
		{
			float num = Vector3.Distance(base.transform.position, this.player.transform.position);
			base.transform.position -= base.transform.right * this.properties.homingSpeed * CupheadTime.Delta;
			this.aim.LookAt2D(2f * base.transform.position - this.player.center);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.properties.homingRotation * CupheadTime.Delta);
			if (Mathf.Abs(num) < this.maxDist && this.isHoming)
			{
				base.StartCoroutine(this.attack_cr());
				this.isHoming = false;
			}
		}
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x0000E298 File Offset: 0x0000C498
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x000911C0 File Offset: 0x0008F3C0
	public IEnumerator attack_cr()
	{
		base.animator.SetTrigger("Warning");
		yield return CupheadTime.WaitForSeconds(this, this.properties.floatWarningDuration);
		base.animator.SetTrigger("Attack");
		base.GetComponent<Collider2D>().enabled = true;
		yield return CupheadTime.WaitForSeconds(this, this.properties.attackDuration);
		base.animator.SetTrigger("End");
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.float_up_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x000911DC File Offset: 0x0008F3DC
	public IEnumerator float_up_cr()
	{
		float t = 0f;
		float duration = 0f;
		Parser.FloatTryParse(this.durationString[this.durationIndex], out duration);
		this.player = PlayerManager.GetNext();
		while (t < duration)
		{
			base.transform.AddPosition(0f, this.properties.floatSpeed * CupheadTime.Delta, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.isHoming = true;
		this.durationIndex %= this.durationString.Length;
		yield return null;
		yield break;
	}

	// Token: 0x04000DA7 RID: 3495
	public LevelProperties.Bat.WolfSoul properties;

	// Token: 0x04000DA8 RID: 3496
	public AbstractPlayerController player;

	// Token: 0x04000DA9 RID: 3497
	public DamageDealer damageDealer;

	// Token: 0x04000DAA RID: 3498
	public int durationIndex;

	// Token: 0x04000DAB RID: 3499
	public Transform aim;

	// Token: 0x04000DAC RID: 3500
	public Transform targetPos;

	// Token: 0x04000DAD RID: 3501
	public float maxDist = 100f;

	// Token: 0x04000DAE RID: 3502
	public bool isHoming;

	// Token: 0x04000DAF RID: 3503
	public string[] durationString;
}
