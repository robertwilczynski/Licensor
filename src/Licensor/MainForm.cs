#region License

// Copyright 2010 Robert Wilczynski (http://github.com/robertwilczynski/gitprise)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Licensor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenCodeDir_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var files = GetFiles(dialog.SelectedPath, txtExtensions.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList());
                    txtFiles.Lines = files.ToArray();
                }
            }
        }

        private List<string> GetFiles(string selectedPath, IEnumerable<string> extensions)
        {
            var paths = new List<string>(Directory.GetFiles(selectedPath).Where(file =>
                extensions.Contains(".*") ||
                extensions.Contains(Path.GetExtension(file))));

            Array.ForEach(Directory.GetDirectories(selectedPath), 
                dir => paths.AddRange(GetFiles(dir, extensions)));

            return paths;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtLicense.Text))
            {
                return;
            }

            Array.ForEach(txtFiles.Lines, file =>
                {
                    var content = File.ReadAllText(file);
                    if (!content.Contains(txtLicense.Text))
                    {
                        File.WriteAllText(file, txtLicense.Text + content);
                    }                    
                });

            MessageBox.Show("Finished!");
        }
    }
}
