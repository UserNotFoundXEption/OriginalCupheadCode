using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class BatLevelPentagram : AbstractCollidableObject
{
	// Token: 0x060010DB RID: 4315 RVA: 0x00091294 File Offset: 0x0008F494
	public void Init(Vector2 pos, LevelProperties.Bat.Pentagrams properties, AbstractPlayerController player, bool onRight)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.player = player;
		this.onRight = onRight;
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.move_cr());
		base.transform.SetScale(new float?(properties.pentagramSize), new float?(properties.pentagramSize), new float?(1f));
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x0009130C File Offset: 0x0008F50C
	public IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		float endPos = 560f;
		if (this.onRight)
		{
			while (base.transform.position.x > this.player.transform.position.x)
			{
				pos.x = Mathf.MoveTowards(base.transform.position.x, this.player.transform.position.x, this.properties.xSpeed * CupheadTime.Delta);
				base.transform.position = pos;
				yield return null;
			}
		}
		else
		{
			while (base.transform.position.x < this.player.transform.position.x)
			{
				pos.x = Mathf.MoveTowards(base.transform.position.x, this.player.transform.position.x, this.properties.xSpeed * CupheadTime.Delta);
				base.transform.position = pos;
				yield return null;
			}
		}
		base.GetComponent<Collider2D>().enabled = true;
		while (base.transform.position.y < endPos)
		{
			pos.y = Mathf.MoveTowards(base.transform.position.y, endPos, this.properties.ySpeed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x0000E36D File Offset: 0x0000C56D
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000DB6 RID: 3510
	public LevelProperties.Bat.Pentagrams properties;

	// Token: 0x04000DB7 RID: 3511
	public AbstractPlayerController player;

	// Token: 0x04000DB8 RID: 3512
	public bool onRight;
}
