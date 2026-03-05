using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniAddressBook.Models
{
    public class Contact : INotifyPropertyChanged
    {
        private int _id;
        [Key]
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string _firstName = string.Empty;
        [Required]
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        private string _lastName = string.Empty;
        [Required]
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        private string _email = string.Empty;
        [Required]
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        private string _city = string.Empty;
        public string City
        {
            get => _city;
            set { _city = value; OnPropertyChanged(nameof(City)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}