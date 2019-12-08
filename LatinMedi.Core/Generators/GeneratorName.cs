using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.Generators
{
    public class GeneratorName
    {
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
