using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace VRCSDK2
{
	// Token: 0x02000C9F RID: 3231
	public class RuntimeWorldCreation : RuntimeAPICreation
	{
		// Token: 0x0400495E RID: 18782
		public GameObject waitingPanel;

		// Token: 0x0400495F RID: 18783
		public GameObject blueprintPanel;

		// Token: 0x04004960 RID: 18784
		public GameObject errorPanel;

		// Token: 0x04004961 RID: 18785
		public Text titleText;

		// Token: 0x04004962 RID: 18786
		public InputField blueprintName;

		// Token: 0x04004963 RID: 18787
		public InputField blueprintDescription;

		// Token: 0x04004964 RID: 18788
		public InputField worldCapacity;

		// Token: 0x04004965 RID: 18789
		public RawImage bpImage;

		// Token: 0x04004966 RID: 18790
		public Image liveBpImage;

		// Token: 0x04004967 RID: 18791
		public Toggle shouldUpdateImageToggle;

		// Token: 0x04004968 RID: 18792
		public Toggle releasePublic;

		// Token: 0x04004969 RID: 18793
		public Toggle contentNsfw;

		// Token: 0x0400496A RID: 18794
		public Toggle contentSex;

		// Token: 0x0400496B RID: 18795
		public Toggle contentViolence;

		// Token: 0x0400496C RID: 18796
		public Toggle contentGore;

		// Token: 0x0400496D RID: 18797
		public Toggle contentOther;

		// Token: 0x0400496E RID: 18798
		public Toggle contentFeatured;

		// Token: 0x0400496F RID: 18799
		public Toggle contentSDKExample;

		// Token: 0x04004970 RID: 18800
		public Image showInWorldsMenuGroup;

		// Token: 0x04004971 RID: 18801
		public Toggle showInActiveWorlds;

		// Token: 0x04004972 RID: 18802
		public Toggle showInPopularWorlds;

		// Token: 0x04004973 RID: 18803
		public Toggle showInNewWorlds;

		// Token: 0x04004974 RID: 18804
		public Button uploadButton;

		// Token: 0x04004975 RID: 18805
		private ApiWorld worldRecord;
	}
}
