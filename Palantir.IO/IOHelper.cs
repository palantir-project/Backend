namespace Palantir.IO
{
    using System.IO;
    using System.Threading.Tasks;

    public static class IOHelper
    {
        public static async Task WriteFile(string path, string data)
        {
            using (FileStream fs = new FileStream(path, FileMode.Truncate, FileAccess.Write))
            using (StreamWriter textWriter = new StreamWriter(fs))
            {
                await textWriter.WriteAsync(data);
            }
        }

        public static async Task<string> ReadFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return await sr.ReadToEndAsync();
                }
            }
            catch (FileNotFoundException exception)
            {
                return exception.Message;
            }
        }
    }
}