using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Threading;

namespace SilentEvil
{
   

    public partial class Form1 : Form
    {



        MainForm frm = new MainForm();
        static string sha256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(password), 0, Encoding.ASCII.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public string GetMachineGuid()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey localMachineX64View =
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(
                            string.Format("Key Not Found: {0}", location));

                    object machineGuid = rk.GetValue(name);
                    if (machineGuid == null)
                        throw new IndexOutOfRangeException(
                            string.Format("Index Not Found: {0}", name));

                    SHA256 mySHA256 = SHA256Managed.Create();
                    string mGUID = sha256(machineGuid.ToString());
                    return mGUID;
                }
            }
        }

        public class Hash
        {
            public string hash { get; set; }
        }

        public Hash Hsh;

        public class Error
        {
            public int error { get; set; }
            public string message { get; set; }
        }

        Error Err;


        public class custom_fields
        {
            public string facebook { get; set; }
            public string hwid { get; set; }
            public string skype { get; set; }
            public string twitter { get; set; }

        }

        custom_fields LocalUserCustom;

        public class User
        {
            public int user_id{ get; set; }
            public string username{ get; set; }
            public string email{ get; set; }
            public string gender{ get; set; }
            public string custom_title{ get; set; }
            public int language_id{ get; set; }
            public string timezone{ get; set; }
            public int visible { get; set; }
            public int activity_visible { get; set; }
            public string user_group_id{ get; set; }
            public string secondary_group_ids{ get; set; }
            public int message_count{ get; set; }
            public int conversations_unread{ get; set; }
            public int register_date{ get; set; }
            public int last_activity{ get; set; }
            public int trophy_points{ get; set; }
            public int alerts_unread{ get; set; }
            public int avatar_date{ get; set; }
            public int avatar_width{ get; set; }
            public int avatar_height{ get; set; }
            public string gravatar{ get; set; }
            public string user_state{ get; set; }
            public int is_moderator { get; set; }
            public int is_admin { get; set; }
            public int like_count{ get; set; }
            public int warning_points{ get; set; }
            public int is_staff { get; set; }
            public custom_fields custom { get; set; }
            
        }

        User LocalUser;

        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = button1;
            this.Icon = SilentEvil.Properties.Resources.favicon;
        }


        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void nsLabel1_Click(object sender, EventArgs e)
        {
         
        }


        private void nsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                bool custom = true;
                string url = "http://www.silentevil.net/forum/api.php?action=authenticate&username=" + nsTextBox1.Text + "&password=" + nsTextBox2.Text;
                WebClient client = new WebClient();
                string reply = client.DownloadString(url);

                if (reply.Contains("error"))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    Err = ser.Deserialize<Error>(reply);
                    label3.Visible = true;
                    label3.Text = Err.message;
                    return;
                }
                else
                {
                    if (reply.Contains("hash"))
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        Hsh = ser.Deserialize<Hash>(reply);
                        string userUrl = "http://www.silentevil.net/forum/api.php?action=getUser&hash=" + nsTextBox1.Text + ":" + Hsh.hash;
                        string userReply = client.DownloadString(userUrl);
                        if (!userReply.Contains(":{"))
                        {
                            custom = false;
                        }
                        LocalUser = ser.Deserialize<User>(userReply);

                        if (custom)
                        {
                            userReply = userReply.Remove(0, userReply.IndexOf(":{") + 1);
                            userReply = userReply.Remove(userReply.Length - 1, 1);
                            LocalUserCustom = ser.Deserialize<custom_fields>(userReply);
                        }

                        if (!LocalUser.secondary_group_ids.Contains("5") && !LocalUser.user_group_id.Contains("5"))
                        {
                            label3.Visible = true;
                            label3.Text = "No active subscription found";
                            return;
                        }

                        if (custom)
                        {
                            if (LocalUserCustom.hwid.Length == 0)
                            {
                                client.OpenRead("http://www.silentevil.net/forum/api.php?action=editUser&hash=FC7vj5GCuE1U7Vn&user=" + nsTextBox1.Text + "&custom_fields=hwid=" + GetMachineGuid());
                                label3.Visible = true;
                                label3.Text = "New HWID set. Successful Login";
                                if (!frm.Visible)
                                {
                                    frm.Show();
                                    this.Hide();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            client.OpenRead("http://www.silentevil.net/forum/api.php?action=editUser&hash=FC7vj5GCuE1U7Vn&user=" + nsTextBox1.Text + "&custom_fields=hwid=" + GetMachineGuid());
                            label3.Visible = true;
                            label3.Text = "New HWID set. Successful Login";
                            if (!frm.Visible)
                            {
                                frm.Show();
                                this.Hide();
                                return;
                            }

                        }

                        if(LocalUserCustom.hwid != GetMachineGuid())
                        {
                            label3.Visible = true;
                            label3.Text = "HWID not matching";
                            return;
                        }

                        
                        label3.Visible = true;
                        label3.Text = "Successful Login";
                        if (!frm.Visible)
                        {
                            frm.Show();
                           this.Hide();
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            nsButton1_Click(sender, e);
        }

        private void nsControlButton1_Click(object sender, EventArgs e)
        {

        }

        private void nsTheme1_Click(object sender, EventArgs e)
        {

        }

    }
}
