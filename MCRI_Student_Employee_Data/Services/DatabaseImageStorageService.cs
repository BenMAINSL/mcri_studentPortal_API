using MCRI_Student_Employee_Data.Data;

namespace MCRI_Student_Employee_Data.Services;

/// <summary>
/// Keeps profile images in the People table so the portal runs against
/// LocalDB alone. Swap the registration in Program.cs for
/// SupabaseStorageService once bucket credentials are available.
/// </summary>
public class DatabaseImageStorageService : IImageStorageService
{
    // Uploads are resized client-side; this is a backstop against a caller
    // that skips that step.
    private const long MaxBytes = 2 * 1024 * 1024;

    private static readonly string[] AllowedContentTypes =
    [
        "image/jpeg", "image/png", "image/webp", "image/gif"
    ];

    private readonly AppDbContext db;

    public DatabaseImageStorageService(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<string> Upload(int personId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No image file was supplied.", nameof(file));
        }

        if (file.Length > MaxBytes)
        {
            throw new ArgumentException($"Image is larger than {MaxBytes / 1024 / 1024} MB.", nameof(file));
        }

        var contentType = string.IsNullOrWhiteSpace(file.ContentType)
            ? "image/jpeg"
            : file.ContentType.ToLowerInvariant();

        if (!AllowedContentTypes.Contains(contentType))
        {
            throw new ArgumentException($"Unsupported image type '{contentType}'.", nameof(file));
        }

        var person = await db.People.FindAsync(personId)
            ?? throw new ArgumentException($"Person {personId} was not found.", nameof(personId));

        using var memory = new MemoryStream();
        await file.CopyToAsync(memory);

        person.ImageData = memory.ToArray();
        person.ImageContentType = contentType;

        // PersonService saves the entity; the stamp makes the browser refetch
        // after a re-upload instead of showing the cached old photo.
        return $"/api/people/{personId}/image?v={DateTime.UtcNow.Ticks}";
    }
}
