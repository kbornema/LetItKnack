// C# example:
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Diagnostics;

public static class MyBuildPostProcess 
{
    [MenuItem("Build/All")]
    public static void Foo()
    {
        Build(BuildTarget.StandaloneWindows64, "Win64", ".exe");
        Build(BuildTarget.StandaloneLinux64, "Linux64", ".x86_64");
        Build(BuildTarget.StandaloneOSX, "Mac");
        Process.Start("zipThem.cmd");
    }

    private static string[] GetScenePaths()
    {
        var scenes = EditorBuildSettings.scenes;
        string[] sceneNames = new string[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
            sceneNames[i] = scenes[i].path;

        return sceneNames;
    }

    private static void Build(BuildTarget target, string subfolderName, string ending = "")
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetScenePaths(),
            locationPathName = "Build/" + subfolderName + "/" + PlayerSettings.productName + ending,
            target = target,
            options = BuildOptions.StrictMode
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    //[PostProcessBuild(1)]
    //private static void PostProcessFunc(BuildTarget target, string pathToBuiltProject)
    //{
    //    
    //}
}