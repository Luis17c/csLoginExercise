namespace Interfaces {
    public interface IStorage {
        public string upload(IFormFile file);
        public bool delete(string path);
    }
}