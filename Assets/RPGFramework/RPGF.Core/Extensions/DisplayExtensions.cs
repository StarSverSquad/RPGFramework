using UnityEngine;

namespace RPGF.Core.Extensions
{
    public static class DisplayExtensions
    {
        public static bool IsSupportedResolution(this Display display, Resolution resolution)
        {
            if (resolution.width < 1280 || resolution.height < 720)
                return false;

            if (resolution.width > display.systemWidth || resolution.height > display.systemHeight)
                return false;

            var is16by9 = resolution.width * 9 == resolution.height * 16;
            var is16by10 = resolution.width * 10 == resolution.height * 16;
            return is16by9 || is16by10;
        }
    }
}
