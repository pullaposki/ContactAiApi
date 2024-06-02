using Newtonsoft.Json;

internal class Program
{
    public static async Task Main(string[] args)
    {

        var url = "http://localhost:1234/v1/chat/completions";
        var model = "TheBloke/Mistral-7B-Instruct-v0.2-GGUF";
        var messages = new Message[]
        {
            new Message { role = "system", content = "Always answer in rhymes." },
            new Message { role = "user", content = "Introduce yourself." }
        };
        var sender = new ChatCompletionRequestSender();
        var response = await sender.SendChatCompletionRequest(url, model, messages);
        Console.WriteLine(response);
    }

    public class ChatCompletionRequestSender
    {
        public async Task<string> SendChatCompletionRequest(string url, string model, Message[] messages)
        {
            // Create the request body as a JSON object
            var jsonBody = new
            {
                model = model,
                messages = messages,
                temperature = 0.7f,
                max_tokens = -1,
                stream = false
            };

            using (var httpClient = new HttpClient())
            {
                // Serialize the request body to a JSON string
                var jsonRequestBody = JsonConvert.SerializeObject(jsonBody);

                // Configure the HTTP request headers and content
                var content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(url, content))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new ApplicationException("The request failed with status code: " + response.StatusCode);

                    // Read the response stream as a string and return it
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }
    }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}