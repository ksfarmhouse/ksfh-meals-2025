var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages services to the dependency injection container
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // Use the custom error page for non-development environments
    app.UseExceptionHandler("/Error");

    // Enable HTTP Strict Transport Security (HSTS) for production scenarios
    // The default HSTS value is 30 days. Adjust as needed.
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Serve static files like CSS, JS, and images
app.UseStaticFiles();

// Enable routing for incoming requests
app.UseRouting();

// Enable authorization middleware
app.UseAuthorization();

// Map Razor Pages endpoints
app.MapRazorPages();

// Start the application
app.Run();
