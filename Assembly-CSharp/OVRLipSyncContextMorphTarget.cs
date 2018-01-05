using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006FB RID: 1787
public class OVRLipSyncContextMorphTarget : MonoBehaviour
{
	// Token: 0x06003A9E RID: 15006 RVA: 0x001279F8 File Offset: 0x00125DF8
	public void Initialize()
	{
		if (this.skinnedMeshRenderer == null)
		{
			Debug.Log("LipSyncContextMorphTarget.Start WARNING: Please set required public components!");
			return;
		}
		this.lipsyncContext = base.GetComponent<OVRLipSyncContext>();
		if (this.lipsyncContext == null)
		{
			Debug.Log("LipSyncContextMorphTarget.Start WARNING: No phoneme context component set to object");
		}
		this.sequencer = base.GetComponent<OVRLipSyncContextSequencer>();
		if (this.sequencer == null)
		{
		}
		this.lipsyncContext.SendSignal(OVRLipSync.ovrLipSyncSignals.VisemeSmoothing, this.SmoothAmount, 0);
	}

	// Token: 0x06003A9F RID: 15007 RVA: 0x00127A7C File Offset: 0x00125E7C
	private void Update()
	{
		if (this.lipsyncContext != null && this.skinnedMeshRenderer != null)
		{
			if (this.lipsyncContext.GetCurrentPhonemeFrame(ref this.frame) == 0)
			{
				this.SetVisemeToMorphTarget();
			}
			this.ControlSequencer();
			this.SendSignals();
		}
	}

	// Token: 0x06003AA0 RID: 15008 RVA: 0x00127AD4 File Offset: 0x00125ED4
	private void SendSignals()
	{
		if (!this.enableVisemeSignals)
		{
			return;
		}
		if (this.SendVisemeSignalsPlaySequence())
		{
			if (this.sequencer != null)
			{
				OVRLipSyncDebugConsole.Clear();
				OVRLipSyncDebugConsole.Log("Playing recorded sequence.");
				string message = string.Empty + this.sequencer.GetCurrentTimer();
				OVRLipSyncDebugConsole.Log(message);
			}
		}
		else
		{
			this.SendVisemeSignalsKeys();
		}
	}

	// Token: 0x06003AA1 RID: 15009 RVA: 0x00127B44 File Offset: 0x00125F44
	private void SetVisemeToMorphTarget()
	{
		for (int i = 0; i < this.VisemeToBlendTargets.Length; i++)
		{
			if (this.VisemeToBlendTargets[i] != -1)
			{
				this.skinnedMeshRenderer.SetBlendShapeWeight(this.VisemeToBlendTargets[i], this.frame.Visemes[i] * 100f);
			}
		}
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x00127BA0 File Offset: 0x00125FA0
	private void SendVisemeSignalsKeys()
	{
		this.SendVisemeSignal(KeyCode.Alpha1, 0, 100);
		this.SendVisemeSignal(KeyCode.Alpha2, 1, 100);
		this.SendVisemeSignal(KeyCode.Alpha3, 2, 100);
		this.SendVisemeSignal(KeyCode.Alpha4, 3, 100);
		this.SendVisemeSignal(KeyCode.Alpha5, 4, 100);
		this.SendVisemeSignal(KeyCode.Alpha6, 5, 100);
		this.SendVisemeSignal(KeyCode.Alpha7, 6, 100);
		this.SendVisemeSignal(KeyCode.Alpha8, 7, 100);
		this.SendVisemeSignal(KeyCode.Alpha9, 8, 100);
		this.SendVisemeSignal(KeyCode.Alpha0, 9, 100);
		this.SendVisemeSignal(KeyCode.Q, 10, 100);
		this.SendVisemeSignal(KeyCode.W, 11, 100);
		this.SendVisemeSignal(KeyCode.E, 12, 100);
		this.SendVisemeSignal(KeyCode.R, 13, 100);
		this.SendVisemeSignal(KeyCode.T, 14, 100);
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x00127C58 File Offset: 0x00126058
	private void SendVisemeSignal(KeyCode key, int viseme, int arg1)
	{
		int num = 0;
		if (Input.GetKeyDown(key))
		{
			num = this.lipsyncContext.SendSignal(OVRLipSync.ovrLipSyncSignals.VisemeAmount, this.KeySendVisemeSignal[viseme], arg1);
			this.RecordVisemeSignalSequenceEntry(OVRLipSync.ovrLipSyncSignals.VisemeAmount, this.KeySendVisemeSignal[viseme], arg1, 0);
		}
		if (Input.GetKeyUp(key))
		{
			num = this.lipsyncContext.SendSignal(OVRLipSync.ovrLipSyncSignals.VisemeAmount, this.KeySendVisemeSignal[viseme], 0);
			this.RecordVisemeSignalSequenceEntry(OVRLipSync.ovrLipSyncSignals.VisemeAmount, this.KeySendVisemeSignal[viseme], 0, 0);
		}
		if (num != 0)
		{
			Debug.Log("LipSyncContextMorphTarget.SendVisemeSignal WARNING: Possible bad range on arguments.");
		}
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x00127CDC File Offset: 0x001260DC
	private void RecordVisemeSignalSequenceEntry(OVRLipSync.ovrLipSyncSignals signal, int viseme, int arg1, int arg2)
	{
		if (this.sequencer != null && this.sequencer.IsRecording())
		{
			OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = default(OVRLipSyncContextSequencer.SequenceEntry);
			sequenceEntry.action = (int)signal;
			sequenceEntry.data1 = viseme;
			sequenceEntry.data2 = arg1;
			sequenceEntry.data3 = arg2;
			this.sequencer.AddEntryToRecording(ref sequenceEntry);
		}
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x00127D44 File Offset: 0x00126144
	private bool SendVisemeSignalsPlaySequence()
	{
		if (this.sequencer == null)
		{
			return false;
		}
		ArrayList arrayList = new ArrayList();
		bool result = this.sequencer.UpdatePlayback(ref arrayList);
		for (int i = 0; i < arrayList.Count; i++)
		{
			OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = (OVRLipSyncContextSequencer.SequenceEntry)arrayList[i];
			this.lipsyncContext.SendSignal((OVRLipSync.ovrLipSyncSignals)sequenceEntry.action, sequenceEntry.data1, sequenceEntry.data2);
		}
		return result;
	}

	// Token: 0x06003AA6 RID: 15014 RVA: 0x00127DC0 File Offset: 0x001261C0
	private void ControlSequencer()
	{
		string text = "TestSequence1";
		string sequenceInfo = "Hello world. This is a test.";
		if (this.sequencer == null)
		{
			return;
		}
		if (!this.sequencer.IsRecording())
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				this.sequencer.StopPlayback();
				if (!this.sequencer.StartRecording(text, sequenceInfo))
				{
					Debug.Log("LipSyncContextMorphTarget.ControlSequencer WARNING: Cannot start recording: " + text);
				}
			}
			else if (Input.GetKeyDown(KeyCode.P))
			{
				this.sequencer.StopPlayback();
				if (!this.sequencer.StartPlayback("TestSequence1"))
				{
					Debug.Log("LipSyncContextMorphTarget.ControlSequencer WARNING: Cannot play sequence: " + text);
				}
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				this.sequencer.StopPlayback();
				if (!this.sequencer.SaveSequence("TestSequence1"))
				{
					Debug.Log("LipSyncContextMorphTarget.ControlSequencer WARNING: Cannot save sequence: " + text);
				}
				else
				{
					OVRLipSyncDebugConsole.Clear();
					OVRLipSyncDebugConsole.Log("Saving sequence " + text);
					OVRLipSyncDebugConsole.Log("Press 'P' to play..");
				}
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				this.sequencer.StopPlayback();
				if (!this.sequencer.LoadSequence("TestSequence1"))
				{
					Debug.Log("LipSyncContextMorphTarget.ControlSequencer WARNING: Cannot load sequence: " + text);
				}
				else
				{
					OVRLipSyncDebugConsole.Clear();
					OVRLipSyncDebugConsole.Log("Loading sequence " + text);
					OVRLipSyncDebugConsole.Log("Press 'P' to play..");
				}
			}
		}
		else
		{
			OVRLipSyncDebugConsole.Clear();
			OVRLipSyncDebugConsole.Log("Recording sequence " + text + ". Press 'X' to stop recording..");
			string message = string.Empty + this.sequencer.GetCurrentTimer();
			OVRLipSyncDebugConsole.Log(message);
			if (Input.GetKeyDown(KeyCode.X))
			{
				OVRLipSyncDebugConsole.Clear();
				OVRLipSyncDebugConsole.Log("Stopped recording sequence " + text);
				OVRLipSyncDebugConsole.Log("Press 'P' to play..");
				if (!this.sequencer.StopRecording())
				{
					Debug.Log("LipSyncContextMorphTarget.ControlSequencer WARNING: Sequence not recording at this time.");
				}
			}
		}
	}

	// Token: 0x04002360 RID: 9056
	public SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x04002361 RID: 9057
	private OVRLipSync.ovrLipSyncFrame frame = new OVRLipSync.ovrLipSyncFrame(0);

	// Token: 0x04002362 RID: 9058
	public int[] VisemeToBlendTargets = new int[15];

	// Token: 0x04002363 RID: 9059
	public bool enableVisemeSignals;

	// Token: 0x04002364 RID: 9060
	public int[] KeySendVisemeSignal = new int[10];

	// Token: 0x04002365 RID: 9061
	public int SmoothAmount = 100;

	// Token: 0x04002366 RID: 9062
	private OVRLipSyncContext lipsyncContext;

	// Token: 0x04002367 RID: 9063
	private OVRLipSyncContextSequencer sequencer;

	// Token: 0x04002368 RID: 9064
	private const string testSeq1Name = "TestSequence1";

	// Token: 0x04002369 RID: 9065
	private const string textSeq1Info = "Hello world. This is a test.";
}
