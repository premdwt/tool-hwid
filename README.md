# ⚡ DWT Utility

<p align="center">
  <strong>An Ultimate All-in-One Windows Disk Cleaner, System Repair, Post-Install Automation, and Media Downloader Tool.</strong><br>
  <em>Lightweight, native, ultra-fast, and highly effective.</em>
</p>

---

## 📌 Apa itu DWT Utility?

**DWT Utility** adalah aplikasi *command-line interface* (CLI) berbasis **C# (.NET)** yang dirancang untuk merawat, mempercepat, mendownload media, dan mengoptimalkan sistem operasi Windows secara instan. 

Berbeda dengan aplikasi pembersih pihak ketiga yang sering kali berat atau diam-diam memakan resource RAM, **DWT Utility** berjalan secara *native* menggunakan API Windows resmi. Aplikasi ini mengeksekusi perintah langsung di level sistem dengan kecepatan tinggi, ringan, tanpa ribet, dan otomatis mendeteksi serta meminta hak akses **Administrator** sejak pertama kali dijalankan.

---

## 🚀 Fitur Utama

Aplikasi ini dibagi menjadi beberapa modul krusial yang bisa dieksekusi melalui menu interaktif:

### 🧹 Premium Disk Cleaner
* **File Temp Cleaner:** Membersihkan folder `%temp%` User dan `C:\Windows\Temp` secara aman (automatis melewati file yang sedang dikunci oleh sistem).
* **Recycle Bin Shredder:** Mengosongkan tempat sampah secara permanen langsung via API Windows tanpa memunculkan pop-up konfirmasi yang mengganggu.
* **Smart Browser Cache Cleaner:** Menghapus puluhan GB sampah cache dari tiga browser utama (**Google Chrome, Microsoft Edge, dan Brave Browser**) secara aman tanpa membuat Anda ter-logout dari akun-akun penting.
* **COD Warzone Cache Cleaner:** Menghapus folder cache tersumbat milik Activision, Battle.net, dan Blizzard yang sering menyebabkan masalah pada game Call of Duty Warzone.

### 🔍 Disk & Startup Analyzer
* **Folder Raksasa Scanner:** Memindai folder `C:\Program Files` dan mengurutkan folder/game mana saja yang memakan ruang penyimpanan lebih dari 500 MB dari yang terbesar ke terkecil.
* **Downloads Folder Analyzer:** Menghitung dan melacak file-file berukuran besar (>100MB) di folder unduhan Anda.
* **Startup Analyzer PRO:** Membongkar Windows Registry secara *real-time* untuk menampilkan aplikasi apa saja yang otomatis berjalan saat *booting*, lengkap dengan status **[Enabled / Disabled]**.

### 🛠️ Network & System Repair
* **Internet Connection Fixer:** Melakukan *Flush DNS* dan reset jaringan (*Winsock reset*) untuk mengatasi koneksi internet yang macet atau tidak stabil.
* **Windows System Repair:** Menjalankan pemindaian mendalam menggunakan utilitas bawaan Windows seperti `DISM RestoreHealth` dan `SFC Scannow` untuk memperbaiki file sistem yang korup.
* **HWID Checker:** Mengambil informasi Hardware ID sensitif secara akurat (Motherboard Serial, SSD/HDD Serial, MAC Address, dan System UUID).
* **RAM / Memory Auto-Flush:** Membebaskan ruang RAM yang tersangkut di *Standby List* dari seluruh proses background agar PC kembali segar instan tanpa *restart*.

### 🎥 Media Downloader (New Update!)
* **YouTube Shorts HD Downloader:** Mengunduh video YouTube Shorts langsung dalam format MP4 HD. Format ini memaksa server menyediakan file video yang sudah menyatu dengan audionya secara instan sehingga dijamin **100% ber-suara** tanpa membutuhkan aplikasi *encoder* tambahan.
* **TikTok No Watermark Downloader:** Mengunduh dan mengamankan video TikTok bersih dari logo/watermark bawaan langsung ke folder aplikasi Anda.
* *Note: Modul ini dirancang ultra-lightweight karena bebas dari dependensi berat seperti FFmpeg, membuat proses download jauh lebih cepat, stabil, dan anti-gagal.*

### 📦 Post-Install Downloader
* **Auto-Downloader Sub-Menu:** Menyediakan menu otomatis untuk mengunduh installer resmi aplikasi krusial pasca-reinstall Windows seperti **Discord**, **DirectX SDK (June 2010)**, dan **NVIDIA App**, lengkap dengan animasi *Progress Bar* dan indikator kecepatan internet (MB/s).

---

## 🎨 Tampilan Antarmuka (UI/UX)
* **Interactive Idle Animation:** Memiliki animasi pengetikan menu `[ =  ]` warna kuning yang terus bergerak dinamis saat aplikasi sedang *idle* menunggu input dari Anda.
* **Smooth Progress Bar:** Menggunakan visualisasi bar persentase (`████████--`) untuk memantau proses penghapusan atau pengunduhan file.
* **Native Notification Sound & Pop-up:** Terintegrasi dengan sistem API Windows (`user32.dll`) yang akan memunculkan pop-up pesan sukses beserta bunyi notifikasi singkat ("Ting!") setiap kali tugas berhasil diselesaikan.

---

## 💻 Cara Menjalankan Project

### Persyaratan Sistem
* Windows 10 atau Windows 11
* .NET SDK (Versi terbaru direkomendasikan)

### Cara Eksekusi (Masa Development)
Buka terminal Anda di folder project ini, lalu jalankan:
```bash
dotnet run