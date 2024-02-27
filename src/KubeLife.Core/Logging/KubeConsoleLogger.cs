using System;

namespace KubeLife.Core.Logging
{
    public class KubeConsoleLogger : IKubeLogger
    {
        public void Error(string msg, Exception ex)
        {
            string tmp = $"[{DateTime.Now}] => {msg}";
            if(ex != null)
                tmp = $"[{DateTime.Now}] => {msg}. ExDetail=[{ex}]";

            Console.WriteLine(tmp);
        }
    }
}
