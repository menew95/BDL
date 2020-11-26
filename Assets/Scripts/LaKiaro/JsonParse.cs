using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;


public class JsonParse : MonoBehaviour
{
    public void CreateJsonFile(string createPath, string fileName, object objData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}", createPath, fileName + ".json"), FileMode.Create);

        string jsonData = ConvertSaveDataToJson(objData);

        byte[] data = Encoding.UTF8.GetBytes(jsonData);

        fileStream.Write(data, 0, data.Length);

        fileStream.Close();
    }

    public T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        Debug.Log(jsonData);
        return JsonUtility.FromJson<T>(jsonData);
    }

    public T LoadJsonFile<T>(FileStream fileStream)
    {
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void SaveJsonFile(object obj, string loadPath, string fileName)
    {

        string jsonData = ConvertSaveDataToJson(obj);

        byte[] data = Encoding.UTF8.GetBytes(jsonData);

        if (File.Exists(string.Format("{0}/{1}", loadPath, fileName + ".json")))
        {
            Debug.Log("있음");

            jsonData = ConvertSaveDataToJson(obj);

            Debug.Log(jsonData);

            data = Encoding.UTF8.GetBytes(jsonData);

            File.WriteAllBytes(string.Format("{0}/{1}", loadPath, fileName + ".json"), data);

        }
        else
        {
            Debug.Log("없음");
            FileStream fileStream = new FileStream(string.Format("{0}/{1}", loadPath, fileName + ".json"), FileMode.Create);

            fileStream.Write(data, 0, data.Length);

            fileStream.Close();
        }


    }

    public string ConvertSaveDataToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public T ConvertJsonToSaveData<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }
}
