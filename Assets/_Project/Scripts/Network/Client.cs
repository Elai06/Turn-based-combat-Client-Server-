using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private BinaryReader reader;
    private BinaryWriter writer;

    void Start()
    {
        try
        {
            // Подключаемся к серверу на порту 8080
            client = new TcpClient("127.0.0.1", 8080);
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);

            // Отправляем запрос на сервер
            SendMessageToServer("get_randomSkill");

            // Получаем JSON-ответ от сервера
            string jsonResponse = ReceiveJsonFromServer();

            // Десериализуем JSON в объект
            Response response = JsonUtility.FromJson<Response>(jsonResponse);
            Debug.Log("Ответ от сервера: " + response.Message + ", Значение: " + response.Value);
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка при подключении к серверу: " + e.Message);
        }
    }

    // Отправка запроса серверу в формате JSON
    void SendMessageToServer(string action)
    {
        var request = new Request
        {
            Action = action
        };

        // Сериализация запроса в JSON
        string jsonRequest = JsonUtility.ToJson(request);
        byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(jsonRequest);

        // Отправляем запрос
        writer.Write(requestBytes);
        writer.Flush();
    }

    // Получение JSON-ответа от сервера
    string ReceiveJsonFromServer()
    {
        // Чтение байтов ответа от сервера
        byte[] data = new byte[1024];
        int bytesRead = reader.Read(data, 0, data.Length);
        if (bytesRead == 0)
        {
            Debug.LogError("Ошибка при получении данных.");
            return null;
        }

        // Преобразуем байты в строку
        string jsonResponse = System.Text.Encoding.UTF8.GetString(data, 0, bytesRead);
        return jsonResponse;
    }

    // Закрываем соединение
    void OnApplicationQuit()
    {
        if (client != null)
        {
            reader.Close();
            writer.Close();
            stream.Close();
            client.Close();
        }
    }
}

[Serializable]
public class Request
{
    public string Action { get; set; }
}

[Serializable]
public class Response
{
    public string Message { get; set; }
    public int Value { get; set; }
}