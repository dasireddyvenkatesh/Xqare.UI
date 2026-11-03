using MudBlazor;

namespace Xqare.Theme
{
    public static class XqareTheme
    {
        // Keep these values aligned with the CSS custom properties in wwwroot/css/app.css.
        public const string Primary = "#F5A623";
        public const string PrimaryHover = "#E99A12";
        public const string Navy = "#1A2540";
        public const string NavyDeep = "#0F2A44";
        public const string NavyAlt = "#243B5B";
        public const string PageBackground = "#F5F7FB";
        public const string Surface = "#FFFFFF";
        public const string Border = "#E5EAF0";
        public const string TextPrimary = "#1F2937";
        public const string TextSecondary = "#64748B";
        public const string Success = "#16A34A";
        public const string Warning = "#F5A623";
        public const string Error = "#DC2626";
        public const string Info = "#2563EB";

        public static MudTheme Create() => new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = Primary,
                Secondary = Navy,
                Background = PageBackground,
                Surface = Surface,
                AppbarBackground = Navy,
                AppbarText = "#FFFFFF",
                DrawerBackground = Navy,
                DrawerText = "#E7EDF5",
                TextPrimary = TextPrimary,
                TextSecondary = TextSecondary,
                Divider = Border,
                Success = Success,
                Warning = Warning,
                Error = Error,
                Info = Info
            }
        };
    }
}
