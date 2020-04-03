using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Serializer ser = new Serializer();
            string path = string.Empty;
            string xmlInputData = string.Empty;
            path = "C:\\Users\\Dani\\Desktop\\db\\lab1\\windowsForms\\WindowsFormsApp1\\WindowsFormsApp1\\queries.xml";
            xmlInputData = File.ReadAllText(path);
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "data";
            xRoot.IsNullable = true;
            Queries queries = ser.Deserialize<Queries>(xmlInputData, xRoot);

            path = "C:\\Users\\Dani\\Desktop\\db\\lab1\\windowsForms\\WindowsFormsApp1\\WindowsFormsApp1\\Problem.xml";
            xmlInputData = File.ReadAllText(path);
            xRoot.ElementName = "problem";
            Problem problem = ser.Deserialize<Problem>(xmlInputData, xRoot);
            MyQuery queryData = null;
            foreach(MyQuery q in queries.queries)
            {
                if (q.relation == problem.name) queryData = q;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(queryData));

            
        }
    }
}
