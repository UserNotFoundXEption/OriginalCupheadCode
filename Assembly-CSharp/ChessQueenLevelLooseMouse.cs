using System;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class ChessQueenLevelLooseMouse : MonoBehaviour
{
	// Token: 0x06001309 RID: 4873 RVA: 0x0001003D File Offset: 0x0000E23D
	public void Start()
	{
		this.jumpSwitchTime = Random.Range(2f, 3f);
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00096A50 File Offset: 0x00094C50
	public void Update()
	{
		this.anim.SetBool("Right", this.queen.transform.position.x > -200f);
		if (!this.won && this.activeCannonball == null)
		{
			this.jumpSwitchTime -= CupheadTime.Delta;
			if (this.jumpSwitchTime < 0f)
			{
				this.anim.SetBool("Jump", !this.anim.GetBool("Jump"));
				this.jumpSwitchTime = ((!this.anim.GetBool("Jump")) ? Random.Range(3f, 4f) : Random.Range(0.5f, 2f));
			}
		}
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x00010054 File Offset: 0x0000E254
	public void HitQueen()
	{
		this.anim.SetTrigger("HitQueen");
		this.anim.SetBool("Jump", true);
		this.jumpSwitchTime = Random.Range(2f, 3f);
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x0001008C File Offset: 0x0000E28C
	public void CannonFired(GameObject cannonBall)
	{
		this.anim.SetBool("Jump", false);
		this.activeCannonball = cannonBall;
		this.jumpSwitchTime = Random.Range(3f, 4f);
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x000100BB File Offset: 0x0000E2BB
	public void Win()
	{
		this.anim.SetBool("Jump", true);
		this.won = true;
	}

	// Token: 0x04000F5F RID: 3935
	[SerializeField]
	public Animator anim;

	// Token: 0x04000F60 RID: 3936
	[SerializeField]
	public ChessQueenLevelQueen queen;

	// Token: 0x04000F61 RID: 3937
	public bool won;

	// Token: 0x04000F62 RID: 3938
	public float jumpSwitchTime;

	// Token: 0x04000F63 RID: 3939
	public GameObject activeCannonball;
}
