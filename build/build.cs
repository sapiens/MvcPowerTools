#r "..\src\packages\MakeSharp.1.1.0\tools\MakeSharp.Windows.Helpers.dll"

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MakeSharp;
using MakeSharp.Windows.Helpers;
using NuGet;

// public class _
// {
    // public _()
    // {

        Project.StaticName = "MvcPowerTools";
        Solution.FileName = @"..\src\MvcPowerTools.sln";  
		
    // }
// }



 public class clean
 {
     public void Run()
     {
         
         BuildScript.TempDirectory.CleanupDir();
        Solution.Instance.FilePath.MsBuildClean();         
	 }

 }

[Default]
[Depends("clean")]
public class build
{

	public void Run()
    {
        Solution.Instance.FilePath.MsBuildRelease();
	}
}


[Depends("build")]
public class pack
{
    public ITaskContext Context {get;set;}

	public void Run()	
    {
       Project.Current.ReleaseDirOffset = "Net45";
        var nuspec = BuildScript.GetNuspecFile(Project.Current.Name);
        nuspec.Metadata.Version = Project.Current.GetAssemblySemanticVersion();
	    
//        Project.Current.AssemblyReleasePath.ToConsole();
        var deps = new ExplicitDependencyVersion_(Project.Current);
        deps.UpdateDependencies(nuspec);
        
        var tempDir = BuildScript.GetProjectTempDirectory(Project.Current);
	    var projDir = Path.Combine(Project.Current.Solution.Directory, Project.Current.Name);
        var nupkg=nuspec.Save(tempDir).CreateNuget(projDir,tempDir);
	    Context.Data["pack"] = nupkg;
    }
}


[Depends("pack")]
public class push
{
    public ITaskContext Context { get; set; }

    
    public void Run()
    {
     return;
        var nupkg=Context.Data.GetValue<string>("pack");
     
        BuildScript.NugetExePath.Exec("push", nupkg);
    }
}




class ExplicitDependencyVersion_
{
    private readonly Project _project;

    public ExplicitDependencyVersion_(Project project)
    {
        _project = project;
    }

    public void UpdateDependencies(NuSpecFile nuspec)
    {
         nuspec.Metadata.DependencySets[0].Dependencies.Where(d=>d.Version.Contains("replace"))
         .ForEach(d=> 
                d.Version=_project.ReleasePathForAssembly(d.Id+".dll").GetAssemblyVersion().ToString());
    }
}


