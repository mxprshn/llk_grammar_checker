using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LLkGrammarChecker.Parser
{
    interface IParser
    {
        public Cfg FromFile(string path);

        public Cfg FromString(string source);
    }
}
