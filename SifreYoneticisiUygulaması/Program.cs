using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class PasswordManager
{
    private static readonly string filePath = "passwords.txt";
    private static readonly string encryptionKey = "your-encryption-key"; // 16 karakterlik bir anahtar kullanın

    static void Main()
    {

        while (true)
        {   




            Console.Clear();
            Console.WriteLine("Şifre Yöneticisi Uygulaması");
            Console.WriteLine("1. Şifre Ekle");
            Console.WriteLine("2. Şifreleri Listele");
            Console.WriteLine("3. Şifreyi Sil");
            Console.WriteLine("4. Çıkış");
            Console.Write("Seçiminizi yapın: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddPassword();
                    break;
                case "2":
                    ListPasswords();
                    break;
                case "3":
                    RemovePassword();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                    break;
            }
        }
    }

    static void AddPassword()
    {
        Console.Write("Servis Adı: ");
        string service = Console.ReadLine();

        Console.Write("Kullanıcı Adı: ");
        string username = Console.ReadLine();

        Console.Write("Şifre: ");
        string password = Console.ReadLine();

        string encryptedPassword = Encrypt(password, encryptionKey);
        File.AppendAllText(filePath, $"{service},{username},{encryptedPassword}{Environment.NewLine}");

        Console.WriteLine("Şifre eklendi! Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void ListPasswords()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            Console.WriteLine("Kaydedilmiş Şifreler:");
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                string decryptedPassword = Decrypt(parts[2], encryptionKey);
                Console.WriteLine($"Servis: {parts[0]}, Kullanıcı Adı: {parts[1]}, Şifre: {decryptedPassword}");
            }
        }
        else
        {
            Console.WriteLine("Henüz kaydedilmiş şifre yok.");
        }

        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void RemovePassword()
    {
        ListPasswords();
        Console.Write("Silmek istediğiniz şifrenin servis adını girin: ");
        string serviceToRemove = Console.ReadLine();

        if (File.Exists(filePath))
        {
            var lines = new List<string>(File.ReadAllLines(filePath));
            lines.RemoveAll(line => line.StartsWith(serviceToRemove + ","));
            File.WriteAllLines(filePath, lines);
            Console.WriteLine("Şifre silindi! Devam etmek için bir tuşa basın...");
        }
        else
        {
            Console.WriteLine("Dosya bulunamadı.");
        }

        Console.ReadKey();
    }

    static string Encrypt(string plainText, string key)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }


        return Convert.ToBase64String(array);


    }

    static string Decrypt(string cipherText, string key)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
