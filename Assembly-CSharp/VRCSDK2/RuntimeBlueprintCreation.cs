using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace VRCSDK2
{
	// Token: 0x02000C9E RID: 3230
	public class RuntimeBlueprintCreation : RuntimeAPICreation
	{
		// Token: 0x0400494E RID: 18766
		public GameObject waitingPanel;

		// Token: 0x0400494F RID: 18767
		public GameObject blueprintPanel;

		// Token: 0x04004950 RID: 18768
		public GameObject errorPanel;

		// Token: 0x04004951 RID: 18769
		public Text titleText;

		// Token: 0x04004952 RID: 18770
		public InputField blueprintName;

		// Token: 0x04004953 RID: 18771
		public InputField blueprintDescription;

		// Token: 0x04004954 RID: 18772
		public RawImage bpImage;

		// Token: 0x04004955 RID: 18773
		public Image liveBpImage;

		// Token: 0x04004956 RID: 18774
		public Toggle shouldUpdateImageToggle;

		// Token: 0x04004957 RID: 18775
		public Toggle contentSex;

		// Token: 0x04004958 RID: 18776
		public Toggle contentViolence;

		// Token: 0x04004959 RID: 18777
		public Toggle contentGore;

		// Token: 0x0400495A RID: 18778
		public Toggle contentOther;

		// Token: 0x0400495B RID: 18779
		public Toggle developerAvatar;

		// Token: 0x0400495C RID: 18780
		public Button uploadButton;

		// Token: 0x0400495D RID: 18781
		private ApiAvatar apiAvatar;
	}
}
