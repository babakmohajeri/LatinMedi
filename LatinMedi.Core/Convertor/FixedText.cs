using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.Convertor
{
    public static class FixedText
    {
        public static string FixedEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
