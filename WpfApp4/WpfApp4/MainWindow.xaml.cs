using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private List<User> users = new List<User>
        {
            new User { Login = "admin", Password = "admin123", Name = "Администратор", Role = "Admin" },
            new User { Login = "user", Password = "user123", Name = "Обычный пользователь", Role = "User" },
            new User { Login = "guest", Password = "guest123", Name = "Гость", Role = "Guest" }
        };

        public MainWindow()
        {
            InitializeComponent(); /
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            User authenticatedUser = users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (authenticatedUser != null)
            {
                ErrorLabel.Text = "";
                switch (authenticatedUser.Role)
                {
                    case "Admin":
                        new AdminWindow(authenticatedUser).Show();
                        break;
                    case "User":
                        new UserWindow(authenticatedUser).Show();
                        break;
                    case "Guest":
                        new UserWindow(authenticatedUser).Show();
                        break;
                }
                this.Close();
            }
            else
            {
                ErrorLabel.Text = "Неверный логин или пароль";
            }
        }

        // --- Внутренние классы ---

        public class User
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string Role { get; set; }
        }

        public class AdminWindow : Window
        {
            private User currentUser;

            public AdminWindow(User user)
            {
                currentUser = user;
                InitializeUI();
            }

            private void InitializeUI()
            {
                Title = "Окно администратора";
                Height = 300;
                Width = 400;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                var panel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var label = new Label { Content = "Добро пожаловать, администратор!", FontSize = 18 };
                var profileBtn = new Button { Content = "Личный кабинет", Margin = new Thickness(0, 10, 0, 0), Padding = new Thickness(10, 5, 10, 5) };
                var logoutBtn = new Button { Content = "Выйти из аккаунта", Margin = new Thickness(0, 10, 0, 0), Padding = new Thickness(10, 5, 10, 5) };

                profileBtn.Click += (s, e) => new UserProfileWindow(currentUser).Show();
                logoutBtn.Click += (s, e) =>
                {
                    new MainWindow().Show();
                    this.Close();
                };

                panel.Children.Add(label);
                panel.Children.Add(profileBtn);
                panel.Children.Add(logoutBtn);

                Content = panel;
            }
        }

        public class UserWindow : Window
        {
            private User currentUser;

            public UserWindow(User user)
            {
                currentUser = user;
                InitializeUI();
            }

            private void InitializeUI()
            {
                Title = "Окно пользователя";
                Height = 300;
                Width = 400;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                var panel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                var label = new Label { Content = "Добро пожаловать, пользователь!", FontSize = 18 };
                var profileBtn = new Button { Content = "Личный кабинет", Margin = new Thickness(0, 10, 0, 0), Padding = new Thickness(10, 5, 10, 5) };
                var logoutBtn = new Button { Content = "Выйти из аккаунта", Margin = new Thickness(0, 10, 0, 0), Padding = new Thickness(10, 5, 10, 5) };

                profileBtn.Click += (s, e) => new UserProfileWindow(currentUser).Show();
                logoutBtn.Click += (s, e) =>
                {
                    new MainWindow().Show();
                    this.Close();
                };

                panel.Children.Add(label);
                panel.Children.Add(profileBtn);
                panel.Children.Add(logoutBtn);

                Content = panel;
            }
        }

        public class UserProfileWindow : Window
        {
            private User currentUser;

            public UserProfileWindow(User user)
            {
                currentUser = user;
                InitializeUI();
            }

            private void InitializeUI()
            {
                Title = "Личный кабинет";
                Height = 300;
                Width = 400;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                var margin = new Thickness(20);

                var panel = new StackPanel { Margin = margin };

                var welcomeLabel = new Label { Content = $"Добро пожаловать, {currentUser.Name}", FontSize = 20, HorizontalAlignment = HorizontalAlignment.Center };
                var loginLabel = new Label { Content = $"Логин: {currentUser.Login}", FontSize = 14 };
                var passwordLabel = new Label { Content = $"Пароль: {currentUser.Password}", FontSize = 14 };
                var roleLabel = new Label { Content = $"Роль: {currentUser.Role}", FontSize = 14 };

                var logoutBtn = new Button { Content = "Выйти из аккаунта", Margin = new Thickness(0, 20, 0, 0), Padding = new Thickness(10, 5, 10, 5) };
                logoutBtn.Click += (s, e) =>
                {
                    new MainWindow().Show();
                    this.Close();
                };

                panel.Children.Add(welcomeLabel);
                panel.Children.Add(loginLabel);
                panel.Children.Add(passwordLabel);
                panel.Children.Add(roleLabel);
                panel.Children.Add(logoutBtn);

                Content = panel;
            }
        }
    }
}