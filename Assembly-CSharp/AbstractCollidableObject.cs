using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class AbstractCollidableObject : AbstractPausableComponent
{
	// Token: 0x0600080A RID: 2058 RVA: 0x00007D0B File Offset: 0x00005F0B
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.UnregisterAllCollisionChildren();
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00007D19 File Offset: 0x00005F19
	public virtual void OnTriggerEnter2D(Collider2D col)
	{
		this.checkCollision(col, CollisionPhase.Enter);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00007D23 File Offset: 0x00005F23
	public virtual void OnCollisionEnter2D(Collision2D col)
	{
		this.checkCollision(col.collider, CollisionPhase.Enter);
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00007D32 File Offset: 0x00005F32
	public virtual void OnTriggerStay2D(Collider2D col)
	{
		this.checkCollision(col, CollisionPhase.Stay);
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00007D3C File Offset: 0x00005F3C
	public virtual void OnCollisionStay2D(Collision2D col)
	{
		this.checkCollision(col.collider, CollisionPhase.Stay);
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x00007D4B File Offset: 0x00005F4B
	public virtual void OnTriggerExit2D(Collider2D col)
	{
		this.checkCollision(col, CollisionPhase.Exit);
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00007D55 File Offset: 0x00005F55
	public virtual void OnCollisionExit2D(Collision2D col)
	{
		this.checkCollision(col.collider, CollisionPhase.Exit);
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x000756B4 File Offset: 0x000738B4
	public virtual void checkCollision(Collider2D col, CollisionPhase phase)
	{
		GameObject gameObject = col.gameObject;
		this.OnCollision(gameObject, phase);
		if (gameObject.CompareTag("Wall"))
		{
			this.OnCollisionWalls(gameObject, phase);
		}
		else if (gameObject.CompareTag("Ceiling"))
		{
			this.OnCollisionCeiling(gameObject, phase);
		}
		else if (gameObject.CompareTag("Ground"))
		{
			this.OnCollisionGround(gameObject, phase);
		}
		else if (gameObject.CompareTag("Enemy"))
		{
			if (this.allowCollisionEnemy)
			{
				this.OnCollisionEnemy(gameObject, phase);
			}
		}
		else if (gameObject.CompareTag("EnemyProjectile"))
		{
			this.OnCollisionEnemyProjectile(gameObject, phase);
		}
		else if (gameObject.CompareTag("Player"))
		{
			if (this.allowCollisionPlayer)
			{
				this.OnCollisionPlayer(gameObject, phase);
			}
		}
		else if (gameObject.CompareTag("PlayerProjectile"))
		{
			this.OnCollisionPlayerProjectile(gameObject, phase);
		}
		else
		{
			this.OnCollisionOther(gameObject, phase);
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000812 RID: 2066 RVA: 0x00007D64 File Offset: 0x00005F64
	public virtual bool allowCollisionPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x00007D67 File Offset: 0x00005F67
	public virtual bool allowCollisionEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00007D6A File Offset: 0x00005F6A
	public virtual void OnCollision(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00007D6C File Offset: 0x00005F6C
	public virtual void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00007D6E File Offset: 0x00005F6E
	public virtual void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00007D70 File Offset: 0x00005F70
	public virtual void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00007D72 File Offset: 0x00005F72
	public virtual void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00007D74 File Offset: 0x00005F74
	public virtual void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00007D76 File Offset: 0x00005F76
	public virtual void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00007D78 File Offset: 0x00005F78
	public virtual void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00007D7A File Offset: 0x00005F7A
	public virtual void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x000757BC File Offset: 0x000739BC
	public void RegisterCollisionChild(GameObject go)
	{
		CollisionChild component = go.GetComponent<CollisionChild>();
		if (component == null)
		{
			return;
		}
		this.RegisterCollisionChild(component);
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x000757E4 File Offset: 0x000739E4
	public void RegisterCollisionChild(CollisionChild s)
	{
		this.collisionChildren.Add(s);
		s.OnAnyCollision += this.OnCollision;
		s.OnWallCollision += this.OnCollisionWalls;
		s.OnGroundCollision += this.OnCollisionGround;
		s.OnCeilingCollision += this.OnCollisionCeiling;
		s.OnPlayerCollision += this.OnCollisionPlayer;
		s.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
		s.OnEnemyCollision += this.OnCollisionEnemy;
		s.OnEnemyProjectileCollision += this.OnCollisionEnemyProjectile;
		s.OnOtherCollision += this.OnCollisionOther;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x000758A8 File Offset: 0x00073AA8
	public void UnregisterCollisionChild(CollisionChild s)
	{
		if (this.collisionChildren.Contains(s))
		{
			s.OnAnyCollision -= this.OnCollision;
			s.OnWallCollision -= this.OnCollisionWalls;
			s.OnGroundCollision -= this.OnCollisionGround;
			s.OnCeilingCollision -= this.OnCollisionCeiling;
			s.OnPlayerCollision -= this.OnCollisionPlayer;
			s.OnPlayerProjectileCollision -= this.OnCollisionPlayerProjectile;
			s.OnEnemyCollision -= this.OnCollisionEnemy;
			s.OnEnemyProjectileCollision -= this.OnCollisionEnemyProjectile;
			s.OnOtherCollision -= this.OnCollisionOther;
			this.collisionChildren.Remove(s);
		}
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00075980 File Offset: 0x00073B80
	public void UnregisterAllCollisionChildren()
	{
		for (int i = this.collisionChildren.Count - 1; i >= 0; i--)
		{
			this.UnregisterCollisionChild(this.collisionChildren[i]);
		}
	}

	// Token: 0x04000637 RID: 1591
	public List<CollisionChild> collisionChildren = new List<CollisionChild>();
}
