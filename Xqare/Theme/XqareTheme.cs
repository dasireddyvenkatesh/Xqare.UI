using MudBlazor;

namespace Xqare.Theme
{
    public static class XqareTheme
    {
        public static MudTheme Create() => new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#F5A623",
                Secondary = "#1A2540",
                Background = "#F5F7FB",
                Surface = "#FFFFFF",
                AppbarBackground = "#1A2540",
                AppbarText = "#FFFFFF",
                DrawerBackground = "#1A2540",
                DrawerText = "#E7EDF5",
                TextPrimary = "#1F2937",
                TextSecondary = "#64748B",
                Divider = "#E5EAF0",
                Success = "#16A34A",
                Warning = "#F5A623",
                Error = "#DC2626"
            }
        };
    }
}
