using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020006FC RID: 1788
public class OVRLipSyncContextSequencer : MonoBehaviour
{
	// Token: 0x06003AA8 RID: 15016 RVA: 0x00127FE3 File Offset: 0x001263E3
	public float GetCurrentTimer()
	{
		return Time.time - this.startTime;
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x00127FF4 File Offset: 0x001263F4
	public bool LoadSequence(string sequenceName)
	{
		for (int i = 0; i < this.sequences.Count; i++)
		{
			if (((OVRLipSyncContextSequencer.Sequence)this.sequences[i]).name == sequenceName)
			{
				this.currentSequence = i;
				return true;
			}
		}
		if (PlayerPrefs.GetString(sequenceName, string.Empty) == string.Empty)
		{
			Debug.Log("OVRLipSyncContextSequencer WARNING: Cannot load sequence " + sequenceName);
			return false;
		}
		OVRLipSyncContextSequencer.Sequence sequence = new OVRLipSyncContextSequencer.Sequence(0);
		sequence.name = sequenceName;
		string text = string.Empty;
		text = sequenceName + "_INFO";
		sequence.info = PlayerPrefs.GetString(text, "NO INFO");
		text = sequenceName + "_NE";
		sequence.numEntries = PlayerPrefs.GetInt(text, 0);
		for (int j = 0; j < sequence.numEntries; j++)
		{
			OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = new OVRLipSyncContextSequencer.SequenceEntry(0);
			text = sequenceName + "_E_";
			text += j;
			string text2 = string.Empty;
			text2 = text;
			text2 += "_TIMESTAMP";
			sequenceEntry.timestamp = PlayerPrefs.GetFloat(text2, 0f);
			text2 = text;
			text2 += "_ACTION";
			sequenceEntry.action = PlayerPrefs.GetInt(text2, -1);
			text2 = text;
			text2 += "_DATA1";
			sequenceEntry.data1 = PlayerPrefs.GetInt(text2, 0);
			text2 = text;
			text2 += "_DATA2";
			sequenceEntry.data2 = PlayerPrefs.GetInt(text2, 0);
			text2 = text;
			text2 += "_DATA3";
			sequenceEntry.data3 = PlayerPrefs.GetInt(text2, 0);
			sequence.entries.Add(sequenceEntry);
		}
		if (sequence.numEntries != sequence.entries.Count)
		{
			Debug.Log("OVRLipSyncContextSequencer WARNING: " + sequenceName + " might be corrupted.");
			return false;
		}
		this.sequences.Add(sequence);
		return true;
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x0012820C File Offset: 0x0012660C
	public bool SaveSequence(string sequenceName)
	{
		int num = -1;
		for (int i = 0; i < this.sequences.Count; i++)
		{
			if (((OVRLipSyncContextSequencer.Sequence)this.sequences[i]).name == sequenceName)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			Debug.Log("OVRPhonemeContextSequencer WARNING: " + sequenceName + " does not exist, cannot save.");
			return false;
		}
		OVRLipSyncContextSequencer.Sequence sequence = (OVRLipSyncContextSequencer.Sequence)this.sequences[num];
		PlayerPrefs.SetString(sequenceName, sequence.name);
		string text = sequenceName + "_INFO";
		PlayerPrefs.SetString(text, sequence.info);
		text = sequenceName + "_NE";
		PlayerPrefs.SetInt(text, sequence.numEntries);
		for (int j = 0; j < sequence.entries.Count; j++)
		{
			OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = (OVRLipSyncContextSequencer.SequenceEntry)sequence.entries[j];
			text = sequenceName + "_E_";
			text += j;
			string text2 = string.Empty;
			text2 = text;
			text2 += "_TIMESTAMP";
			PlayerPrefs.SetFloat(text2, sequenceEntry.timestamp);
			text2 = text;
			text2 += "_ACTION";
			PlayerPrefs.SetInt(text2, sequenceEntry.action);
			text2 = text;
			text2 += "_DATA1";
			PlayerPrefs.SetInt(text2, sequenceEntry.data1);
			text2 = text;
			text2 += "_DATA2";
			PlayerPrefs.SetInt(text2, sequenceEntry.data2);
			text2 = text;
			text2 += "_DATA3";
			PlayerPrefs.SetInt(text2, sequenceEntry.data3);
		}
		return true;
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x001283D8 File Offset: 0x001267D8
	public bool IsRecording()
	{
		return this.recording;
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x001283E0 File Offset: 0x001267E0
	public bool StartRecording(string sequenceName, string sequenceInfo)
	{
		this.currentSequence = -1;
		this.recording = true;
		this.recordingSequence = new OVRLipSyncContextSequencer.Sequence(0);
		this.recordingSequence.name = sequenceName;
		this.recordingSequence.info = sequenceInfo;
		this.startTime = Time.time;
		return true;
	}

	// Token: 0x06003AAD RID: 15021 RVA: 0x00128420 File Offset: 0x00126820
	public bool AddEntryToRecording(ref OVRLipSyncContextSequencer.SequenceEntry entry)
	{
		if (!this.recording)
		{
			return false;
		}
		OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = entry.Clone();
		sequenceEntry.timestamp = Time.time - this.startTime;
		this.recordingSequence.entries.Add(sequenceEntry);
		this.recordingSequence.numEntries = this.recordingSequence.numEntries + 1;
		return true;
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x00128480 File Offset: 0x00126880
	public bool StopRecording()
	{
		if (!this.recording)
		{
			return false;
		}
		OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = new OVRLipSyncContextSequencer.SequenceEntry(0);
		sequenceEntry.timestamp = Time.time - this.startTime;
		this.recordingSequence.entries.Add(sequenceEntry);
		for (int i = 0; i < this.sequences.Count; i++)
		{
			if (((OVRLipSyncContextSequencer.Sequence)this.sequences[i]).name == this.recordingSequence.name)
			{
				this.sequences.RemoveAt(i);
			}
		}
		this.sequences.Add(this.recordingSequence.Clone());
		this.recording = false;
		return true;
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x00128546 File Offset: 0x00126946
	public bool IsPlaying()
	{
		return this.currentSequence != -1;
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x00128558 File Offset: 0x00126958
	public bool StartPlayback(string sequenceName)
	{
		if (this.recording)
		{
			Debug.Log("OVRLipSyncContextSequencer WARNING: Currently recording a sequence.");
			return false;
		}
		for (int i = 0; i < this.sequences.Count; i++)
		{
			if (((OVRLipSyncContextSequencer.Sequence)this.sequences[i]).name == sequenceName)
			{
				this.currentSequence = i;
				this.currentSequenceEntry = 0;
				this.startTime = Time.time;
				return true;
			}
		}
		Debug.Log("OVRLipSyncContextSequencer WARNING: " + sequenceName + " does not exist.");
		return false;
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x001285ED File Offset: 0x001269ED
	public bool StopPlayback()
	{
		if (this.currentSequence == -1)
		{
			return false;
		}
		this.currentSequence = -1;
		this.currentSequenceEntry = 0;
		return true;
	}

	// Token: 0x06003AB2 RID: 15026 RVA: 0x0012860C File Offset: 0x00126A0C
	public bool UpdatePlayback(ref ArrayList entries)
	{
		if (!this.IsPlaying())
		{
			return false;
		}
		OVRLipSyncContextSequencer.Sequence sequence = (OVRLipSyncContextSequencer.Sequence)this.sequences[this.currentSequence];
		if (this.currentSequenceEntry >= sequence.entries.Count)
		{
			this.currentSequence = -1;
			this.currentSequenceEntry = 0;
			return false;
		}
		float num = Time.time - this.startTime;
		OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = (OVRLipSyncContextSequencer.SequenceEntry)sequence.entries[this.currentSequenceEntry];
		float timestamp = sequenceEntry.timestamp;
		while (num > timestamp)
		{
			if (sequenceEntry.action != -1)
			{
				entries.Add(sequenceEntry.Clone());
			}
			this.currentSequenceEntry++;
			if (this.currentSequenceEntry >= sequence.entries.Count)
			{
				this.currentSequence = -1;
				return false;
			}
			sequenceEntry = (OVRLipSyncContextSequencer.SequenceEntry)sequence.entries[this.currentSequenceEntry];
			timestamp = sequenceEntry.timestamp;
		}
		return true;
	}

	// Token: 0x0400236A RID: 9066
	private ArrayList sequences = new ArrayList(0);

	// Token: 0x0400236B RID: 9067
	private int currentSequence = -1;

	// Token: 0x0400236C RID: 9068
	private int currentSequenceEntry;

	// Token: 0x0400236D RID: 9069
	private OVRLipSyncContextSequencer.Sequence recordingSequence;

	// Token: 0x0400236E RID: 9070
	private float startTime;

	// Token: 0x0400236F RID: 9071
	private bool recording;

	// Token: 0x020006FD RID: 1789
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ovrLipSyncContextSequencerValues
	{
		// Token: 0x04002370 RID: 9072
		public const int NullEntry = -1;

		// Token: 0x04002371 RID: 9073
		public const int CurrentSequenceNotSet = -1;
	}

	// Token: 0x020006FE RID: 1790
	public struct SequenceEntry
	{
		// Token: 0x06003AB3 RID: 15027 RVA: 0x0012870D File Offset: 0x00126B0D
		public SequenceEntry(int init)
		{
			this.timestamp = 0f;
			this.action = -1;
			this.data1 = 0;
			this.data2 = 0;
			this.data3 = 0;
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00128738 File Offset: 0x00126B38
		public OVRLipSyncContextSequencer.SequenceEntry Clone()
		{
			return new OVRLipSyncContextSequencer.SequenceEntry(0)
			{
				timestamp = this.timestamp,
				action = this.action,
				data1 = this.data1,
				data2 = this.data2,
				data3 = this.data3
			};
		}

		// Token: 0x04002372 RID: 9074
		public float timestamp;

		// Token: 0x04002373 RID: 9075
		public int action;

		// Token: 0x04002374 RID: 9076
		public int data1;

		// Token: 0x04002375 RID: 9077
		public int data2;

		// Token: 0x04002376 RID: 9078
		public int data3;
	}

	// Token: 0x020006FF RID: 1791
	public struct Sequence
	{
		// Token: 0x06003AB5 RID: 15029 RVA: 0x0012878F File Offset: 0x00126B8F
		public Sequence(int init)
		{
			this.name = "INIT";
			this.info = "INIT";
			this.numEntries = 0;
			this.entries = new ArrayList();
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x001287BC File Offset: 0x00126BBC
		public OVRLipSyncContextSequencer.Sequence Clone()
		{
			OVRLipSyncContextSequencer.Sequence result = new OVRLipSyncContextSequencer.Sequence(0);
			result.name = this.name;
			result.info = this.info;
			result.numEntries = this.numEntries;
			for (int i = 0; i < this.entries.Count; i++)
			{
				OVRLipSyncContextSequencer.SequenceEntry sequenceEntry = (OVRLipSyncContextSequencer.SequenceEntry)this.entries[i];
				result.entries.Add(sequenceEntry.Clone());
			}
			return result;
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x00128840 File Offset: 0x00126C40
		private void Clear()
		{
			this.name = "INIT";
			this.info = "INIT";
			this.numEntries = 0;
			this.entries.Clear();
		}

		// Token: 0x04002377 RID: 9079
		public string name;

		// Token: 0x04002378 RID: 9080
		public string info;

		// Token: 0x04002379 RID: 9081
		public int numEntries;

		// Token: 0x0400237A RID: 9082
		public ArrayList entries;
	}
}
