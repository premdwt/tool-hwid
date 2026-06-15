using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
internal static class DownloadService
{
    private const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

    public static void TampilkanMenuPostInstall()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("=================================================");
        Console.WriteLine("        AUTO-DOWNLOADER (POST-INSTALL APPS)      ");
        Console.WriteLine("=================================================");
        Console.ResetColor();
        Console.WriteLine("[1] Discord (Installer Resmi Windows)");
        Console.WriteLine("[2] DirectX SDK (June 2010) - Offline Installer 571MB");
        Console.WriteLine("[3] NVIDIA App (Pengganti GeForce Experience)");
        Console.WriteLine("[0] Batal / Kembali");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================");
        Console.ResetColor();
        Console.Write("Pilih aplikasi yang mau disedot (0-3): ");

        string subPilihan = ConsoleUi.BacaInputMenuIdle();
        if (subPilihan == "0")
            return;

        (string url, string namaFile)? target = subPilihan switch
        {
            "1" => ("https://discord.com/api/download?platform=win", "DiscordSetup.exe"),
            "2" => ("https://download.microsoft.com/download/A/E/7/AE743F1F-632B-4809-87A9-AA1BB3458E31/DXSDK_Jun10.exe", "DXSDK_Jun10.exe"),
            "3" => ("https://us.download.nvidia.com/nvapp/client/NVIDIA_app_beta_latest.exe", "NVIDIA_App_Setup.exe"),
            _ => null
        };

        if (target is null)
        {
            ConsoleUi.CetakError("Pilihan tidak valid.");
            return;
        }

        string pathSimpan = Path.Combine(Environment.CurrentDirectory, target.Value.namaFile);
        Console.WriteLine("\n[~] Mengontak Server...");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[~] Mulai mendownload {target.Value.namaFile}...");
        Console.WriteLine("[~] Pantau speed dan progres internet lo di bawah ini:");
        Console.ResetColor();

        if (DownloadFileDenganProgress(target.Value.url, pathSimpan))
            ConsoleUi.CetakSukses($"{target.Value.namaFile} berhasil didownload!");
    }

    public static void TampilkanMenuMediaDownloader()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("=================================================");
        Console.WriteLine("         DWT MEDIA DOWNLOADER SYSTEM             ");
        Console.WriteLine("=================================================");
        Console.ResetColor();
        Console.WriteLine("[1] YouTube (MP4 1080p + Audio)");
        Console.WriteLine("[2] TikTok Video (Tanpa Watermark/No WM)");
        Console.WriteLine("[3] Audio Saja (MP3) - YouTube & TikTok");
        Console.WriteLine("[0] Kembali");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================");
        Console.ResetColor();
        Console.Write("Pilih jenis download (0-3): ");

        string modeMedia = ConsoleUi.BacaInputMenuIdle();
        if (modeMedia is not ("1" or "2" or "3"))
            return;

        Console.CursorVisible = true;
        Console.Write("\n[+] Masukkan/Paste Link URL Video: ");
        string? urlVideo = Console.ReadLine();
        Console.CursorVisible = false;

        if (string.IsNullOrWhiteSpace(urlVideo))
        {
            ConsoleUi.CetakError("URL tidak boleh kosong.");
            return;
        }

        string ytDlpPath = CekDanPrepareYtDlp();
        if (!File.Exists(ytDlpPath))
        {
            ConsoleUi.CetakError("yt-dlp.exe tidak tersedia. Download media dibatalkan.");
            return;
        }

        string outputTemplate = Path.Combine(Environment.CurrentDirectory, "%(title)s.%(ext)s");

        switch (modeMedia)
        {
            case "1":
                DownloadYoutubeVideo(ytDlpPath, outputTemplate, urlVideo);
                break;
            case "2":
                DownloadTikTokVideo(ytDlpPath, outputTemplate, urlVideo);
                break;
            case "3":
                DownloadAudioMp3(ytDlpPath, outputTemplate, urlVideo);
                break;
        }
    }

    private static void DownloadYoutubeVideo(string ytDlpPath, string outputTemplate, string urlVideo)
    {
        string? ffmpegDir = CekDanPrepareFfmpeg("merge video 1080p + audio");
        if (ffmpegDir is null)
        {
            ConsoleUi.CetakError("FFmpeg tidak tersedia. Download 1080p + audio dibatalkan.");
            return;
        }

        Console.WriteLine("\n[~] Sedang download YouTube 1080p + audio (merge via FFmpeg)...");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("[i] Pertama kali butuh download FFmpeg (~80MB). Proses merge butuh waktu sedikit lebih lama.");
        Console.ResetColor();

        string format = "bestvideo[height<=1080][ext=mp4]+bestaudio[ext=m4a]/bestvideo[height<=1080]+bestaudio/best[height<=1080]/best";
        string arg =
            $"--no-playlist -f \"{format}\" --merge-output-format mp4 --ffmpeg-location \"{ffmpegDir}\" --no-mtime -o \"{outputTemplate}\" \"{urlVideo}\"";

        ProcessHelper.JalankanPerintahSistem(ytDlpPath, arg, false);
        ConsoleUi.CetakSukses("YouTube MP4 1080p + audio berhasil diamankan!");
    }

    private static void DownloadTikTokVideo(string ytDlpPath, string outputTemplate, string urlVideo)
    {
        Console.WriteLine("\n[~] Sedang mengambil video TikTok Tanpa Watermark...");
        string arg = $"--no-mtime -o \"{outputTemplate}\" \"{urlVideo}\"";
        ProcessHelper.JalankanPerintahSistem(ytDlpPath, arg, false);
        ConsoleUi.CetakSukses("Video TikTok Tanpa Watermark berhasil diamankan!");
    }

    private static void DownloadAudioMp3(string ytDlpPath, string outputTemplate, string urlVideo)
    {
        string? ffmpegDir = CekDanPrepareFfmpeg("konversi audio ke MP3");
        if (ffmpegDir is null)
        {
            ConsoleUi.CetakError("FFmpeg tidak tersedia. Download audio MP3 dibatalkan.");
            return;
        }

        Console.WriteLine("\n[~] Sedang mengekstrak audio & konversi ke MP3...");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("[i] Mendukung link YouTube & TikTok. Kualitas audio: terbaik.");
        Console.ResetColor();

        string arg =
            $"--no-playlist -x --audio-format mp3 --audio-quality 0 --ffmpeg-location \"{ffmpegDir}\" --no-mtime -o \"{outputTemplate}\" \"{urlVideo}\"";

        ProcessHelper.JalankanPerintahSistem(ytDlpPath, arg, false);
        ConsoleUi.CetakSukses("Audio MP3 berhasil diamankan!");
    }

    private static string? CekDanPrepareFfmpeg(string tujuan)
    {
        string appDir = Environment.CurrentDirectory;
        string ffmpegExe = Path.Combine(appDir, "ffmpeg.exe");
        string ffprobeExe = Path.Combine(appDir, "ffprobe.exe");

        if (File.Exists(ffmpegExe) && File.Exists(ffprobeExe))
            return appDir;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[!] Mendownload FFmpeg untuk {tujuan}...");
        Console.ResetColor();

        const string urlZip = "https://github.com/yt-dlp/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip";
        string zipPath = Path.Combine(appDir, "ffmpeg-download-temp.zip");

        if (!DownloadFileDenganProgress(urlZip, zipPath))
            return null;

        try
        {
            using ZipArchive archive = ZipFile.OpenRead(zipPath);

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string fileName = Path.GetFileName(entry.FullName);
                if (fileName is not ("ffmpeg.exe" or "ffprobe.exe"))
                    continue;

                entry.ExtractToFile(Path.Combine(appDir, fileName), overwrite: true);
            }
        }
        catch (Exception ex)
        {
            ConsoleUi.CetakError($"Gagal extract FFmpeg: {ex.Message}");
            return null;
        }
        finally
        {
            if (File.Exists(zipPath))
            {
                try { File.Delete(zipPath); }
                catch { /* zip sementara bisa dibiarkan */ }
            }
        }

        if (!File.Exists(ffmpegExe))
        {
            ConsoleUi.CetakError("ffmpeg.exe tidak ditemukan setelah extract.");
            return null;
        }

        return appDir;
    }

    private static string CekDanPrepareYtDlp()
    {
        string pathExe = Path.Combine(Environment.CurrentDirectory, "yt-dlp.exe");

        if (!File.Exists(pathExe))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[!] Mendownload core engine media downloader...");
            Console.ResetColor();

            const string urlCore = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";
            DownloadFileDenganProgress(urlCore, pathExe);
        }

        return pathExe;
    }

    private static bool DownloadFileDenganProgress(string url, string path)
    {
        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            using HttpResponseMessage response = client
                .GetAsync(url, HttpCompletionOption.ResponseHeadersRead)
                .GetAwaiter()
                .GetResult();

            response.EnsureSuccessStatusCode();
            long? totalBytes = response.Content.Headers.ContentLength;

            using Stream contentStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            byte[] buffer = new byte[8192];
            long totalRead = 0;
            Stopwatch timer = Stopwatch.StartNew();

            while (true)
            {
                int read = contentStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                {
                    Console.WriteLine();
                    break;
                }

                fileStream.Write(buffer, 0, read);
                totalRead += read;

                if (totalBytes.HasValue && timer.Elapsed.TotalSeconds > 0)
                {
                    double progress = (double)totalRead / totalBytes.Value * 100;
                    double speed = (totalRead / 1024.0 / 1024.0) / timer.Elapsed.TotalSeconds;
                    const int barSize = 30;
                    int filled = (int)(progress / 100 * barSize);
                    string bar = new string('█', filled) + new string('-', barSize - filled);
                    Console.Write($"\r[{bar}] {progress:0.00}% | Speed: {speed:0.00} MB/s ");
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            ConsoleUi.CetakError($"Download gagal: {ex.Message}");
            return false;
        }
    }
}