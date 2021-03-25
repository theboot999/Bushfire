using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BushFire.Engine.Files
{
    static class FileSave
    {
        public static void SaveXmlFile(object objectClass, string fileName)
        {
            //TODO: Add Exceptions for file not found
            string fullPath = Data.gameFolder + fileName;

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                XmlSerializer serializer = new XmlSerializer(objectClass.GetType());
                serializer.Serialize(writer, objectClass);
            }
        }

        public static void SaveBinaryFile(object objectClass, string folder, string fileName)
        {
            //TODO: Add Exceptions for file not found
            string fullPath = Data.gameFolder + folder;
            Directory.CreateDirectory(fullPath);

            fullPath = Data.gameFolder + folder + fileName;
            FileStream fs = new FileStream(fullPath, FileMode.Create);

            try
            {

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, objectClass);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
     

    }
}
