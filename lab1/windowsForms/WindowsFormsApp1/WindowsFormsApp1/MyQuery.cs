using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    [XmlType("query", IncludeInSchema = true)]
    public class MyQuery
	{
		public string relation { get; set; }
		public string parent { get; set; }
		public string child { get; set; }
		public string select_parent { get; set; }
		public string select_children { get; set; }
		public string add_children { get; set; }
		public string remove_children { get; set; }
		public string update_children { get; set; }

	}
}
