using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class DicePalaceFlyingMemoryLevelBot : AbstractProjectile
{
	// Token: 0x060016CC RID: 5836 RVA: 0x000136AC File Offset: 0x000118AC
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x0009FC20 File Offset: 0x0009DE20
	public void Init(LevelProperties.DicePalaceFlyingMemory.Bots properties, DicePalaceFlyingMemoryLevelContactPoint startingPoint, bool moveOnY, float health, AbstractPlayerController player)
	{
		base.transform.position = startingPoint.transform.position;
		this.currentPoint = startingPoint;
		this.health = health;
		this.player = player;
		this.moveOnY = moveOnY;
		this.properties = properties;
		base.transform.SetScale(new float?(properties.botsScale), new float?(properties.botsScale), null);
		this.gameManager = DicePalaceFlyingMemoryLevelGameManager.Instance;
		this.targetPoint = this.gameManager.contactPoints[1, 1];
		this.movementString = properties.movementString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.directionString = properties.directionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.movementIndex = Random.Range(0, this.movementString.Length);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x000136D7 File Offset: 0x000118D7
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x000136F5 File Offset: 0x000118F5
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x00013713 File Offset: 0x00011913
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x0009FD18 File Offset: 0x0009DF18
	public void CalculateYTarget(bool followPlayer)
	{
		this.reachedEnd = false;
		if (followPlayer)
		{
			if (this.player.transform.position.y <= base.transform.position.y)
			{
				this.MoveUp();
			}
			else
			{
				this.MoveDown();
			}
		}
		else if (this.currentPoint.Ycoord <= (this.gameManager.contactDimY - 1) / 2)
		{
			this.MoveUp();
		}
		else
		{
			this.MoveDown();
		}
		if (this.reachedEnd)
		{
			this.targetPos.y = this.gameManager.contactPoints[this.currentPoint.Xcoord, this.setPosition].transform.position.y + this.offsetEnd;
		}
		else
		{
			this.targetPoint = this.gameManager.contactPoints[this.currentPoint.Xcoord, this.setPosition];
			this.targetPos = this.targetPoint.transform.position;
		}
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x0009FE3C File Offset: 0x0009E03C
	public void CalculateXTarget(bool followPlayer)
	{
		this.reachedEnd = false;
		if (followPlayer)
		{
			if (this.player.transform.position.x <= base.transform.position.x)
			{
				this.MoveRight();
			}
			else
			{
				this.MoveLeft();
			}
		}
		else if (this.currentPoint.Xcoord <= (this.gameManager.contactDimX - 1) / 2)
		{
			this.MoveRight();
		}
		else
		{
			this.MoveLeft();
		}
		if (this.reachedEnd)
		{
			this.targetPos.x = this.gameManager.contactPoints[this.setPosition, this.currentPoint.Ycoord].transform.position.x + this.offsetEnd;
		}
		else
		{
			this.targetPoint = this.gameManager.contactPoints[this.setPosition, this.currentPoint.Ycoord];
			this.targetPos = this.targetPoint.transform.position;
		}
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x0009FF60 File Offset: 0x0009E160
	public IEnumerator move_cr()
	{
		bool followPlayer = false;
		Vector3 pos = base.transform.position;
		for (;;)
		{
			Parser.IntTryParse(this.movementString[this.movementIndex], out this.movement);
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			this.GetMovement(followPlayer);
			if (this.moveOnY)
			{
				this.CalculateYTarget(followPlayer);
			}
			else
			{
				this.CalculateXTarget(followPlayer);
			}
			if (this.moveOnY)
			{
				while (base.transform.position.y != this.targetPos.y)
				{
					pos.y = Mathf.MoveTowards(base.transform.position.y, this.targetPos.y, this.properties.botsSpeed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
			}
			else
			{
				while (base.transform.position.x != this.targetPos.x)
				{
					pos.x = Mathf.MoveTowards(base.transform.position.x, this.targetPos.x, this.properties.botsSpeed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
			}
			if (this.reachedEnd)
			{
				this.OnDestroy();
			}
			else
			{
				this.currentPoint = this.targetPoint;
				this.moveOnY = !this.moveOnY;
				this.movementIndex = (this.movementIndex + 1) % this.movementString.Length;
				this.directionIndex = (this.directionIndex + 1) % this.directionString.Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x0009FF7C File Offset: 0x0009E17C
	public bool GetMovement(bool followPlayer)
	{
		if (this.directionString[this.directionIndex][0] == 'N')
		{
			followPlayer = false;
		}
		else if (this.directionString[this.directionIndex][0] == 'P')
		{
			followPlayer = true;
		}
		return followPlayer;
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x0009FFCC File Offset: 0x0009E1CC
	public void MoveUp()
	{
		int num = this.currentPoint.Ycoord + this.movement;
		if (num > this.gameManager.contactDimY - 1)
		{
			this.setPosition = this.gameManager.contactDimY - 1;
			this.reachedEnd = true;
			this.offsetEnd = -200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000A0038 File Offset: 0x0009E238
	public void MoveDown()
	{
		int num = this.currentPoint.Ycoord - this.movement;
		if (num < 0)
		{
			this.setPosition = 0;
			this.reachedEnd = true;
			this.offsetEnd = 200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x000A008C File Offset: 0x0009E28C
	public void MoveLeft()
	{
		int num = this.currentPoint.Xcoord - this.movement;
		if (num < 0)
		{
			this.setPosition = 0;
			this.reachedEnd = true;
			this.offsetEnd = -200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x000A00E0 File Offset: 0x0009E2E0
	public void MoveRight()
	{
		int num = this.currentPoint.Xcoord + this.movement;
		if (num > this.gameManager.contactDimX - 1)
		{
			this.setPosition = this.gameManager.contactDimX - 1;
			this.reachedEnd = true;
			this.offsetEnd = 200f;
		}
		else
		{
			this.setPosition = num;
			this.reachedEnd = false;
		}
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x000A014C File Offset: 0x0009E34C
	public IEnumerator shoot_bullets_cr()
	{
		for (;;)
		{
			base.animator.Play("Bot_Warning");
			yield return CupheadTime.WaitForSeconds(this, this.properties.bulletWarningDuration);
			this.FireProjectile();
			this.player = PlayerManager.GetNext();
			base.animator.Play("Off");
			yield return null;
			yield return CupheadTime.WaitForSeconds(this, this.properties.bulletDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x000A0168 File Offset: 0x0009E368
	public void FireProjectile()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 vector = this.player.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(vector);
		this.projectile.Create(base.transform.position, rotation, this.properties.bulletSpeed);
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x0001373E File Offset: 0x0001193E
	public override void Die()
	{
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.OnDestroy();
		base.Die();
	}

	// Token: 0x04001284 RID: 4740
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04001285 RID: 4741
	public DicePalaceFlyingMemoryLevelGameManager gameManager;

	// Token: 0x04001286 RID: 4742
	public LevelProperties.DicePalaceFlyingMemory.Bots properties;

	// Token: 0x04001287 RID: 4743
	public DicePalaceFlyingMemoryLevelContactPoint currentPoint;

	// Token: 0x04001288 RID: 4744
	public DicePalaceFlyingMemoryLevelContactPoint targetPoint;

	// Token: 0x04001289 RID: 4745
	public AbstractPlayerController player;

	// Token: 0x0400128A RID: 4746
	public DamageReceiver damageReceiver;

	// Token: 0x0400128B RID: 4747
	public bool moveOnY;

	// Token: 0x0400128C RID: 4748
	public bool reachedEnd;

	// Token: 0x0400128D RID: 4749
	public float health;

	// Token: 0x0400128E RID: 4750
	public int movement;

	// Token: 0x0400128F RID: 4751
	public int movementIndex;

	// Token: 0x04001290 RID: 4752
	public int directionIndex;

	// Token: 0x04001291 RID: 4753
	public int setPosition;

	// Token: 0x04001292 RID: 4754
	public float offsetEnd;

	// Token: 0x04001293 RID: 4755
	public Vector3 targetPos;

	// Token: 0x04001294 RID: 4756
	public string[] movementString;

	// Token: 0x04001295 RID: 4757
	public string[] directionString;
}
