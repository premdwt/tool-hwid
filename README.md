# ⚡ DWT Utility (KerenCleaner)

<p align="center">
  <strong>All-in-One Windows Tool — Disk Cleaner, System Repair, Post-Install Downloader, dan Media Downloader.</strong><br>
  <em>Native, ringan, cepat, dan dibuat untuk Windows 10/11.</em>
</p>

<p align="center">
  <strong>Bahasa:</strong> C# (.NET 10.0) &nbsp;|&nbsp;
  <strong>Platform:</strong> Windows x64 &nbsp;|&nbsp;
  <strong>Author:</strong> P R E M
</p>

---

## 📌 Apa itu DWT Utility?

**DWT Utility** (project name: `KerenCleaner`) adalah aplikasi **CLI (Command Line Interface)** berbasis **C# (.NET 10.0)** untuk merawat, mengoptimalkan, dan mengotomasi tugas-tugas Windows — dari bersih-bersih sampah disk, perbaikan sistem, download installer pasca-reinstall, hingga download media dari YouTube & TikTok.

Aplikasi ini:
- Berjalan **native** memakai API Windows (`shell32`, `user32`, Registry, dll.)
- Otomatis meminta hak **Administrator** saat pertama dijalankan
- Punya menu interaktif dengan animasi idle, progress bar, dan pop-up sukses Windows

**Repository GitHub:** https://github.com/premdwt/tool-hwid

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

Modul ini memakai **yt-dlp** sebagai engine download. File pendukung di-download otomatis ke folder yang sama dengan aplikasi.

### Sub-menu

| Opsi | Fitur | Output |
|------|-------|--------|
| `1` | YouTube Video | MP4 **1080p + audio** (video & audio di-merge via FFmpeg) |
| `2` | TikTok Video | MP4 tanpa watermark |
| `3` | Audio Saja | **MP3** kualitas terbaik — mendukung link YouTube & TikTok |

### Dependensi otomatis (first-run)

Saat fitur media pertama kali dipakai, aplikasi akan otomatis mendownload (jika belum ada):

| File | Fungsi | Sumber |
|------|--------|--------|
| `yt-dlp.exe` | Engine download media | [yt-dlp releases](https://github.com/yt-dlp/yt-dlp/releases) |
| `ffmpeg.exe` + `ffprobe.exe` | Merge video 1080p & konversi MP3 | [yt-dlp FFmpeg-Builds](https://github.com/yt-dlp/FFmpeg-Builds/releases) |

> **Catatan:** FFmpeg (~80 MB) hanya didownload sekali. Diperlukan untuk YouTube 1080p+suara dan ekstraksi MP3.

---

## 🧹 Fitur Cleaner & System (Detail)

### Premium Disk Cleaner
- **Temp Cleaner** — `%temp%` & `C:\Windows\Temp`
- **Recycle Bin** — via `SHEmptyRecycleBin` API
- **Browser Cache** — Chrome, Edge, Brave (Cache + Code Cache, semua profil)
- **COD Warzone Cleaner** — hapus folder Activision/Battle.net/Blizzard *(sengaja agresif untuk troubleshooting game)*

### Disk & Startup Analyzer
- Scan folder besar di Program Files (>500 MB) & Downloads (>100 MB)
- Startup Registry analyzer dengan status enable/disable dari `StartupApproved`

### Network & System Repair
- Flush DNS + Winsock reset *(restart PC disarankan setelah winsock reset)*
- DISM + SFC untuk perbaikan file sistem Windows
- HWID checker via `Get-CimInstance` (kompatibel Windows 11, tidak pakai `wmic` deprecated)
- RAM working set trim ke semua proses yang bisa diakses

### Post-Install Downloader (Menu 13)
- Discord (installer resmi Windows)
- DirectX SDK June 2010 (~571 MB)
- NVIDIA App
- Progress bar + indikator kecepatan download (MB/s)

---

## 🏗️ Struktur Project

```
KerenCleaner/
├── Program.cs          # Entry point & routing menu
├── ConsoleUi.cs        # Banner, animasi idle, progress bar, pesan UI
├── NativeMethods.cs    # Windows API (Recycle Bin, MessageBox, RAM)
├── ProcessHelper.cs    # Elevate admin & jalankan perintah sistem
├── FileCleaner.cs      # Temp, browser, COD, scan folder
├── SystemTools.cs      # DNS, DISM/SFC, HWID, startup, RAM
├── DownloadService.cs  # Post-install apps, yt-dlp, FFmpeg, MP3
├── KerenCleaner.csproj
└── README.md
```

---

## 💻 Cara Menjalankan

### Persyaratan
- **OS:** Windows 10 / 11 (x64)
- **Development:** [.NET 10 SDK](https://dotnet.microsoft.com/download) atau versi SDK yang support `net10.0`
- **Runtime (untuk .exe publish):** Tidak perlu install .NET jika pakai build `self-contained`

### Mode Development
```bash
git clone https://github.com/premdwt/tool-hwid.git
cd tool-hwid
dotnet run
```

> Jalankan terminal **as Administrator** agar semua fitur berjalan maksimal.

### Build jadi EXE (Single File)
```bash
dotnet publish -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:EnableCompressionInSingleFile=true ^
  -o publish
```

Hasilnya: `publish/KerenCleaner.exe` (~36 MB, standalone, tidak perlu install .NET).

**Cara pakai EXE:**
1. Klik kanan `KerenCleaner.exe` → **Run as administrator**
2. Pilih menu yang diinginkan
3. File hasil download (video, MP3, installer) tersimpan di folder yang sama dengan EXE

---

## 📦 Cara Buat GitHub Release

GitHub Release dipakai untuk distribusi **versi rilis resmi** — misalnya upload `KerenCleaner.exe` supaya orang bisa download tanpa clone repo.

### Metode 1: Lewat Website GitHub (Paling Mudah)

1. Buka repo: https://github.com/premdwt/tool-hwid
2. Klik tab **Releases** (sidebar kanan) → **Create a new release**
3. Isi form:
   - **Choose a tag:** ketik `v1.0.0` → klik **Create new tag: v1.0.0 on publish**
   - **Release title:** `DWT Utility v1.0.0`
   - **Description:** tulis changelog (fitur baru, perbaikan, dll.)
4. **Attach binaries:** drag & drop file `KerenCleaner.exe` dari folder `publish/`
5. Klik **Publish release**

Selesai! User bisa download EXE langsung dari halaman Releases.

### Metode 2: Lewat GitHub CLI (`gh`)

```bash
# Install GitHub CLI dulu: https://cli.github.com/

# Login (sekali saja)
gh auth login

# Buat release + upload EXE sekaligus
gh release create v1.0.0 "publish/KerenCleaner.exe" ^
  --title "DWT Utility v1.0.0" ^
  --notes "Rilis pertama dengan media downloader 1080p, TikTok, dan MP3."
```

### Tips Penomoran Versi (Semantic Versioning)

| Versi | Artinya | Contoh |
|-------|---------|--------|
| `v1.0.0` | Rilis stabil pertama | Fitur lengkap, siap dipakai |
| `v1.1.0` | Minor update | Tambah fitur baru |
| `v1.0.1` | Patch | Bug fix kecil |
| `v2.0.0` | Major | Perubahan besar / breaking change |

### Workflow Rilis yang Disarankan

```bash
# 1. Pastikan kode sudah di-push
git add .
git commit -m "persiapan rilis v1.0.0"
git push origin main

# 2. Build EXE terbaru
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish

# 3. Buat release di GitHub (website atau gh CLI)
gh release create v1.0.0 "publish/KerenCleaner.exe" --title "DWT Utility v1.0.0" --notes "Lihat README untuk daftar fitur."
```

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

## 🎨 UI/UX
- **Idle animation** `[ =  ]` kuning saat menunggu input
- **Progress bar** visual (`████████--`) untuk proses download & cleaning
- **Pop-up Windows native** + notifikasi suara setiap tugas sukses

---

## 📄 Lisensi & Credit

- **Created by:** P R E M
- **Engine media:** [yt-dlp](https://github.com/yt-dlp/yt-dlp) & [FFmpeg](https://ffmpeg.org/)
- Project ini dibuat untuk keperluan pribadi / utilitas Windows. Gunakan dengan tanggung jawab sendiri.

---

<p align="center">
  <strong>DWT Utility — Created by P R E M</strong>
</p>