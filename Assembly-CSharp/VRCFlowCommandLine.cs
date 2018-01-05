using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

// Token: 0x02000C54 RID: 3156
public class VRCFlowCommandLine : MonoBehaviour
{
	// Token: 0x060061D8 RID: 25048 RVA: 0x002286D0 File Offset: 0x00226AD0
	public void ReadCommandLine()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		this.DebugFlags.Clear();
		Debug.Log("Launching with args: " + commandLineArgs.Length);
		foreach (string text in commandLineArgs)
		{
			Debug.Log("Arg: " + text);
			if (text.StartsWith("vrchat://"))
			{
				this.Url = text;
			}
			else if (text.StartsWith("--url="))
			{
				this.Url = text.Split(new char[]
				{
					'='
				}, 2)[1];
			}
			else if (text.StartsWith("--api="))
			{
				ApiModel.SetApiUrl(text.Split(new char[]
				{
					'='
				}, 2)[1]);
			}
			else if (text.StartsWith("--photon="))
			{
				VRCApplicationSetup.Instance.gameServerVersionOverride = text.Split(new char[]
				{
					'='
				}, 2)[1];
			}
			else if (text.CompareTo("--no-vr") == 0)
			{
				this.DisableVR = true;
			}
			else if (text.CompareTo("--debug-fake-public-ban") == 0)
			{
				this.DebugFlags.Add(VRCFlowCommandLine.DebugFlag.SimulatePublicBan);
			}
			else if (text.CompareTo("--debug-fake-load-error") == 0)
			{
				this.DebugFlags.Add(VRCFlowCommandLine.DebugFlag.SimulateRoomLoadError);
			}
		}
		this.LoadSavedUrl();
		this.ParseUrl(this.Url);
	}

	// Token: 0x060061D9 RID: 25049 RVA: 0x00228848 File Offset: 0x00226C48
	private void ParseUrl(string url)
	{
		string[] array = url.Split(new char[]
		{
			'?'
		});
		if (url.ToLower().StartsWith(this.ProtocolName))
		{
			array = url.Remove(0, this.ProtocolName.Length).Split(new char[]
			{
				'?'
			});
		}
		if (array[0] == "launch" || array[0] == "launch/")
		{
			if (array.Length > 1)
			{
				string[] array2 = array[1].Split(new char[]
				{
					'&'
				});
				foreach (string text in array2)
				{
					string[] array4 = text.Split(new char[]
					{
						'='
					});
					if (array4[0] == "id")
					{
						this.launchId = array4[1];
					}
				}
				if (!string.IsNullOrEmpty(this.launchId))
				{
					this.CommandLineOperation = VRCFlowCommandLine.Operation.LaunchRoom;
				}
			}
		}
		else if (array[0] == "create" || array[0] == "create/")
		{
			this.launchId = "local:" + SessionManager.GetRandomDigits(10);
			string text2 = string.Empty;
			string pluginUrl = string.Empty;
			string[] array5 = array[1].Split(new char[]
			{
				'&'
			});
			foreach (string text3 in array5)
			{
				string[] array7 = text3.Split(new char[]
				{
					'='
				});
				if (array7[0] == "url")
				{
					text2 = array7[1];
				}
				else if (array7[0] == "pluginUrl")
				{
					pluginUrl = array7[1];
				}
				else if (array7[0] == "roomId")
				{
					this.launchId = "local:" + array7[1];
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.CommandLineOperation = VRCFlowCommandLine.Operation.CreateRoom;
				this.world = new ApiWorld();
				this.world.id = this.launchId;
				this.world.assetUrl = text2;
				this.world.pluginUrl = pluginUrl;
				this.world.name = "Local Test";
				ApiWorld.AddLocal(this.world);
			}
		}
	}

	// Token: 0x060061DA RID: 25050 RVA: 0x00228AA9 File Offset: 0x00226EA9
	public bool IsCommandLineLaunch()
	{
		return this.CommandLineOperation != VRCFlowCommandLine.Operation.None;
	}

	// Token: 0x060061DB RID: 25051 RVA: 0x00228AB7 File Offset: 0x00226EB7
	private void LoadSavedUrl()
	{
		if (PlayerPrefs.HasKey("DeferredUrl"))
		{
			this.Url = PlayerPrefs.GetString("DeferredUrl");
			PlayerPrefs.DeleteKey("DeferredUrl");
		}
	}

	// Token: 0x060061DC RID: 25052 RVA: 0x00228AE2 File Offset: 0x00226EE2
	public void SaveUrlForNextLaunch()
	{
		PlayerPrefs.SetString("DeferredUrl", this.Url);
	}

	// Token: 0x04004777 RID: 18295
	private string ProtocolName = "vrchat://";

	// Token: 0x04004778 RID: 18296
	public VRCFlowCommandLine.Operation CommandLineOperation;

	// Token: 0x04004779 RID: 18297
	public string args = string.Empty;

	// Token: 0x0400477A RID: 18298
	public string Url = string.Empty;

	// Token: 0x0400477B RID: 18299
	public string launchId = string.Empty;

	// Token: 0x0400477C RID: 18300
	public ApiWorld world;

	// Token: 0x0400477D RID: 18301
	public bool DisableVR;

	// Token: 0x0400477E RID: 18302
	public List<VRCFlowCommandLine.DebugFlag> DebugFlags = new List<VRCFlowCommandLine.DebugFlag>();

	// Token: 0x02000C55 RID: 3157
	public enum Operation
	{
		// Token: 0x04004780 RID: 18304
		None,
		// Token: 0x04004781 RID: 18305
		LaunchRoom,
		// Token: 0x04004782 RID: 18306
		CreateRoom
	}

	// Token: 0x02000C56 RID: 3158
	public enum DebugFlag
	{
		// Token: 0x04004784 RID: 18308
		SimulatePublicBan,
		// Token: 0x04004785 RID: 18309
		SimulateRoomLoadError
	}
}
