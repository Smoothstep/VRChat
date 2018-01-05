using System;
using UIWidgets;
using UnityEngine;

// Token: 0x0200091D RID: 2333
[RequireComponent(typeof(Progressbar))]
public class SampleProgressbar : MonoBehaviour
{
	// Token: 0x06004606 RID: 17926 RVA: 0x0017D04C File Offset: 0x0017B44C
	private void Start()
	{
		Progressbar component = base.GetComponent<Progressbar>();
		component.TextFunc = ((Progressbar x) => string.Format("Exp to next level: {0} / {1}", x.Value, x.Max));
	}
}
