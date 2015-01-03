using PawJershauge.IMDBFlatFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.FtpClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MyMDb
{
    public partial class Form1 : Form
    {
        private volatile bool RunFTP = true;
        private DateTime starttime = DateTime.Now;
        private MovieListFile mlf;
        private TagLineListFile tlf;
        private PlotListFile plf;
        private GenreListFile glf;
        private CountryListFile clf;
        private FtpClient ftpClient = new FtpClient();
        private TabPage[] hidePages = new TabPage[3];
        private string ftpServer = "ftp.sunet.se";
        private string ftpServerPath = "pub/tv+movies/imdb/";
        bool NoErrors = true;
        bool working = false;
        Thread DlT = null;
        Thread BT = null;

        public Form1()
        {
            InitializeComponent();
            cbIMDBInterfaces.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.WorkingDirectory) && Directory.Exists(Properties.Settings.Default.WorkingDirectory))
                txtBrowseLocalFolder.Text = Properties.Settings.Default.WorkingDirectory;
            txtServer.Text = string.IsNullOrEmpty(Properties.Settings.Default.SQLServer) ? Environment.MachineName : Properties.Settings.Default.SQLServer;
            cbIntegratedSecurity.Checked = Properties.Settings.Default.SQLSecurity;
            txtUsername.Text = Properties.Settings.Default.User;
            txtPassword.Text = Properties.Settings.Default.Pass;
            TabPages(false);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ftpClient.Dispose();
        }

		private void TabPages(bool show)
		{
			int showupto = 3;
			if (tabControl1.InvokeRequired)
			{
				tabControl1.Invoke((MethodInvoker)delegate
				{
					if (show)
					{
						for (int i = 0; i < showupto; i++)
						{
							if (!tabControl1.TabPages.Contains(hidePages[i]))
								tabControl1.TabPages.Add(hidePages[i]);
						}
					}
					else if (tabControl1.TabPages.Count > 1)
					{
						for (int i = 0; i < 3; i++)
							hidePages[i] = tabControl1.TabPages[i + 1];
						for (int i = 3; i > 0; i--)
							tabControl1.TabPages.RemoveAt(i);
					}
				});
			}
			else
			{
				if (show)
					for (int i = 0; i < showupto; i++)
					{
						if (!tabControl1.TabPages.Contains(hidePages[i]))
							tabControl1.TabPages.Add(hidePages[i]);
					}
				else
				{
					for (int i = 0; i < 3; i++)
					{
						hidePages[i] = tabControl1.TabPages[i + 1];
					}
					for (int i = 3; i > 0; i--)
					{
						tabControl1.TabPages.RemoveAt(i);
					}
				}
			}
		}

        private Dictionary<string, short> GetGenresTable()
        {
            Dictionary<string, short> gl = new Dictionary<string, short>();
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            { 
                conn.Open();
                using (SqlCommand com = new SqlCommand("SELECT [Id],[Name] FROM [dbo].[Genre]", conn))
                {
                    SqlDataReader rs = com.ExecuteReader();
                    while (rs.Read())
                    {
                        gl.Add(rs.GetString(1), rs.GetInt16(0));
                    }
                }
            }
            return gl;
        }
        private Dictionary<string, string> GetISO3166Table()
        {
            Dictionary<string, string> gl = new Dictionary<string, string>();
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            { 
                conn.Open();
                using (SqlCommand com = new SqlCommand(@"SELECT CONVERT(varchar(80),Alpha3) AS [KeyLookUp],Alpha3 FROM [dbo].[ISO3166]
UNION ALL
    SELECT CONVERT(varchar(80),Alpha2) AS [KeyLookUp],Alpha3 FROM [dbo].[ISO3166]
UNION ALL
    SELECT CONVERT(varchar(80),[Name]) AS [KeyLookUp],Alpha3 FROM [dbo].[ISO3166]
UNION ALL
    SELECT CONVERT(varchar(80),[IncorrectValue]) AS [KeyLookUp],[CorrectValue] AS [Alpha3] FROM [imdb].[CountryCorrection]", conn))
                {
                    SqlDataReader rs = com.ExecuteReader();
                    while (rs.Read())
                    {
                        gl.Add(rs.GetString(0).ToLower(), rs.GetString(1));
                    }
                }
            }
            return gl;
        }

        private void BulkUploadMovies()
        {
            bool goOn = false;
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                connection.Open();
                starttime = DateTime.Now;
                try
                {
                    SqlBulkCopy copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls, null);
                    foreach (DataColumn col in ((IDataReader)mlf).GetSchemaTable().Columns)
                        copy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    //copy.NotifyAfter = 100000;
                    copy.BatchSize = 100000;
                    copy.BulkCopyTimeout = 300; // 5 minutes
                    copy.DestinationTableName = "[imdb].[MediaEntry]";
                    copy.WriteToServer((IDataReader)mlf);
                    goOn = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (goOn)
                MergeMediaEntry();
        }
        private void MergeMediaEntry()
        { 
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                int total = 0;
                int inserted = 0;
                int updated = 0;
                int deleted = 0;
                connection.Open();
                using (SqlCommand com = new SqlCommand(@"[dbo].[MergeMediaEntry]", connection))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 600;
                    using (SqlDataReader rs = com.ExecuteReader())
                    { 
                        rs.Read();
                        total = rs.GetInt32(0);
                        rs.NextResult();
                        rs.Read();
                        inserted = rs.GetInt32(0);
                        updated = rs.GetInt32(1);
                        deleted = rs.GetInt32(2);
                    }
                }
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText(string.Format("{0} Movies handled, {1} New, {2} Updated and {3} Deleted", total, inserted, updated, deleted), Color.DarkOrange);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
            }
        }

        private void BulkUploadTaglines()
        {
            bool goOn = false;
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                connection.Open();
                starttime = DateTime.Now;
                try
                {
                    SqlBulkCopy copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls, null);
                    foreach (DataColumn col in ((IDataReader)tlf).GetSchemaTable().Columns)
                        copy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    //copy.NotifyAfter = 100000;
                    copy.BatchSize = 100000;
                    copy.BulkCopyTimeout = 300; // 5 minutes
                    copy.DestinationTableName = "[imdb].[TagLine]";
                    copy.WriteToServer((IDataReader)tlf);
                    goOn = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (goOn)
                MergeTagLine();
        }
        private void MergeTagLine()
        {
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                int total = 0;
                int inserted = 0;
                int updated = 0;
                int deleted = 0;
                connection.Open();
                using (SqlCommand com = new SqlCommand(@"[dbo].[MergeTagLine]", connection))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 600;
                    using (SqlDataReader rs = com.ExecuteReader())
                    {
                        rs.Read();
                        total = rs.GetInt32(0);
                        rs.NextResult();
                        rs.Read();
                        inserted = rs.GetInt32(0);
                        updated = rs.GetInt32(1);
                        deleted = rs.GetInt32(2);
                    }
                }
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText(string.Format("{0} Tag Lines handled, {1} New, {2} Updated and {3} Deleted", total, inserted, updated, deleted), Color.DarkOrange);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
            }
        }

        private void BulkUploadPlots()
        {
            bool goOn = false;
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                connection.Open();
                starttime = DateTime.Now;
                try
                {
                    SqlBulkCopy copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls, null);
                    foreach (DataColumn col in ((IDataReader)plf).GetSchemaTable().Columns)
                        copy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    //copy.NotifyAfter = 100000;
                    copy.BatchSize = 100000;
                    copy.BulkCopyTimeout = 300; // 5 minutes
                    copy.DestinationTableName = "[imdb].[Plot]";
                    copy.WriteToServer((IDataReader)plf);
                    goOn = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (goOn)
                MergePlot();
        }
        private void MergePlot()
        {
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                int total = 0;
                int inserted = 0;
                int updated = 0;
                int deleted = 0;
                connection.Open();
                using (SqlCommand com = new SqlCommand(@"[dbo].[MergePlots]", connection))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 600; // 10 minutes.
                    using (SqlDataReader rs = com.ExecuteReader())
                    {
                        rs.Read();
                        total = rs.GetInt32(0);
                        rs.NextResult();
                        rs.Read();
                        inserted = rs.GetInt32(0);
                        updated = rs.GetInt32(1);
                        deleted = rs.GetInt32(2);
                    }
                }
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText(string.Format("{0} Plots handled, {1} New, {2} Updated and {3} Deleted", total, inserted, updated, deleted), Color.DarkOrange);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
            }
        }

        private void BulkUploadGenres()
        {
            bool goOn = false;
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                connection.Open();
                starttime = DateTime.Now;
                try
                {
                    SqlBulkCopy copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls, null);
                    foreach (DataColumn col in ((IDataReader)glf).GetSchemaTable().Columns)
                        copy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    //copy.NotifyAfter = 100000;
                    copy.BatchSize = 100000;
                    copy.BulkCopyTimeout = 300; // 5 minutes
                    copy.DestinationTableName = "[imdb].[MediaEntry_Genre_Rel]";
                    copy.WriteToServer((IDataReader)glf);
                    goOn = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (goOn)
            {
                SecureNewGenres();
                MergeGenre();
            }
        }
        private void SecureNewGenres()
        {
            Dictionary<string, short> knownGenres = GetGenresTable();
            foreach (string key in glf.GenreList.Keys)
            {
                if (!knownGenres.ContainsKey(key))
                {
                    using (SqlConnection conn = new SqlConnection(Program.connstr))
                    {
                        conn.Open();
                        using (SqlCommand com = new SqlCommand("INSERT INTO [dbo].[Genre]([Id],[Name]) VALUES(@Id,@Name)", conn))
                        {
                            com.Parameters.AddWithValue("@Id", glf.GenreList[key]);
                            com.Parameters.AddWithValue("@Name",key);
                            com.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        private void MergeGenre()
        {
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                int total = 0;
                int inserted = 0;
                int updated = 0;
                int deleted = 0;
                connection.Open();
                using (SqlCommand com = new SqlCommand(@"[dbo].[MergeGenres]", connection))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 600; // 10 minutes.
                    using (SqlDataReader rs = com.ExecuteReader())
                    {
                        rs.Read();
                        total = rs.GetInt32(0);
                        rs.NextResult();
                        rs.Read();
                        inserted = rs.GetInt32(0);
                        updated = rs.GetInt32(1);
                        deleted = rs.GetInt32(2);
                    }
                }
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText(string.Format("{0} Genres handled, {1} New, {2} Updated and {3} Deleted", total, inserted, updated, deleted), Color.DarkOrange);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
            }
        }

        private void BulkUploadCountry()
        {
            bool goOn = false;
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                connection.Open();
                starttime = DateTime.Now;
                try
                {
                    SqlBulkCopy copy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock & SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls, null);
                    foreach (DataColumn col in ((IDataReader)clf).GetSchemaTable().Columns)
                        copy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    //copy.NotifyAfter = 100000;
                    copy.BatchSize = 100000;
                    copy.BulkCopyTimeout = 300; // 5 minutes
                    copy.DestinationTableName = "[imdb].[MediaEntry_Country_Rel]";
                    copy.WriteToServer((IDataReader)clf);
                    goOn = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (goOn)
            {
                MergeCountry();
            }
        }
        private void MergeCountry()
        {
            using (SqlConnection connection = new SqlConnection(Program.connstr))
            {
                int total = 0;
                int inserted = 0;
                int updated = 0;
                int deleted = 0;
                connection.Open();
                using (SqlCommand com = new SqlCommand(@"[dbo].[MergeCountry]", connection))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandTimeout = 600; // 10 minutes.
                    using (SqlDataReader rs = com.ExecuteReader())
                    {
                        rs.Read();
                        total = rs.GetInt32(0);
                        rs.NextResult();
                        rs.Read();
                        inserted = rs.GetInt32(0);
                        updated = rs.GetInt32(1);
                        deleted = rs.GetInt32(2);
                    }
                }
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText(string.Format("{0} Countries handled, {1} New, {2} Updated and {3} Deleted", total, inserted, updated, deleted), Color.DarkOrange);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
            }
        }

        private void bntTestSqlServer_Click(object sender, EventArgs e)
        {
            if (working)
                return;
            gbConnectionError.Visible = false;
            if (!cbIntegratedSecurity.Checked && (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text)))
            {
                MessageBox.Show("Login information insufficient...");
                return;
            }
            Thread t = new Thread(TestConnection);
            t.IsBackground = true;
            working = true;
            bntTestSqlServer.Enabled = false;
            t.Start();
        }

        private void TestConnection()
        {
            try
            {
                SqlConnectionStringBuilder connstrbld = new SqlConnectionStringBuilder();
                connstrbld.ConnectTimeout = 5;
                connstrbld.DataSource = txtServer.Text;
                connstrbld.IntegratedSecurity = cbIntegratedSecurity.Checked;
                if (!connstrbld.IntegratedSecurity)
                {
                    connstrbld.UserID = txtUsername.Text;
                    connstrbld.Password = txtPassword.Text;
                }
                using (SqlConnection con = new SqlConnection(connstrbld.ToString()))
                {
                    try
                    {
                        con.Open();
                        bntTestSqlServer.Invoke((MethodInvoker)delegate { bntTestSqlServer.BackColor = Color.Green; });
                        connstrbld.InitialCatalog = "MyMDB";
                        Program.connstr = connstrbld.ToString();
                        Properties.Settings.Default.SQLServer = connstrbld.DataSource;
                        Properties.Settings.Default.SQLSecurity = connstrbld.IntegratedSecurity;
                        Properties.Settings.Default.User = connstrbld.UserID;
                        Properties.Settings.Default.Pass = connstrbld.Password;
                        Properties.Settings.Default.Save();
                        TabPages(true);
                    }
                    catch (Exception err)
                    {
                        gbConnectionError.Invoke((MethodInvoker)delegate { gbConnectionError.Visible = true; });
                        txtConnectionError.Invoke((MethodInvoker)delegate { txtConnectionError.Text = err.Message; });
                        bntTestSqlServer.Invoke((MethodInvoker)delegate { bntTestSqlServer.BackColor = Color.Red; });
                        Program.connstr = null;
                        Properties.Settings.Default.SQLServer = string.Empty;
                        Properties.Settings.Default.SQLSecurity = true;
                        Properties.Settings.Default.User = string.Empty;
                        Properties.Settings.Default.Pass = string.Empty;
                        Properties.Settings.Default.Save();
                        TabPages(false);
                    }
                }
            }
            finally
            {
                working = false;
                bntTestSqlServer.Invoke((MethodInvoker)delegate { bntTestSqlServer.Enabled = true; });
            }
        }

        private bool PrepareBulkOperations()
        {
            try
            {
                NoErrors = true;
                if (working)
                    return false;
                if (Program.connstr == null)
                {
                    MessageBox.Show("Please test connection first...", "Prepare Bulk Operations", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (!DBExists("MyMDb"))
                    return false;
                try
                {
                    working = true;
                    using (SqlConnection conn = new SqlConnection(Program.connstr))
                    {
                        conn.Open();
                        using (SqlCommand com = conn.CreateCommand())
                        {
                            try
                            {
                                string[] Commands = ZipFile.DecompressTextFile(Path.Combine(Environment.CurrentDirectory, @"DB Files\Bulk\Prepare Bulk Operations.zip")).SplitRegEx(@"\bGO\b", StringSplitOptions.RemoveEmptyEntries);
                                foreach (string command in Commands)
                                    ExecuteCommand(command, com, txtProcessData);
                            }
                            catch (Exception err)
                            {
                                NoErrors = false;
                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText(err.Message, Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                            finally
                            {

                            }
                        }
                    }
                }
                catch (Exception errfiles)
                {
                    NoErrors = false;
                    txtProcessData.Invoke((MethodInvoker)delegate
                    {
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                        txtProcessData.AppendText(errfiles.Message, Color.Red);
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.ScrollToCaret();
                    });
                }
                finally
                {
                    //if (NoErrors)
                    //{
                    //    txtProcessData.Invoke((MethodInvoker)delegate
                    //    {
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //        txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    //        txtProcessData.AppendText("Database is prepared for bulk operations!, your good to go...", Color.Blue);
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //    });
                    //}
                    //else
                    //{
                    //    txtProcessData.Invoke((MethodInvoker)delegate
                    //    {
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //        txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    //        txtProcessData.AppendText("Database was not prepared correctly!", Color.Red);
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //    });
                    //}
                }
            }
            finally
            {
                working = false;
                txtProcessData.Invoke((MethodInvoker)delegate { txtProcessData.Enabled = true; });
            }
            return NoErrors;
        }

        private void CleanupBulkOperation()
        {
            try
            {
                NoErrors = true;
                if (working)
                    return;
                if (Program.connstr == null)
                {
                    MessageBox.Show("Please test connection first...", "Cleanup Bulk Operations", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (!DBExists("MyMDb"))
                    return;
                try
                {
                    working = true;
                    using (SqlConnection conn = new SqlConnection(Program.connstr))
                    {
                        conn.Open();
                        using (SqlCommand com = conn.CreateCommand())
                        {
                            try
                            {
                                string[] Commands = ZipFile.DecompressTextFile(Path.Combine(Environment.CurrentDirectory, @"DB Files\Bulk\Cleanup Bulk Operation.zip")).SplitRegEx(@"\bGO\b", StringSplitOptions.RemoveEmptyEntries);
                                foreach (string command in Commands)
                                    ExecuteCommand(command, com, txtProcessData);
                            }
                            catch (Exception err)
                            {
                                NoErrors = false;
                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText(err.Message, Color.DarkRed);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                            finally
                            {

                            }
                        }
                    }
                }
                catch (Exception errfiles)
                {
                    NoErrors = false;
                    txtProcessData.Invoke((MethodInvoker)delegate
                    {
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                        txtProcessData.AppendText(errfiles.Message, Color.DarkRed);
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.ScrollToCaret();
                    });
                }
                finally
                {
                    //if (NoErrors)
                    //{
                    //    txtProcessData.Invoke((MethodInvoker)delegate
                    //    {
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //        txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    //        txtProcessData.AppendText("Database has been cleaned!, Start using the Movie Database...", Color.Blue);
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //    });
                    //}
                    //else
                    //{
                    //    txtProcessData.Invoke((MethodInvoker)delegate
                    //    {
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //        txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    //        txtProcessData.AppendText("Database was not cleaned correctly!", Color.Red);
                    //        txtProcessData.AppendText(Environment.NewLine);
                    //    });
                    //}
                }
            }
            finally
            {
                working = false;
                txtProcessData.Invoke((MethodInvoker)delegate { txtProcessData.Enabled = true; });
            }
        }

        private void bntBuildDB_Click(object sender, EventArgs e)
        {
            if (working)
                return;
            Thread t = new Thread(BuildDB);
            t.IsBackground = true;
            working = true;
            bntBuildDB.Enabled = false;
            t.Start();
        }

        private void BuildDB()
        {
            try
            {
                if (Program.connstr == null)
                {
                    MessageBox.Show("Please test connection first...", "Build DB", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(Program.connstr);
                    scsb.InitialCatalog = "master";
                    Program.connstr = scsb.ToString();
                }
                if (DBExists("MyMDb") && DialogResult.No == MessageBox.Show("The Database already exists, Do you want to rebuild the whole DB??.", "Build DB", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    return;
                try
                {
                    working = true;
                    NoErrors = true;
                    using (SqlConnection conn = new SqlConnection(Program.connstr))
                    {
                        conn.Open();
                        using (SqlCommand com = conn.CreateCommand())
                        {
                            foreach (FileInfo fi in new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, @"DB Files\Create")).GetFiles("*.zip"))
                            {
                                FileInfo fo = null;
                                try
                                {
                                    string[] Commands = ZipFile.DecompressTextFile(fi).SplitRegEx(@"\bGO\b", StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string command in Commands)
                                        ExecuteCommand(command, com, txtBuildDB);
                                }
                                catch (Exception err)
                                {
                                    NoErrors = false;
                                    txtBuildDB.Invoke((MethodInvoker)delegate
                                    {
                                        txtBuildDB.AppendText(Environment.NewLine);
                                        txtBuildDB.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                        txtBuildDB.AppendText(err.Message, Color.DarkRed);
                                        txtBuildDB.AppendText(Environment.NewLine);
                                        txtBuildDB.ScrollToCaret();
                                    });
                                }
                                finally
                                {
                                    try
                                    {
                                        if (fo != null)
                                            fo.Delete();
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch (Exception errfiles)
                {
                    NoErrors = false;
                    txtBuildDB.Invoke((MethodInvoker)delegate
                    {
                        txtBuildDB.AppendText(Environment.NewLine);
                        txtBuildDB.AppendText(string.Format("[{0}]\t", DateTime.Now));
                        txtBuildDB.AppendText(errfiles.Message, Color.DarkRed);
                        txtBuildDB.AppendText(Environment.NewLine);
                        txtBuildDB.ScrollToCaret();
                    });
                }
                finally
                {
                    if (NoErrors)
                    {
                        txtBuildDB.Invoke((MethodInvoker)delegate
                        {
                            txtBuildDB.AppendText(Environment.NewLine);
                            txtBuildDB.AppendText(string.Format("[{0}]\t", DateTime.Now));
                            txtBuildDB.AppendText("DONE!, you're good to go...", Color.Blue);
                            txtBuildDB.AppendText(Environment.NewLine);
                        });
                    }
                    else
                    {
                        txtBuildDB.Invoke((MethodInvoker)delegate
                        {
                            txtBuildDB.AppendText(Environment.NewLine);
                            txtBuildDB.AppendText(string.Format("[{0}]\t", DateTime.Now));
                            txtBuildDB.AppendText("DONE!, Some errors did happen, don't know if you're good to go, you can try...", Color.Red);
                            txtBuildDB.AppendText(Environment.NewLine);
                        });
                    }
                }
            }
            finally
            {
                working = false;
                bntBuildDB.Invoke((MethodInvoker)delegate { bntBuildDB.Enabled = true; });
            }
        }

        private void ExecuteCommand(string value, SqlCommand com, RichTextBox outputBox)
        {
            value = RemoveNewLine(value);
            string commandname = "Unknown command...";
            string command = string.Empty;
            if (value.StartsWith("--"))
            {
                string[] arr = value.Split(Environment.NewLine.ToCharArray());
                commandname = arr[0].Substring(2);
                command = string.Join(Environment.NewLine, arr, 1, arr.Length - 1);
            }
            else
                command = value;
            outputBox.Invoke((MethodInvoker)delegate
            {
                outputBox.AppendText(string.Format("[{0}]\t", DateTime.Now));
                outputBox.AppendText(commandname, Color.DarkBlue);
                outputBox.ScrollToCaret();
            });

            try
            {
                com.CommandText = command;
                com.ExecuteNonQuery();
                outputBox.Invoke((MethodInvoker)delegate { outputBox.AppendText(" Success!", Color.Green); });
            }
            catch (Exception err)
            {
                NoErrors = false;
                outputBox.Invoke((MethodInvoker)delegate
                {
                    outputBox.AppendText(" ERROR!", Color.Red);
                    outputBox.AppendText(Environment.NewLine);
                    outputBox.AppendText(err.Message, Color.DarkRed);
                });
            }
            outputBox.Invoke((MethodInvoker)delegate
            {
                outputBox.AppendText(Environment.NewLine);
                outputBox.ScrollToCaret();
            });
        }

        private string RemoveNewLine(string value)
        { 
            if (value.StartsWith(Environment.NewLine))
                value = value.Substring(2);
            if (value.EndsWith(Environment.NewLine))
                value = value.Substring(0, value.Length - 2);

            if (value.StartsWith(Environment.NewLine) || value.EndsWith(Environment.NewLine))
                return RemoveNewLine(value);
            return value;
        }

		private bool DBExists(string dbname)
		{
			if (string.IsNullOrEmpty(Program.connstr))
				return false;
			bool dbExists = true;
			try
			{
				using (SqlConnection conn = new SqlConnection(Program.connstr))
				{
					conn.Open();
					using (SqlCommand com = new SqlCommand("SELECT name FROM sys.databases WHERE name = @dbname", conn))
					{
						com.Parameters.AddWithValue("@dbname", dbname);
						using (SqlDataReader rs = com.ExecuteReader())
						{
							rs.Read();
							dbExists = rs.HasRows && !rs.IsDBNull(0);
						}
					}
				}
			}
			finally
			{

			}
			return dbExists;
		}

        private long RowCount(string objName)
        {
            if (string.IsNullOrEmpty(Program.connstr))
                return 0;
            long rtn = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(Program.connstr))
                {
                    conn.Open();
                    using (SqlCommand com = new SqlCommand("SELECT SUM(CASE WHEN (index_id < 2) THEN row_count ELSE 0 END) AS [Rows] FROM sys.dm_db_partition_stats WHERE object_id = OBJECT_ID(@objname)", conn))
                    {
                        com.Parameters.AddWithValue("@objname", objName);
                        using (SqlDataReader rs = com.ExecuteReader())
                        {
                            rs.Read();
                            rtn = rs.IsDBNull(0) ? 0L : rs.GetInt64(0);
                        }
                    }
                }
            }
            finally
            {

            }
            return rtn;
        }

        private void txtBrowseLocalFolder_TextChanged(object sender, EventArgs e)
        {
            LocalFolder(txtBrowseLocalFolder.Text);
        }

        private void LocalFolder(string path)
        {
            if (Directory.Exists(path))
            { 
                cbFtp_Mov.Enabled = bntDownloadData.Enabled && RowCount("dbo.MediaEntry") > 0; //Must download if no media
                cbFtp_Gen.Enabled = bntDownloadData.Enabled;
                cbFtp_Plo.Enabled = bntDownloadData.Enabled;
                cbFtp_Tag.Enabled = bntDownloadData.Enabled;
                cbFtp_Cou.Enabled = bntDownloadData.Enabled;
                //cbFtp_Aka.Enabled = bntDownloadData.Enabled;
                //cbFtp_Act.Enabled = bntDownloadData.Enabled;
                //cbFtp_Acs.Enabled = bntDownloadData.Enabled;
				checkBoxDistributors.Enabled = bntDownloadData.Enabled;
                FreeSpaceOK();
            }
        }

        private void bntBrowseLocalFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select an download folder";
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (!string.IsNullOrEmpty(txtBrowseLocalFolder.Text) & Directory.Exists(txtBrowseLocalFolder.Text))
                fbd.SelectedPath = txtBrowseLocalFolder.Text;
            fbd.ShowNewFolderButton = true;
            if (DialogResult.OK == fbd.ShowDialog() && Directory.Exists(fbd.SelectedPath))
            {
                txtBrowseLocalFolder.Text = fbd.SelectedPath;
                Properties.Settings.Default.WorkingDirectory = fbd.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }
        
        private void bntDownloadData_Click(object sender, EventArgs e)
        {
            RunFTP = true;
            if (bntDownloadData.Text == "Download Data")
                bntDownloadData.Text = "Cancel Download!";
            else
            {
                bntDownloadData.Enabled = false;
                RunFTP = false;
                bntDownloadData.Text = "Download Data";
            }
            if (DlT != null)
            {
                DlT.Abort();
                DlT.Join();
                bntDownloadData.Enabled = true;
            }

            DlT = new Thread(DownloadFTPData);
            DlT.IsBackground = true;
            DlT.Start();
        }

        private void DownloadFTPData()
        {
            try
            {
                if (cbFtp_Mov.Checked)
                    FRPDownload("movies.list.gz", ftp_mov);
                if (cbFtp_Gen.Checked)
                    FRPDownload( "genres.list.gz", ftp_gen);
                if (cbFtp_Plo.Checked)
                    FRPDownload("plot.list.gz", ftp_plo);
                if (cbFtp_Tag.Checked)
                    FRPDownload("taglines.list.gz", ftp_tag);
                if (cbFtp_Cou.Checked)
                    FRPDownload( "countries.list.gz", ftp_cou);
                if (cbFtp_Aka.Checked)
                    FRPDownload("aka-titles.list.gz", ftp_aka);
                if (cbFtp_Act.Checked)
                    FRPDownload("actors.list.gz", ftp_act);
                if (cbFtp_Acs.Checked)
                    FRPDownload("actresses.list.gz", ftp_acs);
				if (checkBoxDistributors.Checked)
				{
					FRPDownload("distributors.list.gz", ftp_distributors);
				}
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FRPDownload(string filename, CopyToHandle handle)
        {
            try
            {
                if (!RunFTP)
                    return;
                string gzfile = Path.Combine(txtBrowseLocalFolder.Text, filename);
                using (Stream s = ftpClient.OpenRead(ftpServerPath + filename))
                {
                    using (FileStream fs = new FileStream(gzfile, FileMode.Create))
                    {
                        s.CopyTo(fs, handle);
                        fs.Close();
                        s.Close();
                    }
                }
                if(RunFTP)
                    DeComp(gzfile, handle);
            }
            catch { }
        }

        private void ftp_mov(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Mov.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Mov, sender, e);
            });
            pb_Mov.Invoke((MethodInvoker)delegate
            {
                pb_Mov.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }
        private void ftp_gen(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Gen.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Gen, sender, e);
            });
            pb_Gen.Invoke((MethodInvoker)delegate
            {
                pb_Gen.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }
        private void ftp_plo(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Plo.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Plo, sender, e);
            });
            pb_Plo.Invoke((MethodInvoker)delegate
            {
                pb_Plo.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }
        private void ftp_tag(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Tag.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Tag, sender, e);
            });
            pb_Tag.Invoke((MethodInvoker)delegate
            {
                pb_Tag.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }
        private void ftp_cou(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Cou.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Cou, sender, e);
            });
            pb_Cou.Invoke((MethodInvoker)delegate
            {
                pb_Cou.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }
        private void ftp_aka(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Aka.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Aka, sender, e);
            });
            pb_Aka.Invoke((MethodInvoker)delegate
            {
                pb_Aka.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }
        private void ftp_act(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Act.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Act, sender, e);
            });
            pb_Act.Invoke((MethodInvoker)delegate
            {
                pb_Act.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }
        private void ftp_acs(object sender, CopyToArgs e)
        {
            lblFtpDLSizeSpeed_Acs.Invoke((MethodInvoker)delegate
            {
                Setlabel(lblFtpDLSizeSpeed_Acs, sender, e);
            });
            pb_Acs.Invoke((MethodInvoker)delegate
            {
                pb_Acs.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
            });
        }

		private void ftp_distributors(object sender, CopyToArgs e)
		{
			labelFtpDlSizeDistributors.Invoke((MethodInvoker)delegate
			{
				Setlabel(labelFtpDlSizeDistributors, sender, e);
			});
			progressBarDistributors.Invoke((MethodInvoker)delegate
			{
				progressBarDistributors.Value = Math.Max(0, Math.Min(100, (int)(((double)e.CurrentPosition / (double)e.TotalLength) * 100)));
			});
		}

        private void Setlabel(Label lbl, object sender, CopyToArgs e)
        {
            if (e.TotalLength == e.CurrentPosition)
                lbl.Text = "Done";
            else
                lbl.Text = string.Format("{0} {1} of {2} | {3}/s", sender == null ? "Downloading:" : "DeCompressing:", e.CurrentPosition.ToFileSize(0), e.TotalLength.ToFileSize(0), e.LengthPrSecond.ToFileSize());
        }

        private void DeComp(string gzfile, CopyToHandle handle)
        {
            Thread t = new Thread((ThreadStart)delegate { DoDeComp(gzfile, handle);});
            t.IsBackground = true;
            t.Start();
        }

        private void DoDeComp(string gzfile, CopyToHandle handle)
        {
            ZipFile.DecompressGz(gzfile, Path.Combine(Path.GetDirectoryName(gzfile), Path.GetFileNameWithoutExtension(gzfile)), handle);
            if (cbRemCompress.Checked)
                File.Delete(gzfile);
        }

        private void cbIMDBInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIMDBInterfaces.SelectedText)
            {
                case "Sweden":
                    ftpServer = "ftp.sunet.se";
                    ftpServerPath = "pub/tv+movies/imdb/";
                    break;
                case "Finland":
                    ftpServer = "ftp.funet.fi";
                    ftpServerPath = "pub/mirrors/ftp.imdb.com/pub/";
                    break;
                case "Germany":
                    ftpServer = "ftp.fu-berlin.de";
                    ftpServerPath = "pub/misc/movies/database/";
                    break;
                default: 
                    ftpServer = "ftp.sunet.se";
                    ftpServerPath = "pub/tv+movies/imdb/";
                    break;
            }
            SetFTPServer();
        }

        private void SetFTPServer()
        {
            ftpClient.Host = ftpServer;
            ftpClient.Credentials = new System.Net.NetworkCredential();
            ftpClient.Connect();
            lblFtpDLSizeSpeed_Mov.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "movies.list.gz").ToFileSize(0));
            lblFtpDLSizeSpeed_Gen.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "genres.list.gz").ToFileSize(0));
            lblFtpDLSizeSpeed_Plo.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "plot.list.gz").ToFileSize(0));
            lblFtpDLSizeSpeed_Tag.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "taglines.list.gz").ToFileSize(0));
            lblFtpDLSizeSpeed_Cou.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "countries.list.gz").ToFileSize(0));
            lblFtpDLSizeSpeed_Aka.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "aka-titles.list.gz").ToFileSize(0));
            lblFtpDLSizeSpeed_Act.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "actors.list.gz").ToFileSize(0));
            lblFtpDLSizeSpeed_Acs.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "actresses.list.gz").ToFileSize(0));
			labelFtpDlSizeDistributors.Text = string.Format("0 Mb of {0}", ftpClient.GetFileSize(ftpServerPath + "distributors.list.gz").ToFileSize(0));
        }

        private void FreeSpaceOK()
        {
            if (!Directory.Exists(txtBrowseLocalFolder.Text))
                return;
            long TotalToDownload = 0;
            ftpClient.Host = ftpServer;
            ftpClient.Credentials = new System.Net.NetworkCredential();
            ftpClient.Connect();
            if(cbFtp_Mov.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "movies.list.gz");
            if (cbFtp_Gen.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "genres.list.gz");
            if (cbFtp_Plo.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "plot.list.gz");
            if (cbFtp_Tag.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "taglines.list.gz");
            if (cbFtp_Cou.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "countries.list.gz");
            if (cbFtp_Aka.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "aka-titles.list.gz");
            if (cbFtp_Act.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "actors.list.gz");
            if (cbFtp_Acs.Checked)
                TotalToDownload += ftpClient.GetFileSize(ftpServerPath + "actresses.list.gz");
            DriveInfo Drive = new DriveInfo(Directory.GetDirectoryRoot(txtBrowseLocalFolder.Text));
            if (Drive.AvailableFreeSpace > (TotalToDownload * 5.2)) //Estimated compression of 1 to 4.2 plus the file itself...
                bntDownloadData.Enabled = true;
            else
            {
                bntDownloadData.Enabled = false;
                MessageBox.Show(string.Format(@"Please select a directory on a partition with aleast {2} free space.
Current Drive: {0}
Drive Available Free Space: {1}", Drive.Name, Drive.AvailableFreeSpace.ToFileSize(), (TotalToDownload * 5).ToFileSize()), "Insufficient free space", MessageBoxButtons.OK);
            }
            //return Drive.AvailableFreeSpace > (TotalToDownload * 5);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                LocalFolder(txtBrowseLocalFolder.Text);
            }
            if (tabControl1.SelectedIndex == 3)
            {
                chklbSyncFiles.Items.Clear();
                if (!Directory.Exists(txtBrowseLocalFolder.Text))
                    return;
                foreach (FileInfo fi in new DirectoryInfo(txtBrowseLocalFolder.Text).GetFiles("*.list"))
                {
                    chklbSyncFiles.Items.Add(Path.GetFileNameWithoutExtension(fi.FullName));
                    chklbSyncFiles.SetItemChecked(chklbSyncFiles.Items.Count - 1, true);
                }
            }
        }

        private void cbFtp_XXX_CheckedChanged(object sender, EventArgs e)
        {
            FreeSpaceOK();
        }

        private void BulkData()
        {
            try
            {
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText("Prepare SQL Server for Operations...", Color.Blue);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
                if (PrepareBulkOperations()) //Prepare the database for data bulk operations...
                {
                    //txtProcessData.Invoke((MethodInvoker)delegate
                    //{
                    //    txtProcessData.AppendText(" Success!!", Color.Blue);
                    //    txtProcessData.AppendText(Environment.NewLine);
                    //    txtProcessData.ScrollToCaret();
                    //});
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { this.UseWaitCursor = false; });
                        if (chklbSyncFiles.CheckedItems.Contains("movies"))
                        {
                            txtProcessData.Invoke((MethodInvoker)delegate
                            {
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                txtProcessData.AppendText("Movies are being processed, please wait...", Color.DarkGreen);
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.ScrollToCaret();
                            });
                            try
                            {
                                mlf = new MovieListFile(Path.Combine(txtBrowseLocalFolder.Text, "movies.list"));
                                mlf.ReadingFile += ProcessFTPData_ReadingFile;
                                BulkUploadMovies();

                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Movies where successfully synchronized", Color.Blue);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                            catch (Exception err)
                            {
                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Movies failed to be synchronized!, please see the following Error message...", Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(err.Message, Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                        }
                        this.Invoke((MethodInvoker)delegate { this.UseWaitCursor = false; });
                        if (chklbSyncFiles.CheckedItems.Contains("taglines"))
                        {
                            txtProcessData.Invoke((MethodInvoker)delegate
                            {
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                txtProcessData.AppendText("Tag Lines are being processed, please wait...", Color.DarkGreen);
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.ScrollToCaret();
                            });
                            try
                            {
                                tlf = new TagLineListFile(Path.Combine(txtBrowseLocalFolder.Text, "taglines.list"));
                                tlf.ReadingFile += ProcessFTPData_ReadingFile;
                                BulkUploadTaglines();

                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Tag Lines where successfully synchronized", Color.Blue);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                            catch (Exception err)
                            {
                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Tag Lines failed to be synchronized!, please see the following Error message...", Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(err.Message, Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                        }
                        this.Invoke((MethodInvoker)delegate { this.UseWaitCursor = false; });
                        if (chklbSyncFiles.CheckedItems.Contains("plot"))
                        {
                            txtProcessData.Invoke((MethodInvoker)delegate
                            {
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                txtProcessData.AppendText("Plots are being processed, please wait...", Color.DarkGreen);
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.ScrollToCaret();
                            });
                            try
                            {
                                plf = new PlotListFile(Path.Combine(txtBrowseLocalFolder.Text, "plot.list"));
                                plf.ReadingFile += ProcessFTPData_ReadingFile;
                                BulkUploadPlots();

                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Plots where successfully synchronized", Color.Blue);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                            catch (Exception err)
                            {
                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Plots failed to be synchronized!, please see the following Error message...", Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(err.Message, Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                        }
                        this.Invoke((MethodInvoker)delegate { this.UseWaitCursor = false; });
                        if (chklbSyncFiles.CheckedItems.Contains("genres"))
                        {
                            txtProcessData.Invoke((MethodInvoker)delegate
                            {
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                txtProcessData.AppendText("Genres are being processed, please wait...", Color.DarkGreen);
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.ScrollToCaret();
                            });
                            try
                            {
                                glf = new GenreListFile(Path.Combine(txtBrowseLocalFolder.Text, "genres.list"), GetGenresTable());
                                glf.ReadingFile += ProcessFTPData_ReadingFile;
                                BulkUploadGenres();

                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Genres where successfully synchronized", Color.Blue);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                            catch (Exception err)
                            {
                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Genres failed to be synchronized!, please see the following Error message...", Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(err.Message, Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                        }
                        this.Invoke((MethodInvoker)delegate { this.UseWaitCursor = false; });
                        if (chklbSyncFiles.CheckedItems.Contains("countries"))
                        {
                            txtProcessData.Invoke((MethodInvoker)delegate
                            {
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                txtProcessData.AppendText("Countries are being processed, please wait...", Color.DarkGreen);
                                txtProcessData.AppendText(Environment.NewLine);
                                txtProcessData.ScrollToCaret();
                            });
                            try
                            {
                                clf = new CountryListFile(Path.Combine(txtBrowseLocalFolder.Text, "countries.list"), GetISO3166Table());
                                clf.ReadingFile += ProcessFTPData_ReadingFile;
                                BulkUploadCountry();

                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Countries where successfully synchronized", Color.Blue);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                            catch (Exception err)
                            {
                                txtProcessData.Invoke((MethodInvoker)delegate
                                {
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                                    txtProcessData.AppendText("Countries failed to be synchronized!, please see the following Error message...", Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.AppendText(err.Message, Color.Red);
                                    txtProcessData.AppendText(Environment.NewLine);
                                    txtProcessData.ScrollToCaret();
                                });
                            }
                        }
                    }
                    finally
                    {

                    }
                }
            }
            catch (Exception err)
            {
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText("Prepare SQL Server Failed, please see the following Error message...", Color.Red);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(err.Message, Color.Red);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
            }
            finally
            {
                try
                {
                    txtProcessData.Invoke((MethodInvoker)delegate
                    {
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                        txtProcessData.AppendText("Cleaning SQL Server after Operations...", Color.Blue);
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.ScrollToCaret();
                    });
                    CleanupBulkOperation();

                }
                catch (Exception err)
                {
                    txtProcessData.Invoke((MethodInvoker)delegate
                    {
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.AppendText("Cleaning SQL Server Failed, please see the following Error message...", Color.Red);
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.AppendText(err.Message, Color.Red);
                        txtProcessData.AppendText(Environment.NewLine);
                        txtProcessData.ScrollToCaret();
                    });
                }
                finally
                {
                    this.Invoke((MethodInvoker)delegate { this.UseWaitCursor = false; });
                }
            }
        }

        private void bntBullk_Click(object sender, EventArgs e)
        {
            BT = new Thread(BulkData);
            BT.IsBackground = true;
            BT.Start();
        }

        void ProcessFTPData_ReadingFile(object sender, ProgressEventArgs e)
        {
            int cp = (int)Math.Min(Math.Round(((double)e.StreamPosistion / (double)e.StreamLength) * 100D, 3), 100D);
            pbProcessFTPData.Invoke((MethodInvoker)delegate { pbProcessFTPData.Value = cp; });
            lblProcessFTPData.Invoke((MethodInvoker)delegate { lblProcessFTPData.Text = string.Format("{1}/sec [{0}]", e.StreamPosistion.ToFileSize(), ((long)(e.StreamPosistion / (DateTime.Now - starttime).TotalSeconds)).ToFileSize()); });
            if (e.StreamLength == e.StreamPosistion)
            {
                txtProcessData.Invoke((MethodInvoker)delegate
                {
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText("Data has been send to the server...", Color.DarkCyan);
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.AppendText(string.Format("[{0}]\t", DateTime.Now));
                    txtProcessData.AppendText("Commencing Synchronization, please wait on the SQL Server to finish...", Color.DarkCyan);
                    this.Invoke((MethodInvoker)delegate { this.UseWaitCursor = true; });
                    txtProcessData.AppendText(Environment.NewLine);
                    txtProcessData.ScrollToCaret();
                });
            }
        }
    }
}
