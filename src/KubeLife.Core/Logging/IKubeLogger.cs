using System;
using System.Collections.Generic;
using System.Text;

namespace KubeLife.Core.Logging
{
    public interface IKubeLogger
    {
        void Error(string msg, Exception ex);
    }
}
