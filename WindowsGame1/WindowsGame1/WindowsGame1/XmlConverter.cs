using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace WindowsGame1
{
    public class XmlConverter<T> 
    {

        XmlSerializer serializer;
        public void Serialize(string pathToFile, T obj)
        {
            TextWriter writer = new StreamWriter(pathToFile);
            serializer = new XmlSerializer(typeof(T));

            using (writer)
            {
                serializer.Serialize(writer, obj);
            }
        }

        public T Deserialize(string pathToFile)
        {
            T result;

            TextReader reader = new StreamReader(pathToFile);
            serializer = new XmlSerializer(typeof(T));

            using (reader)
            {
                result = (T)serializer.Deserialize(reader);
            }

            return result;

        }
    }
}
