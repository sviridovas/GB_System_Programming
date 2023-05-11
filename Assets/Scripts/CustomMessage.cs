using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class CustomMessage
{
    public string type;
    public string message;

    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
    public static CustomMessage ByteArraToObject(byte[] arr, int index, int count)
    {
        if (arr == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(arr, index, count))
        {
            return bf.Deserialize(ms) as CustomMessage;
        }
    }
}
