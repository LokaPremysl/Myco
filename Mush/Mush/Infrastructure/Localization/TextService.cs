using System.Collections.Generic;
using Mush.AppLayer.Ports;

namespace Mush.Infrastructure.Localization
{
    public sealed class TextService : ITextService
    {
        private readonly string _currentLanguage;
        private readonly Dictionary<string, string> _en;
        private readonly Dictionary<string, string> _cs;
        private string _lang;
        public string CurrentLanguage => _lang;
        public event EventHandler? LanguageChanged;
        //public TextService(string initialLang = "cs") => _lang = initialLang;
        
        
        public TextService(string initialLang = "cs")
        {
            //_currentLanguage = currentLanguage;
            _lang = initialLang;
            // 1) Základní EN slovník
            _en = new Dictionary<string, string>
            {
                // Login
                ["LoginForm.Username"] = "Username",
                ["LoginForm.Password"] = "Password",
                ["LoginForm.LoginButton"] = "Sign in",
                ["LoginForm.InvalidCredentials"] = "Invalid username or password.",

                // Main form tabs / sections / buttons
                ["MainForm.Tab.Culture"] = "Culture / Mycelium",
                ["MainForm.Tab.Spawn"] = "Spawn (grain jars)",
                ["MainForm.Tab.Bulk"] = "Bulk / Fruiting",
                ["MainForm.Tab.Harvest"] = "Harvest / Flushes",

                ["MainForm.AddCultureButton"] = "Add culture",
                ["MainForm.AddSpawnButton"] = "Add spawn",
                ["MainForm.AddBulkButton"] = "Add bulk",
                ["MainForm.AddHarvestButton"] = "Add harvest",

                ["MainForm.DeleteButton"] = "Remove",
                ["MainForm.DetailsHeader"] = "Details / lineage / yield",
                ["MainForm.ColumnsButton"] = "Columns…",
                ["MainForm.ColumnsDialogTitle"] = "Select visible columns",

                // Dialog labels
                ["SpawnDialog.Title"] = "New spawn",
                ["SpawnDialog.DateInoc"] = "Inoculated at:",
                ["SpawnDialog.GrainType"] = "Grain:",
                ["SpawnDialog.InoculumInfo"] = "Inoculum used:",
                ["SpawnDialog.Ok"] = "OK",
                ["SpawnDialog.Cancel"] = "Cancel",
                
            ["Main.Col.Mycelium"] = "Mycelium",
            ["Main.Col.Origin"] = "Origin",
            ["Main.Col.Medium"] = "Medium",
            ["Main.Col.Status"] = "Status",
            ["Main.Col.Date"] = "Date",
            ["Main.Col.Notes"] = "Notes",

            // Spawn
            ["Spawn.Col.Material"] = "Spawn",
            ["Spawn.Col.Inoculum"] = "Inoculum",
            ["Spawn.Col.Jars"] = "Jars/Bags",
            ["Spawn.Col.Temperature"] = "Incubation temp",
            ["Spawn.Col.Colonization"] = "Colonization (%)",
            ["Spawn.Col.Date"] = "Inoculated at",
            ["Spawn.Col.Notes"] = "Notes",

            // Bulk
            ["Bulk.Col.Material"] = "Bulk (substrate)",
            ["Bulk.Col.Ratio"] = "Spawn : substrate",
            ["Bulk.Col.SpawnAmount"] = "Spawn amount",
            ["Bulk.Col.Hydration"] = "Hydration",
            ["Bulk.Col.FruitingStart"] = "Fruiting start",
            ["Bulk.Col.FlushCount"] = "Flushes",
            ["Bulk.Col.Date"] = "Prepared at",
            ["Bulk.Col.Notes"] = "Notes",
                
                ["MainForm.DeleteButton"] = "Delete selected",
                ["Dialog.Confirm"] = "Confirm",
                ["Confirm.DeleteBulk"] = "Delete selected bulk?",
                ["Confirm.DeleteSpawn"] = "Delete spawn including {0} bulks?",
                ["Confirm.DeleteMycelium"] = "Delete mycelium including {0} spawns and {1} bulks?",
            };

            _cs = new Dictionary<string, string>
            {
                // Login
                ["LoginForm.Username"] = "Uživatel",
                ["LoginForm.Password"] = "Heslo",
                ["LoginForm.LoginButton"] = "Přihlásit",
                ["LoginForm.InvalidCredentials"] = "Neplatné jméno nebo heslo.",

                // Main form tabs / sections / buttons
                ["MainForm.Tab.Culture"] = "Mycelium / kultura",
                ["MainForm.Tab.Spawn"] = "Spawn (zrno)",
                ["MainForm.Tab.Bulk"] = "Bulk / plodící boxy",
                ["MainForm.Tab.Harvest"] = "Sklizně / flush",
                ["MainForm.ColumnsButton"] = "Sloupce…",
                ["MainForm.ColumnsDialogTitle"] = "Výběr sloupců",

                ["MainForm.AddCultureButton"] = "Přidat kulturu",
                ["MainForm.AddSpawnButton"] = "Přidat spawn",
                ["MainForm.AddBulkButton"] = "Přidat bulk",
                ["MainForm.AddHarvestButton"] = "Zapsat sklizeň",

                ["MainForm.DeleteButton"] = "Smazat",
                ["MainForm.DetailsHeader"] = "Detail / linie / výnos",

                // Dialog labels
                ["SpawnDialog.Title"] = "Nový spawn",
                ["SpawnDialog.DateInoc"] = "Inokulováno:",
                ["SpawnDialog.GrainType"] = "Zrno:",
                ["SpawnDialog.InoculumInfo"] = "Použité inokulum:",
                ["SpawnDialog.Ok"] = "OK",
                ["SpawnDialog.Cancel"] = "Zrušit",

                // Mycelium
            ["Main.Col.Mycelium"] = "Mycelium",
            ["Main.Col.Origin"] = "Původ",
            ["Main.Col.Medium"] = "Médium",
            ["Main.Col.Status"] = "Stav",
            ["Main.Col.Date"] = "Datum",
            ["Main.Col.Notes"] = "Poznámky",

            // Spawn
            ["Spawn.Col.Material"] = "Spawn",
            ["Spawn.Col.Inoculum"] = "Inokulum",
            ["Spawn.Col.Jars"] = "Sklenice/Pytle",
            ["Spawn.Col.Temperature"] = "Teplota inkubace",
            ["Spawn.Col.Colonization"] = "Kolonizace (%)",
            ["Spawn.Col.Date"] = "Inokulováno",
            ["Spawn.Col.Notes"] = "Poznámky",

            // Bulk
            ["Bulk.Col.Material"] = "Bulk (substrát)",
            ["Bulk.Col.Ratio"] = "Poměr spawn:substrát",
            ["Bulk.Col.SpawnAmount"] = "Množství spawn",
            ["Bulk.Col.Hydration"] = "Zavlhčení",
            ["Bulk.Col.FruitingStart"] = "Začátek plodění",
            ["Bulk.Col.FlushCount"] = "Počet flushů",
            ["Bulk.Col.Date"] = "Připraveno",
            ["Bulk.Col.Notes"] = "Poznámky",
                ["MainForm.DeleteButton"] = "Smazat vybrané",
                ["Dialog.Confirm"] = "Potvrdit",
                ["Confirm.DeleteBulk"] = "Smazat vybraný bulk?",
                ["Confirm.DeleteSpawn"] = "Smazat spawn včetně {0} bulků?",
                ["Confirm.DeleteMycelium"] = "Smazat mycelium včetně {0} spawnů a {1} bulků?",
            };
        }

        //public string T(string key)
        //{
        //    // 1. zkus aktivní jazyk
        //    if (_currentLanguage.StartsWith("cs"))
        //    {
        //        if (_cs.TryGetValue(key, out var cz)) return cz;
        //    }
        //    // budoucí jazyky: if (_currentLanguage.StartsWith("de")) ...

        //    // 2. fallback EN
        //    if (_en.TryGetValue(key, out var en)) return en;

        //    // 3. fallback klíč samotný (debug)
        //    return key;
        //}

        public string T(string key)
        {
            var d = _lang == "cs" ? _cs : _en;
            if (d.TryGetValue(key, out var v)) return v;
            // fallback
            if (_en.TryGetValue(key, out var en)) return en;
            return key;
        }

        public void SetLanguage(string lang)
        {
            if (string.Equals(_lang, lang, StringComparison.OrdinalIgnoreCase)) return;
            _lang = lang;
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
