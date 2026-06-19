using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
class Program
{
    private const long MinScanProgramFiles = 500L * 1024 * 1024;
    private const long MinScanDownloads = 100L * 1024 * 1024;

    static void Main()
    {
        ConsoleUi.Inisialisasi();

        if (!ProcessHelper.IsAdministrator())
        {
            ConsoleUi.TampilkanPesanElevasi();
            ProcessHelper.CobaElevateAdmin();
            return;
        }

        while (true)
        {
            string? pilihan = ConsoleUi.PilihMenuUtama();
            if (pilihan is null)
                break;

            ConsoleUi.MulaiPanelKerja($"Menu {pilihan}");
            EksekusiMenu(pilihan);
            ConsoleUi.TungguKembaliKeMenu();
        }
    }

    static void EksekusiMenu(string pilihan)
    {
        bool sikatSemua = pilihan == "10";

        if (pilihan == "1" || sikatSemua)
            FileCleaner.BersihkanFileTemp();

        if (pilihan == "2" || sikatSemua)
            FileCleaner.KosongkanRecycleBin();

        if (pilihan == "3" || sikatSemua)
            FileCleaner.BersihkanBrowserCache();

        if (pilihan == "6" || sikatSemua)
            FileCleaner.BersihkanCacheCodWarzone();

        switch (pilihan)
        {
            case "4":
                FileCleaner.EksekusiScanFolder(
                    @"C:\Program Files",
                    "SCAN FOLDER PROGRAM FILES",
                    MinScanProgramFiles,
                    ">500MB");
                break;

            case "5":
                FileCleaner.EksekusiScanFolder(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                    "SCAN FOLDER DOWNLOADS",
                    MinScanDownloads,
                    ">100MB");
                break;

            case "7":
                SystemTools.FlushDnsDanResetNetwork();
                break;

            case "8":
                SystemTools.JalankanSystemRepair();
                break;

            case "9":
                SystemTools.TampilkanInfoHwid();
                break;

            case "11":
                SystemTools.FlushRamWorkingSet();
                break;

            case "12":
                SystemTools.TampilkanStartupAnalyzer();
                break;

            case "13":
                DownloadService.TampilkanMenuPostInstall();
                break;

            case "14":
                DownloadService.TampilkanMenuMediaDownloader();
                break;
        }
    }
}