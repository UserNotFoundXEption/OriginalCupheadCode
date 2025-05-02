using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class ScrollingGameObject : AbstractMonoBehaviour
{
	// Token: 0x06000A46 RID: 2630 RVA: 0x0007B4D0 File Offset: 0x000796D0
	public override void Awake()
	{
		base.Awake();
		GameObject gameObject = new GameObject("Container");
		gameObject.transform.SetParent(base.transform);
		if (this.resetTransforms)
		{
			base.transform.ResetLocalTransforms();
		}
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.SetParent(gameObject.transform);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject.gameObject);
		gameObject2.transform.SetParent(base.transform);
		gameObject2.transform.ResetLocalTransforms();
		gameObject2.transform.SetLocalPosition(new float?((float)this.size), new float?(0f), new float?(0f));
		GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2);
		gameObject3.transform.SetParent(base.transform);
		gameObject3.transform.SetLocalPosition(new float?((float)(-(float)this.size)), new float?(0f), new float?(0f));
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0007B614 File Offset: 0x00079814
	public void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (localPosition.x <= (float)(-(float)this.size))
		{
			localPosition.x += (float)this.size;
		}
		if (localPosition.x >= 1280f)
		{
			localPosition.x -= (float)this.size;
		}
		localPosition.x -= (float)((!this.negativeDirection) ? 1 : -1) * this.speed * CupheadTime.Delta;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x04000838 RID: 2104
	public ScrollingGameObject.Axis axis;

	// Token: 0x04000839 RID: 2105
	[SerializeField]
	public bool negativeDirection;

	// Token: 0x0400083A RID: 2106
	[Range(0f, 500f)]
	[SerializeField]
	public float speed;

	// Token: 0x0400083B RID: 2107
	[SerializeField]
	public int size = 1280;

	// Token: 0x0400083C RID: 2108
	[SerializeField]
	public bool resetTransforms = true;

	// Token: 0x0200094E RID: 2382
	public enum Axis
	{
		// Token: 0x04004602 RID: 17922
		X,
		// Token: 0x04004603 RID: 17923
		Y
	}
}
