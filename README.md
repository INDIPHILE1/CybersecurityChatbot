# 🛡️ Cybersecurity Awareness Bot

A comprehensive C# cybersecurity awareness application that evolves from a console chatbot into a full-featured WPF desktop application.  
**Developed for the PROG6221 POE (Portfolio of Evidence) assignment using .NET 8.0.**

---

##  Project Overview

This project is split into three progressive parts, each building on the previous one:

| Part | Description |
|------|-------------|
| **Part 1** | Console-based chatbot with voice greeting, ASCII art, keyword responses, input validation, and enhanced console UI. |
| **Part 2** | WPF GUI upgrade with sentiment detection, memory (user name & favourite topic), random responses, and follow-up handling. |
| **Part 3** | Full POE submission adding a Task Assistant (with SQLite database), Cybersecurity Quiz (12+ questions), NLP simulation, and an Activity Log. |

---

## Features

### Part 1 – Console Chatbot
-  **Voice greeting** – plays `Greeting.wav` on startup
-  **ASCII art logo** – cybersecurity-themed header
-  **Personalised interaction** – asks for and remembers the user's name
-  **Cybersecurity Q&A** – responds to keywords like *password*, *phishing*, *scam*, *privacy*, *2FA*, *malware*
-  **Input validation** – handles empty or unrecognised inputs gracefully
-  **Enhanced console UI** – coloured text, borders, and a typing effect
-  **Clean code structure** – separate classes (`AudioPlayer`, `User`, `Chatbot`, `Program`)

### Part 2 – WPF GUI Upgrade
-  **Modern graphical interface** – clean, colour‑blended WPF design
-  **Sentiment detection** – recognises *worried*, *curious*, *frustrated*, *scared* and responds empathetically
-  **Random responses** – varied answers for topics like *phishing tip* or *password tip*
-  **Follow‑up handling** – understands *"tell me more"*, *"another tip"*, *"explain"*
-  **Memory & recall** – remembers the user's name and favourite cybersecurity topic

### Part 3 – Full POE Features
-  **Task Assistant with Reminders** (SQLite database)
  - Add, view, complete, and delete cybersecurity tasks
  - Each task includes a title, description, and optional reminder
  - Tasks persist between sessions via Entity Framework Core + SQLite
-  **Cybersecurity Mini‑Game (Quiz)**
  - 12+ questions covering phishing, password safety, safe browsing, social engineering, 2FA, malware, and privacy
  - Mix of multiple‑choice and true/false questions
  - Immediate feedback with explanations after each answer
  - Final score with a motivational message
-  **NLP Simulation**
  - Detects natural language intents: *add task*, *set reminder*, *start quiz*, *show activity log*
  - Handles varied phrasings (e.g., *"Remind me to update my password"*, *"I need to enable 2FA"*)
  - Uses keyword detection and string manipulation (no external NLP libraries)
-  **Activity Log**
  - Records every significant action with a timestamp
  - Logs: task additions, completions, deletions, quiz attempts, NLP interactions, and user registration
  - Displays the last 10 entries with a **"Show More"** option for the full history
-  **SQLite Database Integration**
  - All tasks and logs are stored persistently in `database.db`
  - Loads automatically on application startup

##  Prerequisites

- **.NET 8.0 SDK** or later
- **Visual Studio 2022** (or any C# IDE with WPF support)
- **Git** and a **GitHub account**
- **NuGet packages** (installed automatically when you build):
  - `Microsoft.EntityFrameworkCore.Sqlite`
  - `Microsoft.EntityFrameworkCore.Proxies`
  - `System.Windows.Extensions` (for voice playback)

##  Installation & Setup

### 1. Clone the repository
```bash
git clone https://github.com/INDIPHILE1/CybersecurityChatbot.git
cd CybersecurityChatbot
