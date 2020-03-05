
//////////////////////////////////////////////////////////
//                                                      //
// JsonUtility 는 UnityEngine 네임스페이스에 포함되어 있다   //
//                                                      //
//////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

public class JsonMgr
{
    public string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public T JsonToObject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream filestream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        filestream.Write(data, 0, data.Length);
        filestream.Close();
    }

    public T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream filestream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[filestream.Length];
        filestream.Read(data, 0, data.Length);
        filestream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
}
