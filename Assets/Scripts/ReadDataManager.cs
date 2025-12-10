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

    public Cartas queryCartas;
    public Cartas resultCartas;


    public Sobres querySobres;
    public Sobres resultSobres;

    public Usuarios queryUsuarios;
    public Usuarios resultUsuarios;


    public bool hello;

    public void ReadHero()
    {
        StartCoroutine(ReadHeroConnection());
    }

    
    public void InsertDataMethod()
    {
        StartCoroutine(InsertDataConnection());
    }

    private IEnumerator ReadHeroConnection()
    {
        string url = $"{SERVER_URL}/{READDATA_URL}";

        string jsonData = JsonUtility.ToJson(queryData); 
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("HERO RESPONSE" + request.downloadHandler.text);

        HeroesResponse result = JsonUtility.FromJson<HeroesResponse>(request.downloadHandler.text);

        if (result.Hero.Length > 0)
            resultData = result.Hero[0];
    }
    public void ReadCarta()
    {
        StartCoroutine(ReadCartaConnection());
    }

    private IEnumerator ReadCartaConnection()
    {
        string url = $"{SERVER_URL}/{READDATA_URL}";

        string jsonData = JsonUtility.ToJson(queryCartas); 
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("CARTA RESPONSE" + request.downloadHandler.text);

        CartasResponse result = JsonUtility.FromJson<CartasResponse>(request.downloadHandler.text);

        if (result.Carta.Length > 0)
            resultCartas = result.Carta[0];
    }

    public void ReadSobre()
    {
        StartCoroutine(ReadSobreConnection());
    }

    private IEnumerator ReadSobreConnection()
    {
        string url = $"{SERVER_URL}/{READDATA_URL}";

        string jsonData = JsonUtility.ToJson(querySobres);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);


        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("SOBRE RESPONSE" + request.downloadHandler.text);

        SobresResponse result = JsonUtility.FromJson<SobresResponse>(request.downloadHandler.text);

        if (result.Sobre.Length > 0)
            resultSobres = result.Sobre[0];
    }
    public void ReadUsuario()
    {
        StartCoroutine(ReadUsuarioConnection());
    }

    private IEnumerator ReadUsuarioConnection()
    {
        string url = $"{SERVER_URL}/{READDATA_URL}";

        string jsonData = JsonUtility.ToJson(queryUsuarios);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);


        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("USUARIO RESPONSE" + request.downloadHandler.text);

        UsuariosResponse result = JsonUtility.FromJson<UsuariosResponse>(request.downloadHandler.text);

        if (result.Usuario.Length > 0)
            resultUsuarios = result.Usuario[0];
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

[System.Serializable]
public class Cartas
{
    public string IDCarta;
    public string Name;
    public string Bando;
    public string Clase;
    public string TipoCarta;
    public string Coleccion;
    public string Rareza;
    public string Coste;
    public string Fuerza;
    public string Vida;
    public string Habilidad;
}

public class CartasResponse
{
    public bool success;
    public string message;
    public Cartas[] Carta;
}

[System.Serializable]
public class Sobres
{
    public string IDSobre;
    public string Name;
    public string PrecioGemas;
    public string PrecioSoles;
    public string Descripcion;
}

public class SobresResponse
{
    public bool success;
    public string message;
    public Sobres[] Sobre;
}

[System.Serializable]
public class Usuarios
{
    public string IDUsuario;
    public string Name;
    public string Correo;
    public string Pais;
    public string Gemas;
    public string RangoActual;
    public string LigaActual;
    public string Activo;
}

public class UsuariosResponse
{
    public bool success;
    public string message;
    public Usuarios[] Usuario;
}



