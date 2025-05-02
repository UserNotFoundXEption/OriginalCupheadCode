using System;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class PirateLevelBoatContainer : AbstractPausableComponent
{
	// Token: 0x0600218B RID: 8587 RVA: 0x0001CA09 File Offset: 0x0001AC09
	public override void Awake()
	{
		base.Awake();
		this.startPos = base.transform.position;
		this.endPos = this.startPos + new Vector3(0f, this.targetY, 0f);
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000BAC58 File Offset: 0x000B8E58
	public void Update()
	{
		if (PauseManager.state == PauseManager.State.Paused || this.state == PirateLevelBoatContainer.State.Static)
		{
			return;
		}
		float num = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, Mathf.PingPong(this.time, 1f));
		base.transform.position = Vector3.Lerp(this.startPos, this.endPos, num);
		this.time += Time.deltaTime / 2f;
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x0001CA48 File Offset: 0x0001AC48
	public void EndBobbing()
	{
		base.transform.position = this.startPos;
		this.state = PirateLevelBoatContainer.State.Static;
	}

	// Token: 0x04001BA4 RID: 7076
	[SerializeField]
	public float targetY;

	// Token: 0x04001BA5 RID: 7077
	public PirateLevelBoatContainer.State state;

	// Token: 0x04001BA6 RID: 7078
	public Vector3 startPos;

	// Token: 0x04001BA7 RID: 7079
	public Vector3 endPos;

	// Token: 0x04001BA8 RID: 7080
	public float time;

	// Token: 0x02000E0D RID: 3597
	public enum State
	{
		// Token: 0x040065CA RID: 26058
		Bobbing,
		// Token: 0x040065CB RID: 26059
		ToStatic,
		// Token: 0x040065CC RID: 26060
		Static
	}
}
