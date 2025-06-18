namespace SozTopar.Services;

public interface IEmojiCodeService
{
    string EncodeWord(string word);
    Dictionary<char, string> GetCurrentCodeMap();
    void GenerateNewCodeMap();
}