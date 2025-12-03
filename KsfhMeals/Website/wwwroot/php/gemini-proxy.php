<?php
// gemini-proxy.php

// 1. SECURE YOUR KEY HERE
// Ideally, load this from an environment variable, but for a basic 
// non-database app, keeping it here is safer than in the browser.
$apiKey = 'AIzaSyB5SzhQCpkxWZuRyo31yER7USEYBnBYW5U';

// 2. GET DATA FROM YOUR JAVASCRIPT
// We expect the browser to send JSON data
$inputData = json_decode(file_get_contents('php://input'), true);
$userPrompt = $inputData['prompt'] ?? '';

if (empty($userPrompt)) {
    echo json_encode(['error' => 'No prompt provided']);
    exit;
}

// 3. PREPARE THE REQUEST TO GEMINI
$apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" . $apiKey;

$data = [
    "contents" => [
        [
            "parts" => [
                ["text" => $userPrompt]
            ]
        ]
    ]
];

// 4. SEND REQUEST USING CURL (Standard PHP tool)
$ch = curl_init($apiUrl);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, [
    'Content-Type: application/json'
]);
curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($data));

$response = curl_exec($ch);

if (curl_errno($ch)) {
    echo json_encode(['error' => 'Request failed: ' . curl_error($ch)]);
} else {
    // 5. RETURN GEMINI'S RESPONSE TO YOUR JAVASCRIPT
    // We pass the response exactly as we got it (or you can filter it here)
    header('Content-Type: application/json');
    echo $response;
}

curl_close($ch);
?>