using Interfaces;

namespace Utils {
    public class DiskStorage : IStorage {
        private readonly ICrypt _crypt;
        public DiskStorage (ICrypt crypt) {
            _crypt = crypt;
        }
        public string upload(IFormFile file) {
            string path = Environment.CurrentDirectory + "\\Tmp\\";

            string newFileName = _crypt.encrypt(file.FileName) + "_" + file.FileName;

            while (newFileName.Contains("/")) {
                newFileName = _crypt.encrypt(file.FileName) + "_" + file.FileName;
            }

            if (! Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            using var stream = File.Create(path + newFileName);
            file.CopyToAsync(stream);

            return newFileName;
        }
        public bool delete(string path) {
            return true;
        }
    }
}