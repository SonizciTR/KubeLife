using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Core.Tests.Extensions.Mocks
{
    internal class SimpleClass1
    {
        public string Name1 { get; set; }

        public SimpleClass1()
        {            
        }
        public SimpleClass1(string name)
        {
            Name1 = name;
        }
    }
}
