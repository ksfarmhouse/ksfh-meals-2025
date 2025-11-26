# ksfh-meals-2025

This is the code behind the K-State FarmHouse meal website. It manages member sign-ups and the weekly food schedule.

## Background Knowledge
This website is built using **ASP.NET Core Razor Pages**.
* The tools, C# patterns, and architecture used in this project are taught in **CIS 400 (Object-Oriented Design & Implementation)**. If you have taken that class, you should be familiar with this setup.

## How it works
* **Frontend:** HTML and CSS are used for the look and feel.
* **Backend:** C# (ASP.NET Core Razor Pages) handles the logic and data saving.
* **AI Feature:** We use Google Gemini to read PDF menus so you don't have to type them out manually.

## Where to find things
The main work happens in the **Website** project folder:

* **`wwwroot/`**:
    * Go here to change colors, fonts, or table sizes (`style.css`).
    * Go here if the AI button looks wrong (`admin-ai.css`).
* **`Pages/`**:
    * Contains the **Razor Pages** (.cshtml files) for every visible page on the site.
    * **`Admin.cshtml`**: This is where the menu editing logic lives.
    * **`Shared/_Layout.cshtml`**: This is the Navigation Bar. Edit this if you want to add a new link to the top menu.

## Updating the AI
If the auto-fill feature stops working, you probably need a new API Key.
1.  Go to Google AI Studio and generate a new key.
2.  Open `Pages/Admin.cshtml.cs`.
3.  Paste the new key into the `OnPostAskGeminiAsync` function.

## How to Run
Open the `.sln` file in Visual Studio and click the Green Play Button (IIS Express).