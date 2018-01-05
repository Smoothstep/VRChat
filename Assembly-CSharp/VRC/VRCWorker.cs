using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace VRC
{
	// Token: 0x02000B49 RID: 2889
	public class VRCWorker : MonoBehaviour
	{
		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06005890 RID: 22672 RVA: 0x001EAF84 File Offset: 0x001E9384
		public static bool IsMainThread
		{
			get
			{
				return VRCWorker.spawnedThreadID != null && Thread.CurrentThread.ManagedThreadId == VRCWorker.spawnedThreadID;
			}
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x001EAFC8 File Offset: 0x001E93C8
		public static void DispatchToMain(Func<IEnumerator> coroutine)
		{
			if (VRCWorker.IsMainThread)
			{
				VRCWorker.Instance.StartCoroutine(coroutine());
			}
			else
			{
				VRCWorker.AddJob<object>(() => new object(), delegate(object b)
				{
					VRCWorker.Instance.StartCoroutine(coroutine());
				});
			}
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x001EB038 File Offset: 0x001E9438
		public static void DispatchToMain(Action action)
		{
			if (VRCWorker.IsMainThread)
			{
				action();
			}
			else
			{
				VRCWorker.AddJob<object>(() => new object(), delegate(object b)
				{
					action();
				});
			}
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x001EB09C File Offset: 0x001E949C
		public static VRCWorker.Job AddJob<OutcomeType>(Func<OutcomeType> workToDo, Action<OutcomeType> onDone) where OutcomeType : class
		{
			if (VRCWorker.Instance == null)
			{
				return null;
			}
			VRCWorker.Job newJob = new VRCWorker.Job
			{
				Outcome = null,
				Completed = false
			};
			newJob.Action = delegate
			{
				newJob.Outcome = workToDo();
			};
			newJob.OnDone = delegate
			{
				onDone((OutcomeType)((object)newJob.Outcome));
				newJob.Completed = true;
			};
			VRCWorker.Instance.activeJobs.Enqueue(newJob);
			return newJob;
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x001EB134 File Offset: 0x001E9534
		private void OnDestroy()
		{
			this.alive = false;
			int num = 0;
			while (this.threadPool != null && num < this.threadPool.Length)
			{
				this.threadPool[num].Join();
				num++;
			}
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x001EB17C File Offset: 0x001E957C
		private void Awake()
		{
			if (VRCWorker.Instance != null)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			VRCWorker.spawnedThreadID = new int?(Thread.CurrentThread.ManagedThreadId);
			VRCWorker.Instance = this;
			this.alive = true;
			this.activeJobs = new ConcurrentQueue<VRCWorker.Job>(this.ThreadCount * 10);
			this.finishedJobs = new ConcurrentQueue<VRCWorker.Job>(this.ThreadCount * 10);
			this.threadPool = new Thread[this.ThreadCount];
			for (int i = 0; i < this.ThreadCount; i++)
			{
				this.threadPool[i] = new Thread(new ThreadStart(this.ThreadLoop));
				this.threadPool[i].Priority = System.Threading.ThreadPriority.Lowest;
				this.threadPool[i].Start();
			}
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x001EB248 File Offset: 0x001E9648
		private void Update()
		{
			try
			{
				VRCWorker.Job job = this.finishedJobs.Dequeue();
				object obj = job;
				lock (obj)
				{
					if (job != null)
					{
						job.OnDone();
					}
				}
			}
			catch (InvalidOperationException)
			{
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x001EB2C8 File Offset: 0x001E96C8
		private void ThreadLoop()
		{
			while (this.alive)
			{
				try
				{
					VRCWorker.Job job = this.activeJobs.Dequeue();
					object obj = job;
					lock (obj)
					{
						if (job != null)
						{
							job.Action();
							this.finishedJobs.Enqueue(job);
						}
					}
				}
				catch (InvalidOperationException)
				{
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				Thread.Sleep(this.ThreadWait);
			}
		}

		// Token: 0x04003F72 RID: 16242
		[Range(1f, 32f)]
		public int ThreadCount = 4;

		// Token: 0x04003F73 RID: 16243
		[Range(1f, 100f)]
		public int ThreadWait = 10;

		// Token: 0x04003F74 RID: 16244
		private Thread[] threadPool;

		// Token: 0x04003F75 RID: 16245
		private bool alive = true;

		// Token: 0x04003F76 RID: 16246
		private ConcurrentQueue<VRCWorker.Job> activeJobs;

		// Token: 0x04003F77 RID: 16247
		private ConcurrentQueue<VRCWorker.Job> finishedJobs;

		// Token: 0x04003F78 RID: 16248
		private static VRCWorker Instance;

		// Token: 0x04003F79 RID: 16249
		private static int? spawnedThreadID;

		// Token: 0x02000B4A RID: 2890
		public class Job
		{
			// Token: 0x04003F7C RID: 16252
			public Action Action;

			// Token: 0x04003F7D RID: 16253
			public Action OnDone;

			// Token: 0x04003F7E RID: 16254
			public object Outcome;

			// Token: 0x04003F7F RID: 16255
			public bool Completed;
		}
	}
}
