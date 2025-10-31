using System.Collections.Generic;

namespace MushApp.src.Infrastructure.Localization
{
    public sealed class TextService : ITextService
    {
        private readonly string _currentLanguage;
        private readonly Dictionary<string, string> _en;
        private readonly Dictionary<string, string> _cs;

        public TextService(string currentLanguage = "en")
        {
            _currentLanguage = currentLanguage;

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

                // Dialog labels
                ["SpawnDialog.Title"] = "New spawn",
                ["SpawnDialog.DateInoc"] = "Inoculated at:",
                ["SpawnDialog.GrainType"] = "Grain:",
                ["SpawnDialog.InoculumInfo"] = "Inoculum used:",
                ["SpawnDialog.Ok"] = "OK",
                ["SpawnDialog.Cancel"] = "Cancel",
            };

            // 2) Český slovník (zatím jen pár, zbytek doplníme kdykoli)
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
            };
        }

        public string T(string key)
        {
            // 1. zkus aktivní jazyk
            if (_currentLanguage.StartsWith("cs"))
            {
                if (_cs.TryGetValue(key, out var cz)) return cz;
            }
            // budoucí jazyky: if (_currentLanguage.StartsWith("de")) ...

            // 2. fallback EN
            if (_en.TryGetValue(key, out var en)) return en;

            // 3. fallback klíč samotný (debug)
            return key;
        }
    }
}
