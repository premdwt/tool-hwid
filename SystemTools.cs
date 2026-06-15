using System.Diagnostics;
using System.Runtime.Versioning;
using Microsoft.Win32;

[SupportedOSPlatform("windows")]
internal static class SystemTools
{
    public static void FlushDnsDanResetNetwork()
    {
        ConsoleUi.AnimasiProgressBar("[~] Melakukan Flush DNS...");
        ProcessHelper.JalankanPerintahSistem("ipconfig", "/flushdns", true);
        ProcessHelper.JalankanPerintahSistem("netsh", "winsock reset", true);
        ConsoleUi.CetakSukses("DNS di-flush dan Network di-reset! Restart PC disarankan.");
    }

    public static void JalankanSystemRepair()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[!] PERINGATAN: Proses ini memakan waktu 5 - 20 menit.");
        Console.ResetColor();

        Console.WriteLine("\nMenjalankan DISM RestoreHealth...");
        ProcessHelper.JalankanPerintahSistem("DISM", "/Online /Cleanup-Image /RestoreHealth", false);

        Console.WriteLine("\nMenjalankan SFC Scannow...");
        ProcessHelper.JalankanPerintahSistem("sfc", "/scannow", false);

        ConsoleUi.CetakSukses("System Repair Selesai!");
    }

    public static void TampilkanInfoHwid()
    {
        ConsoleUi.AnimasiProgressBar("[~] Mengumpulkan HWID...");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n=================== INFO HWID LO ===================");
        Console.ResetColor();

        JalankanPowerShell("Write-Host '--- Motherboard ---'; Get-CimInstance Win32_BaseBoard | Format-List Manufacturer,Product,Version,SerialNumber");
        JalankanPowerShell("Write-Host '--- Storage ---'; Get-CimInstance Win32_DiskDrive | Format-List Model,SerialNumber");
        JalankanPowerShell("Write-Host '--- MAC Address ---'; Get-CimInstance Win32_NetworkAdapterConfiguration -Filter 'IPEnabled=True' | Format-List Description,MACAddress");
        JalankanPowerShell("Write-Host '--- System UUID ---'; Get-CimInstance Win32_ComputerSystemProduct | Format-List UUID");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("====================================================");
        Console.ResetColor();
        NativeMethods.ShowInfoPopup("Data HWID berhasil dimuat!", "Sukses");
    }

    public static void FlushRamWorkingSet()
    {
        ConsoleUi.AnimasiProgressBar("[~] Membebaskan Memory RAM...");
        int dibersihkan = 0;

        foreach (Process proc in Process.GetProcesses())
        {
            try
            {
                NativeMethods.EmptyWorkingSet(proc.Handle);
                dibersihkan++;
            }
            catch { /* skip proses sistem yang tidak bisa diakses */ }
        }

        ConsoleUi.CetakSukses($"RAM working set di-trim dari {dibersihkan} proses!");
    }

    public static void TampilkanStartupAnalyzer()
    {
        ConsoleUi.AnimasiProgressBar("[~] Membaca Windows Registry...");

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("=========================================================================================");
        Console.WriteLine("                        STARTUP ANALYZER (Aplikasi Booting)                              ");
        Console.WriteLine("=========================================================================================");
        Console.ResetColor();
        Console.WriteLine($"{ "Status",-12} | {"Nama Aplikasi",-25} | {"Lokasi/Target",-45}");
        Console.WriteLine("-----------------------------------------------------------------------------------------");

        BacaStartupRegistry(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Run");
        BacaStartupRegistry(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Run");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("=========================================================================================");
        Console.ResetColor();
        NativeMethods.ShowInfoPopup("Analisis Startup Registry selesai!", "Sukses");
    }

    private static void JalankanPowerShell(string command) =>
        ProcessHelper.JalankanPerintahSistem("powershell", $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"", false);

    private static void BacaStartupRegistry(RegistryKey baseKey, string runPath)
    {
        using RegistryKey? runKey = baseKey.OpenSubKey(runPath);
        if (runKey is null)
            return;

        foreach (string appName in runKey.GetValueNames())
        {
            string data = runKey.GetValue(appName)?.ToString() ?? "";
            string status = CekStatusStartupApproved(baseKey, appName);
            string formatNama = appName.Length > 23 ? appName[..20] + "..." : appName;
            string formatData = data.Length > 43 ? data[..40] + "..." : data;

            Console.ForegroundColor = status == "Enabled"
                ? ConsoleColor.Green
                : ConsoleColor.DarkGray;

            Console.WriteLine($"[{status,-10}] | {formatNama,-25} | {formatData,-45}");
            Console.ResetColor();
        }
    }

    private static string CekStatusStartupApproved(RegistryKey baseKey, string appName)
    {
        try
        {
            using RegistryKey? approvedKey = baseKey.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run");

            if (approvedKey?.GetValue(appName) is byte[] bytes && bytes.Length > 0)
            {
                if (bytes[0] == 0x03 || bytes[0] % 2 != 0)
                    return "Disabled";
            }
        }
        catch { /* registry key tidak bisa dibaca */ }

        return "Enabled";
    }
}