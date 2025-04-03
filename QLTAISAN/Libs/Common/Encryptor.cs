namespace Libs.Common
{
    public static class Encryptor
    {
        public static string MD5Hash(string input)
        {
            // Khởi tạo MD5
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển chuỗi thành mảng byte
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);

                // Tính toán giá trị băm MD5
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển giá trị băm thành chuỗi hex
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));  // x2 sẽ định dạng mỗi byte thành 2 ký tự hex
                }

                return sb.ToString();
            }
        }
    }
}
