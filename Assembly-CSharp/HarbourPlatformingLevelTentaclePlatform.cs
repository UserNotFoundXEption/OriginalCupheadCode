using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class HarbourPlatformingLevelTentaclePlatform : LevelPlatform
{
	// Token: 0x06002E75 RID: 11893 RVA: 0x00026BCC File Offset: 0x00024DCC
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		if (!this.startedSinking)
		{
			base.StartCoroutine(this.sink_cr());
		}
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x000DFA80 File Offset: 0x000DDC80
	public IEnumerator sink_cr()
	{
		this.startedSinking = true;
		float t = 0f;
		Vector2 start = this.drag.transform.position;
		Vector2 end = new Vector2(this.drag.transform.position.x, this.drag.transform.position.y - 500f);
		while (t < this.timeToSink)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / this.timeToSink);
			this.drag.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		Object.Destroy(this.drag.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x04002692 RID: 9874
	[SerializeField]
	public Transform drag;

	// Token: 0x04002693 RID: 9875
	public float timeToSink = 5f;

	// Token: 0x04002694 RID: 9876
	public bool startedSinking;
}
