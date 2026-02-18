#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
#elif UNITY_IPHONE

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using System.IO;

public class AdfurikunMovieRewardPostProcess {

    private static bool useAppLovin = true;
    private static bool useMaio = true;
    private static bool useAfio = true;
    private static bool useAdMob = true;
    private static bool useInMobi = true;
    
    // ビルド時に実行される
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {

        if (buildTarget == BuildTarget.iOS) {
        
             string projectPath = PBXProject.GetPBXProjectPath(path);
             PBXProject project = new PBXProject();
             project.ReadFromFile(projectPath);
             string mainTargetGuid = project.GetUnityMainTargetGuid();

            // AppLovinを導入する場合にはこちらのコードをPostProcessに追加する
            if (useAppLovin) {
                string framework = "AppLovinSDK";
                string sdkVersion = "13.5.1"; // 導入予定のAppLovin SDK Versionを設定する
                string frameworkDir = "AppLovinSDK/applovin-ios-sdk-" + sdkVersion; 
                AddEmbeddedFramework(project, mainTargetGuid, framework, frameworkDir);
            }
            // maioを導入する場合にはこちらのコードをPostProcessに追加する
            if (useMaio) {
                string framework = "Maio";
                string frameworkDir = "MaioSDK-v2";
                AddEmbeddedFramework(project, mainTargetGuid, framework, frameworkDir);
            }
            // Afioを導入する場合にはこちらのコードをPostProcessに追加する
            if (useAfio) {
                string framework = "OMSDK_Cyberagentcojp3";
                string frameworkDir = "AMoAd";
                AddEmbeddedFramework(project, mainTargetGuid, framework, frameworkDir);
            }
            // InMobiを導入する場合にはこちらのコードをPostProcessに追加する
            if (useInMobi) {
                string framework = "InMobiSDK";
                string frameworkDir = "InMobiSDK";
                AddEmbeddedFramework(project, mainTargetGuid, framework, frameworkDir);
            }
            // AdMobを導入する場合、plistにGADApplicationIdentifierを設定するようにする
            if(useAdMob){
                string plistPath = Path.Combine (path, "Info.plist");
                var plist = new PlistDocument ();
                plist.ReadFromFile (plistPath);
                // AdMob管理画面で発行されているGADApplicationIdentifierを設定する
                string applicationIdentifier = "ca-app-pub-3940256099942544~1458002511";
                plist.root.SetString("GADApplicationIdentifier", applicationIdentifier);
                plist.WriteToFile (plistPath);
            }
             // Write.
             project.WriteToFile(projectPath);
        }
    }

    private static void AddEmbeddedFramework(PBXProject project, string targetGuid, string framework, string frameworkDir) {
        string frameworkFileName = framework + ".xcframework";        
        var src = Path.Combine("Pods", frameworkDir, frameworkFileName);
        var frameworkPath = project.AddFile(src, src);
        project.AddFileToEmbedFrameworks(targetGuid, frameworkPath);        
    }
}
#endif
