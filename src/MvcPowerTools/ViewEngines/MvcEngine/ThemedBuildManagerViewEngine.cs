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
using System.Web.Mvc;

namespace MvcPowerTools.ViewEngines.MvcEngine
{
	public abstract class ThemedBuildManagerViewEngine : ThemedVirtualPathProviderViewEngine
	{
		private IBuildManager _buildManager;
		private IViewPageActivator _viewPageActivator;
		private IResolver<IViewPageActivator> _activatorResolver;

		protected ThemedBuildManagerViewEngine()
			: this(null, null, null)
		{
		}

		protected ThemedBuildManagerViewEngine(IViewPageActivator viewPageActivator)
			: this(viewPageActivator, null, null)
		{
		}

		internal ThemedBuildManagerViewEngine(IViewPageActivator viewPageActivator, IResolver<IViewPageActivator> activatorResolver, IDependencyResolver dependencyResolver)
		{
			if (viewPageActivator != null)
			{
				_viewPageActivator = viewPageActivator;
			}
			else
			{
				_activatorResolver = activatorResolver ?? new SingleServiceResolver<IViewPageActivator>(
				                                          	() => null,
				                                          	new DefaultViewPageActivator(dependencyResolver),
				                                          	"BuildManagerViewEngine constructor"
				                                          	);
			}
		}

		internal IBuildManager BuildManager
		{
			get
			{
				if (_buildManager == null)
					_buildManager = new BuildManagerWrapper();
				return _buildManager;
			}
			set
			{
				_buildManager = value;
			}
		}

		protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
		{
			return BuildManager.FileExists(virtualPath);
		}

		protected IViewPageActivator ViewPageActivator
		{
			get
			{
				if (_viewPageActivator != null)
				{
					return _viewPageActivator;
				}
				_viewPageActivator = _activatorResolver.Current;
				return _viewPageActivator;
			}
		}

		internal class DefaultViewPageActivator : IViewPageActivator
		{
			Func<IDependencyResolver> _resolverThunk;

			public DefaultViewPageActivator()
				: this(null)
			{
			}

			public DefaultViewPageActivator(IDependencyResolver resolver)
			{
				if (resolver == null)
				{
					_resolverThunk = () => DependencyResolver.Current;
				}
				else
				{
					_resolverThunk = () => resolver;
				}
			}

			public object Create(ControllerContext controllerContext, Type type)
			{
				return _resolverThunk().GetService(type) ?? Activator.CreateInstance(type);
			}
		}
	}
}