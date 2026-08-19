using System.Net.Http.Headers;

namespace MCRI_Student_Employee_Data.Services;

public class SupabaseStorageService : IImageStorageService
{
    private readonly HttpClient http;
    private readonly string url;
    private readonly string key;
    private readonly string bucket;

    public SupabaseStorageService(HttpClient http, IConfiguration config)
    {
        this.http = http;
        url = config["Supabase:Url"]!.TrimEnd('/');
        key = config["Supabase:Key"]!;
        bucket = config["Supabase:Bucket"]!;
    }

    public async Task<string> Upload(int personId, IFormFile file)
    {
        var fileName = $"{personId}-{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";

        var request = new HttpRequestMessage(HttpMethod.Post, $"{url}/storage/v1/object/{bucket}/{fileName}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);

        using var stream = file.OpenReadStream();
        request.Content = new StreamContent(stream);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

        var response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return $"{url}/storage/v1/object/public/{bucket}/{fileName}";
    }
}
