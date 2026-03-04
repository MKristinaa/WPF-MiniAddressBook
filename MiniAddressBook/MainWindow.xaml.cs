using MiniAddressBook.Data;
using MiniAddressBook.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniAddressBook
{
    public partial class MainWindow : Window
    {
        private AppDbContext _context;
        private ObservableCollection<Contact> _contact;
        private ObservableCollection<Contact> _allContacts;
        private Contact _selectedContact;

        public MainWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();

            LoadContacts();

            btnSave.IsEnabled = false;
            txtSearch.TextChanged += TxtSearch_TextChanged;
        }

        private void LoadContacts()
        {
            _allContacts = new ObservableCollection<Contact>(_context.Contacts.ToList());
            _contact = new ObservableCollection<Contact>(_allContacts);
            dgContacts.ItemsSource = _contact;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearch.Text.ToLower().Trim();
            if (string.IsNullOrEmpty(filter))
            {
                _contact.Clear();
                foreach (var c in _allContacts)
                    _contact.Add(c);
            }
            else
            {
                var filtered = _allContacts.Where(c =>
                    (c.FirstName != null && c.FirstName.ToLower().Contains(filter)) ||
                    (c.LastName != null && c.LastName.ToLower().Contains(filter)) ||
                    (c.Email != null && c.Email.ToLower().Contains(filter))
                ).ToList();

                _contact.Clear();
                foreach (var c in filtered)
                    _contact.Add(c);
            }
        }

        private void txtFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = HasChanges() && IsFormValid();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedContact != null)
            {
                dgContacts.SelectedItem = null;
                ClearForm();
                MessageBox.Show("Selection cleared. You can now add a new contact.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in First Name, Last Name and Email.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(txtEmail.Text);
                if (addr.Address != txtEmail.Text)
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

            var newContact = new Contact
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                City = txtCity.Text,
            };

            _context.Contacts.Add(newContact);
            _context.SaveChanges();

            _allContacts.Add(newContact);
            _contact.Add(newContact);

            MessageBox.Show("Contact added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearForm();
        }

        private void dgContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedContact = dgContacts.SelectedItem as Contact;

            if (_selectedContact != null)
            {
                txtFirstName.Text = _selectedContact.FirstName;
                txtLastName.Text = _selectedContact.LastName;
                txtEmail.Text = _selectedContact.Email;
                txtPhone.Text = _selectedContact.Phone;
                txtCity.Text = _selectedContact.City;

                btnSave.IsEnabled = false;
            }
            else
            {
                ClearForm();
            }
        }

        private bool HasChanges()
        {
            if (_selectedContact == null) return false;

            return txtFirstName.Text != _selectedContact.FirstName ||
                   txtLastName.Text != _selectedContact.LastName ||
                   txtEmail.Text != _selectedContact.Email ||
                   txtPhone.Text != _selectedContact.Phone ||
                   txtCity.Text != _selectedContact.City;
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
               string.IsNullOrWhiteSpace(txtLastName.Text) ||
               string.IsNullOrWhiteSpace(txtEmail.Text))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(txtEmail.Text);
                return addr.Address == txtEmail.Text;
            }
            catch
            {
                return false;
            }
        }

        private void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtCity.Clear();

            _selectedContact = null;
            dgContacts.SelectedItem = null;

            btnSave.IsEnabled = false;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedContact == null)
            {
                MessageBox.Show("Please select a contact first to edit.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show("You can now edit the selected contact. Make your changes and click Save.",
                "Edit Mode", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedContact == null)
                return;

            if (!IsFormValid())
            {
                MessageBox.Show("Please fill in all required fields with valid values.",
                    "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedContact.FirstName = txtFirstName.Text;
            _selectedContact.LastName = txtLastName.Text;
            _selectedContact.Email = txtEmail.Text;
            _selectedContact.Phone = txtPhone.Text;
            _selectedContact.City = txtCity.Text;

            _context.SaveChanges();

            MessageBox.Show("Contact saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            dgContacts.Items.Refresh();

            ClearForm();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedContact == null)
            {
                MessageBox.Show("Please select a contact first to delete.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {_selectedContact.FirstName} {_selectedContact.LastName}?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _context.Contacts.Remove(_selectedContact);
                _context.SaveChanges();

                _allContacts.Remove(_selectedContact);
                _contact.Remove(_selectedContact);

                MessageBox.Show("Contact deleted successfully!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
            }
        }
    }
}