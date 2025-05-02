using System;
using System.Collections.Generic;

namespace DialoguerCore
{
	// Token: 0x020005EE RID: 1518
	public abstract class AbstractDialoguePhase
	{
		// Token: 0x06003E95 RID: 16021 RVA: 0x0011CD34 File Offset: 0x0011AF34
		public AbstractDialoguePhase(List<int> outs)
		{
			if (outs != null)
			{
				int[] array = outs.ToArray();
				this.outs = (array.Clone() as int[]);
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06003E96 RID: 16022 RVA: 0x00032529 File Offset: 0x00030729
		// (set) Token: 0x06003E97 RID: 16023 RVA: 0x0011CD68 File Offset: 0x0011AF68
		public PhaseState state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				switch (this._state)
				{
				case PhaseState.Start:
					this.onStart();
					break;
				case PhaseState.Action:
					this.onAction();
					break;
				case PhaseState.Complete:
					this.onComplete();
					break;
				}
			}
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x00032531 File Offset: 0x00030731
		public void Start(DialoguerVariables localVars)
		{
			this.Reset();
			this._localVariables = localVars;
			this.state = PhaseState.Start;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0011CDC4 File Offset: 0x0011AFC4
		public virtual void Continue(int outId)
		{
			int num = 0;
			if (this.outs != null && this.outs[outId] >= 0)
			{
				num = this.outs[outId];
			}
			this.nextPhaseId = num;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00032547 File Offset: 0x00030747
		public virtual void onStart()
		{
			this.state = PhaseState.Action;
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x00032550 File Offset: 0x00030750
		public virtual void onAction()
		{
			this.state = PhaseState.Complete;
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x00032559 File Offset: 0x00030759
		public virtual void onComplete()
		{
			this.dispatchPhaseComplete(this.nextPhaseId);
			this.state = PhaseState.Inactive;
			this.Reset();
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x00032574 File Offset: 0x00030774
		public virtual void Reset()
		{
			this.nextPhaseId = ((this.outs == null || this.outs[0] < 0) ? 0 : this.outs[0]);
			this._localVariables = null;
		}

		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06003E9E RID: 16030 RVA: 0x0011CE04 File Offset: 0x0011B004
		// (remove) Token: 0x06003E9F RID: 16031 RVA: 0x0011CE3C File Offset: 0x0011B03C
		public event AbstractDialoguePhase.PhaseCompleteHandler onPhaseComplete;

		// Token: 0x06003EA0 RID: 16032 RVA: 0x000325AA File Offset: 0x000307AA
		public void dispatchPhaseComplete(int nextPhaseId)
		{
			if (this.onPhaseComplete != null)
			{
				this.onPhaseComplete(nextPhaseId);
			}
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x000325C3 File Offset: 0x000307C3
		public void resetEvents()
		{
			this.onPhaseComplete = null;
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x000325CC File Offset: 0x000307CC
		public override string ToString()
		{
			return "AbstractDialoguePhase";
		}

		// Token: 0x0400326F RID: 12911
		public readonly int[] outs;

		// Token: 0x04003270 RID: 12912
		public int nextPhaseId;

		// Token: 0x04003271 RID: 12913
		public DialoguerVariables _localVariables;

		// Token: 0x04003272 RID: 12914
		public PhaseState _state;

		// Token: 0x02001260 RID: 4704
		// (Invoke) Token: 0x06008188 RID: 33160
		public delegate void PhaseCompleteHandler(int nextPhaseId);
	}
}
