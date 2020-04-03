using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public class Serializer
    {
        public T Deserialize<T>(string input, XmlRootAttribute xRoot) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T), xRoot);

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

    }
}
