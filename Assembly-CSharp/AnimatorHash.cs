using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public static class AnimatorHash
{
	// Token: 0x0200086B RID: 2155
	public static class level_oldman_man
	{
		// Token: 0x02001598 RID: 5528
		public static class Layer
		{
			// Token: 0x04009010 RID: 36880
			public static readonly int BaseLayer;

			// Token: 0x04009011 RID: 36881
			public static readonly int Beard = 1;

			// Token: 0x04009012 RID: 36882
			public static readonly int Eyes = 2;

			// Token: 0x04009013 RID: 36883
			public static readonly int GnomeA = 3;

			// Token: 0x04009014 RID: 36884
			public static readonly int Cauldron = 4;

			// Token: 0x04009015 RID: 36885
			public static readonly int GnomeB = 5;
		}

		// Token: 0x02001599 RID: 5529
		public static class ShortHash
		{
			// Token: 0x04009016 RID: 36886
			public static readonly int Idle_Part_1 = Animator.StringToHash("Idle_Part_1");

			// Token: 0x04009017 RID: 36887
			public static readonly int Idle_Part_2 = Animator.StringToHash("Idle_Part_2");

			// Token: 0x04009018 RID: 36888
			public static readonly int Spit_Transition_A = Animator.StringToHash("Spit_Transition_A");

			// Token: 0x04009019 RID: 36889
			public static readonly int Spit_Loop = Animator.StringToHash("Spit_Loop");

			// Token: 0x0400901A RID: 36890
			public static readonly int Spit_Transition_B = Animator.StringToHash("Spit_Transition_B");

			// Token: 0x0400901B RID: 36891
			public static readonly int Spit_Intro_Continued = Animator.StringToHash("Spit_Intro_Continued");

			// Token: 0x0400901C RID: 36892
			public static readonly int Spit_Outro = Animator.StringToHash("Spit_Outro");

			// Token: 0x0400901D RID: 36893
			public static readonly int Phase_Trans = Animator.StringToHash("Phase_Trans");

			// Token: 0x0400901E RID: 36894
			public static readonly int Phase_Trans_Cont = Animator.StringToHash("Phase_Trans_Cont");

			// Token: 0x0400901F RID: 36895
			public static readonly int Beard_Boil = Animator.StringToHash("Beard_Boil");

			// Token: 0x04009020 RID: 36896
			public static readonly int Blank = Animator.StringToHash("Blank");

			// Token: 0x04009021 RID: 36897
			public static readonly int Loop = Animator.StringToHash("Loop");
		}

		// Token: 0x0200159A RID: 5530
		public static class FullHash
		{
			// Token: 0x04009022 RID: 36898
			public static readonly int BaseLayer_Idle_Part_1 = Animator.StringToHash("Base Layer.Idle_Part_1");

			// Token: 0x04009023 RID: 36899
			public static readonly int BaseLayer_Idle_Part_2 = Animator.StringToHash("Base Layer.Idle_Part_2");

			// Token: 0x04009024 RID: 36900
			public static readonly int BaseLayer_Spit_Transition_A = Animator.StringToHash("Base Layer.Spit_Transition_A");

			// Token: 0x04009025 RID: 36901
			public static readonly int BaseLayer_Spit_Loop = Animator.StringToHash("Base Layer.Spit_Loop");

			// Token: 0x04009026 RID: 36902
			public static readonly int BaseLayer_Spit_Transition_B = Animator.StringToHash("Base Layer.Spit_Transition_B");

			// Token: 0x04009027 RID: 36903
			public static readonly int BaseLayer_Spit_Intro_Continued = Animator.StringToHash("Base Layer.Spit_Intro_Continued");

			// Token: 0x04009028 RID: 36904
			public static readonly int BaseLayer_Spit_Outro = Animator.StringToHash("Base Layer.Spit_Outro");

			// Token: 0x04009029 RID: 36905
			public static readonly int BaseLayer_Phase_Trans = Animator.StringToHash("Base Layer.Phase_Trans");

			// Token: 0x0400902A RID: 36906
			public static readonly int BaseLayer_Phase_Trans_Cont = Animator.StringToHash("Base Layer.Phase_Trans_Cont");

			// Token: 0x0400902B RID: 36907
			public static readonly int Beard_Beard_Boil = Animator.StringToHash("Beard.Beard_Boil");

			// Token: 0x0400902C RID: 36908
			public static readonly int Eyes_Blank = Animator.StringToHash("Eyes.Blank");

			// Token: 0x0400902D RID: 36909
			public static readonly int Eyes_Spit_Loop = Animator.StringToHash("Eyes.Spit_Loop");

			// Token: 0x0400902E RID: 36910
			public static readonly int GnomeA_Loop = Animator.StringToHash("GnomeA.Loop");

			// Token: 0x0400902F RID: 36911
			public static readonly int Cauldron_Loop = Animator.StringToHash("Cauldron.Loop");

			// Token: 0x04009030 RID: 36912
			public static readonly int GnomeB_Loop = Animator.StringToHash("GnomeB.Loop");
		}

		// Token: 0x0200159B RID: 5531
		public static class Parameter
		{
			// Token: 0x04009031 RID: 36913
			public static readonly int IsSpitAttack = Animator.StringToHash("IsSpitAttack");

			// Token: 0x04009032 RID: 36914
			public static readonly int IsSpitAttackEyeLoop = Animator.StringToHash("IsSpitAttackEyeLoop");

			// Token: 0x04009033 RID: 36915
			public static readonly int Phase2 = Animator.StringToHash("Phase2");
		}
	}
}
