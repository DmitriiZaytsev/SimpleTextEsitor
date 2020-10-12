using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private async void ReadFileAsinc(string FilePath)
        {
            Text = "";
            if (!File.Exists(FilePath)) return;
            using (var rider = File.OpenText(FilePath))
                Text = await rider.ReadToEndAsync().ConfigureAwait(true);
        }
        


    }
}
