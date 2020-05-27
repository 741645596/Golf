using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;

namespace IGG.EditorTools
{
	public class BuildiOSPlayer
	{
		[PostProcessBuild]
		static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
		{
			ChangeProject(pathToBuildProject);
			ChangePlist(pathToBuildProject);
		}

		static void ChangeProject(string pathToBuildProject)
		{
			string path = PBXProject.GetPBXProjectPath(pathToBuildProject);

			PBXProject pbx = new PBXProject();
			pbx.ReadFromString(File.ReadAllText(path));

			string target = pbx.TargetGuidByName("Unity-iPhone");
			if (!string.IsNullOrEmpty(target))
			{
				pbx.AddFrameworkToProject(target, "AdSupport.framework", true);
				pbx.AddFrameworkToProject(target, "CoreTelephony.framework", true);
				pbx.AddFrameworkToProject(target, "GameKit.framework", true);
				pbx.AddFrameworkToProject(target, "MobileCoreServices.framework", true);
				pbx.AddFrameworkToProject(target, "SystemConfiguration.framework", true);

				//pbx.AddFileToBuild(target, pbx.AddFile("IGGSDK.framework", "Frameworks/IGGSDK.framework", PBXSourceTree.Source));

				pbx.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
				pbx.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)");

				pbx.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

				// 技术部文档要求用-all_load,但是wwise会报链接时会报符号重复.后面有需要可以使用-force_load xxx.a
				//pbx.SetBuildProperty(target, "OTHER_LDFLAGS", "-all_load");
			}

			File.WriteAllText(path, pbx.WriteToString());
		}

		static void ChangePlist(string pathToBuildProject)
		{
			string path = string.Format("{0}/Info.plist", pathToBuildProject);
			PlistDocument doc = new PlistDocument();

			doc.ReadFromString(File.ReadAllText(path));
			PlistElementDict root = doc.root;

			// 对iOS9的影响
			{
				PlistElementDict security = root.CreateDict("NSAppTransportSecurity");
				security.SetBoolean("NSAllowsArbitraryLoads", true);
			}

			// 合规证明
			{
				root.SetBoolean("ITSAppUsesNonExemptEncryption", false);
			}

			// 添加Scheme白名单
			{
				PlistElementArray schemes = root.CreateArray("LSApplicationQueriesSchemes");
				schemes.AddString("fbapi");
				schemes.AddString("fbauth2");
				schemes.AddString("fb-messenger-api");
			}

			// fb
			{
				string idFacebook = "212915212149962";

				root.SetString("FacebookAppID", idFacebook);
				root.SetString("FacebookDisplayName", PlayerSettings.productName);

				PlistElementArray types = root.CreateArray("CFBundleURLTypes");

				PlistElementDict dict = types.AddDict();
				dict.SetString("CFBundleTypeRole", "Editor");

				PlistElementArray schemes = dict.CreateArray("CFBundleURLSchemes");
				schemes.AddString("fb" + idFacebook);
			}

			// 权限
			{
				root.SetString("NSPhotoLibraryUsageDescription", "使用相册");
				root.SetString("NSCameraUsageDescription", "使用相机");

			}

			File.WriteAllText(path, doc.WriteToString());
		}
	}
}
#endif