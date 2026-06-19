using SM_TPS_.Commands;
using SM_TPS_.Data;
using SM_TPS_.Models;
using SM_TPS_.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SM_TPS_.ViewModels
{
    public class UserManagementViewModel : ViewModelBase
    {
        private readonly UserRepository _repository;

        public ObservableCollection<User> Users { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public ICommand AddUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }

        public UserManagementViewModel()
        {
            _repository = new UserRepository();

            Users = new ObservableCollection<User>(
                _repository.GetAllUsers());

            AddUserCommand =
                new RelayCommand(AddUser);
            DeleteUserCommand =
                new RelayCommand(DeleteUser);
            ResetPasswordCommand =
                new RelayCommand(ResetPassword);
        }

        private void AddUser(object obj)
        {
            User user = new User
            {
                Username = Username,
                Password = Password,
                Role = Role
            };

            _repository.AddUser(user);

            Users.Add(user);
        }
        private User _selectedUser;

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
        private void DeleteUser(object obj)
        {
            if (SelectedUser == null)
                return;

            _repository.DeleteUser(SelectedUser.Id);

            Users.Remove(SelectedUser);
        }
        private void ResetPassword(object obj)
        {
            if (SelectedUser == null)
                return;

            ChangePasswordWindow window =
                new ChangePasswordWindow();

            if (window.ShowDialog() == true)
            {
                _repository.ChangePassword(
                    SelectedUser.Id,
                    window.NewPassword);

                MessageBox.Show(
                    "Password Updated Successfully");

                Users.Clear();

                foreach (var user in _repository.GetAllUsers())
                {
                    Users.Add(user);
                }
            }
        }
    }
}