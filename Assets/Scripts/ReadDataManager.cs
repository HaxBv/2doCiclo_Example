using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ReadDataManager : MonoBehaviour
{
    private readonly string SERVER_URL = "localhost:80/TF/";

    private readonly string READDATA_URL = "Utils/ReadData.php";
    private readonly string INSERTDATA_URL = "Utils/InsertData.php";

    public Heroes queryData;
    public Heroes resultData;

    public bool hello;

    public void ReadDataMethod()
    {
        StartCoroutine(ReadDataConnection());
    }

    public void InsertDataMethod()
    {
        StartCoroutine(InsertDataConnection());
    }

    private IEnumerator ReadDataConnection()
    {
        string CONNECT_USER_PHP = $"{SERVER_URL}/{READDATA_URL}";

        string jsonData = JsonUtility.ToJson(queryData);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(CONNECT_USER_PHP, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error de Red (Unity): " + request.error);
        }

        Debug.Log(request.downloadHandler.text);

        string jsonResult = request.downloadHandler.text;
        HeroesResponse resultDataResponse = JsonUtility.FromJson<HeroesResponse>(jsonResult);
        
        if (resultDataResponse.Hero.Length > 0)
        {
            resultData = resultDataResponse.Hero[0];
        }
    }

    private IEnumerator InsertDataConnection()
    {
        string CONNECT_USER_PHP = $"{SERVER_URL}/{INSERTDATA_URL}";

        string jsonData = JsonUtility.ToJson(queryData);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(CONNECT_USER_PHP, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error de Red (Unity): " + request.error);
        }

        Debug.Log(request.downloadHandler.text);

        string jsonResult = request.downloadHandler.text;

        Debug.Log("JSON RECIBIDO: " + jsonResult);
        HeroesResponse resultDataResponse = JsonUtility.FromJson<HeroesResponse>(jsonResult);
    }
}

[System.Serializable]
public class Heroes
{
    public string IDHeroe;
    public string Name;
    public string Bando;
    public string Clase1;
    public string Clase2;
    public string Descripcion;
}


public class HeroesResponse
{
    public bool success;
    public string message;
    public Heroes[] Hero;
}