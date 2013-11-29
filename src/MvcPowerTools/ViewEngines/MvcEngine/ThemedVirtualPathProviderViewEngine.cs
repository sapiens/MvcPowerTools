/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * This software is subject to the Microsoft Public License (Ms-PL). 
 * A copy of the license can be found in the license.htm file included 
 * in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

/* Modified for CavemanTools*/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPowerTools.Extensions;

namespace MvcPowerTools.ViewEngines.MvcEngine
{
	public abstract class ThemedVirtualPathProviderViewEngine : IViewEngine
	{
		// format is ":ViewCacheEntry:{cacheType}:{theme}:{prefix}:{name}:{controllerName}:{areaName}:"
		private const string _cacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:{5}";
		private const string _cacheKeyPrefix_Master = "Master";
		private const string _cacheKeyPrefix_Partial = "Partial";
		private const string _cacheKeyPrefix_View = "View";
		private static readonly string[] _emptyLocations = new string[0];

		private VirtualPathProvider _vpp;
		internal Func<string, string> GetExtensionThunk = VirtualPathUtility.GetExtension;

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public string[] AreaMasterLocationFormats
		{
			get;
			set;
		}

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public string[] AreaPartialViewLocationFormats
		{
			get;
			set;
		}

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public string[] AreaViewLocationFormats
		{
			get;
			set;
		}

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public string[] FileExtensions
		{
			get;
			set;
		}

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public string[] MasterLocationFormats
		{
			get;
			set;
		}

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public string[] PartialViewLocationFormats
		{
			get;
			set;
		}

		public IViewLocationCache ViewLocationCache
		{
			get;
			set;
		}

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public string[] ViewLocationFormats
		{
			get;
			set;
		}

		protected VirtualPathProvider VirtualPathProvider
		{
			get
			{
				if (_vpp == null)
				{
					_vpp = HostingEnvironment.VirtualPathProvider;
				}
				return _vpp;
			}
			set
			{
				_vpp = value;
			}
		}

		protected ThemedVirtualPathProviderViewEngine()
		{
			if (HttpContext.Current == null || HttpContext.Current.IsDebuggingEnabled)
			{
				ViewLocationCache = DefaultViewLocationCache.Null;
			}
			else
			{
				ViewLocationCache = new DefaultViewLocationCache();
			}
		}

		private string CreateCacheKey(string theme,string prefix, string name, string controllerName, string areaName)
		{
			return String.Format(CultureInfo.InvariantCulture, _cacheKeyFormat,
				GetType().AssemblyQualifiedName,theme, prefix, name, controllerName, areaName);
		}

		protected abstract IView CreatePartialView(ControllerContext controllerContext, string partialPath);

		protected abstract IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath);

		protected virtual bool FileExists(ControllerContext controllerContext, string virtualPath)
		{
			return VirtualPathProvider.FileExists(virtualPath);
		}

		public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
		{
			if (controllerContext == null)
			{
				throw new ArgumentNullException("controllerContext");
			}
			if (String.IsNullOrEmpty(partialViewName))
			{
				throw new ArgumentException("Value cannot be null or empty", "partialViewName");
			}

			string[] searched;
			string controllerName = controllerContext.RouteData.GetRequiredString("controller");
			string partialPath = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, controllerName, _cacheKeyPrefix_Partial, useCache, out searched);

			if (String.IsNullOrEmpty(partialPath))
			{
				return new ViewEngineResult(searched);
			}

			return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
		}

		public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
		{
			if (controllerContext == null)
			{
				throw new ArgumentNullException("controllerContext");
			}
			if (String.IsNullOrEmpty(viewName))
			{
				throw new ArgumentException("Value cannot be null or empty", "viewName");
			}

			string[] viewLocationsSearched;
			string[] masterLocationsSearched;

			string controllerName = controllerContext.RouteData.GetRequiredString("controller");
			string viewPath = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats, "ViewLocationFormats", viewName, controllerName, _cacheKeyPrefix_View, useCache, out viewLocationsSearched);
			string masterPath = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats, "MasterLocationFormats", masterName, controllerName, _cacheKeyPrefix_Master, useCache, out masterLocationsSearched);

			if (String.IsNullOrEmpty(viewPath) || (String.IsNullOrEmpty(masterPath) && !String.IsNullOrEmpty(masterName)))
			{
				return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
			}

			return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
		}

		private string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
		{
			searchedLocations = _emptyLocations;

			if (String.IsNullOrEmpty(name))
			{
				return String.Empty;
			}

			string areaName = GetAreaName(controllerContext.RouteData);

		    string theme = controllerContext.HttpContext.GetCurrentTheme();
                //controllerContext.HttpContext.Items["theme"] as string;
			if (string.IsNullOrEmpty(theme)) theme = "default";
			
			bool usingAreas = !String.IsNullOrEmpty(areaName);
			List<ViewLocation> viewLocations = GetViewLocations(locations, (usingAreas) ? areaLocations : null);

			if (viewLocations.Count == 0)
			{
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
					"The property '{0}' cannot be null or empty.", locationsPropertyName));
			}

			bool nameRepresentsPath = IsSpecificPath(name);
			string cacheKey = CreateCacheKey(theme,cacheKeyPrefix, name, (nameRepresentsPath) ? String.Empty : controllerName, areaName);

			if (useCache)
			{
				return ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
			}

			return (nameRepresentsPath) ?
				GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations) :
				GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName,theme, cacheKey, ref searchedLocations);
		}

		private static string GetAreaName(RouteData routeData)
		{
			object area;

			if (routeData.DataTokens.TryGetValue("area", out area))
			{
				return area as string;
			}

			return GetAreaName(routeData.Route);
		}

		private static string GetAreaName(RouteBase route)
		{
			IRouteWithArea routeWithArea = route as IRouteWithArea;

			if (routeWithArea != null)
			{
				return routeWithArea.Area;
			}

			Route castRoute = route as Route;

			if (castRoute != null && castRoute.DataTokens != null)
			{
				return castRoute.DataTokens["area"] as string;
			}

			return null;
		}


		private string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string controllerName, string areaName,string theme, string cacheKey, ref string[] searchedLocations)
		{
			string result = String.Empty;
			searchedLocations = new string[locations.Count];

			for (int i = 0; i < locations.Count; i++)
			{
				ViewLocation location = locations[i];
				string virtualPath = location.Format(name, controllerName, areaName,theme);

				if (FileExists(controllerContext, virtualPath))
				{
					searchedLocations = _emptyLocations;
					result = virtualPath;
					ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
					break;
				}

				searchedLocations[i] = virtualPath;
			}

			return result;
		}

		private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
		{
			string result = name;

			if (!(FilePathIsSupported(name) && FileExists(controllerContext, name)))
			{
				result = String.Empty;
				searchedLocations = new[] { name };
			}

			ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
			return result;
		}

		private bool FilePathIsSupported(string virtualPath)
		{
			if (FileExtensions == null)
			{
				// legacy behavior for custom ViewEngine that might not set the FileExtensions property
				return true;
			}
			else
			{
				// get rid of the '.' because the FileExtensions property expects extensions withouth a dot.
				string extension = GetExtensionThunk(virtualPath).TrimStart('.');
				return FileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
			}
		}

		private static List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
		{
			List<ViewLocation> allLocations = new List<ViewLocation>();

			if (areaViewLocationFormats != null)
			{
				foreach (string areaViewLocationFormat in areaViewLocationFormats)
				{
					allLocations.Add(new AreaAwareViewLocation(areaViewLocationFormat));
				}
			}

			if (viewLocationFormats != null)
			{
				foreach (string viewLocationFormat in viewLocationFormats)
				{
					allLocations.Add(new ViewLocation(viewLocationFormat));
				}
			}

			return allLocations;
		}

		private static bool IsSpecificPath(string name)
		{
			char c = name[0];
			return (c == '~' || c == '/');
		}

		public virtual void ReleaseView(ControllerContext controllerContext, IView view)
		{
			IDisposable disposable = view as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		private class ViewLocation
		{

			protected string _virtualPathFormatString;

			public ViewLocation(string virtualPathFormatString)
			{
				_virtualPathFormatString = virtualPathFormatString;
			}

			public virtual string Format(string viewName, string controllerName, string areaName,string theme)
			{
				return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName,theme);
			}

		}

		private class AreaAwareViewLocation : ViewLocation
		{

			public AreaAwareViewLocation(string virtualPathFormatString)
				: base(virtualPathFormatString)
			{
			}

			public override string Format(string viewName, string controllerName, string areaName,string theme)
			{
				return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName,theme);
			}

		}
	}
}
