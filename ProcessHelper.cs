using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Principal;

[SupportedOSPlatform("windows")]
internal static class ProcessHelper
{
    public static bool IsAdministrator()
    {
        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static bool CobaElevateAdmin()
    {
        string? exePath = Environment.ProcessPath
            ?? Process.GetCurrentProcess().MainModule?.FileName;

        if (string.IsNullOrEmpty(exePath))
        {
            ConsoleUi.CetakError("Tidak bisa menemukan path executable untuk elevate admin.");
            return false;
        }

        ProcessStartInfo proc = new()
        {
            UseShellExecute = true,
            WorkingDirectory = Environment.CurrentDirectory,
            FileName = exePath,
            Verb = "runas"
        };

        try
        {
            Process.Start(proc);
            return true;
        }
        catch
        {
            ConsoleUi.CetakError("Akses Administrator ditolak. Beberapa fitur butuh hak admin.");
            return false;
        }
    }

    public static void JalankanPerintahSistem(string command, string arguments, bool sembunyikanOutput)
    {
        using Process proc = new();
        proc.StartInfo.FileName = command;
        proc.StartInfo.Arguments = arguments;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;

        if (sembunyikanOutput)
        {
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
        }
        else
        {
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            proc.OutputDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine(e.Data);
            };
            proc.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine(e.Data);
            };
        }

        proc.Start();

        if (!sembunyikanOutput)
        {
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
        }
        else
        {
            proc.StandardOutput.ReadToEnd();
            proc.StandardError.ReadToEnd();
        }

        proc.WaitForExit();
    }
}