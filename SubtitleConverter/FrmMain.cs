using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml;

namespace SubtitleConverter
{
    public partial class FrmMain : Form
    {
        static string[] Commas = new string[] { "," };
        static string[] NewLines = new string[] { "\n" };
        static string[] Spaces = new string[] { " " };
        List<SubTitle> SubTitles;

        class SubTitle
        {
            public DateTime StartTime;
            public DateTime EndTime;
            public string Content;

            public SubTitle(DateTime startTime, DateTime endTime, string content)
            {
                StartTime = startTime;
                EndTime = endTime;
                Content = content;
            }
        }

        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog() { Filter = "Subtitle files (*.ass; *.dfxp; *.srt; *.sub; *.vtt; *.wsrt)|*.ass; *.dfxp; *.srt; *.sub; *.vtt; *.wsrt|All files (*.*)|*.*" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSFile.Text = dlg.FileName;
                ReadSubtitle(txtSFile.Text);
                lvwSub.Items.Clear();
                lvwSub.BeginUpdate();
                for (int i = 0; i < SubTitles.Count; i++)
                {
                    var sub = new string[] { (i + 1).ToString(), SubTitles[i].StartTime.ToString("HH:mm:ss.ff"), SubTitles[i].EndTime.ToString("HH:mm:ss.ff"), SubTitles[i].Content };
                    lvwSub.Items.Add(new ListViewItem(sub));
                }
                lvwSub.EndUpdate();
                btnSave.Enabled = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog() { Filter = "Advanced SubStation Alpha (*.ass)|*.ass|Distribution Format Exchange Profile (*.dfxp)|*.dfxp|SubRip Text (*.srt)|*.srt|SubViewer (*.sub)|*.sub|Web Video Text Tracks Format (*.vtt)|*.vtt|WebSubrip Text (*.wsrt)|*.wsrt|All files (*.*)|*.*" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtDFile.Text = dlg.FileName;
                AdjustDelayTiming(Convert.ToInt32(nudDelay.Value));
                WriteSubtitle(txtDFile.Text);
            }
        }

        public void ReadSubtitle(string file)
        {
            SubTitles = new List<SubTitle>();
            string ext = new FileInfo(file).Extension.ToLower();
            switch (ext)
            {
                case ".ass":
                    ReadASS(file);
                    break;
                case (".dfxp"):
                    ReadDFXP(file);
                    break;
                case ".srt":
                    ReadSRT(file);
                    break;
                case ".sub":
                    ReadSUB(file);
                    break;
                case (".vtt"):
                    ReadVTT(file);
                    break;
                case (".wsrt"):
                    ReadWSRT(file);
                    break;
            }
            TrimSubtitle();
        }

        public void WriteSubtitle(string file)
        {
            string ext = new FileInfo(file).Extension.ToLower();
            switch (ext)
            {
                case ".ass":
                    WriteASS(file);
                    break;
                case (".dfxp"):
                    WriteDFXP(file);
                    break;
                case ".srt":
                case (".wsrt"):
                    WriteSRTnWSRT(file);
                    break;
                case ".sub":
                    WriteSUB(file);
                    break;
                case (".vtt"):
                    WriteVTT(file);
                    break;
            }
        }

        private void TrimSubtitle()
        {
            for (int i = 0; i < SubTitles.Count; i++)
            {
                if (SubTitles[i].Content.Trim() == string.Empty)
                {
                    SubTitles.RemoveAt(i);
                    i--;
                }
            }

            SubTitles = SubTitles.OrderBy(q => q.StartTime).ToList();

            for (int i = 0; i < SubTitles.Count - 1; i++)
            {
                if (SubTitles[i].StartTime.Equals(SubTitles[i + 1].StartTime))
                {
                    SubTitles[i].Content += "\n" + SubTitles[i + 1].Content;
                    SubTitles.RemoveAt(i + 1);
                    i--;
                }
            }
        }

        private void AdjustDelayTiming(int ms)
        {
            if (ms == 0)
                return;
            foreach (var st in SubTitles)
            {
                st.StartTime = st.StartTime.AddMilliseconds(ms);
                st.EndTime = st.EndTime.AddMilliseconds(ms);
            }
        }

        private void ReadASS(string file)
        {
            string text = File.ReadAllText(file).Replace("\r\n", "\n");
            text = Regex.Replace(text, @"\{[^}]*\}", "").Substring(text.IndexOf("\nDialogue: ") + 10);
            string[] subs = Regex.Split(text, "\nDialogue: ");
            foreach (string sub in subs)
            {
                string[] split = sub.Split(Commas, 10, StringSplitOptions.None);
                var sTime = DateTime.ParseExact(split[1], "H:mm:ss.ff", CultureInfo.InvariantCulture);
                var eTime = DateTime.ParseExact(split[2], "H:mm:ss.ff", CultureInfo.InvariantCulture);
                string tmp = split[9].Trim().Replace(@"\N", "\n");
                SubTitles.Add(new SubTitle(sTime, eTime, tmp));
            }
        }

        private void ReadDFXP(string file)
        {
            using (var xr = new XmlTextReader(file))
            {
                xr.Namespaces = false;
                while (xr.ReadToFollowing("p"))
                {
                    DateTime beginTime;
                    DateTime endTime;
                    beginTime = DateTime.ParseExact(xr.GetAttribute("begin"), "HH:mm:ss.ff", CultureInfo.InvariantCulture);
                    endTime = DateTime.ParseExact(xr.GetAttribute("end"), "HH:mm:ss.ff", CultureInfo.InvariantCulture);
                    string tmp = Regex.Replace(xr.ReadInnerXml(), "(\r\n?|\n) *", "").Replace("<br/>", "\n");
                    SubTitles.Add(new SubTitle(beginTime, endTime, tmp));
                }
            }
        }

        private void ReadSRT(string file)
        {
            string text = File.ReadAllText(file).Replace("\r\n", "\n");
            text = Regex.Replace(text, "<[^>]*>", "").Substring(text.IndexOf("\n") + 1);
            string[] subs = Regex.Split(text, "\n\n[0-9]+\n");
            foreach (string sub in subs)
            {
                string[] split = sub.Split(NewLines, 2, StringSplitOptions.None);
                string[] times = Regex.Split(split[0], " --> ");
                var sTime = DateTime.ParseExact(times[0], "HH:mm:ss,fff", CultureInfo.InvariantCulture);
                var eTime = DateTime.ParseExact(times[1], "HH:mm:ss,fff", CultureInfo.InvariantCulture);
                string tmp = split[1].Trim();
                SubTitles.Add(new SubTitle(sTime, eTime, tmp));
            }
        }

        private void ReadSUB(string file)
        {
            string text = File.ReadAllText(file).Replace("\r\n", "\n");
            text = Regex.Replace(text, @"\{[^}]*\}", "").Substring(text.IndexOf("\n00:") + 1);
            string[] subs = Regex.Split(text, "\n\n");
            foreach (string sub in subs)
            {
                string[] split = sub.Split(NewLines, 2, StringSplitOptions.None);
                string[] times = Regex.Split(split[0], ",");
                var sTime = DateTime.ParseExact(times[0], "HH:mm:ss.ff", CultureInfo.InvariantCulture);
                var eTime = DateTime.ParseExact(times[1], "HH:mm:ss.ff", CultureInfo.InvariantCulture);
                string tmp = split[1].Trim().Replace("[br]", "\n");
                SubTitles.Add(new SubTitle(sTime, eTime, tmp));
            }
        }

        private void ReadVTT(string file)
        {
            string text = File.ReadAllText(file).Replace("\r\n", "\n");
            text = Regex.Replace(text, @"<v(.*? )(.*?)>", "$2: ");
            text = Regex.Replace(text, @"<[^>]*>", "").Substring(text.IndexOf("\n\n") + 4);
            string[] subs = Regex.Split(text, "\n\n[0-9]+\n");
            foreach (string sub in subs)
            {
                string[] split = sub.Split(NewLines, 2, StringSplitOptions.None);
                string[] times = Regex.Split(split[0], " --> ");
                var sTime = DateTime.ParseExact(times[0], "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                var eTime = DateTime.ParseExact(times[1], "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                string tmp = split[1].Trim();
                SubTitles.Add(new SubTitle(sTime, eTime, tmp));
            }
        }

        private void ReadWSRT(string file)
        {
            string text = File.ReadAllText(file).Replace("\r\n", "\n");
            text = Regex.Replace(text, "<[^>]*>", "").Substring(text.IndexOf("\n00:") + 1);
            string[] subs = Regex.Split(text, "\n\n[0-9]+\n");
            foreach (string sub in subs)
            {
                string[] split = sub.Split(NewLines, 2, StringSplitOptions.None);
                string[] times = Regex.Split(split[0], " --> ");
                var sTime = DateTime.ParseExact(times[0], "HH:mm:ss,fff", CultureInfo.InvariantCulture);
                var eTime = DateTime.ParseExact(times[1].Split(Spaces, 2, StringSplitOptions.None)[0], "HH:mm:ss,fff", CultureInfo.InvariantCulture);
                string tmp = split[1].Trim();
                SubTitles.Add(new SubTitle(sTime, eTime, tmp));
            }
        }

        private void WriteASS(string file)
        {
            string head = "[Script Info]\nTitle: <untitled>\nScriptType: v4.00\nCollisions: Normal\nPlayDepth: 0\n\n";
            string styles = "[v4+ Styles]\nFormat: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, " +
                "Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, " +
                "Encoding\nStyle: Default,Arial,20,&H00FFFFFF,&H000080FF,&H00000000,&H80000000,0,0,0,0,100,100,0,0,1,2,2,2,10,10,20,0\n\n";
            string events = "[Events]\nFormat: Layer, Start, End, Style, Actor, MarginL, MarginR, MarginV, Effect, Text\n";
            var sb = new StringBuilder();
            sb.Append(head);
            sb.Append(styles);
            sb.Append(events);
            for (int i = 0; i < SubTitles.Count; i++)
            {
                string sTime = SubTitles[i].StartTime.ToString("H:mm:ss.ff");
                string eTime = SubTitles[i].EndTime.ToString("H:mm:ss.ff");
                string conent = SubTitles[i].Content.Replace("\n", "\\N");
                sb.Append($"Dialogue: 0,{sTime},{eTime},Default,,0,0,0,,{conent}\n");
            }
            File.WriteAllText(file, sb.ToString());
        }

        private void WriteDFXP(string file)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n"
            };
            using (XmlWriter xw = XmlWriter.Create(file, settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("tt", "http://www.w3.org/ns/ttml");
                xw.WriteStartElement("body");
                xw.WriteStartElement("div");
                xw.WriteAttributeString("xml", "lang", null, "en");
                for (int i = 0; i < SubTitles.Count; i++)
                {
                    string sTime = SubTitles[i].StartTime.ToString("HH:mm:ss.ff");
                    string eTime = SubTitles[i].EndTime.ToString("HH:mm:ss.ff");
                    string tmp = SubTitles[i].Content.Replace("\n", "<br/>");
                    xw.WriteStartElement("p");
                    xw.WriteAttributeString("begin", sTime);
                    xw.WriteAttributeString("end", eTime);
                    xw.WriteAttributeString("xml", "id", null, $"caption {i + 1}");
                    xw.WriteRaw(tmp);
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndElement();
                xw.WriteEndElement();
            }
        }

        private void WriteSRTnWSRT(string file)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < SubTitles.Count; i++)
            {
                string sTime = SubTitles[i].StartTime.ToString("HH:mm:ss,fff");
                string eTime = SubTitles[i].EndTime.ToString("HH:mm:ss,fff");
                string content = SubTitles[i].Content;
                sb.Append($"{i + 1}\n{sTime} --> {eTime}\n{content}\n\n");
            }
            File.WriteAllText(file, sb.ToString());
        }

        private void WriteSUB(string file)
        {
            string head = "[INFORMATION]\n[TITLE]\n[AUTHOR]\n[SOURCE]\n[PRG]\n[FILEPATH]\n" +
                "[DELAY]\n[CD TRACK]\n[COMMENT]\n[END INFORMATION]\n" +
                "[SUBTITLE]\n[COLF]&HFFFFFF,[STYLE]no,[SIZE]18,[FONT]Arial\n";
            var sb = new StringBuilder(head);
            for (int i = 0; i < SubTitles.Count; i++)
            {
                string sTime = SubTitles[i].StartTime.ToString("HH:mm:ss.ff");
                string eTime = SubTitles[i].EndTime.ToString("HH:mm:ss.ff");
                string content = SubTitles[i].Content;
                sb.Append($"{sTime},{eTime}\n{content}\n\n");
            }
            File.WriteAllText(file, sb.ToString());
        }

        private void WriteVTT(string file)
        {
            var sb = new StringBuilder("WEBVTT\n\n");
            for (int i = 0; i < SubTitles.Count; i++)
            {
                string sTime = SubTitles[i].StartTime.ToString("HH:mm:ss.fff");
                string eTime = SubTitles[i].EndTime.ToString("HH:mm:ss.fff");
                string content = SubTitles[i].Content;
                sb.Append($"{i + 1}\n{sTime} --> {eTime}\n{content}\n\n");
            }
            File.WriteAllText(file, sb.ToString());
        }
    }
}