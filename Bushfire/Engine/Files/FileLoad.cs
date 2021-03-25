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
    static class FileLoad
    {
        public static object LoadXmlFile(Type objectType, string fileName)
        {
            //TODO: Add exceptions for no files
            object myObject = null;
            string fullPath = Data.gameFolder + fileName;

            if (File.Exists(fullPath))
            {
                try
                {
                    using (FileStream stream = File.OpenRead(fileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(objectType);
                        myObject = serializer.Deserialize(stream);
                    }
                }
                catch
                {
                    return myObject;
                }
            }
            return myObject;
        }

        public static object LoadBinaryFile(string folder, string fileName)
        {
            string fullPath = Data.gameFolder + folder + fileName;
            FileStream fs;
            object returnObject;

            if (File.Exists(fullPath))
            {
                fs = new FileStream(fullPath, FileMode.Open);

                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    returnObject = formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
                return returnObject;
            }
            return null;           
        }
    }
}