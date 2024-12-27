/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.IO;

using UnityEditor;

#if UNITY_ANDROID && UNITY_EDITOR
using System.Collections.Generic;

[InitializeOnLoad]
public class OneSignalEditorScriptAndroid : AssetPostprocessor {

   static OneSignalEditorScriptAndroid() {
      createOneSignalAndroidManifest();
   }
   
   // Copies `AndroidManifestTemplate.xml` to `AndroidManifest.xml`
   //   then replace `${manifestApplicationId}` with current packagename in the Unity settings.
   private static void createOneSignalAndroidManifest() {
      string oneSignalConfigPath = "Assets/Plugins/Android/OneSignalConfig/";
      string manifestFullPath = oneSignalConfigPath + "AndroidManifest.xml";

      File.Copy(oneSignalConfigPath + "AndroidManifestTemplate.xml", manifestFullPath, true);

      StreamReader streamReader = new StreamReader(manifestFullPath);
      string body = streamReader.ReadToEnd();
      streamReader.Close();

      #if UNITY_5_6_OR_NEWER
         body = body.Replace("${manifestApplicationId}", PlayerSettings.applicationIdentifier);
      #else
         body = body.Replace("${manifestApplicationId}", PlayerSettings.bundleIdentifier);
      #endif
      using (var streamWriter = new StreamWriter(manifestFullPath, false)) {
         streamWriter.Write(body);
      }
   }
}
#endif
