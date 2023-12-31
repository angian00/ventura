using System.Collections.Generic;

namespace Ventura.Generators
{
    public class TemplatedStringGenerator : FileStringGenerator
    {
        public TemplatedStringGenerator(string sourceFile) : base(sourceFile) { }


        public string GenerateString(Dictionary<string, string> templateVars)
        {
            var templateStr = GenerateString();
            var res = templateStr;

            foreach (var key in templateVars.Keys)
            {
                res = templateStr.Replace($"[{key}]", templateVars[key]);
            }

            return res;
        }
    }
}
