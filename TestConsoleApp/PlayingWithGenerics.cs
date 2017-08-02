using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class PlayingWithGenerics
    {
        class myClass<T>
        where T : new()
        {
            public T type { get; set; }
            public string Name { get; set; }

            public myClass()
            {
                type = new T();
            }

            public string WriteType()
            {
                return this.ToString();
            }

        }

        class One : Number
        {
            public const string NAME = "Ben";

            public override string SayMyName()
            {
                return NAME;
            }
        }

        class Two : Number
        {
            public const string NAME = "Mickey";

            public override string SayMyName()
            {
                return NAME;
            }
        }

        class Three : Number
        {
            private const string NAME = "Donald";

            public override string SayMyName()
            {
                return NAME;
            }
        }

        class Number
        {
            private const string NAME = "NumberBase";
            public virtual string SayMyName()
            {
                return NAME;
            }
        }
    }
}
