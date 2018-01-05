using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BestHTTP;
using UnityEngine;
using UnityEngine.Profiling;
using VRC.Core;

namespace VRC
{
	// Token: 0x02000B42 RID: 2882
	public class VRCLog : MonoBehaviour
	{
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06005869 RID: 22633 RVA: 0x001E9F1F File Offset: 0x001E831F
		// (set) Token: 0x0600586A RID: 22634 RVA: 0x001E9F26 File Offset: 0x001E8326
		public static VRCLog Instance { get; private set; }

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x0600586B RID: 22635 RVA: 0x001E9F30 File Offset: 0x001E8330
		// (remove) Token: 0x0600586C RID: 22636 RVA: 0x001E9F64 File Offset: 0x001E8364
		public static event Application.LogCallback MessageLogged;

		// Token: 0x0600586D RID: 22637 RVA: 0x001E9F98 File Offset: 0x001E8398
		private void Update()
		{
			if (UnityEngine.Debug.isDebugBuild && Input.GetKeyDown(KeyCode.U) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				VRCLog.EnableProfilerWithLogging(!VRCLog._profiler_logging_enabled);
			}
			VRCLog.profiler_enabled = Profiler.enabled;
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x001E9FF0 File Offset: 0x001E83F0
		private void OnDestroy()
		{
			VRCLog.Instance = null;
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x001E9FF8 File Offset: 0x001E83F8
		private void OnEnable()
		{
			if (VRCLog.Instance != null && VRCLog.Instance != this)
			{
				UnityEngine.Debug.LogError("Instance of VRCLog already created! Destroying new one. ");
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			VRCLog.Instance = this;
			if (this.uploadLog == null)
			{
				this.uploadLog = new ConcurrentQueue<string>(34);
			}
			if (Application.isEditor && !VRCLog.profilerEnabled)
			{
				this.logHandler = new VRCLog.ThreadedDiskWriteHandler(UnityEngine.Debug.logger.logHandler, this.uploadLog);
			}
			else
			{
				this.logHandler = new VRCLog.ThreadedDiskWriteHandler(null, this.uploadLog);
			}
			UnityEngine.Debug.logger.logHandler = this.logHandler;
			Application.logMessageReceivedThreaded += this.LogMessageReceived;
			if (Application.isEditor)
			{
				foreach (LogType logType in Enum.GetValues(typeof(LogType)).Cast<LogType>())
				{
					this._savedStackTraceLogType[(int)logType] = Application.GetStackTraceLogType(logType);
					Application.SetStackTraceLogType(logType, StackTraceLogType.ScriptOnly);
				}
			}
			UnityEngine.Debug.Log(VRCApplicationSetup.GetBuildDescriptionString());
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x001EA13C File Offset: 0x001E853C
		private void OnDisable()
		{
			Application.logMessageReceivedThreaded -= this.LogMessageReceived;
			if (Application.isEditor)
			{
				foreach (LogType logType in Enum.GetValues(typeof(LogType)).Cast<LogType>())
				{
					Application.SetStackTraceLogType(logType, this._savedStackTraceLogType[(int)logType]);
				}
			}
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x001EA1C8 File Offset: 0x001E85C8
		public void LogMessageReceived(string condition, string stackTrace, LogType logType)
		{
			if (!Application.isEditor)
			{
				this.logHandler.LogFormat(logType, null, string.Format("{0}\n{1}", condition, stackTrace), new object[0]);
			}
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x001EA1F4 File Offset: 0x001E85F4
		public static void EnableProfilerWithLogging(bool enable)
		{
			if (enable)
			{
				string text = string.Concat(new string[]
				{
					Path.Combine(Application.persistentDataPath, "ProfilerCaptures"),
					"/",
					(!RoomManager.inRoom) ? "Menu" : (RoomManager.currentRoom.name.Replace(' ', '_').Trim() + "_" + RoomManager.currentRoom.currentInstanceIdWithTags),
					"_",
					DateTime.Now.ToString().Replace('/', '_').Replace(':', '_').Replace(' ', '_'),
					".txt"
				});
				Directory.CreateDirectory(Path.GetDirectoryName(text));
				if (!File.Exists(text))
				{
					FileStream fileStream = File.Create(text);
					fileStream.Close();
				}
				UnityEngine.Debug.Log("PROFILER RECORD START " + text);
				Profiler.logFile = text;
				Profiler.enableBinaryLog = true;
				Profiler.enabled = true;
				VRCLog._lastProfilerLogFilename = text;
			}
			else
			{
				UnityEngine.Debug.Log("PROFILER RECORD END " + VRCLog._lastProfilerLogFilename);
				Profiler.enableBinaryLog = false;
				Profiler.enabled = false;
			}
			VRCLog._profiler_logging_enabled = enable;
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06005873 RID: 22643 RVA: 0x001EA328 File Offset: 0x001E8728
		public static bool profilerEnabled
		{
			get
			{
				return VRCLog.profiler_enabled;
			}
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x001EA32F File Offset: 0x001E872F
		public static bool UploadMiniLog(string why)
		{
			return VRCLog.Instance != null && VRCLog.Instance._uploadMiniLog(why);
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x001EA350 File Offset: 0x001E8750
		private bool _uploadMiniLog(string why)
		{
            return false;
			if (!base.enabled)
			{
				if (!string.IsNullOrEmpty(this.uploadLogReason))
				{
					UnityEngine.Debug.LogError("Upload of mini-log was triggered, but logging is disabled! Reason: " + this.uploadLogReason);
				}
				return false;
			}
			if (VRCApplicationSetup.IsEditor())
			{
				if (!string.IsNullOrEmpty(this.uploadLogReason))
				{
					UnityEngine.Debug.LogError("Mini-log upload disabled in editor. Trigger Reason: " + this.uploadLogReason);
				}
				return false;
			}
			if (this.isUploadingLog)
			{
				if (!string.IsNullOrEmpty(this.uploadLogReason))
				{
					UnityEngine.Debug.LogError("Already uploading mini-log now. Reason: " + this.uploadLogReason);
				}
				return false;
			}
			if (this.uploadReasonsAlreadyTriggered.Contains(why))
			{
				return false;
			}
			this.uploadReasonsAlreadyTriggered.Add(why);
			this.isUploadingLog = true;
			this.uploadLogReason = why;
			if (!string.IsNullOrEmpty(this.uploadLogReason))
			{
				UnityEngine.Debug.LogError("Upload of mini-log was triggered! Reason: " + this.uploadLogReason);
			}
			base.StartCoroutine(this.UploadMiniLogCoroutine());
			return true;
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x001EA453 File Offset: 0x001E8853
		public void ResetMiniLogUpload()
		{
			this.uploadReasonsAlreadyTriggered.Clear();
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x001EA460 File Offset: 0x001E8860
		private IEnumerator UploadMiniLogCoroutine()
		{
			UnityEngine.Debug.Log("Generating mini-log for upload...");
			List<string> list = this.GatherCurrentStateInfo();
			int num = 0;
			foreach (string text in list)
			{
				num += text.Length;
			}
			VRCLog.JSONLog jsonlog = new VRCLog.JSONLog();
			List<string> list2 = this.uploadLog.CopyToArray().ToList<string>();
			int num2 = list2.Aggregate(0, (int a, string s) => a + s.Length);
			while (num2 > 1024 && list2.Count != 0)
			{
				num2 -= list2.First<string>().Length;
				list2.RemoveAt(0);
			}
			int capacity = this.currentUploadLogSize + num + 1000;
			StringBuilder stringBuilder = new StringBuilder(capacity);
			string str = "(Steam)";
			stringBuilder.AppendLine("Mini-log uploaded, VRChat v" + jsonlog.version + " " + str);
			stringBuilder.AppendLine("Reason: " + this.uploadLogReason);
			stringBuilder.AppendLine(string.Empty);
			stringBuilder.AppendLine("... <log snippet starts here> ...");
			foreach (string value in list2)
			{
				stringBuilder.AppendLine(value);
			}
			stringBuilder.AppendLine("... <end log> ...\r\n");
			stringBuilder.AppendLine("===== Current state ==========");
			foreach (string value2 in list)
			{
				stringBuilder.AppendLine(value2);
			}
			jsonlog.logs = stringBuilder.ToString();
			UnityEngine.Debug.Log("Uploading mini-log to http://api.vrchat.cloud/api/1/error (" + jsonlog.logs.Length + " bytes) ...");
			HTTPRequest httprequest = new HTTPRequest(new Uri("http://api.vrchat.cloud/api/1/error"), HTTPMethods.Post, delegate(HTTPRequest req, HTTPResponse resp)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Upload mini log HTTP response: \r\n[",
					resp.StatusCode,
					"] ",
					resp.Message
				}));
				this.FinishMiniLogUpload();
			});
			httprequest.ConnectTimeout = TimeSpan.FromSeconds(20.0);
			httprequest.Timeout = TimeSpan.FromSeconds(60.0);
			httprequest.RawData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(jsonlog));
			httprequest.AddHeader("Content-Type", "application/json");
			httprequest.AddHeader("Content-Length", httprequest.RawData.Length.ToString());
			httprequest.Send();
			yield break;
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x001EA47B File Offset: 0x001E887B
		private void FinishMiniLogUpload()
		{
			this.isUploadingLog = false;
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x001EA484 File Offset: 0x001E8884
		private List<string> GatherCurrentStateInfo()
		{
			List<string> list = new List<string>();
			list.Add(HMDManager.strFPS);
			string text = "Current room: ";
			if (!RoomManager.inRoom)
			{
				text += "<none>";
			}
			else
			{
				ApiWorld currentRoom = RoomManager.currentRoom;
				text += "\r\n";
				if (currentRoom != null)
				{
					text += string.Format("{0} (id: {1})\r\n", currentRoom.name, currentRoom.id);
				}
				list.Add(text);
			}
			return list;
		}

		// Token: 0x04003F5A RID: 16218
		private const string UPLOAD_URL = "http://api.vrchat.cloud/api/1/error";

		// Token: 0x04003F5B RID: 16219
		private const int MAX_UPLOAD_LOG_SIZE = 1024;

		// Token: 0x04003F5C RID: 16220
		private const int UPLOAD_QUEUE_CAPACITY = 34;

		// Token: 0x04003F5D RID: 16221
		private ConcurrentQueue<string> uploadLog;

		// Token: 0x04003F5E RID: 16222
		private int currentUploadLogSize;

		// Token: 0x04003F5F RID: 16223
		private bool isUploadingLog;

		// Token: 0x04003F60 RID: 16224
		private string uploadLogReason;

		// Token: 0x04003F61 RID: 16225
		private static bool profiler_enabled;

		// Token: 0x04003F62 RID: 16226
		private static bool _profiler_logging_enabled;

		// Token: 0x04003F63 RID: 16227
		private static string _lastProfilerLogFilename;

		// Token: 0x04003F64 RID: 16228
		private HashSet<string> uploadReasonsAlreadyTriggered = new HashSet<string>();

		// Token: 0x04003F65 RID: 16229
		private StackTraceLogType[] _savedStackTraceLogType = new StackTraceLogType[Enum.GetValues(typeof(LogType)).Length];

		// Token: 0x04003F66 RID: 16230
		private VRCLog.ThreadedDiskWriteHandler logHandler;

		// Token: 0x02000B43 RID: 2883
		[Serializable]
		private class JSONLog
		{
			// Token: 0x0600587B RID: 22651 RVA: 0x001EA504 File Offset: 0x001E8904
			public JSONLog()
			{
				VRCApplicationSetup vrcapplicationSetup = UnityEngine.Object.FindObjectOfType<VRCApplicationSetup>();
				this.version = vrcapplicationSetup.appVersion;
			}

			// Token: 0x04003F67 RID: 16231
			public string client = "VrChat";

			// Token: 0x04003F68 RID: 16232
			public string version = string.Empty;

			// Token: 0x04003F69 RID: 16233
			public string logs = string.Empty;
		}

		// Token: 0x02000B44 RID: 2884
		private class ThreadedDiskWriteHandler : ILogHandler, IDisposable
		{
			// Token: 0x0600587C RID: 22652 RVA: 0x001EA54C File Offset: 0x001E894C
			public ThreadedDiskWriteHandler(ILogHandler _original, ConcurrentQueue<string> _uploadLog)
			{
				this.uploadLog = _uploadLog;
				this.original = _original;
				this.filePath = Application.persistentDataPath + Path.PathSeparator + "output_log";
				this.alive = true;
				this.writeQueue = new ConcurrentQueue<string>();
				this.writeThread = new Thread(new ThreadStart(this.WriteLoop));
				this.writeThread.Priority = System.Threading.ThreadPriority.Lowest;
				this.writeThread.Start();
			}

			// Token: 0x0600587D RID: 22653 RVA: 0x001EA5CC File Offset: 0x001E89CC
			public void LogException(Exception exception, UnityEngine.Object context)
			{
				this.InternalLog(string.Format("[{0}:{1}] {2}\r\n{3}", new object[]
				{
					LogType.Exception.ToString(),
					DateTime.Now.ToString(),
					exception.Message,
					exception.StackTrace
				}));
				if (this.original != null)
				{
					this.original.LogException(exception, context);
				}
				if (VRCLog.MessageLogged != null)
				{
					VRCLog.MessageLogged(exception.Message, exception.StackTrace, LogType.Exception);
				}
			}

			// Token: 0x0600587E RID: 22654 RVA: 0x001EA664 File Offset: 0x001E8A64
			public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
			{
				if (logType != LogType.Log && logType != LogType.Warning)
				{
					StackTrace stackTrace = new StackTrace(3);
					this.InternalLog(string.Format("[{0}:{1}] {2}\r\n{3}", new object[]
					{
						logType.ToString(),
						DateTime.Now.ToString(),
						string.Format(format, args),
						stackTrace.ToString()
					}));
				}
				else
				{
					this.InternalLog(string.Format("[{0}:{1}] {2}", logType.ToString(), DateTime.Now.ToString(), string.Format(format, args)));
				}
				if (this.original != null)
				{
					this.original.LogFormat(logType, context, format, args);
				}
				if (VRCLog.MessageLogged != null)
				{
					VRCLog.MessageLogged(string.Format(format, args), (!(context == null)) ? context.name : "null", logType);
				}
			}

			// Token: 0x0600587F RID: 22655 RVA: 0x001EA767 File Offset: 0x001E8B67
			private void InternalLog(string message)
			{
				this.writeQueue.Enqueue(message);
				this.uploadLog.Enqueue(message);
			}

			// Token: 0x06005880 RID: 22656 RVA: 0x001EA784 File Offset: 0x001E8B84
			private bool isFileOpen(string path)
			{
				if (!File.Exists(path))
				{
					return false;
				}
				FileStream fileStream = null;
				try
				{
					fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
				}
				catch (IOException)
				{
					return true;
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
				return false;
			}

			// Token: 0x06005881 RID: 22657 RVA: 0x001EA7E4 File Offset: 0x001E8BE4
			private void WriteLoop()
			{
				string path = this.filePath + ".txt";
				int num = 0;
				while (this.isFileOpen(path))
				{
					num++;
					path = this.filePath + num.ToString() + ".txt";
				}
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				using (StreamWriter streamWriter = new StreamWriter(path, true))
				{
					while (this.alive)
					{
						string[] array = this.writeQueue.DumpToArray();
						foreach (string str in array)
						{
							streamWriter.WriteLine(str + "\n\n");
							streamWriter.Flush();
						}
						Thread.Sleep(10);
					}
				}
			}

			// Token: 0x06005882 RID: 22658 RVA: 0x001EA8D0 File Offset: 0x001E8CD0
			public void Dispose()
			{
				this.alive = false;
				this.writeThread.Join();
			}

			// Token: 0x04003F6A RID: 16234
			private ConcurrentQueue<string> writeQueue;

			// Token: 0x04003F6B RID: 16235
			private Thread writeThread;

			// Token: 0x04003F6C RID: 16236
			private bool alive;

			// Token: 0x04003F6D RID: 16237
			private string filePath;

			// Token: 0x04003F6E RID: 16238
			private ConcurrentQueue<string> uploadLog;

			// Token: 0x04003F6F RID: 16239
			private ILogHandler original;
		}

		// Token: 0x02000B45 RID: 2885
		private class NullHandler : ILogHandler
		{
			// Token: 0x06005884 RID: 22660 RVA: 0x001EA8EC File Offset: 0x001E8CEC
			public void LogException(Exception exception, UnityEngine.Object context)
			{
			}

			// Token: 0x06005885 RID: 22661 RVA: 0x001EA8EE File Offset: 0x001E8CEE
			public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
			{
			}
		}
	}
}
