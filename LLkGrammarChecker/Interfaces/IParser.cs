using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LLkGrammarChecker.Parser
{
    interface IParser
    {
        public Task<CFG> FromFile(string path);

        public CFG FromString(string source);
    }
}
