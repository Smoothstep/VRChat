using System;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BB1 RID: 2993
	public class ItemPackage : MonoBehaviour
	{
		// Token: 0x04004241 RID: 16961
		public new string name;

		// Token: 0x04004242 RID: 16962
		public ItemPackage.ItemPackageType packageType;

		// Token: 0x04004243 RID: 16963
		public GameObject itemPrefab;

		// Token: 0x04004244 RID: 16964
		public GameObject otherHandItemPrefab;

		// Token: 0x04004245 RID: 16965
		public GameObject previewPrefab;

		// Token: 0x04004246 RID: 16966
		public GameObject fadedPreviewPrefab;

		// Token: 0x02000BB2 RID: 2994
		public enum ItemPackageType
		{
			// Token: 0x04004248 RID: 16968
			Unrestricted,
			// Token: 0x04004249 RID: 16969
			OneHanded,
			// Token: 0x0400424A RID: 16970
			TwoHanded
		}
	}
}
