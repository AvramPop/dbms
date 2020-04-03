using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    [XmlRoot(ElementName = "data")]
    public class Queries
    {
        [XmlArray("queries")]
        public List<MyQuery> queries { get; set; }
    }
}
