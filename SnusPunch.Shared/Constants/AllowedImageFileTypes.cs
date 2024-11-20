namespace SnusPunch.Shared.Constants
{
    public class AllowedImageFileTypes
    {
        public static int ImageMaximumBytes = 2097152;
        public static int ImageMinimumBytes = 512;
        public static string[] AllowedMimeTypes = ["image/jpg", "image/jpeg", "image/png"];
        public static string[] AllowedExtensions = [".jpg", ".png", ".jpeg"];
    }
}
