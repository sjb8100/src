using System;
using System.Collections.Generic;
using System.Security;

namespace Mono.Xml
{
    public class XmlParser
    {
        public static SecurityElement Parser(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            SecurityParser sp = new SecurityParser();
            sp.LoadXml(xml);
            SecurityElement se = sp.ToXml();
            return se;
        }
    }
}
