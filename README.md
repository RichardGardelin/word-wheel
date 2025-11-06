# WordWheel

WordWheel is a small desktop app that helps you study Chinese vocabulary.
You select which HSK books and lessons to use, then filter by part of speech (POS). Press **Draw Words** to show one or more random words from the selected lists.

The app is inspired by the idea of an ancient Chinese word wheel, a fictional slot machine where you spin a wheel of words and stop it to see which ones appear.
Its color palette and layout are designed to reflect that imagined theme.

![output](https://github.com/user-attachments/assets/97792abf-1460-4fbe-8afa-a5057a1418fb)

This project was created for **personal use**, **learning**, and as a **portfolio project**.
It is open source under the MIT License.

---

## Tech Stack

* C# (.NET 9.0)
* Avalonia UI
* ReactiveUI

Cross-platform by design, but version 0.1.0 currently supports **Windows** only.

---

## Features

* Main study view
* Book and lesson selector (lessons available for HSK books 1–4)
* Word POS (Part of Speech) selector
* Random word draw button

Future updates are planned to add macOS support and more study options such as additional randomization modes, profiles, and settings.

---

## Vocabulary Data

The vocabulary in WordWheel is based on public **HSK** word lists.
Each word includes its part of speech, pinyin, and English meaning.

A unique feature of this dataset is that it also includes **lesson numbers** for every word in HSK books 1–4.
These lesson references are taken from the HSK Standard Course textbooks published by **Beijing Language and Culture University Press**, which indicate what lesson each word first appears in.
They are included here only for educational and personal study purposes.

---

## How to Run (Windows)

1. Download and unzip `WordWheel-0.1.0.zip`.
2. Run `WordWheel.exe`.

Runs as a portable app with no installation required.

---

## Planned Roadmap

Future updates will expand WordWheel beyond the local desktop version. Planned improvements include:

* Full support for **user profiles**, settings, and saved study progress.
* Optional **cloud sync** and **web version**, allowing users to access their progress online.

The optional online version of WordWheel will be part of a future release (2.0) while keeping the offline desktop app fully functional.

---

## License

Released under the **MIT License**.
Created for personal use, learning, and as part of a software development portfolio.
