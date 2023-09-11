namespace ContributorsFacesGenerator.Extensions
{
    public static class FilePathExtension
    {
        public static void CheckFilePath(this string filePath)
        {
            var folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
            {
                // Try to create the folder
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception e)
                {
                    throw new Exception("filePath error", e);
                }
            }
        }
    }
}
