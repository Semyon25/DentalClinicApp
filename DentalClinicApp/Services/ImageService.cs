using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public class ImageService
{
    private static readonly HashSet<string> SupportedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png"
    };

    private readonly string _basePath;

    public ImageService(string basePath)
    {
        _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
    }

    public string GetPatientImageDirectory(int patientId) =>
        Path.Combine(_basePath, "images", patientId.ToString());

    public async Task<string> SaveResizedAsync(string sourcePath, int patientId)
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(sourcePath));

        if (!File.Exists(sourcePath))
            throw new FileNotFoundException("Файл не найден", sourcePath);

        var ext = Path.GetExtension(sourcePath);
        ValidateExtension(ext);

        var destDir = GetPatientImageDirectory(patientId);
        Directory.CreateDirectory(destDir);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var destPath = Path.Combine(destDir, fileName);

        using var image = await Image.LoadAsync(sourcePath).ConfigureAwait(false);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(1920, 1080)
        }));

        await image.SaveAsync(destPath).ConfigureAwait(false);

        return fileName;
    }

    private static void ValidateExtension(string ext)
    {
        if (!SupportedExtensions.Contains(ext))
            throw new InvalidOperationException($"Поддерживаются только: {string.Join(", ", SupportedExtensions)}");
    }
}
