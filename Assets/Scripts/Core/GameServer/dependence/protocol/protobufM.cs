using ProtoBuf;
using System.IO;
using System;
using System.Collections.Generic;

public class protobufM
{
    private static Dictionary<eMsgTypes, Type> g_TypeDict = null;
    private static MemoryStream g_ms = null;
    // 序列化
    public static byte[] Serializerobject<T>(T obj)
    {
        MemoryStream networkStream = new MemoryStream();
        
        ProtoBuf.Serializer.Serialize<T> (networkStream, obj);
        
        return networkStream.ToArray();
    }
    
    
    // 反序列化
    public static object Deserialize(string ProtobufTypenNme, byte[] data, int index, int length)
    {
        Type t = Type.GetType(ProtobufTypenNme);
        g_ms.SetLength(0);
        g_ms.Write(data, index, length);
        g_ms.Seek(0, SeekOrigin.Begin);
        return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(g_ms, null, t);
    }
    
    
    // 反序列化
    public static object Deserialize(eMsgTypes type, byte[] data, int index, int length)
    {
        if (g_TypeDict.ContainsKey(type) == false) {
            UnityEngine.Debug.Log("客户端不存在该proto协议类型：" + type.ToString());
            return null;
        }
        Type t = g_TypeDict[type];
        
        if (null == t) {
            UnityEngine.Debug.Log("客户端不存在该proto协议类型：" + type.ToString());
            return null;
        }
        
        g_ms.SetLength(0);
        g_ms.Write(data, index, length);
        g_ms.Seek(0, SeekOrigin.Begin);
        return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(g_ms, null, t);
    }
    
    
    public  static void Init()
    {
        eMsgTypes test = new eMsgTypes();
        System.Reflection.FieldInfo[] fieldInfos = typeof(eMsgTypes).GetFields();
        int FieldCount = fieldInfos.Length;
        g_TypeDict = new Dictionary<eMsgTypes, Type>(FieldCount, new msgtypeMsgTypeComparer());
        for (int i = 1; i < FieldCount; ++ i) {
            System.Reflection.FieldInfo FiledName = fieldInfos[i];
            eMsgTypes msgt = (eMsgTypes)FiledName.GetValue(test);
            Type t = Type.GetType(FiledName.Name.Replace("Msg", "ProtoMsg.Msg"));
            g_TypeDict.Add(msgt, t);
        }
        
        g_ms = new MemoryStream(64 * 1024);
    }
    
    
    public static void Clear()
    {
        g_ms.Dispose();
        g_ms.Close();
        g_ms = null;
        g_TypeDict.Clear();
    }
    
}