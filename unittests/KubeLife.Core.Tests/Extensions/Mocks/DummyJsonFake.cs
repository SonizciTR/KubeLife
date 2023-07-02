using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Core.Tests.Extensions.Mocks
{
    internal class DummyJsonFake
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static DummyJsonFake Generate() => new DummyJsonFake() { Id = 1, Name = "Test" };
    }
}
