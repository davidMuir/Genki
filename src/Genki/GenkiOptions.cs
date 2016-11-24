using System;
using System.Collections.Generic;

namespace Genki
{
    public class GenkiOptions
    {
        public string ServiceName { get; set; }
        public string Endpoint { get; set; }
        public IEnumerable<Type> Steps { get; set; }
    }
}