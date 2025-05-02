using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class RobotLevelRobot : LevelProperties.Robot.Entity
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x0600235A RID: 9050 RVA: 0x000C0210 File Offset: 0x000BE410
	// (remove) Token: 0x0600235B RID: 9051 RVA: 0x000C0248 File Offset: 0x000BE448
	public event Action OnDeathEvent;

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x0600235C RID: 9052 RVA: 0x000C0280 File Offset: 0x000BE480
	// (remove) Token: 0x0600235D RID: 9053 RVA: 0x000C02B8 File Offset: 0x000BE4B8
	public event Action OnPrimaryDeathEvent;

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x0600235E RID: 9054 RVA: 0x000C02F0 File Offset: 0x000BE4F0
	// (remove) Token: 0x0600235F RID: 9055 RVA: 0x000C0328 File Offset: 0x000BE528
	public event Action OnSecondaryDeathEvent;

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x06002360 RID: 9056 RVA: 0x000C0360 File Offset: 0x000BE560
	// (remove) Token: 0x06002361 RID: 9057 RVA: 0x000C0398 File Offset: 0x000BE598
	public event Action callback;

	// Token: 0x06002362 RID: 9058 RVA: 0x000C03D0 File Offset: 0x000BE5D0
	public override void Awake()
	{
		foreach (CollisionChild s in this.collisionChilds)
		{
			base.RegisterCollisionChild(s);
		}
		base.Awake();
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000C040C File Offset: 0x000BE60C
	public override void LevelInit(LevelProperties.Robot properties)
	{
		Level.Current.OnIntroEvent += this.OnIntro;
		if (Level.Current.mode == Level.Mode.Easy)
		{
			Level.Current.OnWinEvent += this.OnDeathDance;
		}
		this.damageDealer = DamageDealer.NewEnemy();
		this.walkPCT = (this.walkTime = 0f);
		base.StartCoroutine(this.disableIntro_cr());
		base.LevelInit(properties);
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000C0488 File Offset: 0x000BE688
	public IEnumerator disableIntro_cr()
	{
		yield return new WaitForEndOfFrame();
		base.animator.enabled = false;
		yield break;
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000C04A4 File Offset: 0x000BE6A4
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.introEnded)
		{
			float num = Mathf.Max(PlayerManager.GetNext().center.x, PlayerManager.GetNext().center.x);
			if (num > base.transform.position.x)
			{
				this.UpdatePosition(true);
			}
			else
			{
				this.UpdatePosition(false);
			}
		}
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x0001DF09 File Offset: 0x0001C109
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x000C0528 File Offset: 0x000BE728
	public void IntroEnded()
	{
		AudioManager.Play("robot_vocals_laugh");
		this.emitAudioFromObject.Add("robot_vocals_laugh");
		this.introEnded = true;
		base.animator.SetBool("MainAnimationActive", false);
		this.head.GetComponent<RobotLevelRobotHead>().InitBodyPart(this, base.properties, 1, 1, 0f);
		this.chest.GetComponent<RobotLevelRobotChest>().InitBodyPart(this, base.properties, 0, 1, 0f);
		this.hatch.GetComponent<RobotLevelRobotHatch>().InitBodyPart(this, base.properties, 0, 1, 0f);
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x0001DF27 File Offset: 0x0001C127
	public void TriggerPhaseTwo(Action callback)
	{
		base.animator.Play("Phase2 Transition", 2);
		this.callback = callback;
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x0001DF41 File Offset: 0x0001C141
	public void OnDeathDance()
	{
		this.chest.animator.Play("Off", 1);
		base.animator.Play("Death Dance");
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000C05C4 File Offset: 0x000BE7C4
	public IEnumerator death_cr()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < 3; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
		}
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		if (Level.Current.mode != Level.Mode.Easy && this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x0001DF76 File Offset: 0x0001C176
	public void OnRobotIntro()
	{
		this.chest.GetComponent<RobotLevelRobotChest>().InitAnims();
		this.hatch.GetComponent<RobotLevelRobotHatch>().InitAnims();
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x0001DF98 File Offset: 0x0001C198
	public void OnIntro()
	{
		this.SoundRobotIntro();
		base.animator.enabled = true;
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000C05E0 File Offset: 0x000BE7E0
	public void PrimaryDied()
	{
		if (this.OnPrimaryDeathEvent != null)
		{
			this.OnPrimaryDeathEvent();
		}
		this.remainingPrimaryAttacks--;
		if (this.remainingPrimaryAttacks <= 0 && this.OnSecondaryDeathEvent != null)
		{
			this.OnSecondaryDeathEvent();
		}
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000C0634 File Offset: 0x000BE834
	public void UpdatePosition(bool closeGap)
	{
		float duration = 4f;
		float levelTime = Level.Current.LevelTime;
		if (closeGap)
		{
			Vector3 position = this.walkingPositions[0].position;
			Vector3 position2 = this.walkingPositions[1].position;
			this.Move(position, position2, duration, 1);
		}
		else
		{
			Vector3 position = this.walkingPositions[1].position;
			Vector3 position2 = this.walkingPositions[0].position;
			this.Move(position, position2, duration, -1);
		}
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000C06AC File Offset: 0x000BE8AC
	public void Move(Vector3 startPosition, Vector3 endPosition, float duration, int direction)
	{
		this.walkTime += CupheadTime.Delta * (float)direction;
		if (direction < 0)
		{
			if (this.walkTime <= 0f)
			{
				this.walkTime = 0f;
			}
		}
		else if (this.walkTime >= duration)
		{
			this.walkTime = duration;
		}
		this.walkPCT = this.walkTime / duration;
		if (this.walkPCT >= 1f)
		{
			this.walkPCT = 1f;
		}
		if (direction < 0)
		{
			this.walkPCT = 1f - this.walkPCT;
		}
		base.transform.position = startPosition + (endPosition - startPosition) * this.walkPCT;
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x0001DFAC File Offset: 0x0001C1AC
	public void SpawnSmoke()
	{
		this.headcannonSmoke.Create(this.head.transform.position);
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x0001DFCA File Offset: 0x0001C1CA
	public void OnDeathSFX()
	{
		AudioManager.Play("robot_vocals_dying");
		this.emitAudioFromObject.Add("robot_vocals_dying");
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x0001DFE6 File Offset: 0x0001C1E6
	public void SoundRobotIntro()
	{
		AudioManager.Play("robot_intro");
		this.emitAudioFromObject.Add("robot_intro");
	}

	// Token: 0x04001D5D RID: 7517
	public Action attackCallback;

	// Token: 0x04001D62 RID: 7522
	[SerializeField]
	public Effect headcannonSmoke;

	// Token: 0x04001D63 RID: 7523
	[SerializeField]
	public Transform[] walkingPositions;

	// Token: 0x04001D64 RID: 7524
	[SerializeField]
	public RobotLevelRobotHead head;

	// Token: 0x04001D65 RID: 7525
	[SerializeField]
	public RobotLevelRobotBodyPart chest;

	// Token: 0x04001D66 RID: 7526
	[SerializeField]
	public RobotLevelRobotHatch hatch;

	// Token: 0x04001D67 RID: 7527
	[Space(10f)]
	[SerializeField]
	public GameObject finalForm;

	// Token: 0x04001D68 RID: 7528
	public bool introEnded;

	// Token: 0x04001D69 RID: 7529
	public float walkPCT;

	// Token: 0x04001D6A RID: 7530
	public float walkTime;

	// Token: 0x04001D6B RID: 7531
	public int remainingPrimaryAttacks = 3;

	// Token: 0x04001D6C RID: 7532
	public DamageDealer damageDealer;

	// Token: 0x04001D6D RID: 7533
	[SerializeField]
	public CollisionChild[] collisionChilds;
}
