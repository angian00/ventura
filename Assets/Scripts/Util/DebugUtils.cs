
namespace Ventura.Util
{

    public class DebugUtils
    {
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        public static void Warning(string msg)
        {
            UnityEngine.Debug.Log("!!! " + msg);
        }

        public static void Error(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }

    }
}
