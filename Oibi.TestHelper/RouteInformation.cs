using System;
using System.Collections.Generic;
using System.Text;

namespace Oibi.TestHelper
{
    /// <summary>
    /// Route informations
    /// </summary>
    /// <example>https://github.com/kobake/AspNetCore.RouteAnalyzer/blob/01f0194f15e724ddbbb4a670e5f94f73cf313b40/AspNetCore.RouteAnalyzer/RouteInformation.cs</example>
    public class RouteInformation
    {
        public string HttpMethod { get; set; }
        public string Area { get; set; }
        public string Path { get; set; }
        public string Invocation { get; set; }
    }
}