//uncomment to include reference to other assembly you might need (one directive per each)
//#r "mycustom.dll"
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CSake;
using NuGet;   //comment it out if you don't create nuget packs or manipulate nuspec files

//don't use namespaces
//any class you create will be considered an inner class of the CSakeWrapper class

const string SlnFile=@"../src/MvcPowerTools.sln";

const string SlnDir=@"../src";

const string TempDir=@"temp";

const bool Stable=false;

//for nuget
const string PackageDir = @"temp/package";

static string NugetExe= Path.GetFullPath(SlnDir+"/.nuget/nuget.exe");

static string CurrentDir=Path.GetFullPath("./");

static string[] Projects=new[]{"MvcPowerTools"};

static string ReleaseDir=Path.Combine(SlnDir,"MvcPowerTools","bin/Release");

static bool Built=false;

[SkipIf("Built",true)]
public static void CleanUp()
{
    TempDir.CleanupDir();
    SlnFile.MsBuildClean();           
}


[Depends("CleanUp")] 
[SkipIf("Built",true)]
public static void Build()
{
    SlnFile.MsBuildRelease();
    Built=true;
}

[Default]
[Depends("Build")] 
public static void Local()
{
    foreach(var project in Projects)
    {
        var releaseDir=Path.GetFullPath(Path.Combine(SlnDir,project,"bin/Release"));
        "robocopy".Exec(releaseDir,TempDir,"/E","/XN","/NS","/NC","/NJH","/NJS");
    }
}

[Depends("Build")]
[SkipIf("Packed",true)]
public static void Pack()
{
   Pack("MvcPowerTools",new[]{"CavemanTools"});

}

static bool Packed=false;

static void Pack(string project,string[] deps=null)
{
    PackageDir.MkDir();
    var nuspecFile=Path.Combine(CurrentDir,project+".nuspec");
    
    Dictionary<string,string> depsVersions=new Dictionary<string,string>();
    if (deps!=null)
    {
        foreach(var dep in deps)
        {
            depsVersions[dep]=GetDepVersion(dep);             
        }
    }
    
    UpdateVersion(nuspecFile,project,depsVersions);
    BuildNuget(project,Path.Combine(SlnDir,project));
	Packed=true;
}


[Depends("Pack")]
public static void Push()
{
var project=Projects[0];
var nupkg= Path.GetFullPath(Path.Combine(PackageDir,project+"."+GetVersion(project)+".nupkg"));
Nuge.Exec("push",nupkg);
}
//------------------------------ Utils ----------------

//updates version in nuspec file
static void UpdateVersion(string nuspecFile,string assemblyName,Dictionary<string,string> localDeps=null)
{
    var nuspec=nuspecFile.AsNuspec();   
    nuspec.Metadata.Version=GetVersion(assemblyName);
    if (localDeps!=null)
    {
        foreach(var kv in localDeps)
        {
            nuspec.AddDependency(kv.Key,kv.Value);
        }
    }
    nuspec.Save(TempDir);    
}

//basePath= relative path for package files source. Usually is the project dir.
static void BuildNuget(string nuspecFile,string basePath)
{
    //if (!nuspecFile.EndsWith(".nuspec"))
    //{
      var project=nuspecFile;  
        nuspecFile+=".nuspec";
    //}
    Path.Combine(TempDir,nuspecFile).CreateNuget(basePath,PackageDir);    
}

static string GetDepVersion(string asmName)
{
    return Path.Combine(ReleaseDir,"Net45",asmName+".dll").GetAssemblyVersion().ToSemanticVersion().ToString();
}


static string GetVersion(string asmName)
{
    return Path.Combine(GetReleaseDir(asmName),"Net45",asmName+".dll").GetAssemblyVersion().ToSemanticVersion(Stable?null:"pre").ToString();
}

static string GetReleaseDir(string project)
{
 return Path.Combine(SlnDir,project,"bin","Release");
}
