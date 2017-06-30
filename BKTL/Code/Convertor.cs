//using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BKTL
{
    static public class Convertor
    {
        //static public string JsonToObject<T>(object data, out T obj) where T : class
        //{
        //    obj = default(T);

        //    try
        //    {
        //        obj = JsonConvert.DeserializeObject<T>(Convert.ToString(data));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //    return "";
        //}
        //static public string ObjectToJson(object obj, out string value)
        //{
        //    value = "";
        //    try
        //    {
        //        value = JsonConvert.SerializeObject(obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //    return "";
        //}

        /// <summary>
        /// Serialize và chuyển sang Base64 1 object bất kỳ (object cần có thuộc tính Serializable)
        /// </summary>
        /// <param name="obj">object cần chuyển sang Base64</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ObjectToBase64String(object obj, out string value)
        {
            string msg = "";
            value = null;

            if (obj == null) return "Error: Object is null @ObjectToBase64String";

            try
            {
                value = ObjectToBase64String(obj);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }
        public static string ObjectToBase64String(object obj)
        {
            if (obj == null) return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
        public static string ObjectToXML(Object obj, string prefixXML, string namespaceXML, out string xml)
        {
            string msg = "";
            xml = "";
            try
            {
                using (var sw = new Utf8StringWriter())
                {
                    using (var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
                    {
                        xw.WriteStartDocument(true); // that bool parameter is called "standalone"
                        var namespaces = new XmlSerializerNamespaces();
                        namespaces.Add(prefixXML, namespaceXML);
                        var xmlSerializer = new XmlSerializer(obj.GetType());
                        xmlSerializer.Serialize(xw, obj, namespaces);

                        xml = sw.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }


        static public string StringToMD5(string s, out string value)
        {
            string msg = "";
            value = null;
            try
            {
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(s);
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                value = sb.ToString();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

        /// <summary>
        /// Deserialize từ Base64 về object 
        /// </summary>
        /// <param name="base64">Chuỗi cần chuyển về object</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64StringToObject(string base64, out object value)
        {
            string msg = "";
            value = null;

            try
            {
                value = Base64StringToObject(base64);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }
        public static object Base64StringToObject(string base64)
        {
            byte[] arrBytes = Convert.FromBase64String(base64);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = bf.Deserialize(memStream);
                return obj;
            }
        }

        public static string Base64StringToFile(string base64Content, string filePath)
        {
            string msg = "";
            try
            {
                byte[] content = Convert.FromBase64String(base64Content);
                File.WriteAllBytes(filePath, content);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

        static public string StringToGuid(string guid, out Guid value)
        {
            return ObjectToGuid(guid, out value);
        }
        static public string ObjectToGuid(object guid, out Guid value)
        {
            value = Guid.Empty;
            try
            {
                value = new Guid(guid.ToString());
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }
        static public Guid ToGuid(this object obj, Guid defaultValue)
        {
            try
            {
                return Guid.Parse(obj.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        static public string StringToNumber(string s, out long value)
        {
            value = 0;
            try
            {
                value = long.Parse(s);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }
        static public long ToNumber(this object obj, long defaultvalue)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        static public string StringToNumber(string s, out int value)
        {
            value = 0;
            try
            {
                value = int.Parse(s);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }
        static public int ToNumber(this object obj, int defaultvalue)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        static public string StringToDatetime(string s, out double value)
        {
            value = 0;
            try
            {
                value = double.Parse(s);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }
        static public string StringToDatetime(string s, out DateTime value)
        {
            value = DateTime.MinValue;
            try
            {
                value = DateTime.Parse(s);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }

        static public string ToString(object obj, string defaultvalue)
        {
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }
        static public bool ToBoolean(this object obj, bool defaultvalue)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }
    }
}
