using System;
using System.Collections;
using System.IO;
using System.Web.Compilation;

namespace MvcPowerTools.ViewEngines.MvcEngine
{
	public class BuildManagerWrapper : IBuildManager
    {
        public bool FileExists(string virtualPath)
        {
			return BuildManager.GetObjectFactory(virtualPath, false) != null;
        }

        public Type GetCompiledType(string virtualPath)
        {
            return BuildManager.GetCompiledType(virtualPath);
        }

		public ICollection GetReferencedAssemblies()
		{
			return BuildManager.GetReferencedAssemblies();
		}

		public Stream ReadCachedFile(string fileName)
		{
			return BuildManager.ReadCachedFile(fileName);
		}

		public Stream CreateCachedFile(string fileName)
		{
			return BuildManager.CreateCachedFile(fileName);
		}
    }
}