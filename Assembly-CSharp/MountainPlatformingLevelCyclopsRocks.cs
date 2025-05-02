using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class MountainPlatformingLevelCyclopsRocks : AbstractPausableComponent
{
	// Token: 0x06002EA5 RID: 11941 RVA: 0x00026E7E File Offset: 0x0002507E
	public void Start()
	{
		this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.UnSpawned;
		base.StartCoroutine(this.start_trigger_cr());
		this.cyclopsAnimator = this.cyclopsBG.GetComponent<Animator>();
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x000DFE04 File Offset: 0x000DE004
	public IEnumerator start_trigger_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.player = PlayerManager.GetNext();
		while (this.player.transform.position.x < this.onTrigger.transform.position.x)
		{
			yield return null;
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
		}
		this.StartCyclops();
		while (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead)
		{
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			this.playerInTrigger = (this.player.transform.position.x > this.onTrigger.transform.position.x);
			if (this.player.transform.position.x > this.offTrigger.transform.position.x)
			{
				this.cyclopsBG.isDead = true;
				this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead;
				break;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x000DFE20 File Offset: 0x000DE020
	public void StartCyclops()
	{
		this.IsIdle = true;
		this.playerInTrigger = true;
		this.cyclopsBG.start = this.cyclopsBG.transform.position;
		this.cyclopsAnimator.SetTrigger("StartCyclops");
		this.cyclopsAnimator.SetBool("isIdle", this.IsIdle);
		this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned;
		base.StartCoroutine(this.walk_and_idle_cr());
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x000DFEAC File Offset: 0x000DE0AC
	public IEnumerator turn_cyclops_cr()
	{
		if (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning)
		{
			string ani = this.IsIdle ? "Turn" : "Turn_To_Walk";
			this.cyclopsAnimator.SetTrigger("OnTurn");
			this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning;
			yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, ani, false, true);
			this.facingLeft = (this.cyclopsBG.transform.localScale.x == 1f);
			this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned;
		}
		yield break;
	}

	// Token: 0x06002EA9 RID: 11945 RVA: 0x000DFEC8 File Offset: 0x000DE0C8
	public IEnumerator walk_and_idle_cr()
	{
		this.facingLeft = true;
		float t = 0f;
		float timer = 1f;
		while (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead)
		{
			if (this.cyclopsState == MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned)
			{
				if (this.IsIdle)
				{
					if (this.player.transform.position.x < this.cyclopsBG.transform.position.x + this.cyclopsStopOffset && this.player.transform.position.x > this.cyclopsBG.transform.position.x - this.cyclopsStopOffset)
					{
						if (this.player.transform.position.x < this.cyclopsBG.transform.position.x && !this.facingLeft)
						{
							yield return base.StartCoroutine(this.turn_cyclops_cr());
						}
						else if (this.player.transform.position.x > this.cyclopsBG.transform.position.x && this.facingLeft)
						{
							yield return base.StartCoroutine(this.turn_cyclops_cr());
						}
					}
					else
					{
						this.IsIdle = false;
						this.cyclopsAnimator.SetBool("isIdle", this.IsIdle);
						yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Idle_To_Walk", false, true);
					}
				}
				else if (this.player.transform.position.x < this.cyclopsBG.transform.position.x - this.cyclopsStopOffset && !this.facingLeft)
				{
					yield return base.StartCoroutine(this.turn_cyclops_cr());
				}
				else if (this.player.transform.position.x > this.cyclopsBG.transform.position.x + this.cyclopsStopOffset && this.facingLeft)
				{
					yield return base.StartCoroutine(this.turn_cyclops_cr());
				}
				else if (t < timer)
				{
					t += CupheadTime.Delta;
				}
				else
				{
					this.IsIdle = true;
					this.cyclopsAnimator.SetBool("isIdle", this.IsIdle);
					yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Walk_To_Idle", false, true);
					t = 0f;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EAA RID: 11946 RVA: 0x000DFEE4 File Offset: 0x000DE0E4
	public IEnumerator move_cr()
	{
		while (this.cyclopsBG != null)
		{
			if (this.cyclopsBG.isWalking)
			{
				this.cyclopsBG.transform.AddPosition(((!this.facingLeft) ? this.walkSpeed : (-this.walkSpeed)) * CupheadTime.Delta, 0f, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EAB RID: 11947 RVA: 0x000DFF00 File Offset: 0x000DE100
	public IEnumerator attack_cr()
	{
		while (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.attackDelayRange.RandomFloat());
			while (!this.playerInTrigger || this.cyclopsState == MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning)
			{
				yield return null;
			}
			this.cyclopsBG.GetPlayer(this.player);
			this.cyclopsAnimator.SetTrigger("OnAttack");
			yield return this.cyclopsAnimator.WaitForAnimationToStart(this, "Attack_Start", false);
			this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Attacking;
			if (this.IsIdle)
			{
				yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Attack_To_Idle", false, true);
			}
			else
			{
				yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Attack_To_Walk", false, true);
			}
			if (this.player.transform.position.x < this.cyclopsBG.transform.position.x && !this.facingLeft)
			{
				yield return base.StartCoroutine(this.turn_cyclops_cr());
			}
			else if (this.player.transform.position.x > this.cyclopsBG.transform.position.x && this.facingLeft)
			{
				yield return base.StartCoroutine(this.turn_cyclops_cr());
			}
			else
			{
				this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		this.cyclopsAnimator.SetTrigger("OnAttack");
		yield return null;
		yield break;
	}

	// Token: 0x06002EAC RID: 11948 RVA: 0x000DFF1C File Offset: 0x000DE11C
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.offTrigger.transform.position, new Vector3(this.offTrigger.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.onTrigger.transform.position, new Vector3(this.onTrigger.transform.position.x, 5000f, 0f));
		Gizmos.color = new Color(1f, 0f, 1f, 1f);
		if (this.cyclopsBG)
		{
			Gizmos.DrawLine(new Vector3(this.cyclopsBG.transform.position.x + this.cyclopsStopOffset, this.cyclopsBG.transform.position.y), new Vector3(this.cyclopsBG.transform.position.x - this.cyclopsStopOffset, this.cyclopsBG.transform.position.y));
		}
	}

	// Token: 0x040026B0 RID: 9904
	[SerializeField]
	public float walkSpeed;

	// Token: 0x040026B1 RID: 9905
	[SerializeField]
	public MinMax attackDelayRange;

	// Token: 0x040026B2 RID: 9906
	[SerializeField]
	public Transform onTrigger;

	// Token: 0x040026B3 RID: 9907
	[SerializeField]
	public Transform offTrigger;

	// Token: 0x040026B4 RID: 9908
	[SerializeField]
	public MountainPlatformingLevelCyclopsBG cyclopsBG;

	// Token: 0x040026B5 RID: 9909
	[SerializeField]
	public float cyclopsStopOffset;

	// Token: 0x040026B6 RID: 9910
	public AbstractPlayerController player;

	// Token: 0x040026B7 RID: 9911
	public MountainPlatformingLevelCyclopsRocks.CyclopsState cyclopsState;

	// Token: 0x040026B8 RID: 9912
	public bool IsIdle;

	// Token: 0x040026B9 RID: 9913
	public bool playerInTrigger;

	// Token: 0x040026BA RID: 9914
	public bool facingLeft;

	// Token: 0x040026BB RID: 9915
	public Animator cyclopsAnimator;

	// Token: 0x0200109E RID: 4254
	public enum CyclopsState
	{
		// Token: 0x0400760A RID: 30218
		UnSpawned,
		// Token: 0x0400760B RID: 30219
		Spawned,
		// Token: 0x0400760C RID: 30220
		Turning,
		// Token: 0x0400760D RID: 30221
		Attacking,
		// Token: 0x0400760E RID: 30222
		Dead
	}
}
