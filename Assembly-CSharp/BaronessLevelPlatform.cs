using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class BaronessLevelPlatform : PlatformingLevelPlatformSag
{
	// Token: 0x06000FBD RID: 4029 RVA: 0x0008E1A8 File Offset: 0x0008C3A8
	public void getProperties(LevelProperties.Baroness.Platform properties)
	{
		this.properties = properties;
		Vector3 position = base.transform.position;
		position.y = properties.YPosition;
		base.transform.position = position;
		this.maxCounter = Random.Range(4, 9);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x0008E1FC File Offset: 0x0008C3FC
	public IEnumerator move_cr()
	{
		bool movingLeft = true;
		for (;;)
		{
			Vector3 pos = base.transform.position;
			if (movingLeft)
			{
				if (this.castle.state == BaronessLevelCastle.State.Chase)
				{
					base.animator.Play("Fast");
				}
				pos.x = Mathf.MoveTowards(base.transform.position.x, -640f + this.properties.LeftBoundaryOffset, this.speed * CupheadTime.Delta);
				movingLeft = (base.transform.position.x != -640f + this.properties.LeftBoundaryOffset);
			}
			else
			{
				if (this.castle.state == BaronessLevelCastle.State.Chase)
				{
					base.animator.Play("Slow");
				}
				pos.x = Mathf.MoveTowards(base.transform.position.x, (float)Level.Current.Right - this.properties.RightBoundaryOffset, this.speed * CupheadTime.Delta);
				movingLeft = (base.transform.position.x == (float)Level.Current.Right - this.properties.RightBoundaryOffset);
			}
			base.transform.position = pos;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x0008E218 File Offset: 0x0008C418
	public void SweatCounter()
	{
		if (this.counter < this.maxCounter)
		{
			this.counter++;
		}
		else
		{
			base.animator.Play("Sweat");
			this.counter = 0;
			this.maxCounter = Random.Range(4, 9);
		}
	}

	// Token: 0x04000CD0 RID: 3280
	[SerializeField]
	public BaronessLevelCastle castle;

	// Token: 0x04000CD1 RID: 3281
	public float speed = 200f;

	// Token: 0x04000CD2 RID: 3282
	public int counter;

	// Token: 0x04000CD3 RID: 3283
	public int maxCounter;

	// Token: 0x04000CD4 RID: 3284
	public LevelProperties.Baroness.Platform properties;
}
