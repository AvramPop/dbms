using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    [XmlRoot(ElementName = "problem")]
    public class Problem
    {
        public string name { get; set; }
    }
}
