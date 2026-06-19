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
        ConsoleUi.TulisPeringatan("Proses ini memakan waktu 5 - 20 menit.");

        ConsoleUi.TulisInfo("Menjalankan DISM RestoreHealth...");
        ProcessHelper.JalankanPerintahSistem("DISM", "/Online /Cleanup-Image /RestoreHealth", false);

        ConsoleUi.TulisInfo("Menjalankan SFC Scannow...");
        ProcessHelper.JalankanPerintahSistem("sfc", "/scannow", false);

        ConsoleUi.CetakSukses("System Repair Selesai!");
    }

    public static void TampilkanInfoHwid()
    {
        ConsoleUi.AnimasiProgressBar("[~] Mengumpulkan HWID...");

        ConsoleUi.TulisInfo("INFO HWID");

        JalankanPowerShell("Write-Host '--- Motherboard ---'; Get-CimInstance Win32_BaseBoard | Format-List Manufacturer,Product,Version,SerialNumber");
        JalankanPowerShell("Write-Host '--- Storage ---'; Get-CimInstance Win32_DiskDrive | Format-List Model,SerialNumber");
        JalankanPowerShell("Write-Host '--- MAC Address ---'; Get-CimInstance Win32_NetworkAdapterConfiguration -Filter 'IPEnabled=True' | Format-List Description,MACAddress");
        JalankanPowerShell("Write-Host '--- System UUID ---'; Get-CimInstance Win32_ComputerSystemProduct | Format-List UUID");

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

        var baris = new List<(string Status, string Nama, string Lokasi)>();
        KumpulkanStartupRegistry(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Run", baris);
        KumpulkanStartupRegistry(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Run", baris);

        ConsoleUi.TampilkanTabelStartup(baris);
        NativeMethods.ShowInfoPopup("Analisis Startup Registry selesai!", "Sukses");
    }

    private static void JalankanPowerShell(string command) =>
        ProcessHelper.JalankanPerintahSistem("powershell", $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"", false);

    private static void KumpulkanStartupRegistry(
        RegistryKey baseKey, string runPath, List<(string Status, string Nama, string Lokasi)> baris)
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
            baris.Add((status, formatNama, formatData));
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