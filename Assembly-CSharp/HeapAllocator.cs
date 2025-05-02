using System;

// Token: 0x020005B8 RID: 1464
public static class HeapAllocator
{
	// Token: 0x06003D71 RID: 15729 RVA: 0x00118F60 File Offset: 0x00117160
	public static void Allocate(int iterations)
	{
		object[] array = new object[iterations];
		for (int i = 0; i < iterations; i++)
		{
			object[] array2 = new object[HeapAllocator.AllocationsPerIteration];
			for (int j = 0; j < HeapAllocator.AllocationsPerIteration; j++)
			{
				array2[j] = new byte[HeapAllocator.BytesPerAllocation];
			}
			array[i] = array2;
		}
	}

	// Token: 0x040030E5 RID: 12517
	public static readonly int BytesPerAllocation = 1024;

	// Token: 0x040030E6 RID: 12518
	public static readonly int AllocationsPerIteration = 1024;
}
