using System;
using UnityEngine;

namespace UIWidgetsSamples
{
	// Token: 0x0200090F RID: 2319
	[Serializable]
	public class ListViewCustomSampleItemDescription
	{
		// Token: 0x060045D1 RID: 17873 RVA: 0x0017A0A3 File Offset: 0x001784A3
		public override int GetHashCode()
		{
			return this.Icon.GetHashCode() ^ this.Name.GetHashCode() ^ this.Progress;
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x0017A0C4 File Offset: 0x001784C4
		public override bool Equals(object obj)
		{
			ListViewCustomSampleItemDescription listViewCustomSampleItemDescription = obj as ListViewCustomSampleItemDescription;
			return listViewCustomSampleItemDescription != null && (!(listViewCustomSampleItemDescription.Icon == null) || !(this.Icon != null)) && (!(listViewCustomSampleItemDescription.Icon != null) || !(this.Icon == null)) && (this.Name == listViewCustomSampleItemDescription.Name && this.Progress == listViewCustomSampleItemDescription.Progress) && this.Icon.Equals(listViewCustomSampleItemDescription.Icon);
		}

		// Token: 0x04002FF0 RID: 12272
		[SerializeField]
		public Sprite Icon;

		// Token: 0x04002FF1 RID: 12273
		[SerializeField]
		public string Name;

		// Token: 0x04002FF2 RID: 12274
		[SerializeField]
		public int Progress;
	}
}
