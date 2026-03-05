using MiniAddressBook.Data;
using MiniAddressBook.Models;
using MiniAddressBook.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;

namespace MiniAddressBook.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;

        public ObservableCollection<Contact> Contacts { get; set; }
        public ObservableCollection<Contact> AllContacts { get; set; }

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged(nameof(SelectedContact));
                LoadSelectedContact();
                UpdateSaveState();
            }
        }

        private string _firstName = "";
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(nameof(FirstName)); UpdateSaveState(); } }

        private string _lastName = "";
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(nameof(LastName)); UpdateSaveState(); } }

        private string _email = "";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); UpdateSaveState(); } }

        private string _phone = "";
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(nameof(Phone)); UpdateSaveState(); } }

        private string _city = "";
        public string City { get => _city; set { _city = value; OnPropertyChanged(nameof(City)); UpdateSaveState(); } }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterContacts(); }
        }


        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }


        private bool _canSave = false;
        public bool CanSave { get => _canSave; set { _canSave = value; OnPropertyChanged(nameof(CanSave)); } }

        public MainViewModel()
        {
            _context = new AppDbContext();

            AllContacts = new ObservableCollection<Contact>(_context.Contacts.ToList());
            Contacts = new ObservableCollection<Contact>(AllContacts);

            AddCommand = new RelayCommand(_ => AddContact());
            SaveCommand = new RelayCommand(_ => SaveContact(), _ => CanSave);
            DeleteCommand = new RelayCommand(_ => DeleteContact());
            EditCommand = new RelayCommand(_ => EditContact());
        }

        private void AddContact()
        {
            if (SelectedContact != null)
            {
                SelectedContact = null;
                ClearForm();
                MessageBox.Show("Selection cleared. You can now add a new contact.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("First Name, Last Name, and Email are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var addr = new MailAddress(Email);
                if (addr.Address != Email)
                {
                    MessageBox.Show("Email format is invalid.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Email format is invalid.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var contact = new Contact
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone,
                City = City
            };

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            AllContacts.Add(contact);
            Contacts.Add(contact);

            MessageBox.Show("Contact added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearForm();
        }

        private void SaveContact()
        {
            if (SelectedContact == null) return;

            if (!IsFormValid())
            {
                MessageBox.Show("Please fill in First Name, Last Name and valid Email.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedContact.FirstName = FirstName;
            SelectedContact.LastName = LastName;
            SelectedContact.Email = Email;
            SelectedContact.Phone = Phone;
            SelectedContact.City = City;

            _context.SaveChanges();

            MessageBox.Show("Contact saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearForm();
        }

        private void DeleteContact()
        {
            if (SelectedContact == null)
            {
                MessageBox.Show("Please select a contact first to delete.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {SelectedContact.FirstName} {SelectedContact.LastName}?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _context.Contacts.Remove(SelectedContact);
                _context.SaveChanges();

                AllContacts.Remove(SelectedContact);
                Contacts.Remove(SelectedContact);

                MessageBox.Show("Contact deleted successfully!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
        }

        private void EditContact()
        {
            if (SelectedContact == null)
            {
                MessageBox.Show("Please select a contact first to edit.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show("You can now edit the selected contact. Make your changes and click Save.", "Edit Mode", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearForm()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            City = "";
            SelectedContact = null;

            CanSave = false; 
        }

        private void LoadSelectedContact()
        {
            if (SelectedContact == null) return;

            FirstName = SelectedContact.FirstName;
            LastName = SelectedContact.LastName;
            Email = SelectedContact.Email;
            Phone = SelectedContact.Phone;
            City = SelectedContact.City;
        }

        private void FilterContacts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Contacts.Clear();
                foreach (var c in AllContacts) Contacts.Add(c);
            }
            else
            {
                var filtered = AllContacts.Where(c =>
                    (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.ToLower().Contains(SearchText.ToLower())) ||
                    (!string.IsNullOrEmpty(c.LastName) && c.LastName.ToLower().Contains(SearchText.ToLower())) ||
                    (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(SearchText.ToLower()))
                ).ToList();

                Contacts.Clear();
                foreach (var c in filtered) Contacts.Add(c);
            }
        }

        private bool HasChanges()
        {
            if (SelectedContact == null) return false;

            return FirstName != SelectedContact.FirstName ||
                   LastName != SelectedContact.LastName ||
                   Email != SelectedContact.Email ||
                   Phone != SelectedContact.Phone ||
                   City != SelectedContact.City;
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email))
                return false;

            try
            {
                var addr = new MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateSaveState()
        {
            CanSave = HasChanges() && IsFormValid();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}