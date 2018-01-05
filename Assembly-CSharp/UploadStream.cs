using System;
using System.IO;
using System.Threading;
using BestHTTP;

// Token: 0x020003ED RID: 1005
public sealed class UploadStream : Stream
{
	// Token: 0x060023F6 RID: 9206 RVA: 0x000B39C2 File Offset: 0x000B1DC2
	public UploadStream(string name) : this()
	{
		this.Name = name;
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000B39D4 File Offset: 0x000B1DD4
	public UploadStream()
	{
		this.ReadBuffer = new MemoryStream();
		this.WriteBuffer = new MemoryStream();
		this.Name = string.Empty;
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000B3A35 File Offset: 0x000B1E35
	// (set) Token: 0x060023F9 RID: 9209 RVA: 0x000B3A3D File Offset: 0x000B1E3D
	public string Name { get; private set; }

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x060023FA RID: 9210 RVA: 0x000B3A48 File Offset: 0x000B1E48
	private bool IsReadBufferEmpty
	{
		get
		{
			object obj = this.locker;
			bool result;
			lock (obj)
			{
				result = (this.ReadBuffer.Position == this.ReadBuffer.Length);
			}
			return result;
		}
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000B3A98 File Offset: 0x000B1E98
	public override int Read(byte[] buffer, int offset, int count)
	{
		if (this.noMoreData)
		{
			if (this.ReadBuffer.Position != this.ReadBuffer.Length)
			{
				return this.ReadBuffer.Read(buffer, offset, count);
			}
			if (this.WriteBuffer.Length <= 0L)
			{
				HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Read - End Of Stream", this.Name));
				return -1;
			}
			this.SwitchBuffers();
		}
		if (this.IsReadBufferEmpty)
		{
			this.ARE.WaitOne();
			object obj = this.locker;
			lock (obj)
			{
				if (this.IsReadBufferEmpty && this.WriteBuffer.Length > 0L)
				{
					this.SwitchBuffers();
				}
			}
		}
		int result = -1;
		object obj2 = this.locker;
		lock (obj2)
		{
			result = this.ReadBuffer.Read(buffer, offset, count);
		}
		return result;
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000B3BBC File Offset: 0x000B1FBC
	public override void Write(byte[] buffer, int offset, int count)
	{
		if (this.noMoreData)
		{
			throw new ArgumentException("noMoreData already set!");
		}
		object obj = this.locker;
		lock (obj)
		{
			this.WriteBuffer.Write(buffer, offset, count);
			this.SwitchBuffers();
		}
		this.ARE.Set();
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000B3C2C File Offset: 0x000B202C
	public override void Flush()
	{
		this.Finish();
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000B3C34 File Offset: 0x000B2034
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Dispose", this.Name));
			this.ReadBuffer.Dispose();
			this.ReadBuffer = null;
			this.WriteBuffer.Dispose();
			this.WriteBuffer = null;
			this.ARE.Close();
			this.ARE = null;
		}
		base.Dispose(disposing);
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000B3CA4 File Offset: 0x000B20A4
	public void Finish()
	{
		if (this.noMoreData)
		{
			throw new ArgumentException("noMoreData already set!");
		}
		HTTPManager.Logger.Information("UploadStream", string.Format("{0} - Finish", this.Name));
		this.noMoreData = true;
		this.ARE.Set();
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000B3CFC File Offset: 0x000B20FC
	private bool SwitchBuffers()
	{
		object obj = this.locker;
		lock (obj)
		{
			if (this.ReadBuffer.Position == this.ReadBuffer.Length)
			{
				this.WriteBuffer.Seek(0L, SeekOrigin.Begin);
				this.ReadBuffer.SetLength(0L);
				MemoryStream writeBuffer = this.WriteBuffer;
				this.WriteBuffer = this.ReadBuffer;
				this.ReadBuffer = writeBuffer;
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06002401 RID: 9217 RVA: 0x000B3D90 File Offset: 0x000B2190
	public override bool CanRead
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06002402 RID: 9218 RVA: 0x000B3D97 File Offset: 0x000B2197
	public override bool CanSeek
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06002403 RID: 9219 RVA: 0x000B3D9E File Offset: 0x000B219E
	public override bool CanWrite
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06002404 RID: 9220 RVA: 0x000B3DA5 File Offset: 0x000B21A5
	public override long Length
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06002405 RID: 9221 RVA: 0x000B3DAC File Offset: 0x000B21AC
	// (set) Token: 0x06002406 RID: 9222 RVA: 0x000B3DB3 File Offset: 0x000B21B3
	public override long Position
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000B3DBA File Offset: 0x000B21BA
	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000B3DC1 File Offset: 0x000B21C1
	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040011F5 RID: 4597
	private MemoryStream ReadBuffer = new MemoryStream();

	// Token: 0x040011F6 RID: 4598
	private MemoryStream WriteBuffer = new MemoryStream();

	// Token: 0x040011F7 RID: 4599
	private bool noMoreData;

	// Token: 0x040011F8 RID: 4600
	private AutoResetEvent ARE = new AutoResetEvent(false);

	// Token: 0x040011F9 RID: 4601
	private object locker = new object();
}
