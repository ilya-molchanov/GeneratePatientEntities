using Bogus;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace GeneratePatientEntities
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();

            for (int i = 0; i < 100; i++)
            {
                Faker faker = new Faker("en");

                var newPatient = new
                {
                    faker.Person.FirstName,
                    faker.Person.LastName,
                    BirthDate = DateTime.SpecifyKind(faker.Person.DateOfBirth, DateTimeKind.Utc),
                    faker.Person.Gender,
                    Use = "official",
                    Active = true
                };

                var url = "http://localhost/api/v1/Patient";

                // Serialization settings: camelCase + enum string + ISO8601
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(newPatient, options);
                //Console.WriteLine("The JSON we send:");
                //Console.WriteLine(json);

                var content = new StringContent(json, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));

                //var response = 
                await client.PostAsync(url, content);

                //Console.WriteLine($"Status: {response.StatusCode}");
                //Console.WriteLine("Server response:");
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}