using System;
using DialoguerCore;
using DialoguerEditor;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
public class WaitPhaseComponent : MonoBehaviour
{
	// Token: 0x06003E83 RID: 16003 RVA: 0x000323D4 File Offset: 0x000305D4
	public void Init(WaitPhase phase, DialogueEditorWaitTypes type, float duration)
	{
		this.phase = phase;
		this.type = type;
		this.duration = duration;
		this.elapsed = 0f;
		this.go = true;
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0011C77C File Offset: 0x0011A97C
	public void Update()
	{
		if (!this.go)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		DialogueEditorWaitTypes dialogueEditorWaitTypes = this.type;
		if (dialogueEditorWaitTypes != DialogueEditorWaitTypes.Seconds)
		{
			if (dialogueEditorWaitTypes == DialogueEditorWaitTypes.Frames)
			{
				this.elapsed += 1f;
				if (this.elapsed >= this.duration)
				{
					this.waitComplete();
				}
			}
		}
		else
		{
			this.elapsed += deltaTime;
			if (this.elapsed >= this.duration)
			{
				this.waitComplete();
			}
		}
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x000323FD File Offset: 0x000305FD
	public void waitComplete()
	{
		this.go = false;
		this.phase.waitComplete();
		this.phase = null;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400324D RID: 12877
	public DialogueEditorWaitTypes type;

	// Token: 0x0400324E RID: 12878
	public WaitPhase phase;

	// Token: 0x0400324F RID: 12879
	public bool go;

	// Token: 0x04003250 RID: 12880
	public float duration;

	// Token: 0x04003251 RID: 12881
	public float elapsed;
}
