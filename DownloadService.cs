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
        string? subPilihan = ConsoleUi.PilihSubMenu(
            "AUTO-DOWNLOADER (POST-INSTALL APPS)",
            ("1", "Discord (Installer Resmi Windows)"),
            ("2", "DirectX SDK (June 2010) - Offline Installer 571MB"),
            ("3", "NVIDIA App (Pengganti GeForce Experience)"),
            ("0", "Batal / Kembali"));

        if (subPilihan is null)
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
        ConsoleUi.TulisInfo("Mengontak server...");
        ConsoleUi.TulisPeringatan($"Mulai mendownload {target.Value.namaFile}...");
        ConsoleUi.TulisInfo("Pantau speed dan progres di bawah ini:");

        if (DownloadFileDenganProgress(target.Value.url, pathSimpan))
            ConsoleUi.CetakSukses($"{target.Value.namaFile} berhasil didownload!");
    }

    public static void TampilkanMenuMediaDownloader()
    {
        string? modeMedia = ConsoleUi.PilihSubMenu(
            "Prem Tools — Media Downloader",
            ("1", "YouTube (MP4 1080p + Audio)"),
            ("2", "TikTok Video (Tanpa Watermark/No WM)"),
            ("3", "Audio Saja (MP3) - YouTube & TikTok"),
            ("0", "Kembali"));

        if (modeMedia is not ("1" or "2" or "3"))
            return;

        string? urlVideo = ConsoleUi.BacaInputTeks("\n[+] Masukkan/Paste Link URL Video:");

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

        ConsoleUi.TulisInfo("Sedang download YouTube 1080p + audio (merge via FFmpeg)...");
        ConsoleUi.TulisLog("Pertama kali butuh download FFmpeg (~80MB). Proses merge butuh waktu sedikit lebih lama.");

        string format = "bestvideo[height<=1080][ext=mp4]+bestaudio[ext=m4a]/bestvideo[height<=1080]+bestaudio/best[height<=1080]/best";
        string arg =
            $"--no-playlist -f \"{format}\" --merge-output-format mp4 --ffmpeg-location \"{ffmpegDir}\" --no-mtime -o \"{outputTemplate}\" \"{urlVideo}\"";

        ProcessHelper.JalankanPerintahSistem(ytDlpPath, arg, false);
        ConsoleUi.CetakSukses("YouTube MP4 1080p + audio berhasil diamankan!");
    }

    private static void DownloadTikTokVideo(string ytDlpPath, string outputTemplate, string urlVideo)
    {
        ConsoleUi.TulisInfo("Sedang mengambil video TikTok Tanpa Watermark...");
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

        ConsoleUi.TulisInfo("Sedang mengekstrak audio & konversi ke MP3...");
        ConsoleUi.TulisLog("Mendukung link YouTube & TikTok. Kualitas audio: terbaik.");

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

        ConsoleUi.TulisPeringatan($"Mendownload FFmpeg untuk {tujuan}...");

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
            ConsoleUi.TulisPeringatan("Mendownload core engine media downloader...");

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
                    ConsoleUi.SelesaiProgressDownload();
                    break;
                }

                fileStream.Write(buffer, 0, read);
                totalRead += read;

                if (totalBytes.HasValue && timer.Elapsed.TotalSeconds > 0)
                {
                    double progress = (double)totalRead / totalBytes.Value * 100;
                    double speed = (totalRead / 1024.0 / 1024.0) / timer.Elapsed.TotalSeconds;
                    ConsoleUi.AnimasiProgressDownload("Downloading", progress, speed);
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