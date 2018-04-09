namespace PixelPubApi.Interfaces {
    public interface IRestClientFactory {
        IRestRepository get(string name);
    }
}
