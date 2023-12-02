using Microsoft.AspNetCore.Mvc;

namespace FileManager.Microservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly string _imagesDirectory = "/app/images";
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ImageController(IWebHostEnvironment hostingEnvironment)
    {
       _hostingEnvironment = hostingEnvironment;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        try
        {
            var imageId = Guid.NewGuid().ToString();

            var filePath = Path.Combine(_imagesDirectory, $"{imageId}.jpg");
            Console.WriteLine(filePath);
            var absolutePath = Path.Combine(_hostingEnvironment.ContentRootPath, filePath);
            Console.WriteLine(absolutePath);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ProcessImage(filePath);

            return Ok(new { ImageId = imageId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private void ProcessImage(string filePath)
    {
        using (var image = Image.Load(filePath))
        {
            // Пример изменения размера изображения до 300x300 пикселей
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(300, 300),
                Mode = ResizeMode.Max
            }));

            // Сохранение измененного изображения
            image.Save(filePath);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetImage([FromRoute] string id)
    {
        try
        {
            // Построение пути к файлу изображения
            string filePath = Path.Combine(_imagesDirectory, $"{id}.jpg");

            // Проверка существования файла
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image not found");
            }

            // Чтение файла изображения
            var imageBytes = System.IO.File.ReadAllBytes(filePath);

            // Определение MIME-типа изображения (может потребоваться дополнительная логика)
            var mimeType = "image/jpeg";

            // Возврат изображения в ответе
            return File(imageBytes, mimeType);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}