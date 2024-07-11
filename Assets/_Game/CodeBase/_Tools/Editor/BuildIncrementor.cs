using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CodeBase._Tools.Editor
{
    public class BuildIncrementor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; } = 1;

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_EDITOR
            PlayerSettings.macOS.buildNumber = IncrementBuildIndexString(PlayerSettings.macOS.buildNumber);
            PlayerSettings.iOS.buildNumber = IncrementBuildIndexString( PlayerSettings.iOS.buildNumber);
            PlayerSettings.Android.bundleVersionCode++;
#endif
        }

        private string IncrementBuildIndexString(string index)
        {
            int.TryParse(index, out int indexToIncrement);
            indexToIncrement++;
            return indexToIncrement.ToString();
        }
    }
}