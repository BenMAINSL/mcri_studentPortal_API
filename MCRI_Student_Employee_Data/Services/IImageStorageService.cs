namespace MCRI_Student_Employee_Data.Services;

public interface IImageStorageService
{
    // Uploads the file to Supabase Storage and returns its public URL.
    Task<string> Upload(int personId, IFormFile file);
}
