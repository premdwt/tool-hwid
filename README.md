# ⚡ DWT Utility (KerenCleaner)

<p align="center">
  <img src="https://img.shields.io/github/v/release/premdwt/tool-hwid?style=for-the-badge&label=Release" alt="Latest Release">
  <img src="https://img.shields.io/github/stars/premdwt/tool-hwid?style=for-the-badge" alt="GitHub Stars">
  <img src="https://img.shields.io/badge/Platform-Windows%2010%2F11-blue?style=for-the-badge&logo=windows">
  <img src="https://img.shields.io/badge/Built%20with-C%23%20.NET%2010.0-512BD4?style=for-the-badge&logo=dotnet">
</p>

<p align="center">
  <strong>All-in-One Windows Tool — Disk Cleaner, System Repair, Post-Install Downloader, dan Media Downloader.</strong><br>
  <em>Native, ringan, cepat. Satu file EXE, tanpa install .NET.</em>
</p>

<p align="center">
  <a href="https://github.com/premdwt/tool-hwid/releases/latest"><strong>⬇️ Download Latest Release</strong></a>
  &nbsp;•&nbsp;
  <a href="#-cara-pakai-cepat">Cara Pakai</a>
  &nbsp;•&nbsp;
  <a href="#-daftar-menu-lengkap">Daftar Menu</a>
  &nbsp;•&nbsp;
  <a href="#-changelog">Changelog</a>
</p>

---

## ⬇️ Download

| | |
|---|---|
| **Latest Release** | [**Download KerenCleaner.exe**](https://github.com/premdwt/tool-hwid/releases/latest) |
| **Versi Saat Ini** | `v1.0.0` |
| **Ukuran** | ~36 MB (self-contained single file) |
| **Butuh** | Windows 10/11 x64 + Run as Administrator |

> Tidak perlu clone repo atau install .NET SDK — cukup download EXE dari tab **Releases**, lalu jalankan sebagai admin.

<p align="center">
  <a href="https://github.com/premdwt/tool-hwid/releases/latest">
    <img src="https://img.shields.io/badge/Download-KerenCleaner.exe-2ea44f?style=for-the-badge&logo=github" alt="Download EXE">
  </a>
</p>

---

## 📌 Apa itu DWT Utility?

**DWT Utility** (project: `KerenCleaner`) adalah aplikasi **CLI** berbasis **C# (.NET 10.0)** untuk merawat, mengoptimalkan, dan mengotomasi Windows — dari bersih-bersih sampah disk, perbaikan sistem, download installer pasca-reinstall, hingga download media YouTube & TikTok.

- Berjalan **native** via API Windows (`shell32`, `user32`, Registry, dll.)
- Auto-request hak **Administrator** saat pertama dijalankan
- Menu interaktif + animasi idle, progress bar, pop-up sukses Windows
- **Open source** — https://github.com/premdwt/tool-hwid

---

## 🚀 Cara Pakai Cepat

```
1. Download KerenCleaner.exe  →  tab Releases
2. Klik kanan                 →  Run as administrator
3. Pilih menu (0–14)          →  selesai!
```

File hasil kerja (video, MP3, installer) tersimpan di **folder yang sama** dengan EXE.

---

## ✨ Highlight Fitur

| Kategori | Highlight |
|----------|-----------|
| 🧹 **Cleaner** | Temp, Recycle Bin, browser cache (semua profil), COD Warzone cache |
| 🔍 **Analyzer** | Scan folder besar, startup registry + status enable/disable |
| 🛠️ **System** | Flush DNS, DISM, SFC, HWID checker, RAM trim |
| 📦 **Post-Install** | Auto-download Discord, DirectX SDK, NVIDIA App |
| 🎥 **Media** | YouTube **1080p+audio**, TikTok no watermark, ekstrak **MP3** |

---

## 🚀 Daftar Menu Lengkap

| No | Menu | Keterangan |
|----|------|------------|
| `1` | Bersihkan File Temp | Hapus isi `%temp%` user & `C:\Windows\Temp` (file terkunci dilewati) |
| `2` | Kosongkan Recycle Bin | Kosongkan Recycle Bin via API Windows tanpa konfirmasi pop-up |
| `3` | Bersihkan Browser Cache | Cache Chrome, Edge, & Brave — semua profil (`Default` + `Profile *`) |
| `4` | Scan Program Files | Tampilkan folder > **500 MB** di `C:\Program Files`, urut terbesar |
| `5` | Scan Downloads | Tampilkan folder > **100 MB** di folder Downloads user |
| `6` | Bersihkan Cache COD Warzone | Hapus total folder Activision, Battle.net, & Blizzard *(destructive)* |
| `7` | Flush DNS & Reset Network | `ipconfig /flushdns` + `netsh winsock reset` |
| `8` | System Repair | `DISM RestoreHealth` lalu `SFC /scannow` *(5–20 menit)* |
| `9` | Cek Info HWID | Motherboard, Storage, MAC Address, System UUID via PowerShell CIM |
| `10` | SIKAT SEMUA SAMPAH | Jalankan menu 1, 2, 3, dan 6 sekaligus |
| `11` | Kosongkan RAM | Trim working set semua proses via `EmptyWorkingSet` |
| `12` | Startup Analyzer | Baca Registry startup + status **Enabled / Disabled** |
| `13` | Auto-Download Apps | Download installer Discord, DirectX SDK, NVIDIA App |
| `14` | Media Downloader | YouTube 1080p, TikTok no watermark, & ekstrak **MP3** |
| `0` | Keluar | Tutup aplikasi |

---

## 🎥 Media Downloader (Menu 14)

Engine: **[yt-dlp](https://github.com/yt-dlp/yt-dlp)** + **[FFmpeg](https://ffmpeg.org/)** (auto-download saat pertama dipakai).

| Opsi | Fitur | Output |
|------|-------|--------|
| `1` | YouTube Video | MP4 **1080p + audio** (merge via FFmpeg) |
| `2` | TikTok Video | MP4 tanpa watermark |
| `3` | Audio Saja | **MP3** kualitas terbaik — YouTube & TikTok |

| File Auto-Download | Fungsi |
|--------------------|--------|
| `yt-dlp.exe` | Engine download media |
| `ffmpeg.exe` + `ffprobe.exe` | Merge 1080p & konversi MP3 (~80 MB, sekali download) |

---

## 🧹 Detail Fitur System & Cleaner

**Disk Cleaner**
- Temp (`%temp%` + `C:\Windows\Temp`), Recycle Bin, browser cache (Chrome/Edge/Brave)
- COD Warzone cleaner — hapus folder Activision/Battle.net/Blizzard *(sengaja agresif)*

**Analyzer & Repair**
- Scan Program Files (>500 MB) & Downloads (>100 MB)
- Startup Registry + status Enabled/Disabled
- Flush DNS, Winsock reset, DISM, SFC
- HWID via `Get-CimInstance` (Win11-friendly, tanpa `wmic`)
- RAM working set trim

**Post-Install Downloader (Menu 13)**
- Discord, DirectX SDK June 2010, NVIDIA App — dengan progress bar & speed (MB/s)

---

## 📸 Screenshot

> *(Coming soon — tambahkan screenshot menu utama & progress bar di folder `assets/`)*

<!-- Contoh setelah ada gambar:
<p align="center">
  <img src="assets/menu-screenshot.png" alt="DWT Utility Menu" width="700">
</p>
-->

---

## 🏗️ Struktur Project

```
KerenCleaner/
├── Program.cs          # Entry point & routing menu
├── ConsoleUi.cs        # Banner, animasi idle, progress bar
├── NativeMethods.cs    # Windows API
├── ProcessHelper.cs    # Elevate admin & perintah sistem
├── FileCleaner.cs      # Temp, browser, COD, scan folder
├── SystemTools.cs      # DNS, DISM/SFC, HWID, startup, RAM
├── DownloadService.cs  # Post-install, yt-dlp, FFmpeg, MP3
└── KerenCleaner.csproj
```

---

## 💻 Untuk Developer

### Persyaratan
- Windows 10 / 11 (x64)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Clone & Run
```bash
git clone https://github.com/premdwt/tool-hwid.git
cd tool-hwid
dotnet run
```

### Build EXE Manual
```bash
dotnet publish -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:EnableCompressionInSingleFile=true ^
  -o publish
```

Output: `publish/KerenCleaner.exe`

---

## 📋 Changelog

### `v1.0.0` — Rilis Pertama (15 Jun 2026)
- Rilis stabil pertama dengan single-file EXE
- Disk cleaner lengkap (Temp, Recycle Bin, Browser, COD Warzone)
- Scan folder besar & Startup Analyzer
- System repair (DNS, DISM, SFC) + HWID checker
- Post-install downloader (Discord, DirectX, NVIDIA App)
- Media downloader: YouTube 1080p+audio, TikTok no WM, MP3
- Refactor codebase ke modul terpisah
- Auto-download `yt-dlp` & `FFmpeg` on first use

[**Lihat semua release →**](https://github.com/premdwt/tool-hwid/releases)

---

## ⚠️ Peringatan Penting

| Fitur | Risiko |
|-------|--------|
| Menu `6` / `10` | Menghapus folder game Activision & Battle.net secara total |
| Menu `7` | Winsock reset biasanya butuh **restart PC** |
| Menu `8` | DISM/SFC memakan waktu lama, jangan interrupt |
| Menu `9` | HWID adalah data sensitif — jangan sembarang share |
| Menu `11` | Trim working set, bukan flush standby memory sistem penuh |

---

## ⭐ Dukung Project

Kalau tool ini berguna, kasih **star** di GitHub biar lebih banyak orang yang nemuin:

<p align="center">
  <a href="https://github.com/premdwt/tool-hwid">
    <img src="https://img.shields.io/github/stars/premdwt/tool-hwid?style=social" alt="Star on GitHub">
  </a>
</p>

**Share link ini:**
```
https://github.com/premdwt/tool-hwid
```

---

## 📄 Credit

| | |
|---|---|
| **Author** | P R E M |
| **Media Engine** | [yt-dlp](https://github.com/yt-dlp/yt-dlp) · [FFmpeg](https://ffmpeg.org/) |
| **Latest Release** | [v1.0.0](https://github.com/premdwt/tool-hwid/releases/tag/v1.0.0) |

Gunakan dengan tanggung jawab sendiri. Project ini dibuat sebagai utilitas Windows open source.

---

<p align="center">
  <strong>DWT Utility — Created by P R E M</strong><br>
  <a href="https://github.com/premdwt/tool-hwid/releases/latest">Download</a> ·
  <a href="https://github.com/premdwt/tool-hwid">GitHub</a> ·
  <a href="https://github.com/premdwt/tool-hwid/releases">Releases</a>
</p>