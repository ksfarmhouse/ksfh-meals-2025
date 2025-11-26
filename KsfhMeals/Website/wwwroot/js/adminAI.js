/*
 * FarmHouse Menu Auto-Filler
 * Handles file preview and communication with Gemini 2.5 Flash API
 */

// 1. File Preview Logic
function previewFile() {
    const fileInput = document.getElementById('fileInput');
    const container = document.getElementById('previewContainer');
    const img = document.getElementById('previewImg');
    const pdfIcon = document.getElementById('pdfIcon');
    const fileName = document.getElementById('fileName');

    const file = fileInput.files[0];

    if (!file) {
        container.style.display = 'none';
        return;
    }

    container.style.display = 'flex';
    fileName.innerText = file.name;

    // Show Image vs PDF icon
    if (file.type.startsWith('image/')) {
        img.src = URL.createObjectURL(file);
        img.style.display = 'block';
        pdfIcon.style.display = 'none';
    } else if (file.type === 'application/pdf') {
        img.style.display = 'none';
        pdfIcon.style.display = 'block';
    }
}

// 2. Helper: Convert File to Base64
function fileToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        // Remove the "data:mime/type;base64," prefix
        reader.onload = () => resolve(reader.result.split(',')[1]);
        reader.onerror = error => reject(error);
    });
}

// 3. Map Data to Form Inputs
function populateForm(menuArray) {
    // Mapping of Day -> [LunchInputName, DinnerInputName]
    // These names match the 'name' attributes in your Admin.cshtml
    const map = {
        "Monday": ["ML", "MD"],
        "Tuesday": ["TL", "TD"],
        "Wednesday": ["WL", "WD"],
        "Thursday": ["THL", "THD"],
        "Friday": ["FL", "FD"],
        "Saturday": ["SA", null], // No Saturday Dinner input
        "Sunday": ["SU", null]  // No Sunday Dinner input
    };

    menuArray.forEach(item => {
        const targets = map[item.day];
        if (!targets) return;

        const lunchName = targets[0];
        const dinnerName = targets[1];

        // Fill Lunch Input
        if (lunchName && item.lunch) {
            const inputs = document.getElementsByName(lunchName);
            if (inputs.length > 0) inputs[0].value = item.lunch;
        }

        // Fill Dinner Input (if it exists)
        if (dinnerName && item.dinner) {
            const inputs = document.getElementsByName(dinnerName);
            if (inputs.length > 0) inputs[0].value = item.dinner;
        }
    });
}

// 4. Main Extraction Function
async function extractMenu() {
    const fileInput = document.getElementById('fileInput');
    const status = document.getElementById('aiStatus');
    const btn = document.getElementById('extractBtn');

    if (fileInput.files.length === 0) {
        alert("Please select a file first.");
        return;
    }

    // UI Updates
    btn.disabled = true;
    btn.innerText = "Analyzing Menu...";
    status.innerText = "Sending to Gemini AI... Please wait.";
    status.className = "status-text";

    try {
        const file = fileInput.files[0];
        const base64 = await fileToBase64(file);

        const payload = {
            image: base64,
            mimeType: file.type
        };

        // Call C# Backend Handler (?handler=AskGemini)
        const response = await fetch('?handler=AskGemini', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        const data = await response.json();

        if (data.error) {
            throw new Error(JSON.stringify(data.error));
        }

        // Parse JSON response from Gemini
        const rawText = data.candidates?.[0]?.content?.parts?.[0]?.text;

        // Clean JSON formatting if Gemini adds ```json markdown blocks
        const cleanJson = rawText.replace(/```json/g, '').replace(/```/g, '').trim();
        const menuArray = JSON.parse(cleanJson);

        // Fill inputs
        populateForm(menuArray);

        // Success UI
        status.innerText = "Success! Form updated. Please review before saving.";
        status.style.color = "var(--fh-green)"; // Ensure it matches brand

    } catch (error) {
        console.error(error);
        status.innerText = "Error: " + error.message;
        status.className = "status-text error-msg";
    } finally {
        btn.disabled = false;
        btn.innerText = "Auto-Fill Form";
    }
}