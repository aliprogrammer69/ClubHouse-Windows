using System;
using System.Linq;

namespace ClubHouse.Common {
    public static class Utils {
        public static Random random = new Random();
        public static string GetRandomHexNumber(int digits) {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        public static void OpenExternalUrl(string url) {
            System.Diagnostics.ProcessStartInfo process = new System.Diagnostics.ProcessStartInfo(url) {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(process);
        }
    }
}
