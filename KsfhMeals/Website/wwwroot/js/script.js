async function askGemini(promptText) {
    try {
        // We are calling OUR file, not Google directly
        const response = await fetch('gemini-proxy.php', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ prompt: promptText })
        });

        const data = await response.json();

        // The structure of the response depends on Gemini's API
        // Usually it's data.candidates[0].content.parts[0].text
        if (data.candidates && data.candidates.length > 0) {
            console.log("Gemini says:", data.candidates[0].content.parts[0].text);
            return data.candidates[0].content.parts[0].text;
        } else {
            console.error("Error from API:", data);
        }

    } catch (error) {
        console.error("Network error:", error);
    }
}

// Usage
askGemini("Explain how a rainbow is made in one sentence.");