using MiniAddressBook.Data;
using MiniAddressBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniAddressBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext _context;
        private ObservableCollection<Contact> _contact;
        public MainWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();

            LoadContacts();
        }

        private void LoadContacts()
        {
            _contact = new ObservableCollection<Contact>(_context.Contacts.ToList());
            dgContacts.ItemsSource = _contact;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtFirstName.Text) ||
               string.IsNullOrWhiteSpace(txtLastName.Text) ||
               string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in First Name, Last Name and Email.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            _contact.Add(newContact);

            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtCity.Clear();


            dgContacts.SelectedItem = newContact;
        }
    }
}
