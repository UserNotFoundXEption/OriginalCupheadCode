using System;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
[Serializable]
public class Effect : AbstractCollidableObject
{
	// Token: 0x06003CED RID: 15597 RVA: 0x000312DF File Offset: 0x0002F4DF
	public virtual Effect Create(Vector3 position)
	{
		return this.Create(position, Vector3.one);
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x00117468 File Offset: 0x00115668
	public virtual Effect Create(Vector3 position, Vector3 scale)
	{
		Effect component = Object.Instantiate<GameObject>(base.gameObject).GetComponent<Effect>();
		component.name = component.name.Replace("(Clone)", string.Empty);
		if (this.randomMirrorX)
		{
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
		}
		if (this.randomMirrorY)
		{
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
		}
		component.Initialize(position, scale, this.randomRotation);
		return component;
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x00117510 File Offset: 0x00115710
	public virtual void Initialize(Vector3 position)
	{
		Vector3 scale;
		scale..ctor(1f, 1f);
		if (this.randomMirrorX)
		{
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
		}
		if (this.randomMirrorY)
		{
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
		}
		this.Initialize(position, scale, this.randomRotation);
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x0011759C File Offset: 0x0011579C
	public virtual void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		int num = Random.Range(0, base.animator.GetInteger("Count"));
		base.animator.SetInteger("Effect", num);
		Transform transform = base.transform;
		transform.position = position;
		transform.localScale = scale;
		if (randomR)
		{
			transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
		}
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x000312ED File Offset: 0x0002F4ED
	public virtual void OnEffectCompletePool()
	{
		if (this.removeOnEnd)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			this.inUse = false;
		}
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x00031311 File Offset: 0x0002F511
	public virtual void OnEffectComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x0003131E File Offset: 0x0002F51E
	public void Play()
	{
		base.animator.Play("A");
	}

	// Token: 0x0400306D RID: 12397
	[SerializeField]
	public bool randomRotation;

	// Token: 0x0400306E RID: 12398
	[Space(10f)]
	[SerializeField]
	public bool randomMirrorX;

	// Token: 0x0400306F RID: 12399
	[SerializeField]
	public bool randomMirrorY;

	// Token: 0x04003070 RID: 12400
	public bool inUse;

	// Token: 0x04003071 RID: 12401
	public bool removeOnEnd;
}
