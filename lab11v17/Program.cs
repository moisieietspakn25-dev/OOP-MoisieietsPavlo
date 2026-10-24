
using System;
using System.Collections.Generic;

namespace Lab11v17
{
    // Абстрактний базовий клас
    abstract class AuthenticationService
    {
        public string UserName { get; set; }
        public bool IsAuthenticated { get; protected set; }
        public string Status { get; protected set; }
        public string Token { get; protected set; }

        protected AuthenticationService(string userName)
        {
            UserName = userName;
            IsAuthenticated = false;
            Status = "Не виконано";
            Token = string.Empty;
        }

        // Шаблонний метод
        public void Authenticate()
        {
            Console.WriteLine($"\n=== {GetType().Name} ===");
            Console.WriteLine($"Користувач: {UserName}");

            IsAuthenticated = false;
            Token = string.Empty;

            try
            {
                ValidateInput();
                CheckCredentials();
                GenerateToken();
                LogAccess();

                IsAuthenticated = true;
                Status = "Успішна автентифікація";

                Console.WriteLine($"Статус: {Status}");
                Console.WriteLine($"Токен: {Token}");
            }
            catch (Exception ex)
            {
                Status = "Помилка автентифікації";
                Console.WriteLine($"Статус: {Status}");
                Console.WriteLine($"Причина: {ex.Message}");

                LogAccess();
            }
        }

        // Обов'язкові кроки
        protected abstract void ValidateInput();
        protected abstract void CheckCredentials();
        protected abstract void GenerateToken();

        // Віртуальний крок із базовою реалізацією
        protected virtual void LogAccess()
        {
            Console.WriteLine(
                $"[{DateTime.Now:HH:mm:ss}] " +
                $"Запис журналу для користувача {UserName}."
            );
        }
    }

    // Автентифікація за паролем
    class PasswordAuth : AuthenticationService
    {
        public string Password { get; set; }

        public PasswordAuth(string userName, string password)
            : base(userName)
        {
            Password = password;
        }

        protected override void ValidateInput()
        {
            Console.WriteLine("1. Перевірка введених даних...");

            if (string.IsNullOrWhiteSpace(UserName) ||
                string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentException(
                    "Ім'я користувача та пароль обов'язкові."
                );
            }
        }

        protected override void CheckCredentials()
        {
            Console.WriteLine("2. Перевірка пароля...");

            // Демонстраційна перевірка.
            // У реальній системі пароль перевіряють за хешем у БД.
            if (Password != "Password123!")
            {
                throw new UnauthorizedAccessException(
                    "Неправильний пароль."
                );
            }
        }

        protected override void GenerateToken()
        {
            Console.WriteLine("3. Створення токена...");
            Token = Guid.NewGuid().ToString();
        }

        protected override void LogAccess()
        {
            Console.WriteLine(
                $"4. PasswordAuth: спроба входу користувача {UserName}."
            );
        }
    }

    // Автентифікація через OAuth2
    class OAuth2Auth : AuthenticationService
    {
        public string Provider { get; set; }
        public string AccessToken { get; set; }

        public OAuth2Auth(
            string userName,
            string provider,
            string accessToken)
            : base(userName)
        {
            Provider = provider;
            AccessToken = accessToken;
        }

        protected override void ValidateInput()
        {
            Console.WriteLine("1. Перевірка параметрів OAuth2...");

            if (string.IsNullOrWhiteSpace(UserName) ||
                string.IsNullOrWhiteSpace(Provider) ||
                string.IsNullOrWhiteSpace(AccessToken))
            {
                throw new ArgumentException(
                    "Необхідно вказати користувача, провайдера і токен."
                );
            }
        }

        protected override void CheckCredentials()
        {
            Console.WriteLine($"2. Перевірка токена провайдера {Provider}...");

            // Імітація перевірки зовнішнього токена.
            if (AccessToken != "valid-oauth-token")
            {
                throw new UnauthorizedAccessException(
                    "Недійсний OAuth2-токен."
                );
            }
        }

        protected override void GenerateToken()
        {
            Console.WriteLine("3. Створення внутрішнього токена...");
            Token = Guid.NewGuid().ToString();
        }

        protected override void LogAccess()
        {
            Console.WriteLine(
                $"4. OAuth2Auth: провайдер {Provider}, " +
                $"користувач {UserName}."
            );
        }
    }

    // Біометрична автентифікація
    class BiometricAuth : AuthenticationService
    {
        public bool BiometricMatch { get; set; }

        public BiometricAuth(string userName, bool biometricMatch)
            : base(userName)
        {
            BiometricMatch = biometricMatch;
        }

        protected override void ValidateInput()
        {
            Console.WriteLine("1. Перевірка біометричних даних...");

            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new ArgumentException(
                    "Ім'я користувача не може бути порожнім."
                );
            }
        }

        protected override void CheckCredentials()
        {
            Console.WriteLine("2. Порівняння біометричного зразка...");

            if (!BiometricMatch)
            {
                throw new UnauthorizedAccessException(
                    "Біометричні дані не збігаються."
                );
            }
        }

        protected override void GenerateToken()
        {
            Console.WriteLine("3. Створення токена...");
            Token = Guid.NewGuid().ToString();
        }

        protected override void LogAccess()
        {
            Console.WriteLine(
                $"4. BiometricAuth: біометрична перевірка " +
                $"користувача {UserName}."
            );
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;

            Console.WriteLine(
                "Лабораторна робота №11, варіант 17"
            );
            Console.WriteLine(
                "Тема: Шаблонний метод. Система автентифікації"
            );

            List<AuthenticationService> services = new()
            {
                new PasswordAuth("pavlo", "Password123!"),
                new OAuth2Auth(
                    "student",
                    "Google",
                    "valid-oauth-token"
                ),
                new BiometricAuth("admin", true),

                // Приклад невдалої автентифікації
                new PasswordAuth("guest", "wrong-password")
            };

            foreach (AuthenticationService service in services)
            {
                service.Authenticate();
            }

            Console.WriteLine("\n=== Підсумок ===");

            foreach (AuthenticationService service in services)
            {
                Console.WriteLine(
                    $"{service.GetType().Name}: " +
                    $"{service.UserName} — {service.Status}"
                );
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}
