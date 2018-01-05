using System;
using UnityEngine;

namespace Klak.System
{
	// Token: 0x0200052E RID: 1326
	[AddComponentMenu("Klak/System/Global Config")]
	public class GlobalConfig : MonoBehaviour
	{
		// Token: 0x06002E8A RID: 11914 RVA: 0x000E30A4 File Offset: 0x000E14A4
		private void Start()
		{
			if (this._hideCursor && !Application.isEditor)
			{
				Cursor.visible = false;
			}
		}

		// Token: 0x040018AB RID: 6315
		[SerializeField]
		private bool _hideCursor;
	}
}
