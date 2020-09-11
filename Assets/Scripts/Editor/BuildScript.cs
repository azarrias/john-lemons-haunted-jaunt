using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Linq;

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
		BuildPipeline.BuildPlayer(SCENES, OUTPUT_PATH + RELEASE_SUBFOLDER + "win_x86/" + FILE_NAME + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
	}

	[UnityEditor.MenuItem("Util/Build StandaloneWindows64")]
	static void BuildStandaloneWindows64()
	{
		BuildPipeline.BuildPlayer(SCENES, OUTPUT_PATH + RELEASE_SUBFOLDER + "win_x64/" + FILE_NAME + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
	}

	[UnityEditor.MenuItem("Util/Build StandaloneLinux")]
	static void BuildStandaloneLinux()
	{
		BuildPipeline.BuildPlayer(SCENES, OUTPUT_PATH + RELEASE_SUBFOLDER + "lin_x86/" + FILE_NAME + ".x86", BuildTarget.StandaloneLinux, BuildOptions.None);
	}

	[UnityEditor.MenuItem("Util/Build StandaloneLinux64")]
	static void BuildStandaloneLinux64()
	{
		BuildPipeline.BuildPlayer(SCENES, OUTPUT_PATH + RELEASE_SUBFOLDER + "lin_x64/" + FILE_NAME + ".x86_64", BuildTarget.StandaloneLinux64, BuildOptions.None);
	}

	[UnityEditor.MenuItem("Util/Build WebGL")]
	static void BuildWebGL()
	{
		BuildPipeline.BuildPlayer(SCENES, OUTPUT_PATH + RELEASE_SUBFOLDER + "webgl", BuildTarget.WebGL, BuildOptions.None);
	}

	[UnityEditor.MenuItem("Util/Build All")]
	static void BuildAll()
	{
		BuildStandaloneWindows();
		BuildStandaloneWindows64();
		BuildStandaloneLinux();
		BuildStandaloneLinux64();
		BuildWebGL();
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
}
