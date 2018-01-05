using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIWidgets
{
	// Token: 0x0200096C RID: 2412
	public class Templates<T> where T : MonoBehaviour, ITemplatable
	{
		// Token: 0x0600492C RID: 18732 RVA: 0x00187145 File Offset: 0x00185545
		public Templates(Action<T> onCreateCallback = null)
		{
			this.onCreateCallback = onCreateCallback;
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x0018716A File Offset: 0x0018556A
		public void FindTemplates()
		{
			this.findTemplatesCalled = true;
			Resources.FindObjectsOfTypeAll<T>().ForEach(delegate(T x)
			{
				this.Add(x.name, x, true);
				x.gameObject.SetActive(false);
			});
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x00187189 File Offset: 0x00185589
		public void ClearCache()
		{
			this.cache.Keys.ForEach(delegate(string x)
			{
				this.ClearCache(x);
			});
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x001871A8 File Offset: 0x001855A8
		public void ClearCache(string name)
		{
			if (!this.cache.ContainsKey(name))
			{
				return;
			}
			this.cache[name].ForEach(delegate(T x)
			{
				UnityEngine.Object.Destroy(x);
			});
			this.cache[name].Clear();
			this.cache[name].TrimExcess();
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x00187217 File Offset: 0x00185617
		public bool Exists(string name)
		{
			return this.templates.ContainsKey(name);
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x00187225 File Offset: 0x00185625
		public T Get(string name)
		{
			if (!this.Exists(name))
			{
				throw new ArgumentException("Not found template with name '" + name + "'");
			}
			return this.templates[name];
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x00187255 File Offset: 0x00185655
		public void Delete(string name)
		{
			if (!this.Exists(name))
			{
				return;
			}
			this.templates.Remove(name);
			this.ClearCache(name);
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00187278 File Offset: 0x00185678
		public void Add(string name, T template, bool replace = true)
		{
			if (this.Exists(name))
			{
				if (!replace)
				{
					throw new ArgumentException("Template with name '" + name + "' already exists.");
				}
				this.ClearCache(name);
				this.templates[name] = template;
			}
			else
			{
				this.templates.Add(name, template);
			}
			template.IsTemplate = true;
			template.TemplateName = name;
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x001872F0 File Offset: 0x001856F0
		public T Instance(string name)
		{
			if (!this.findTemplatesCalled)
			{
				this.FindTemplates();
			}
			if (!this.Exists(name))
			{
				throw new ArgumentException("Not found template with name '" + name + "'");
			}
			if (this.templates[name] == null)
			{
				this.templates.Clear();
				this.FindTemplates();
			}
			T t;
			if (this.cache.ContainsKey(name) && this.cache[name].Count > 0)
			{
				t = this.cache[name].Pop();
			}
			else
			{
				t = UnityEngine.Object.Instantiate<T>(this.templates[name]);
				t.TemplateName = name;
				t.IsTemplate = false;
				if (this.onCreateCallback != null)
				{
					this.onCreateCallback(t);
				}
			}
			Transform transform = t.transform;
			T t2 = this.templates[name];
			transform.SetParent(t2.transform.parent, false);
			return t;
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00187414 File Offset: 0x00185814
		public void ToCache(T instance)
		{
			instance.gameObject.SetActive(false);
			if (!this.cache.ContainsKey(instance.TemplateName))
			{
				this.cache[instance.TemplateName] = new Stack<T>();
			}
			this.cache[instance.TemplateName].Push(instance);
		}

		// Token: 0x0400319A RID: 12698
		private Dictionary<string, T> templates = new Dictionary<string, T>();

		// Token: 0x0400319B RID: 12699
		private Dictionary<string, Stack<T>> cache = new Dictionary<string, Stack<T>>();

		// Token: 0x0400319C RID: 12700
		private bool findTemplatesCalled;

		// Token: 0x0400319D RID: 12701
		private Action<T> onCreateCallback;
	}
}
