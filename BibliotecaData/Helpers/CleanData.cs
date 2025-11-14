using System.Text.RegularExpressions;

namespace BibliotecaData.Helpers
{
    public static class CleanData
    {
        public static string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9 ]", "");
        }
    }
}
