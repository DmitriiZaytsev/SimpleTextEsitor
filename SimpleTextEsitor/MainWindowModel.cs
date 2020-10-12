using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace SimpleTextEditor
{
    class MainWindowModel : VeiwModel
    {
        private string f_text;

        public string Text { get => f_text; set => Set(ref f_text, value); }

        private string f_filename;

        public string FileName
        {
            get => f_filename;
            set
            {
                if(Set(ref f_filename, value))
                {
                    ReadFileAsinc(value);
                }
            }
        }

        private async void ReadFileAsinc(string filePath)
        {
            Text = "";
            if (!File.Exists(filePath)) return;
            using (var rider = File.OpenText(filePath))
                Text = await rider.ReadToEndAsync().ConfigureAwait(true);
        }
        
        public ICommand CreateCommand { get; }
        public ICommand SaveCommand { get; }

        public MainWindowModel()
        {
            CreateCommand = new LamdaCommand(OnCreateCommandExecuted);
            SaveCommand = new LamdaCommand(OnSaveCommandExecutedAsync, OnSaveCommandCanExecuted);
        }

        private bool OnSaveCommandCanExecuted(object filePath)
        {
            return !string.IsNullOrEmpty(f_text);
        }

        private async void OnSaveCommandExecutedAsync(object filePath)
        {
            var file_name = filePath as string;
            if (file_name == null)
            {
                var dialog = new SaveFileDialog()
                {
                    Title = "Сохранение файла...",
                    Filter = "Текстовые файлы (*.txt)| *.txt | Все файлы (*.*) | *.*",
                    InitialDirectory = Environment.CurrentDirectory,
                    RestoreDirectory = true
                };
                if (dialog.ShowDialog() != true) return;
                file_name = dialog.FileName;
                using (var writeer = new StreamWriter(new FileStream(file_name, FileMode.Create, FileAccess.Write)))
                    await writeer.WriteAsync(f_text).ConfigureAwait(true);
                FileName = file_name;
            }
        }

        private void OnCreateCommandExecuted(object p)
        {
            Text = "";
            FileName = null;
        }

        public ICommand QuitCommand { get; } = new LamdaCommand(p=>Application.Current.Shutdown());
    }
}
