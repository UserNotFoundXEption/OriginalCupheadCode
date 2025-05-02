using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public class MountainPlatformingLevelFlamer : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002EDB RID: 11995 RVA: 0x000270AE File Offset: 0x000252AE
	public override void OnStart()
	{
		this.isDead = false;
		this.angle = 3.14159274f;
		base.transform.position = this.startPos;
		base.StartCoroutine(this.check_dist_cr());
	}

	// Token: 0x06002EDC RID: 11996 RVA: 0x000E05D4 File Offset: 0x000DE7D4
	public override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
		this.pivotPoint = new GameObject("pivotPoint");
		this.pivotPoint.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 200f);
		this.angle = 3.14159274f;
		base.StartCoroutine(this.check_dist_cr());
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x000E0664 File Offset: 0x000DE864
	public void PathMovement()
	{
		this.angle += this.speed * CupheadTime.FixedDelta;
		Vector3 vector;
		vector..ctor(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 vector2;
		vector2..ctor(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.transform.position;
		base.transform.position += vector + vector2;
	}

	// Token: 0x06002EDE RID: 11998 RVA: 0x000270E0 File Offset: 0x000252E0
	public void MovePivot()
	{
		this.pivotPoint.transform.AddPosition(this.moveSpeed * CupheadTime.FixedDelta, 0f, 0f);
	}

	// Token: 0x06002EDF RID: 11999 RVA: 0x000E070C File Offset: 0x000DE90C
	public IEnumerator check_dist_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.player = PlayerManager.GetNext();
		bool movingLeft = base.transform.position.x > this.player.transform.position.x;
		this.moveSpeed = ((!movingLeft) ? base.Properties.flamerXSpeed.RandomFloat() : (-base.Properties.flamerXSpeed.RandomFloat()));
		AudioManager.PlayLoop("castle_flamer_loop");
		this.emitAudioFromObject.Add("castle_flamer_loop");
		base.animator.SetTrigger("OnFlame");
		yield return base.animator.WaitForAnimationToEnd(this, "Flame_Appear", false, true);
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x000E0728 File Offset: 0x000DE928
	public IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.startDelayRange.RandomFloat());
		YieldInstruction wait = new WaitForFixedUpdate();
		base.StartCoroutine(this.accelerate_speed_cr());
		while (!this.isDead)
		{
			this.PathMovement();
			this.MovePivot();
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x000E0744 File Offset: 0x000DE944
	public IEnumerator accelerate_speed_cr()
	{
		float incrementBy = 1f;
		while (this.speed < base.Properties.flamerCirSpeed && !this.isDead)
		{
			this.speed += incrementBy * CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x00027108 File Offset: 0x00025308
	public void PlayFace()
	{
		base.animator.Play("Flame_Face", 1);
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x0002711B File Offset: 0x0002531B
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(base.transform.position, 100f);
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x00027138 File Offset: 0x00025338
	public override void Die()
	{
		this.Deactivate();
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x000E0760 File Offset: 0x000DE960
	public void Deactivate()
	{
		this.isDead = true;
		AudioManager.Stop("castle_flamer_loop");
		this.speed = 0f;
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Flame_Appear_Loop", 0);
		base.animator.Play("Off", 1);
		base.StartCoroutine(this.activate_cr());
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x000E07C4 File Offset: 0x000DE9C4
	public IEnumerator activate_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.respawnRange.RandomFloat());
		this.OnStart();
		yield break;
	}

	// Token: 0x040026E0 RID: 9952
	[SerializeField]
	public float loopSize;

	// Token: 0x040026E1 RID: 9953
	[SerializeField]
	public MinMax startDelayRange;

	// Token: 0x040026E2 RID: 9954
	[SerializeField]
	public MinMax respawnRange;

	// Token: 0x040026E3 RID: 9955
	public GameObject pivotPoint;

	// Token: 0x040026E4 RID: 9956
	public float angle;

	// Token: 0x040026E5 RID: 9957
	public float speed;

	// Token: 0x040026E6 RID: 9958
	public float moveSpeed;

	// Token: 0x040026E7 RID: 9959
	public AbstractPlayerController player;

	// Token: 0x040026E8 RID: 9960
	public Vector3 startPos;

	// Token: 0x040026E9 RID: 9961
	public bool isDead;
}
