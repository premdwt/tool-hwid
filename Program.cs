using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;
using System.Net.Http; 

class Program
{
    // API Windows
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, uint dwFlags);
    const uint SHERB_NOCONFIRMATION = 0x00000001;

    [DllImport("psapi.dll")]
    static extern int EmptyWorkingSet(IntPtr hwProc);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    static void Main()
    {
        if (!IsAdministrator())
        {
            Console.WriteLine("Meminta akses Administrator...");
            ProcessStartInfo proc = new ProcessStartInfo { UseShellExecute = true, WorkingDirectory = Environment.CurrentDirectory, FileName = Process.GetCurrentProcess().MainModule.FileName, Verb = "runas" };
            try { Process.Start(proc); } catch { } return; 
        }

        Console.Title = "DWT Utility - Created by P R E M";

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"  ____  _        _  _____ ");
            Console.WriteLine(@" |  _ \| |      | ||_   _|");
            Console.WriteLine(@" | | | | |  /\  | |  | |  ");
            Console.WriteLine(@" | |_| | |/  \| |  | |  ");
            Console.WriteLine(@" |____/|___/\___|  |_|  ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("         === Apps created by P R E M ===         ");
            Console.WriteLine("=================================================");
            Console.ResetColor();
            
            Console.WriteLine("[1] Bersihkan File Temp (%temp% & Windows Temp)");
            Console.WriteLine("[2] Kosongkan Recycle Bin");
            Console.WriteLine("[3] Bersihkan Browser Cache (Chrome, Edge, & Brave)");
            Console.WriteLine("[4] Scan Folder Raksasa (>500MB) di Program Files");
            Console.WriteLine("[5] Scan Ukuran Folder Downloads");
            Console.WriteLine("[6] Bersihkan Cache COD Warzone (Activision & Bnet)");
            Console.WriteLine("[7] Flush DNS & Reset Network (Internet Fix)");
            Console.WriteLine("[8] System Repair (SFC & DISM - Butuh Waktu)");
            Console.WriteLine("[9] Cek Info HWID (Mobo, SSD, MAC, UUID)");
            Console.WriteLine("[10] SIKAT SEMUA SAMPAH (Menu 1, 2, 3, & 6)");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[11] Kosongkan RAM / Standby Memory");
            Console.WriteLine("[12] Startup Analyzer (Dengan Status Enable/Disable)");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[13] Auto-Download Aplikasi (Post-Install Windows)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[14] Media Downloader (YouTube Shorts HD & TikTok)");
            Console.ResetColor();
            Console.WriteLine("[0] Keluar");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=================================================");
            Console.ResetColor();
            Console.Write("Pilih menu eksekusi (0-14): ");

            string pilihan = BacaInputMenuIdle();

            if (pilihan == "0") break;

            Console.WriteLine();

            // LOGIKA MENU 1 - 13
            if (pilihan == "1" || pilihan == "10") { AnimasiProgressBar("[~] Membersihkan folder Temp..."); BersihkanFolder(Path.GetTempPath()); BersihkanFolder(@"C:\Windows\Temp"); CetakSukses("File Temp berhasil dibersihkan!"); }
            if (pilihan == "2" || pilihan == "10") { AnimasiProgressBar("[~] Mengosongkan Recycle Bin..."); SHEmptyRecycleBin(IntPtr.Zero, null, SHERB_NOCONFIRMATION); CetakSukses("Recycle Bin berhasil dikosongkan!"); }
            if (pilihan == "3" || pilihan == "10") { AnimasiProgressBar("[~] Membersihkan Browser Cache..."); string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); BersihkanFolder(Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Cache")); BersihkanFolder(Path.Combine(localAppData, @"Microsoft\Edge\User Data\Default\Cache")); BersihkanFolder(Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\Default\Cache")); CetakSukses("Cache Browser berhasil dibersihkan!"); }
            if (pilihan == "4") EksekusiScanFolder(@"C:\Program Files", "SCAN FOLDER PROGRAM FILES");
            if (pilihan == "5") EksekusiScanFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"), "SCAN FOLDER DOWNLOADS");
            if (pilihan == "6" || pilihan == "10") { AnimasiProgressBar("[~] Membumihanguskan cache COD Warzone..."); string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); string prog = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); string app = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); HapusFolderPaksa(Path.Combine(local, "Activision")); HapusFolderPaksa(Path.Combine(local, "Battle.net")); HapusFolderPaksa(Path.Combine(local, "Blizzard Entertainment")); HapusFolderPaksa(Path.Combine(app, "Battle.net")); HapusFolderPaksa(Path.Combine(prog, "Activision")); HapusFolderPaksa(Path.Combine(prog, "Battle.net")); HapusFolderPaksa(Path.Combine(prog, "Blizzard Entertainment")); CetakSukses("Folder Activision & Battle.net musnah total!"); }
            if (pilihan == "7") { AnimasiProgressBar("[~] Melakukan Flush DNS..."); JalankanPerintahSistem("ipconfig", "/flushdns", true); JalankanPerintahSistem("netsh", "winsock reset", true); CetakSukses("DNS di-flush dan Network di-reset!"); }
            if (pilihan == "8") { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("[!] PERINGATAN: Proses ini memakan waktu 5 - 20 menit."); Console.ResetColor(); Console.WriteLine("\nMenjalankan DISM RestoreHealth..."); JalankanPerintahSistem("DISM", "/Online /Cleanup-Image /RestoreHealth", false); Console.WriteLine("\nMenjalankan SFC Scannow..."); JalankanPerintahSistem("sfc", "/scannow", false); CetakSukses("System Repair Selesai!"); }
            if (pilihan == "9") { AnimasiProgressBar("[~] Mengumpulkan HWID..."); Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("\n=================== INFO HWID LO ==================="); Console.ResetColor(); JalankanPerintahSistem("wmic", "baseboard get product,Manufacturer,version,serialnumber", false); JalankanPerintahSistem("wmic", "diskdrive get model,serialnumber", false); JalankanPerintahSistem("getmac", "", false); JalankanPerintahSistem("wmic", "csproduct get uuid", false); Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("===================================================="); Console.ResetColor(); TampilkanPopUpWindows("Data HWID berhasil dimuat!", "Sukses"); }
            if (pilihan == "11") { AnimasiProgressBar("[~] Membebaskan Memory RAM..."); int dibersihkan = 0; foreach (Process proc in Process.GetProcesses()) { try { EmptyWorkingSet(proc.Handle); dibersihkan++; } catch { } } CetakSukses($"RAM di-flush dari {dibersihkan} aplikasi!"); }
            if (pilihan == "12") { AnimasiProgressBar("[~] Membaca Windows Registry..."); Console.Clear(); Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("========================================================================================="); Console.WriteLine("                        STARTUP ANALYZER (Aplikasi Booting)                              "); Console.WriteLine("========================================================================================="); Console.ResetColor(); Console.WriteLine(string.Format("{0,-12} | {1,-25} | {2,-45}", "Status", "Nama Aplikasi", "Lokasi/Target")); Console.WriteLine("-----------------------------------------------------------------------------------------"); BacaStartupRegistry(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Run"); BacaStartupRegistry(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Run"); Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("========================================================================================="); Console.ResetColor(); TampilkanPopUpWindows("Analisis Startup Registry selesai!", "Sukses"); }
            
            if (pilihan == "13")
            {
                Console.Clear(); Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("=================================================");
                Console.WriteLine("        AUTO-DOWNLOADER (POST-INSTALL APPS)      ");
                Console.WriteLine("=================================================");
                Console.ResetColor();
                Console.WriteLine("[1] Discord (Installer Resmi Windows)");
                Console.WriteLine("[2] DirectX SDK (June 2010) - Offline Installer 571MB");
                Console.WriteLine("[3] NVIDIA App (Pengganti GeForce Experience)");
                Console.WriteLine("[0] Batal / Kembali");
                Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("================================================="); Console.ResetColor();
                Console.Write("Pilih aplikasi yang mau disedot (0-3): ");
                string subPilihan = BacaInputMenuIdle(); string urlDownload = ""; string namaFile = "";
                if (subPilihan == "1") { urlDownload = "https://discord.com/api/download?platform=win"; namaFile = "DiscordSetup.exe"; }
                else if (subPilihan == "2") { urlDownload = "https://download.microsoft.com/download/A/E/7/AE743F1F-632B-4809-87A9-AA1BB3458E31/DXSDK_Jun10.exe"; namaFile = "DXSDK_Jun10.exe"; }
                else if (subPilihan == "3") { urlDownload = "https://us.download.nvidia.com/nvapp/client/NVIDIA_app_beta_latest.exe"; namaFile = "NVIDIA_App_Setup.exe"; }
                if (!string.IsNullOrEmpty(urlDownload)) { string pathSimpan = Path.Combine(Environment.CurrentDirectory, namaFile); Console.WriteLine($"\n[~] Mengontak Server..."); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"[~] Mulai mendownload {namaFile}..."); Console.WriteLine($"[~] Pantau speed dan progres internet lo di bawah ini:"); Console.ResetColor(); DownloadFileDenganProgress(urlDownload, pathSimpan); }
            }

            // [UPDATE FIX TOTAL] MENU 14: DOWNLOADER KHUSUS YOUTUBE SHORTS & TIKTOK 
            if (pilihan == "14")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=================================================");
                Console.WriteLine("         DWT MEDIA DOWNLOADER SYSTEM             ");
                Console.WriteLine("=================================================");
                Console.ResetColor();
                Console.WriteLine("[1] YouTube Shorts (MP4 HD Bersuara Instan)");
                Console.WriteLine("[2] TikTok Video (Tanpa Watermark/No WM)");
                Console.WriteLine("[0] Kembali");
                Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine("================================================="); Console.ResetColor();
                Console.Write("Pilih jenis download (0-2): ");
                string modeMedia = BacaInputMenuIdle();

                if (modeMedia != "0" && (modeMedia == "1" || modeMedia == "2"))
                {
                    Console.CursorVisible = true;
                    Console.Write("\n[+] Masukkan/Paste Link URL Video: ");
                    string urlVideo = Console.ReadLine();
                    Console.CursorVisible = false;

                    if (!string.IsNullOrEmpty(urlVideo))
                    {
                        string ytDlpPath = CekDanPrepareYtDlp();

                        if (modeMedia == "1") // YouTube Shorts
                        {
                            Console.WriteLine("\n[~] Sedang mengeksekusi download YouTube Shorts...");
                            
                            // Argumen maut -f "b[ext=mp4]/best" memaksa yt-dlp mengambil format video + audio HD yang sudah menyatu bawaan Shorts
                            string arg = $"--no-playlist -f \"b[ext=mp4]/best\" --no-mtime -o \"{Environment.CurrentDirectory}\\%(title)s.%(ext)s\" \"{urlVideo}\"";
                            
                            JalankanPerintahSistem(ytDlpPath, arg, false);
                            CetakSukses("YouTube Shorts MP4 HD berhasil diamankan!");
                        }
                        else if (modeMedia == "2") // TikTok No Watermark
                        {
                            Console.WriteLine("\n[~] Sedang mengambil video TikTok Tanpa Watermark...");
                            string arg = $"--no-mtime -o \"{Environment.CurrentDirectory}\\%(title)s.%(ext)s\" \"{urlVideo}\"";
                            JalankanPerintahSistem(ytDlpPath, arg, false);
                            CetakSukses("Video TikTok Tanpa Watermark berhasil diamankan!");
                        }
                    }
                }
            }
            
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine("\n[Tekan ENTER untuk kembali ke menu utama]"); Console.ResetColor();
            Console.ReadLine();
        }
    }

    // DOWNLOAD CORE ENGINE YT-DLP SAJA (Bebas FFmpeg, Sangat Ringan!)
    static string CekDanPrepareYtDlp()
    {
        string pathExe = Path.Combine(Environment.CurrentDirectory, "yt-dlp.exe");
        if (!File.Exists(pathExe))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[!] Mendownload core engine media downloader...");
            Console.ResetColor();
            string urlCore = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";
            DownloadFileDenganProgress(urlCore, pathExe);
        }
        return pathExe;
    }

    // FUNGSI ANIMASI IDLE
    static string BacaInputMenuIdle()
    {
        string input = ""; int cursorLeft = Console.CursorLeft; int cursorTop = Console.CursorTop; int counter = 0;
        string[] animasiSliding = { "[=    ]", "[ =   ]", "[  =  ]", "[   = ]", "[    =]", "[   = ]", "[  =  ]", "[ =   ]" };
        Console.CursorVisible = false; 
        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) { Console.SetCursorPosition(cursorLeft + input.Length, cursorTop); Console.Write("          "); Console.WriteLine(); Console.CursorVisible = true; return input; }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0) { input = input.Substring(0, input.Length - 1); Console.SetCursorPosition(cursorLeft, cursorTop); Console.Write(input + "       "); }
                else if (!char.IsControl(key.KeyChar)) { input += key.KeyChar; }
            }
            Console.SetCursorPosition(cursorLeft, cursorTop); Console.Write(input);
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" " + animasiSliding[counter % animasiSliding.Length]); Console.ResetColor();
            counter++; Thread.Sleep(60); 
        }
    }

    static void TampilkanPopUpWindows(string pesan, string judul) { MessageBox(IntPtr.Zero, pesan, $"DWT Utility - {judul}", 0x00000040); }
    static void CetakSukses(string pesan) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"\n[V] {pesan}"); Console.ResetColor(); TampilkanPopUpWindows(pesan, "Berhasil"); }

    static void DownloadFileDenganProgress(string url, string path)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                using (HttpResponseMessage response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult())
                {
                    response.EnsureSuccessStatusCode(); long? totalBytes = response.Content.Headers.ContentLength;
                    using (Stream contentStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
                    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        byte[] buffer = new byte[8192]; bool isMoreToRead = true; long totalRead = 0; Stopwatch timer = new Stopwatch(); timer.Start();
                        do
                        {
                            int read = contentStream.Read(buffer, 0, buffer.Length); if (read == 0) { isMoreToRead = false; Console.WriteLine(); continue; }
                            fileStream.Write(buffer, 0, read); totalRead += read;
                            if (totalBytes.HasValue)
                            {
                                double progress = (double)totalRead / totalBytes.Value * 100; double speed = (totalRead / 1024.0 / 1024.0) / timer.Elapsed.TotalSeconds;
                                int barSize = 30; int filled = (int)((progress / 100) * barSize); string bar = new string('█', filled) + new string('-', barSize - filled);
                                Console.Write($"\r[{bar}] {progress:0.00}% | Speed: {speed:0.00} MB/s ");
                            }
                        } while (isMoreToRead);
                    }
                }
            }
        } catch { }
    }

    static void BacaStartupRegistry(RegistryKey baseKey, string runPath)
    {
        using (RegistryKey runKey = baseKey.OpenSubKey(runPath))
        {
            if (runKey != null)
            {
                foreach (string appName in runKey.GetValueNames())
                {
                    string data = runKey.GetValue(appName)?.ToString() ?? ""; string status = CekStatusStartupApproved(baseKey, appName);
                    string formatNama = appName.Length > 23 ? appName.Substring(0, 20) + "..." : appName; string formatData = data.Length > 43 ? data.Substring(0, 40) + "..." : data;
                    if (status == "Enabled") Console.ForegroundColor = ConsoleColor.Green; else Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(string.Format("[{0,-10}] | {1,-25} | {2,-45}", status, formatNama, formatData)); Console.ResetColor();
                }
            }
        }
    }

    static string CekStatusStartupApproved(RegistryKey baseKey, string appName)
    {
        try { using (RegistryKey approvedKey = baseKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run")) { if (approvedKey != null) { object val = approvedKey.GetValue(appName); if (val != null && val is byte[] bytes && bytes.Length > 0) { if (bytes[0] == 0x03 || bytes[0] % 2 != 0) return "Disabled"; } } } } catch { } return "Enabled";
    }

    static bool IsAdministrator() { using (WindowsIdentity identity = WindowsIdentity.GetCurrent()) { WindowsPrincipal principal = new WindowsPrincipal(identity); return principal.IsInRole(WindowsBuiltInRole.Administrator); } }

    static void AnimasiProgressBar(string pesan)
    {
        Console.WriteLine(pesan); int barSize = 40; 
        for (int i = 0; i <= 100; i += 4) { int filled = (i * barSize) / 100; string bar = new string('█', filled) + new string('-', barSize - filled); Console.Write($"\r[{bar}] {i}% "); Thread.Sleep(20); }
        Console.WriteLine();
    }

    static void JalankanPerintahSistem(string command, string arguments, bool sembunyikanOutput)
    {
        Process proc = new Process(); proc.StartInfo.FileName = command; proc.StartInfo.Arguments = arguments; proc.StartInfo.UseShellExecute = false;
        if (sembunyikanOutput) { proc.StartInfo.RedirectStandardOutput = true; proc.StartInfo.CreateNoWindow = true; }
        proc.Start(); proc.WaitForExit();
    }

    static void HapusFolderPaksa(string folderPath) { if (Directory.Exists(folderPath)) { try { Process proc = new Process(); proc.StartInfo.FileName = "cmd.exe"; proc.StartInfo.Arguments = $"/c rmdir /s /q \"{folderPath}\""; proc.StartInfo.UseShellExecute = false; proc.StartInfo.CreateNoWindow = true; proc.Start(); proc.WaitForExit(); } catch { } } }

    static void BersihkanFolder(string folderPath) { if (!Directory.Exists(folderPath)) return; DirectoryInfo di = new DirectoryInfo(folderPath); foreach (FileInfo file in di.GetFiles()) { try { file.Delete(); } catch { } } foreach (DirectoryInfo dir in di.GetDirectories()) { try { dir.Delete(true); } catch { } } }

    static void EksekusiScanFolder(string path, string judul)
    {
        if (!Directory.Exists(path)) { Console.WriteLine($"-> Folder {path} tidak ditemukan."); return; }
        AnimasiProgressBar($"[~] Sedang menghitung ukuran di {path}...");
        string[] subFolders = Directory.GetDirectories(path); var daftarFolder = new Dictionary<string, long>();
        foreach (string folder in subFolders) { long ukuran = AmbilUkuranFolder(folder); if (ukuran > 100 * 1024 * 1024) daftarFolder.Add(Path.GetFileName(folder), ukuran); }
        var hasilUrut = daftarFolder.OrderByDescending(x => x.Value);
        Console.Clear(); Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("=========================================================================================");
        Console.WriteLine($"                             {judul} (>100MB)                            ");
        Console.WriteLine("=========================================================================================");
        Console.ResetColor();
        Console.WriteLine(string.Format("{0,-45} | {1,-15}", "Nama Folder", "Ukuran Folder"));
        Console.WriteLine("-----------------------------------------------------------------------------------------");
        foreach (var item in hasilUrut) { double ukuranGB = (double)item.Value / (1024 * 1024 * 1024); Console.WriteLine(string.Format("{0,-45} | {1,-15}", item.Key, ukuranGB.ToString("0.00") + " GB")); }
        Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("========================================================================================="); Console.ResetColor();
        TampilkanPopUpWindows($"Pencarian {judul} Selesai!", "Sukses");
    }

    static long AmbilUkuranFolder(string folderPath) { long totalSize = 0; try { var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories); foreach (var file in files) { try { totalSize += new FileInfo(file).Length; } catch { } } } catch { } return totalSize; }
}