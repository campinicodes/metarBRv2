using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;


class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Digite o código ICAO: ");
        string icao = Console.ReadLine().ToUpper();

        if (icao.Length != 4)
        {

            Console.WriteLine("Digite um código ICAO válido.");
            return;

        }

        string apiKey = "fcvRMcgYNDDpETcSVxAUEb5jOgVDIl1AI9ndkbKi";

        string url = $"https://api-redemet.decea.mil.br/mensagens/metar/{icao}?api_key={apiKey}";


        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer{apiKey}");

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBody);
            string metarMessage = jsonDocument
                .RootElement
                .GetProperty("data")
                .GetProperty("data")[0]
                .GetProperty("mens")
                .GetString();

            Console.WriteLine(metarMessage);

        }
        catch (HttpRequestException e)

        {
            Console.WriteLine($"Erro ao buscar o metar de {icao}");
            Console.WriteLine($"Erro: {e.Message}");
        }
    }
}
