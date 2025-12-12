
using System.IO;
using UnityEditor;
using UnityEngine;



    public static class ScreenshotEditor
    {
        /// <summary>
        /// Shortcut Command(Ctrl) + G
        /// </summary>
      //  [MenuItem("ScreenshotEditor/Take Screenshot %g")]
        private static void TakeScreenshot()
        {
            const int superSize = 1;
            
            const string extension = ".png";
            const string folder = "Screenshots";
            
            var date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var path = Application.dataPath.Replace("Assets", "");
            
            date += extension;

            path = Path.Combine(path, folder);
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, date);

            if (File.Exists(path))
            {
                Debug.LogWarning($"File in path:{path} exist");
                return;
            }
            
           // if (!RichUnity.IsPlayMode())
         //   {
           //     Debug.LogWarning("Run play mode");
           //     return;
          //  }
            
            ScreenCapture.CaptureScreenshot(path, superSize);
            
            Debug.Log($"<color=green>Screenshot created in {path}</color>");
        }
    }