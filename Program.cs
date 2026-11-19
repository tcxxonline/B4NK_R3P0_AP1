var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Sunucu açıldığında ana sayfaya girince hoşgeldin mesajı versin
app.MapGet("/", () => "Banka Sunucusu (BankServerApi) Aktif ve Çalışıyor! 🚀");

// 1. KULLANICI KAYIT (REGISTER) ENDPOINT'İ
// Örnek kullanım: POST /register?username=ahmet&password=123
app.MapPost("/register", (string username, string password) => {
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
    {
        return Results.BadRequest(new { message = "Kullanıcı adı ve şifre boş olamaz!" });
    }

    // Şimdilik basitçe başarılı döndürüyoruz (İleride veritabanına bağlarız)
    return Results.Ok(new { message = $"Kullanıcı başarıyla kaydedildi: {username}" });
});

// 2. DOSYA YÜKLEME (FILE UPLOAD) ENDPOINT'İ
app.MapPost("/upload", async (IFormFile file) => {
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest(new { message = "Lütfen geçerli bir dosya yükleyin!" });
    }

    // Dosyaların kaydedileceği klasör (sunucu içinde 'uploads' klasörü)
    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    // Dosya adını rastgele/karışık (obfuscated) hale getiriyoruz ki çakışma olmasın ve güvenli olsun
    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

    // Dosyayı sunucuya kaydediyoruz
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    return Results.Ok(new { 
        message = "Dosya başarıyla yüklendi!", 
        savedAs = uniqueFileName 
    });
});

app.Run();
