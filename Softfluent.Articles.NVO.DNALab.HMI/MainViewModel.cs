using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.Specialized;

namespace Softfluent.Articles.NVO.DNASpecializer.HMI
{
    public class MainViewModel
    {
        public ObservableCollection<string> LastActions { get; private set; }
        public ObservableCollection<AIWrapper> Population { get; private set; }

        private static Type[] _allRulesTypes;
        private static Type[] AllRulesTypes
        {
            get
            {
                if (_allRulesTypes == null)
                    _allRulesTypes = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .Where(x => x.IsSubclassOf(typeof(Softfluent.Articles.NVO.Game2048.AI.AIRule))).ToArray();
                return _allRulesTypes;
            }
        }
        private DNALab _Lab;

        public MainViewModel()
        {
            LastActions = new ObservableCollection<string>();
            Population = new ObservableCollection<AIWrapper>();
            // same on inner population list for DNA lab (two way)

            _Lab = new DNALab(AllRulesTypes);
            _Lab.Population.CollectionChanged += LabPopulation_CollectionChanged;
            _Lab.EventOccurred += (sender, args) => AddLastAction(args.Item);

        }

        private void LabPopulation_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                    Population.Clear();
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    if (e.NewItems != null)
                        foreach (var wrapper in e.NewItems.OfType<AIWrapper>())
                            Population.Add(wrapper);
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (e.OldItems != null)
                        foreach (var wrapper in e.OldItems.OfType<AIWrapper>())
                            Population.Remove(wrapper);
                }
            }));
        }

        public void AddLastAction(string action)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                LastActions.Insert(0, action);
                while (LastActions.Count > 5)
                {
                    LastActions.RemoveAt(LastActions.Count - 1);
                }
            }));
        }

        public ICommand LoadCommand
        {
            get
            {
                return new SimpleCommand(() =>
                {
                    try
                    {
                        OpenFileDialog ofd = new OpenFileDialog { Filter = "2048 DNA Files|*.dna" };
                        if (ofd.ShowDialog() == true)
                        {
                            using (System.IO.StreamReader file = new System.IO.StreamReader(ofd.FileName))
                            {
                                var pop = (AIWrapper[])_PopulationSerializer.Deserialize(file);
                                _Lab.Population.Clear();
                                foreach (var dna in pop)
                                    _Lab.Population.Add(dna);
                                _Lab.PopulationSize = pop.Length;
                            }

                            AddLastAction("DNA file loaded from " + ofd.FileName);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error retreiving DNA from file. Check if not corrupted", "Error");
                    }

                });
            }
        }

        private static readonly System.Xml.Serialization.XmlSerializer _PopulationSerializer =
            new System.Xml.Serialization.XmlSerializer(typeof(AIWrapper[]));

        public ICommand SaveCommand
        {
            get
            {
                return new SimpleCommand(() =>
                {
                    SaveFileDialog sfd = new SaveFileDialog { AddExtension = true, DefaultExt = ".dna", Filter = "2048 DNA Files|*.dna" };
                    if (sfd.ShowDialog() == true)
                    {

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(sfd.FileName))
                        {
                            _PopulationSerializer.Serialize(file, Population.ToArray());
                        }
                        AddLastAction("DNA file saved at " + sfd.FileName);
                    }
                });
            }
        }

        public ICommand PopulateCommand
        {
            get
            {
                return new SimpleCommand(() =>
                {
                    NewRandomPopulation nrp = new NewRandomPopulation();

                    if (nrp.ShowDialog() == true)
                    {
                        var response = nrp.Response;
                        System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
                        {
                            _Lab.PopulateAIs(response);
                        });
                    }
                });
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                return new SimpleCommand(() =>
                    System.Windows.Application.Current.Shutdown(42)
                    );
            }
        }

        public ICommand StartCommand
        {
            get
            {
                return new SimpleCommand(() =>
                {
                    System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        _Lab.Live((elements) => { }, new Predicate<AIWrapper>(x => x.PercentWin > 99));
                    });
                });
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return new SimpleCommand(() =>
                {
                    System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
                    {
                        _Lab.Stop();
                    });
                });
            }
        }

    }
}


public class SimpleCommand : ICommand
{
    public Action CommandAction { get; set; }
    public SimpleCommand(Action commandAction)
    {
        CommandAction = commandAction;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
        if (CommandAction != null)
            CommandAction();
    }
}