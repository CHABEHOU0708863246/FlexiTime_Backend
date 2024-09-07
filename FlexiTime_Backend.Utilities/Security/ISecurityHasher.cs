namespace FlexiTime_Backend.Utilities.Security
{
    public interface ISecurityHasher
    {
        /// <summary>Hash a string</summary>
        /// <param name="rawData">String to hash</param>
        /// <returns>Hashed string</returns>
        string ComputeHash(string rawData);
    }
}
