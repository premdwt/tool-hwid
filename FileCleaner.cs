using System.Diagnostics;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
internal static class FileCleaner
{
    private static readonly string[] BrowserUserDataPaths =
    [
        @"Google\Chrome\User Data",
        @"Microsoft\Edge\User Data",
        @"BraveSoftware\Brave-Browser\User Data"
    ];

    public static void BersihkanFileTemp()
    {
        ConsoleUi.AnimasiProgressBar("[~] Membersihkan folder Temp...");
        BersihkanFolder(Path.GetTempPath());
        BersihkanFolder(@"C:\Windows\Temp");
        ConsoleUi.CetakSukses("File Temp berhasil dibersihkan!");
    }

    public static void KosongkanRecycleBin()
    {
        ConsoleUi.AnimasiProgressBar("[~] Mengosongkan Recycle Bin...");
        NativeMethods.SHEmptyRecycleBin(IntPtr.Zero, null, NativeMethods.SHERB_NOCONFIRMATION);
        ConsoleUi.CetakSukses("Recycle Bin berhasil dikosongkan!");
    }

    public static void BersihkanBrowserCache()
    {
        ConsoleUi.AnimasiProgressBar("[~] Membersihkan Browser Cache...");
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        foreach (string relativePath in BrowserUserDataPaths)
        {
            BersihkanCacheBrowser(Path.Combine(localAppData, relativePath));
        }

        ConsoleUi.CetakSukses("Cache Browser berhasil dibersihkan!");
    }

    public static void BersihkanCacheCodWarzone()
    {
        ConsoleUi.AnimasiProgressBar("[~] Membumihanguskan cache COD Warzone...");

        string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string prog = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        string app = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        string[] targets =
        [
            Path.Combine(local, "Activision"),
            Path.Combine(local, "Battle.net"),
            Path.Combine(local, "Blizzard Entertainment"),
            Path.Combine(app, "Battle.net"),
            Path.Combine(prog, "Activision"),
            Path.Combine(prog, "Battle.net"),
            Path.Combine(prog, "Blizzard Entertainment")
        ];

        foreach (string target in targets)
            HapusFolderPaksa(target);

        ConsoleUi.CetakSukses("Folder Activision & Battle.net musnah total!");
    }

    public static void EksekusiScanFolder(string path, string judul, long minBytes, string labelMinUkuran)
    {
        if (!Directory.Exists(path))
        {
            ConsoleUi.CetakError($"Folder tidak ditemukan: {path}");
            return;
        }

        ConsoleUi.AnimasiProgressBar($"[~] Sedang menghitung ukuran di {path}...");

        var daftarFolder = new Dictionary<string, long>();
        foreach (string folder in Directory.GetDirectories(path))
        {
            long ukuran = AmbilUkuranFolder(folder);
            if (ukuran > minBytes)
                daftarFolder[Path.GetFileName(folder)] = ukuran;
        }

        var baris = daftarFolder
            .OrderByDescending(x => x.Value)
            .Select(x => (x.Key, (double)x.Value / (1024 * 1024 * 1024)))
            .ToList();

        ConsoleUi.TampilkanTabelScan(judul, labelMinUkuran, baris);
        NativeMethods.ShowInfoPopup($"Pencarian {judul} Selesai!", "Sukses");
    }

    private static void BersihkanCacheBrowser(string userDataPath)
    {
        if (!Directory.Exists(userDataPath))
            return;

        BersihkanFolder(Path.Combine(userDataPath, @"Default\Cache"));
        BersihkanFolder(Path.Combine(userDataPath, @"Default\Code Cache"));

        foreach (string profileDir in Directory.GetDirectories(userDataPath, "Profile *"))
        {
            BersihkanFolder(Path.Combine(profileDir, "Cache"));
            BersihkanFolder(Path.Combine(profileDir, "Code Cache"));
        }
    }

    private static void BersihkanFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            return;

        DirectoryInfo di = new(folderPath);

        foreach (FileInfo file in di.GetFiles())
        {
            try { file.Delete(); }
            catch { /* file sedang dikunci sistem */ }
        }

        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            try { dir.Delete(true); }
            catch { /* folder sedang dikunci sistem */ }
        }
    }

    private static void HapusFolderPaksa(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            return;

        try
        {
            using Process proc = new();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = $"/c rmdir /s /q \"{folderPath}\"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
        }
        catch (Exception ex)
        {
            ConsoleUi.CetakError($"Gagal hapus {folderPath}: {ex.Message}");
        }
    }

    private static long AmbilUkuranFolder(string folderPath)
    {
        long totalSize = 0;

        try
        {
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories))
            {
                try { totalSize += new FileInfo(file).Length; }
                catch { /* skip file terkunci */ }
            }
        }
        catch { /* skip folder terkunci */ }

        return totalSize;
    }
}