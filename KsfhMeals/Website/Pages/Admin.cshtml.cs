using System.Text;
using System.Text.Json; // Native JSON handling
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Website.Pages
{
    /// <summary>
    /// Manages admin actions: menu editing, member status updates, and AI auto-fill.
    /// </summary>
    [RequestSizeLimit(50 * 1024 * 1024)] // Allow 50MB uploads (PDFs)
    [IgnoreAntiforgeryToken]             // Bypass token check for API fetch calls
    public class AdminModel : PageModel
    {
        #region Menu Properties

        /// <summary>Monday Lunch input.</summary>
        [BindProperty(SupportsGet = true)] public string? ML { get; set; }

        /// <summary>Monday Dinner input.</summary>
        [BindProperty(SupportsGet = true)] public string? MD { get; set; }

        /// <summary>Tuesday Lunch input.</summary>
        [BindProperty(SupportsGet = true)] public string? TL { get; set; }

        /// <summary>Tuesday Dinner input.</summary>
        [BindProperty(SupportsGet = true)] public string? TD { get; set; }

        /// <summary>Wednesday Lunch input.</summary>
        [BindProperty(SupportsGet = true)] public string? WL { get; set; }

        /// <summary>Wednesday Dinner input.</summary>
        [BindProperty(SupportsGet = true)] public string? WD { get; set; }

        /// <summary>Thursday Lunch input.</summary>
        [BindProperty(SupportsGet = true)] public string? THL { get; set; }

        /// <summary>Thursday Dinner input.</summary>
        [BindProperty(SupportsGet = true)] public string? THD { get; set; }

        /// <summary>Friday Lunch input.</summary>
        [BindProperty(SupportsGet = true)] public string? FL { get; set; }

        /// <summary>Friday Dinner input.</summary>
        [BindProperty(SupportsGet = true)] public string? FD { get; set; }

        /// <summary>Saturday Lunch input.</summary>
        [BindProperty(SupportsGet = true)] public string? SA { get; set; }

        /// <summary>Sunday Lunch input.</summary>
        [BindProperty(SupportsGet = true)] public string? SU { get; set; }

        #endregion

        #region Feedback Messages

        /// <summary>Feedback for menu save actions.</summary>
        public string? SaveScheduleMessage { get; set; }

        /// <summary>Feedback for meal rollover actions.</summary>
        public string? SaveRolloverMessage { get; set; }

        /// <summary>Feedback for meal reset actions.</summary>
        public string? SaveResetMessage { get; set; }

        /// <summary>Feedback for member status updates.</summary>
        public string? UpdateNMMessage { get; set; }

        #endregion

        #region Standard POST Handlers

        /// <summary>
        /// Saves the weekly menu to the data store.
        /// </summary>
        public void OnPost()
        {
            List<string> lunch = new List<string> { ML!, TL!, WL!, THL!, FL!, SA!, SU! };
            List<string> dinner = new List<string> { MD!, TD!, WD!, THD!, FD! };

            if (!lunch.Contains(null!) && !dinner.Contains(null!))
            {
                House.AddLunchItem(lunch);
                House.AddDinnerItem(dinner);
                SaveScheduleMessage = "Meal Schedule has been saved successfully!";
            }
        }

        /// <summary>
        /// Rolls over temporary meals to permanent signups.
        /// </summary>
        public void OnPostRollover()
        {
            foreach (Member m in House.AllMembers)
            {
                for (int i = 0; i < m.TempMealSignUp.Length; i++)
                {
                    bool isLunch = (i % 2 == 0) || (i >= 10);
                    bool isDinner = (i % 2 == 1 && i < 10);

                    if (m.HouseStatus == Status.OutOfHouse && m.TempMealSignUp[i] != MealStatus.Out)
                    {
                        if (isLunch) m.LunchCount++;
                        if (isDinner) m.DinnerCount++;
                    }
                    m.TempMealSignUp[i] = m.DefaultSignUp[i];
                }
            }
            House.Save();
            SaveRolloverMessage = "Meal Rollover has been saved successfully!";
        }

        /// <summary>
        /// Resets all member meal signups to default values.
        /// </summary>
        public IActionResult OnPostResetMeals()
        {
            foreach (Member m in House.AllMembers)
            {
                // Reset to 'In' for residents, 'Out' for others
                var status = (m.HouseStatus == Status.InHouse || m.HouseStatus == Status.NewMember)
                             ? MealStatus.In : MealStatus.Out;

                m.TempMealSignUp = Enumerable.Repeat(status, 12).ToArray();
                m.DefaultSignUp = Enumerable.Repeat(status, 12).ToArray();
                m.DinnerCount = 0;
                m.LunchCount = 0;
            }

            House.Save();
            SaveResetMessage = "Meal Reset has been saved successfully!";
            return Page();
        }

        /// <summary>
        /// Updates 'New Member' status to 'In House'.
        /// </summary>
        public void OnPostUpdateNM()
        {
            foreach (Member m in House.AllMembers)
            {
                if (m.HouseStatus == Status.NewMember) m.HouseStatus = Status.InHouse;
            }
            House.Save();
            UpdateNMMessage = "All New Members are now In House Members";
        }

        #endregion

        #region Gemini API Handler

        /// <summary>
        /// Processes uploaded menu files (PDF/Image) using Gemini AI.
        /// Returns structured JSON to auto-fill the frontend form.
        /// </summary>
        /// <param name="body">JSON payload containing Base64 file data.</param>
        public async Task<IActionResult> OnPostAskGeminiAsync([FromBody] JsonElement body)
        {
            try
            {
                // 1. Extract inputs
                string userPrompt = body.TryGetProperty("prompt", out var p) ? p.ToString() : "";
                string? fileData = null;
                string? mimeType = null;

                if (body.TryGetProperty("image", out var img))
                {
                    fileData = img.ToString();
                    mimeType = body.TryGetProperty("mimeType", out var mime) ? mime.ToString() : "image/jpeg";
                }

                // 2. Define System Prompt (Business Rules)
                // Enforces 20-char limit and weekend/Friday overrides.
                string systemInstruction = @"
                    Extract menu data into JSON: [{ ""day"": ""Monday"", ""lunch"": ""..."", ""dinner"": ""..."" }].
    
                    STRICT CONSTRAINT: Max 20 characters per dish.
    
                    RULES:
                    1. REMOVE sides (salad, bread, rice, veggies), drinks, and desserts.
                    2. EXCEPTION: You may keep 'Potatoes' if it fits within 20 chars (e.g., 'Steak & Potatoes').
                    3. REMOVE adjectives (e.g., 'Spicy', 'Fresh', 'Creamy') unless part of the dish name (e.g., 'Huli Huli').
                    4. ABBREVIATIONS: 'and'->'&', 'with'->'w/', 'Chicken'->'Ckn' (if needed).
    
                    EXAMPLES:
                    - 'Turkey with spinach artichoke melts' -> 'Turkey Melt'
                    - 'Loose meat Philly' -> 'Philly Cheesesteaks'
                    - 'Huli Huli with white rice' -> 'Huli Huli Chicken'
                    - 'Steak in garlic butter diced red potatoes' -> 'Steak & Potatoes'
    
                    OVERRIDES:
                    - Friday Dinner: 'Leftovers'
                    - Saturday Lunch: 'Burgers', Dinner: null
                    - Sunday Lunch: Map any meal to Lunch, Dinner: null
    
                    Return ONLY raw JSON.
                ";

                string finalPrompt = $"{systemInstruction}\n\nUser Note: {userPrompt}";

                // 3. Configure API
                string apiKey = "AIzaSyDP0lD44rPajz-4HVROTYPPYIzq8u01EnE"; // <--- Insert API Key
                string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

                // 4. Build Payload
                var parts = new List<object> { new { text = finalPrompt } };

                if (!string.IsNullOrEmpty(fileData))
                {
                    parts.Add(new { inline_data = new { mime_type = mimeType, data = fileData } });
                }

                var payload = new
                {
                    contents = new[] { new { parts = parts } },
                    generationConfig = new { responseMimeType = "application/json" }
                };

                // 5. Execute Request
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(60); // Extended timeout for PDFs
                    var json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        return new JsonResult(new { error = $"API Error: {response.StatusCode}" });

                    return Content(result, "application/json");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        #endregion
    }
}