using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;


using (HttpListener listener = new HttpListener())
{
    listener.Prefixes.Add("http://127.0.0.1:8000/test/");
    listener.Start();
    Console.WriteLine("Сервер начал работу");
    while (true)
    {
        var context = await listener.GetContextAsync();

        var request = context.Request;  // получаем данные запроса
        var response = context.Response;    // получаем объект для установки ответа
        var user = context.User;        // получаем данные пользователя

        Console.WriteLine(request.RemoteEndPoint);

        string anser = "You connect to KIR\'s server";
        var bytearr = Encoding.UTF8.GetBytes(anser);

        response.ContentLength64 = anser.Length;
        using Stream output = response.OutputStream;
        // отправляем данные
        await output.WriteAsync(bytearr);
        await output.FlushAsync();

        Console.WriteLine("Запрос обработан");

    }
}