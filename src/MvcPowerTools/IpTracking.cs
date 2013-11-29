using System;
using System.Web;
using System.Web.Security;

namespace MvcPowerTools
{
	//todo review ip tracking
    /// <summary>
	/// Class for tracking ip changing
	/// </summary>
	public class IpTracking
	{
		private string _cookieName="_it";
		public string CookieName
		{
			get { return _cookieName; }
			set
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentNullException();
				_cookieName = value;
			}
		}

		public IpTracking(HttpContextBase context)
		{
			if (context == null) throw new ArgumentNullException("context");
			
			if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
			{
				IP = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			}
			else
			{
				IP = context.Request.UserHostAddress;
			}
			
			FrontIp = context.Request.UserHostAddress;
			//check if we have cookie
			var ck = context.Request.Cookies[CookieName];
			if (ck == null)
			{
				ck = IssueCookie();
				IsChanged = true;
				context.Response.AppendCookie(ck);
			}
			else
			{
				OldIp = GetCookieIp(ck);
				if (OldIp != IP)
				{
					IsChanged = true;
					ck = IssueCookie();
					context.Response.SetCookie(ck);
				}
			}
		}

		/// <summary>
		/// Gets user ip, ignoring proxies
		/// </summary>
		public string FrontIp
		{
			get; private set;
		}

		string GetCookieIp(HttpCookie ck)
		{
			if (string.IsNullOrEmpty(ck.Value)) return null;
			try
			{
				var ft = FormsAuthentication.Decrypt(ck.Value);
				if (ft == null) return null;
				return ft.UserData;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void ClearCookie(HttpResponseBase resp)
		{
			if (resp == null) throw new ArgumentNullException("resp");
			resp.SetCookie(new HttpCookie(CookieName, null) { Expires = new DateTime(2010,1,1) });
		}

		HttpCookie IssueCookie()
		{
			var ft = new FormsAuthenticationTicket(2, CookieName, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), false, IP);
			var data =
				FormsAuthentication.Encrypt(ft);
			var ck = new HttpCookie(CookieName, data);
			ck.Expires = DateTime.UtcNow.AddDays(1);
			return ck;
		}


		/// <summary>
		/// Gets current ip, takes proxies into consideration
		/// </summary>
		public string IP
		{
			get;
			private set;
		}

		public string OldIp
		{
			get;
			private set;
		}

		public bool IsChanged
		{
			get;
			private set;
		}
	}
}