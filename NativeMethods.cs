using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
internal static class NativeMethods
{
    public const uint SHERB_NOCONFIRMATION = 0x00000001;
    private const uint MB_ICONINFORMATION = 0x00000040;

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern uint SHEmptyRecycleBin(IntPtr hwnd, string? pszRootPath, uint dwFlags);

    [DllImport("psapi.dll")]
    public static extern int EmptyWorkingSet(IntPtr hwProc);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    public static void ShowInfoPopup(string message, string title) =>
        MessageBox(IntPtr.Zero, message, $"Prem Tools - {title}", MB_ICONINFORMATION);
}