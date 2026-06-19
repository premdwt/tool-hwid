using System.Runtime.Versioning;
using Spectre.Console;

[SupportedOSPlatform("windows")]
internal static class ConsoleUi
{
    public const string AppVersion = "2.0.0";

    private static readonly (string Id, string Label, string ShortLabel, string Description, string Accent)[] MenuItems =
    [
        ("1",  "Bersihkan File Temp",         "File Temp",          "Bersihkan %temp% dan Windows Temp",              "cyan"),
        ("2",  "Kosongkan Recycle Bin",       "Recycle Bin",        "Kosongkan Recycle Bin tanpa konfirmasi",         "cyan"),
        ("3",  "Bersihkan Browser Cache",   "Browser Cache",      "Chrome, Edge, dan Brave",                        "cyan"),
        ("4",  "Scan Folder Raksasa",         "Scan Program Files", "Folder >500MB di Program Files",                 "magenta"),
        ("5",  "Scan Folder Downloads",       "Scan Downloads",     "Folder >100MB di Downloads",                     "magenta"),
        ("6",  "Bersihkan Cache COD Warzone", "COD Warzone",        "Cache Activision dan Battle.net",                "cyan"),
        ("7",  "Flush DNS & Reset Network",   "DNS / Network",      "Flush DNS dan reset winsock",                      "yellow"),
        ("8",  "System Repair (SFC & DISM)",  "System Repair",      "DISM RestoreHealth + SFC scannow",               "red"),
        ("9",  "Cek Info HWID",               "Info HWID",          "Motherboard, SSD, MAC, UUID",                    "magenta"),
        ("10", "SIKAT SEMUA SAMPAH",          "SIKAT SEMUA",        "Jalankan menu 1, 2, 3, dan 6",                  "yellow"),
        ("11", "Kosongkan RAM / Working Set", "Flush RAM",          "Trim working set semua proses",                  "green"),
        ("12", "Startup Analyzer",            "Startup Analyzer",   "Registry Run enable/disable",                    "green"),
        ("13", "Auto-Download Aplikasi",      "Auto-Download",      "Discord, DirectX SDK, NVIDIA App",               "magenta"),
        ("14", "Media Downloader",            "Media Download",     "YouTube, TikTok, MP3 via yt-dlp",                "red"),
        ("0",  "Keluar",                      "Keluar",             "Tutup Prem Tools",                               "grey50"),
    ];

    public static void Inisialisasi()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Prem Tools";
    }

    public static string? PilihMenuUtama()
    {
        int selected = 0;
        string? result = null;
        Console.CursorVisible = false;

        try
        {
            AnsiConsole.Clear();

            AnsiConsole.Live(BuatLayoutUtama(selected))
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .Start(ctx =>
                {
                    ctx.UpdateTarget(BuatLayoutUtama(selected));
                    bool selesai = false;

                    while (!selesai)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                        if (CobaAmbilMenuLangsung(key, out string? langsung))
                        {
                            result = langsung;
                            selesai = true;
                            continue;
                        }

                        bool pindah = false;

                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                int prev = (selected - 1 + MenuItems.Length) % MenuItems.Length;
                                if (prev != selected)
                                {
                                    selected = prev;
                                    pindah = true;
                                }
                                break;

                            case ConsoleKey.DownArrow:
                                int next = (selected + 1) % MenuItems.Length;
                                if (next != selected)
                                {
                                    selected = next;
                                    pindah = true;
                                }
                                break;

                            case ConsoleKey.Enter:
                                result = MenuItems[selected].Id == "0" ? null : MenuItems[selected].Id;
                                selesai = true;
                                break;

                            case ConsoleKey.Escape:
                                result = null;
                                selesai = true;
                                break;
                        }

                        if (pindah)
                            ctx.UpdateTarget(BuatLayoutUtama(selected));
                    }
                });

            return result;
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    public static string? PilihSubMenu(string judul, params (string Id, string Label)[] opsi)
    {
        MulaiPanelKerja(judul);

        var prompt = new SelectionPrompt<string>()
            .Title("[grey50]arrow keys + Enter[/]")
            .PageSize(Math.Min(10, opsi.Length))
            .HighlightStyle(new Style(foreground: Color.Black, background: Color.Magenta1))
            .AddChoices(opsi.Select(o => o.Id));

        prompt.UseConverter(id =>
        {
            var label = opsi.First(o => o.Id == id).Label;
            string idMarkup = FormatNomorMenu(id);
            string labelMarkup = E(label);

            return id == "0"
                ? $"{idMarkup} [grey50]{labelMarkup}[/]"
                : $"{idMarkup} [magenta]{labelMarkup}[/]";
        });

        string selected = AnsiConsole.Prompt(prompt);
        return selected == "0" ? null : selected;
    }

    public static string? BacaInputTeks(string prompt, bool wajib = true)
    {
        string? input = AnsiConsole.Prompt(
            new TextPrompt<string?>($"[cyan]{E(prompt)}[/]")
                .AllowEmpty()
                .DefaultValue(null));

        if (wajib && string.IsNullOrWhiteSpace(input))
            return null;

        return input?.Trim();
    }

    public static void MulaiPanelKerja(string judul)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold cyan]{E(judul)}[/]");
        AnsiConsole.MarkupLine("[grey50]" + new string('─', Math.Min(Console.WindowWidth - 1, 60)) + "[/]");
    }

    public static void AnimasiProgressBar(string pesan)
    {
        AnsiConsole.Progress()
            .AutoClear(false)
            .HideCompleted(false)
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new SpinnerColumn())
            .Start(ctx =>
            {
                var task = ctx.AddTask(E(pesan), maxValue: 100);
                while (!task.IsFinished)
                {
                    task.Increment(2);
                    Thread.Sleep(25);
                }
            });
        AnsiConsole.WriteLine();
    }

    public static void AnimasiProgressDownload(string label, double progress, double speedMbps)
    {
        const int barSize = 30;
        int filled = (int)(progress / 100 * barSize);
        string bar = new string('█', filled) + new string('░', barSize - filled);
        Console.Write($"\r  {label,-12} [{bar}] {progress,6:0.00}%  {speedMbps,5:0.00} MB/s  ");
    }

    public static void SelesaiProgressDownload() => AnsiConsole.WriteLine();

    public static void TulisInfo(string pesan) =>
        AnsiConsole.MarkupLine($"[cyan]i[/] {E(pesan)}");

    public static void TulisPeringatan(string pesan) =>
        AnsiConsole.MarkupLine($"[yellow]![/] {E(pesan)}");

    public static void TulisLog(string pesan) =>
        AnsiConsole.MarkupLine($"[grey50]{E(pesan)}[/]");

    public static void TulisOutputProses(string? baris)
    {
        if (!string.IsNullOrEmpty(baris))
            AnsiConsole.MarkupLine($"[grey70]{E(baris)}[/]");
    }

    public static void CetakSukses(string pesan)
    {
        var panel = new Panel(new Markup($"[bold green]OK[/] [green]{E(pesan)}[/]"))
        {
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Green),
            Padding = new Padding(1, 0)
        };
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
        NativeMethods.ShowInfoPopup(pesan, "Berhasil");
    }

    public static void CetakError(string pesan)
    {
        var panel = new Panel(new Markup($"[bold red]X[/] [red]{E(pesan)}[/]"))
        {
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Red),
            Padding = new Padding(1, 0)
        };
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    public static void TampilkanTabelScan(string judul, string labelMinUkuran,
        IReadOnlyList<(string Nama, double UkuranGb)> baris)
    {
        AnsiConsole.Clear();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Magenta1)
            .Title($"[bold magenta]{E(judul)}[/] [grey50]({E(labelMinUkuran)})[/]")
            .AddColumn(new TableColumn("[bold]Nama Folder[/]").LeftAligned())
            .AddColumn(new TableColumn("[bold]Ukuran[/]").RightAligned());

        if (baris.Count == 0)
            table.AddRow("[grey50]Tidak ada folder di atas threshold.[/]", "[grey50]-[/]");
        else
            foreach (var (nama, gb) in baris)
                table.AddRow(E(nama), $"[cyan]{gb:0.00} GB[/]");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public static void TampilkanTabelStartup(IReadOnlyList<(string Status, string Nama, string Lokasi)> baris)
    {
        AnsiConsole.Clear();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title("[bold green]Startup Analyzer[/] [grey50](Registry Run)[/]")
            .AddColumn(new TableColumn("[bold]Status[/]").Centered())
            .AddColumn(new TableColumn("[bold]Aplikasi[/]").LeftAligned())
            .AddColumn(new TableColumn("[bold]Lokasi / Target[/]").LeftAligned());

        foreach (var (status, nama, lokasi) in baris)
        {
            string statusMarkup = status == "Enabled"
                ? "[green]Enabled[/]"
                : "[grey50]Disabled[/]";
            table.AddRow(statusMarkup, E(nama), E(lokasi));
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public static void TungguKembaliKeMenu()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey50]Tekan [bold]ENTER[/] untuk kembali ke menu utama[/]");
        Console.ReadLine();
    }

    public static void TampilkanPesanElevasi() =>
        AnsiConsole.MarkupLine("[yellow]Meminta akses Administrator...[/]");

    private static Layout BuatLayoutUtama(int selectedIndex)
    {
        var header = BuatPanelHeader();
        var overview = BuatPanelOverview(selectedIndex);
        var menu = BuatPanelMenu(selectedIndex);
        var footer = new Markup(BuatFooterMarkup());

        var body = new Layout("Body")
            .SplitColumns(
                new Layout("Overview").Size(HitungLebarOverview()).Update(overview),
                new Layout("Menu").Update(menu));

        int bodyHeight = MenuItems.Length + 2;

        return new Layout("Root")
            .SplitRows(
                new Layout("Header").Size(3).Update(header),
                new Layout("Body").Size(bodyHeight).Update(body),
                new Layout("Footer").Size(1).Update(footer));
    }

    private static Panel BuatPanelHeader() =>
        new(BuatHeaderMarkup())
        {
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Cyan1),
            Padding = new Padding(0, 0)
        };

    private static Panel BuatPanelOverview(int selectedIndex) =>
        new(BuatOverviewMarkup(selectedIndex))
        {
            Header = new PanelHeader("[cyan] Overview [/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Cyan3),
            Padding = new Padding(0, 0)
        };

    private static Panel BuatPanelMenu(int selectedIndex) =>
        new(new Markup(BuatDaftarMenuMarkup(selectedIndex)))
        {
            Header = new PanelHeader("[cyan] Menu [/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Cyan1),
            Padding = new Padding(0, 0)
        };

    private static Markup BuatHeaderMarkup()
    {
        bool isAdmin = ProcessHelper.IsAdministrator();
        string adminBadge = isAdmin
            ? "[bold green on grey23] ADMIN [/]"
            : "[bold red on grey23] NO ADMIN [/]";

        return new Markup(
            $"[bold cyan]Prem Tools[/] [grey50]v{E(AppVersion)}[/]  {adminBadge}  " +
            $"[grey50]{E(Environment.MachineName)}[/]  [grey50]|[/]  " +
            $"[grey50]{E(DateTime.Now.ToString("dd MMM yyyy HH:mm"))}[/]");
    }

    private static Markup BuatOverviewMarkup(int selectedIndex)
    {
        var item = MenuItems[selectedIndex];
        string warna = WarnaAccent(item.Accent);
        string labelStyle = item.Accent == "yellow" ? "bold yellow" : warna;

        return new Markup(
            "[bold]Prem Tools[/]\n" +
            "[grey50]Windows utility — Clean · Scan · Repair · Download[/]\n\n" +
            $"[{labelStyle}]{E(item.ShortLabel)}[/]\n" +
            $"[grey50]{E(item.Description)}[/]\n\n" +
            "[cyan]>[/] [grey50]1-9 / 0: langsung[/]\n" +
            "[cyan]>[/] [grey50]10-14: arrow + Enter[/]");
    }

    private static string BuatDaftarMenuMarkup(int selectedIndex)
    {
        var lines = new string[MenuItems.Length];

        for (int i = 0; i < MenuItems.Length; i++)
        {
            var item = MenuItems[i];
            string id = E(item.Id.PadLeft(2));
            string label = E(item.ShortLabel);
            string warna = WarnaAccent(item.Accent);
            string style = item.Accent == "yellow" ? "bold yellow" : warna;

            lines[i] = i == selectedIndex
                ? $"[black on cyan1] {id}  {label} [/]"
                : $"[grey50]{id}[/] [{style}]{label}[/]";
        }

        return string.Join("\n", lines);
    }

    private static string BuatFooterMarkup() =>
        "[grey50]1-9/0[/] langsung  [grey50]|[/]  [grey50]arrow keys[/] navigasi  [grey50]|[/]  " +
        "[grey50]Enter[/] eksekusi  [grey50]|[/]  [grey50]Esc[/] keluar  [grey50]|[/]  " +
        $"[cyan]{E(Environment.OSVersion.VersionString)}[/]";

    private static int HitungLebarOverview()
    {
        int lebar = Console.WindowWidth;
        if (lebar < 80) return 32;
        if (lebar < 100) return 38;
        return 42;
    }

    private static bool CobaAmbilMenuLangsung(ConsoleKeyInfo key, out string? menuId)
    {
        menuId = null;

        int? digit = TombolKeAngka(key.Key);
        if (digit is null)
            return false;

        if (digit == 0)
            return true;

        menuId = digit.Value.ToString();
        return true;
    }

    private static int? TombolKeAngka(ConsoleKey key) => key switch
    {
        ConsoleKey.D0 or ConsoleKey.NumPad0 => 0,
        ConsoleKey.D1 or ConsoleKey.NumPad1 => 1,
        ConsoleKey.D2 or ConsoleKey.NumPad2 => 2,
        ConsoleKey.D3 or ConsoleKey.NumPad3 => 3,
        ConsoleKey.D4 or ConsoleKey.NumPad4 => 4,
        ConsoleKey.D5 or ConsoleKey.NumPad5 => 5,
        ConsoleKey.D6 or ConsoleKey.NumPad6 => 6,
        ConsoleKey.D7 or ConsoleKey.NumPad7 => 7,
        ConsoleKey.D8 or ConsoleKey.NumPad8 => 8,
        ConsoleKey.D9 or ConsoleKey.NumPad9 => 9,
        _ => null
    };

    private static string FormatNomorMenu(string id) =>
        $"[grey50]{E(id.PadLeft(2))}[/]";

    private static string WarnaAccent(string accent) => accent switch
    {
        "green" => "green",
        "red" => "red",
        "magenta" => "magenta",
        "yellow" => "yellow",
        "grey50" => "grey50",
        _ => "cyan"
    };

    private static string E(string? text) => Markup.Escape(text ?? string.Empty);
}