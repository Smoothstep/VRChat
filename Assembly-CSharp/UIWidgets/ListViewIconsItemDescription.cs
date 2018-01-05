using System;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200094A RID: 2378
	[Serializable]
	public class ListViewIconsItemDescription
	{
		// Token: 0x06004818 RID: 18456 RVA: 0x001835D2 File Offset: 0x001819D2
		public override int GetHashCode()
		{
			return this.Icon.GetHashCode() ^ this.Name.GetHashCode();
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x001835EC File Offset: 0x001819EC
		public override bool Equals(object obj)
		{
			ListViewIconsItemDescription listViewIconsItemDescription = obj as ListViewIconsItemDescription;
			return listViewIconsItemDescription != null && (!(listViewIconsItemDescription.Icon == null) || !(this.Icon != null)) && (!(listViewIconsItemDescription.Icon != null) || !(this.Icon == null)) && this.Name == listViewIconsItemDescription.Name && ((this.Icon == null && listViewIconsItemDescription.Icon == null) || this.Icon.Equals(listViewIconsItemDescription.Icon));
		}

		// Token: 0x0400310A RID: 12554
		[SerializeField]
		public Sprite Icon;

		// Token: 0x0400310B RID: 12555
		[SerializeField]
		public string Name;
	}
}
