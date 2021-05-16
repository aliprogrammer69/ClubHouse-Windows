using ClubHouse.Common;
using ClubHouse.Common.Configurations;
using ClubHouse.Domain.Services;
using ClubHouse.Domain.Services.Common;

using Microsoft.Win32;

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ClubHouse.Business.Services {
    public class AccountService : IAccountService {
        private readonly ISerializer _serializer;

        public AccountService(ISerializer serializer) {
            _serializer = serializer;
            CurrentConfig = Get();
        }

        public AuthConfiguration CurrentConfig { get; protected set; }

        public bool Authenticated => !string.IsNullOrEmpty(CurrentConfig?.UserToken);

        public void Set(AuthConfiguration config) {
            if (config == null) {
                throw new System.NullReferenceException("Config must have value");
            }
            CurrentConfig = config;

            RegistryKey authKey = Registry.CurrentUser.CreateSubKey(Consts.AuthConfigRegKey, true);
            authKey.SetValue("auth", Encrypt(config), RegistryValueKind.Binary);
            authKey.Close();
        }

        public AuthConfiguration Reset() {
            CurrentConfig = new AuthConfiguration();
            Set(CurrentConfig);
            return CurrentConfig;
        }

        protected virtual AuthConfiguration Get() {
            RegistryKey authKey = Registry.CurrentUser.CreateSubKey(Consts.AuthConfigRegKey, true);
            byte[] buffer = authKey.GetValue("auth", null) as byte[];
            if (buffer == null)
                return new AuthConfiguration();
            return Decrypt(buffer);
        }

        #region Private Methods
        private byte[] Encrypt(AuthConfiguration configuration) {
            string json = _serializer.Serialize(configuration);
            byte[] result;
            using (AesManaged aes = GenerateCsp()) {
                using (ICryptoTransform encryptor = aes.CreateEncryptor()) {
                    using (MemoryStream memory = new MemoryStream()) {
                        using (CryptoStream cryptoStream = new CryptoStream(memory, encryptor, CryptoStreamMode.Write)) {
                            using (StreamWriter writer = new StreamWriter(cryptoStream)) {
                                writer.Write(json);
                            }
                            result = memory.ToArray();
                        }
                    }
                }
            }
            return result;
        }

        private AuthConfiguration Decrypt(byte[] encryptedData) {
            AuthConfiguration result;
            using (AesManaged aes = GenerateCsp()) {
                using (ICryptoTransform decryptor = aes.CreateDecryptor()) {
                    using (MemoryStream memory = new MemoryStream(encryptedData)) {
                        using (CryptoStream cryptoStream = new CryptoStream(memory, decryptor, CryptoStreamMode.Read)) {
                            using (StreamReader reader = new StreamReader(cryptoStream)) {
                                string json = reader.ReadToEnd();
                                result = _serializer.Deserialize<AuthConfiguration>(json);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private AesManaged GenerateCsp() {
            AesManaged aes = new AesManaged();
            Rfc2898DeriveBytes spec = new Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(Consts.AuthPassword),
                                              Encoding.ASCII.GetBytes(Consts.AuthSalt),
                                              1000);
            aes.Key = spec.GetBytes(16);
            aes.IV = new byte[16];
            return aes;
        }
        #endregion
    }
}
