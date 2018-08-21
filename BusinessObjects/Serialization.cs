using System;

namespace BusinessObjects
{
    public class Serialization
    {
        #region Deserialize data
        public object DeSerializeBinary(string request)
        {
            object functionReturnValue = null;
            byte[] bytes = null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                bytes = Convert.FromBase64String(request);
                System.IO.MemoryStream memStream = new System.IO.MemoryStream(bytes);
                functionReturnValue = serializer.Deserialize(memStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                serializer = null;
                bytes = null;
            }
            return functionReturnValue;
        }
        #endregion

        #region Serialize data
        public string SerializeBinary(object request)
        {
            string functionReturnValue = null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            byte[] bytes = null;
            try
            {
                serializer.Serialize(memStream, request);
                bytes = memStream.GetBuffer();
                functionReturnValue = Convert.ToBase64String(bytes);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                serializer = null;
                memStream = null;
                bytes = null;
            }
            return functionReturnValue;
        }
        #endregion
    }
}
