# ⚡ DWT Utility

<p align="center">
  <strong>An Ultimate All-in-One Windows Disk Cleaner, System Repair, and Post-Install Automation Tool.</strong><br>
  <em>Lightweight, native, ultra-fast, and highly effective.</em>
</p>

---

## 📌 Apa itu DWT Utility?

**DWT Utility** adalah aplikasi *command-line interface* (CLI) berbasis **C# (.NET)** yang dirancang untuk merawat, mempercepat, dan mengoptimalkan sistem operasi Windows secara instan. 

Berbeda dengan aplikasi pembersih pihak ketiga yang sering kali berat, lelet saat memuat (*startup time*), atau diam-diam memakan resource RAM, **DWT Utility** berjalan secara *native* menggunakan API Windows resmi. Aplikasi ini mengeksekusi perintah langsung di level sistem dengan kecepatan tinggi, ringan, tanpa ribet, dan otomatis mendeteksi serta meminta hak akses **Administrator** sejak pertama kali dijalankan.

---

## 🚀 Fitur Utama

Aplikasi ini dibagi menjadi beberapa modul krusial yang bisa dieksekusi melalui menu interaktif:

### 🧹 Premium Disk Cleaner
* **File Temp Cleaner:** Membersihkan folder `%temp%` User dan `C:\Windows\Temp` secara aman (otomatis melewati file yang sedang dikunci oleh sistem).
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

### 📦 Post-Install Downloader
* **Auto-Downloader Sub-Menu:** Menyediakan menu otomatis untuk mengunduh installer resmi aplikasi krusial pasca-reinstall Windows seperti **Discord**, **DirectX SDK (June 2010)**, dan **NVIDIA App**, lengkap dengan animasi *Progress Bar* dan indikator kecepatan internet (MB/s).

---

## 🎨 Tampilan Antarmuka (UI/UX)
* **Interactive Idle Animation:** Memiliki animasi pengetikan menu `[ =  ]` yang terus bergerak dinamis saat aplikasi sedang menunggu input dari Anda.
* **Smooth Progress Bar:** Menggunakan visualisasi bar persentase (`████████--`) untuk memantau proses penghapusan atau pengunduhan file.
* **Native Notification Sound:** Terintegrasi dengan sistem audio Windows yang akan memunculkan pop-up pesan dan bunyi notifikasi singkat ("Ting!") setiap kali tugas berhasil diselesaikan.

---

## 💻 Cara Menjalankan Project

### Persyaratan Sistem
* Windows 10 atau Windows 11
* .NET SDK (Versi terbaru direkomendasikan)

### Cara Eksekusi (Masa Development)
Buka terminal Anda di folder project ini, lalu jalankan:
```bash
dotnet run