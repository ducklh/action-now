using ActionNow.Commands;
using ActionNow.Data;
using ActionNow.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace ActionNow.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ApplicationDbContext _context;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        private string _name;
        private string _email;
        private int _age;

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                {
                    Name = value?.Name;
                    Email = value?.Email;
                    Age = value?.Age ?? 0;
                }
            }
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            _context = new ApplicationDbContext();
            _context.Database.EnsureCreated();
            
            LoadUsers();

            AddCommand = new RelayCommand(_ => Add(), _ => CanAdd());
            UpdateCommand = new RelayCommand(_ => Update(), _ => CanUpdate());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => CanDelete());
        }

        private void LoadUsers()
        {
            Users = new ObservableCollection<User>(_context.Users.ToList());
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(Name) && 
                   !string.IsNullOrWhiteSpace(Email) && 
                   Age > 0;
        }

        private void Add()
        {
            var user = new User
            {
                Name = Name,
                Email = Email,
                Age = Age
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            LoadUsers();
            ClearFields();
        }

        private bool CanUpdate()
        {
            return SelectedUser != null && 
                   !string.IsNullOrWhiteSpace(Name) && 
                   !string.IsNullOrWhiteSpace(Email) && 
                   Age > 0;
        }

        private void Update()
        {
            if (SelectedUser != null)
            {
                SelectedUser.Name = Name;
                SelectedUser.Email = Email;
                SelectedUser.Age = Age;

                _context.Users.Update(SelectedUser);
                _context.SaveChanges();
                LoadUsers();
                ClearFields();
            }
        }

        private bool CanDelete()
        {
            return SelectedUser != null;
        }

        private void Delete()
        {
            if (SelectedUser != null)
            {
                _context.Users.Remove(SelectedUser);
                _context.SaveChanges();
                LoadUsers();
                ClearFields();
            }
        }

        private void ClearFields()
        {
            Name = string.Empty;
            Email = string.Empty;
            Age = 0;
            SelectedUser = null;
        }
    }
} 