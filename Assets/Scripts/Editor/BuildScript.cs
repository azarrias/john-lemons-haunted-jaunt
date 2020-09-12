using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;
using System.Linq;
using System;

static public class BuildScript 
{
	static string GAME_TITLE = Application.productName;
	static string FILE_NAME = GAME_TITLE.RemoveSpecialCharacters();
	static string VERSION_FILE = File.ReadAllLines("./VERSION").First();
	static string VERSION = VERSION_FILE.Split(new string[] { "version=" }, System.StringSplitOptions.None).Last();
	static string OUTPUT_PATH = "./build/";
	static string RELEASE_SUBFOLDER = FILE_NAME + "-v" + VERSION + '-';
	static string[] SCENES = GetScenes();

	[UnityEditor.MenuItem("Util/Build StandaloneWindows")]
	static void BuildStandaloneWindows() 
	{
		DoBuild(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows, OUTPUT_PATH + RELEASE_SUBFOLDER + "win_x86/" + FILE_NAME + ".exe");
	}

	[UnityEditor.MenuItem("Util/Build StandaloneWindows64")]
	static void BuildStandaloneWindows64()
	{
		DoBuild(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, OUTPUT_PATH + RELEASE_SUBFOLDER + "win_x64/" + FILE_NAME + ".exe");
	}

	[UnityEditor.MenuItem("Util/Build StandaloneLinux64")]
	static void BuildStandaloneLinux64()
	{
		DoBuild(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64, OUTPUT_PATH + RELEASE_SUBFOLDER + "lin_x64/" + FILE_NAME + ".x86_64");
	}

	[UnityEditor.MenuItem("Util/Build WebGL")]
	static void BuildWebGL()
	{
		DoBuild(BuildTargetGroup.WebGL, BuildTarget.WebGL, OUTPUT_PATH + RELEASE_SUBFOLDER + "webgl");
	}

	[UnityEditor.MenuItem("Util/Build All")]
	static void BuildAll()
	{
		BuildStandaloneWindows();
		BuildStandaloneWindows64();
		//BuildStandaloneLinux();
		BuildStandaloneLinux64();
		BuildWebGL();
	}

	private static void DoBuild(BuildTargetGroup targetGroup, BuildTarget target, string path)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, target);
		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = SCENES;
		buildPlayerOptions.locationPathName = path;
		buildPlayerOptions.target = target;
		buildPlayerOptions.options = BuildOptions.None;

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		WriteSummaryLog(report.summary);
	}

	static string[] GetScenes()
	{
		List<string> scenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (scene.enabled)
				scenes.Add(scene.path);
		}
		return scenes.ToArray();
	}

	public static string RemoveSpecialCharacters(this string str)
	{
		StringBuilder sb = new StringBuilder();
		foreach (char c in str)
		{
			if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
			{
				sb.Append(c);
			}
			else
			{
				sb.Append('_');
			}
		}
		return sb.ToString();
	}

	static void WriteSummaryLog(BuildSummary summary)
	{
		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("Build failed");
		}
	}
}
