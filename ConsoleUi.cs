using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
internal static class ConsoleUi
{
    private static readonly string[] IdleAnimation =
    [
        "[=    ]", "[ =   ]", "[  =  ]", "[   = ]",
        "[    =]", "[   = ]", "[  =  ]", "[ =   ]"
    ];

    public static void TampilkanBanner()
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
    }

    public static void TampilkanMenuUtama()
    {
        Console.WriteLine("[1] Bersihkan File Temp (%temp% & Windows Temp)");
        Console.WriteLine("[2] Kosongkan Recycle Bin");
        Console.WriteLine("[3] Bersihkan Browser Cache (Chrome, Edge, & Brave)");
        Console.WriteLine("[4] Scan Folder Raksasa (>500MB) di Program Files");
        Console.WriteLine("[5] Scan Ukuran Folder Downloads (>100MB)");
        Console.WriteLine("[6] Bersihkan Cache COD Warzone (Activision & Bnet)");
        Console.WriteLine("[7] Flush DNS & Reset Network (Internet Fix)");
        Console.WriteLine("[8] System Repair (SFC & DISM - Butuh Waktu)");
        Console.WriteLine("[9] Cek Info HWID (Mobo, SSD, MAC, UUID)");
        Console.WriteLine("[10] SIKAT SEMUA SAMPAH (Menu 1, 2, 3, & 6)");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[11] Kosongkan RAM / Working Set");
        Console.WriteLine("[12] Startup Analyzer (Dengan Status Enable/Disable)");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("[13] Auto-Download Aplikasi (Post-Install Windows)");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[14] Media Downloader (YouTube, TikTok, & MP3)");
        Console.ResetColor();
        Console.WriteLine("[0] Keluar");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================");
        Console.ResetColor();
    }

    public static string BacaInputMenuIdle()
    {
        string input = "";
        int cursorLeft = Console.CursorLeft;
        int cursorTop = Console.CursorTop;
        int counter = 0;

        Console.CursorVisible = false;

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.SetCursorPosition(cursorLeft + input.Length, cursorTop);
                    Console.Write("          ");
                    Console.WriteLine();
                    Console.CursorVisible = true;
                    return input;
                }

                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input[..^1];
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(input + "       ");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                }
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(input);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" " + IdleAnimation[counter % IdleAnimation.Length]);
            Console.ResetColor();
            counter++;
            Thread.Sleep(60);
        }
    }

    public static void AnimasiProgressBar(string pesan)
    {
        Console.WriteLine(pesan);
        const int barSize = 40;

        for (int i = 0; i <= 100; i += 4)
        {
            int filled = i * barSize / 100;
            string bar = new string('█', filled) + new string('-', barSize - filled);
            Console.Write($"\r[{bar}] {i}% ");
            Thread.Sleep(20);
        }

        Console.WriteLine();
    }

    public static void CetakSukses(string pesan)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n[V] {pesan}");
        Console.ResetColor();
        NativeMethods.ShowInfoPopup(pesan, "Berhasil");
    }

    public static void CetakError(string pesan)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[X] {pesan}");
        Console.ResetColor();
    }

    public static void TungguKembaliKeMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n[Tekan ENTER untuk kembali ke menu utama]");
        Console.ResetColor();
        Console.ReadLine();
    }
}