using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x02000BF0 RID: 3056
[RequireComponent(typeof(AudioListener))]
public class SteamVR_Ears : MonoBehaviour
{
	// Token: 0x06005ED8 RID: 24280 RVA: 0x002132D0 File Offset: 0x002116D0
	private void OnNewPosesApplied()
	{
		Transform origin = this.vrcam.origin;
		Quaternion lhs = (!(origin != null)) ? Quaternion.identity : origin.rotation;
		base.transform.rotation = lhs * this.offset;
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x00213320 File Offset: 0x00211720
	private void OnEnable()
	{
		this.usingSpeakers = false;
		CVRSettings settings = OpenVR.Settings;
		if (settings != null)
		{
			EVRSettingsError evrsettingsError = EVRSettingsError.None;
			if (settings.GetBool("steamvr", "usingSpeakers", ref evrsettingsError))
			{
				this.usingSpeakers = true;
				float @float = settings.GetFloat("steamvr", "speakersForwardYawOffsetDegrees", ref evrsettingsError);
				this.offset = Quaternion.Euler(0f, @float, 0f);
			}
		}
		if (this.usingSpeakers)
		{
			SteamVR_Events.NewPosesApplied.Listen(new UnityAction(this.OnNewPosesApplied));
		}
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x002133AA File Offset: 0x002117AA
	private void OnDisable()
	{
		if (this.usingSpeakers)
		{
			SteamVR_Events.NewPosesApplied.Remove(new UnityAction(this.OnNewPosesApplied));
		}
	}

	// Token: 0x04004478 RID: 17528
	public SteamVR_Camera vrcam;

	// Token: 0x04004479 RID: 17529
	private bool usingSpeakers;

	// Token: 0x0400447A RID: 17530
	private Quaternion offset;
}
