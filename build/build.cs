#r "..\src\packages\MakeSharp.1.1.0\tools\MakeSharp.Windows.Helpers.dll"

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using MakeSharp;
using MakeSharp.Windows.Helpers;
using NuGet;

// public class _
// {
    // public _()
    // {

        
       
		
    // }
// }

public class PowerToolsInit : IScriptParams
{
    public PowerToolsInit()
    {
        ScriptParams=new Dictionary<int, string>();
        Solution.FileName = @"..\src\MvcPowerTools.sln";  
    }

    List<Project> _projects=new List<Project>();

    public IEnumerable<Project> GetProjects()
    {
        if (_projects.Count == 0)
        {
            bool mvc = ScriptParams.Values.Contains("mvc");
            bool api = ScriptParams.Values.Contains("api");
            bool all = !mvc && !api;
            if (mvc || all)
            {
                _projects.Add(new Project("MvcPowerTools",Solution.Instance){ReleaseDirOffset = "net45"});
            }
            if (api || all)
            {
                _projects.Add(new Project("WebApiPowerTools",Solution.Instance){ReleaseDirOffset = "net45"});
            }
        }
        return _projects;
    }

    public IDictionary<int, string> ScriptParams { get; private set; }
}


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

    void Pack(Project project)
    {
        var nuspec = BuildScript.GetNuspecFile(project.Name);
        nuspec.Metadata.Version = project.GetAssemblySemanticVersion("pre");
	    
        var deps = new ExplicitDependencyVersion_(project);
        deps.UpdateDependencies(nuspec);
        
        var tempDir = BuildScript.GetProjectTempDirectory(project);
	    var projDir = Path.Combine(project.Solution.Directory, project.Name);
        var nupkg=nuspec.Save(tempDir).CreateNuget(projDir,tempDir);
	    Context.Data[project.Name+"pack"] = nupkg;
    }

  
	public void Run()	
    {

	    foreach (var project in Context.InitData.As<PowerToolsInit>().GetProjects())
	    {
	        "Packing {0} ".WriteInfo(project.Name);
            Pack(project);
	    }
      
    }
}


[Depends("pack")]
public class push
{
    public ITaskContext Context { get; set; }

    
    public void Run()
    {
     //return;
        foreach (var project in Context.InitData.As<PowerToolsInit>().GetProjects())
	    {
	        var nupkg=Context.Data.GetValue<string>(project.Name+"pack");     
            BuildScript.NugetExePath.Exec("push", nupkg);
	    }
      
        
       
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


