namespace LocusIDBackend.Services.Interfaces
{
    public interface IDecodeTokenService
    {
        public string GetIdFromRawToken(string token);
    }
}