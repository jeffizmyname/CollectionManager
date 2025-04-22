using CollectionManager.Models;
using CollectionManager.Storage;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace CollectionManager.ViewModels
{
    public class SetViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Set> Sets { get; set; }

        private Set _selectedSet;
        public Set SelectedSet
        {
            get => _selectedSet;
            set
            {
                if (_selectedSet != value)
                {
                    _selectedSet = value;
                    OnPropertyChanged(nameof(SelectedSet));
                    Values.Clear();
                    if (_selectedSet?.Values != null)
                    {
                        foreach (var val in _selectedSet.Values)
                            Values.Add(val);
                    }
                }
            }
        }
        public ObservableCollection<string> Values { get; } = new();

        private string _selectedValue;
        public string SelectedValue
        {
            get => _selectedValue;
            set
            {
                if (_selectedValue != value)
                {
                    _selectedValue = value;
                    OnPropertyChanged(nameof(SelectedValue));
                }
            }
        }


        public SetViewModel()
        {
            Sets = new ObservableCollection<Set>(Manager.LoadSets());
            SelectedSet = Sets.FirstOrDefault();
        }

        public void AddSet(Set set)
        {
            Sets.Add(set);
            Manager.SaveSet(set);
        }

        
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
