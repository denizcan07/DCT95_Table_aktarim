using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class DbSettings : Form
    {
        private BackgroundWorker backgroundWorker2;
        public DbSettings()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }
        private void InitializeBackgroundWorker()
        {
            backgroundWorker2 = new BackgroundWorker();
            backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }
        public static string staticKaynakServerName { get; set; }
        public static string staticHedefServerName { get; set; }
        public static string staticKaynakUserName { get; set; }
        public static string staticHedefUserName { get; set; }
        public static string staticKaynakPassword { get; set; }
        public static string staticHedefPassword { get; set; }
        public bool isTransfer { get; set; } = true;
        public DataTable tableNamesDataTable = new DataTable();
        //public DataTable SourcetableNamesDataTable = new DataTable();

        private void NewMainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }



        private void NewMainFrm_Load(object sender, EventArgs e)
        {

        }


        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (txtKaynakServerName.Text.ToString().Trim() == "" || txtKaynakServerName.Text.ToString().Trim() == "" || txtKaynakServerName.Text.ToString().Trim() == "")
            {
                return;
            }
            if (btnKaynakConnection.Text == "Disconnection")
            {
                btnKaynakConnection.Text = "Connection";
                kaynakDataBaseBindingSource.DataSource = null;
                kaynakTablesBindingSource.DataSource = null;
                txtKaynakUserName.ReadOnly = false;
                txtKaynakServerName.ReadOnly = false;
                txtKaynakPassword.ReadOnly = false;
                btnAktar.Enabled = false;
                btnKaynakTableExcelAl.Enabled = false;
                return;
            }
            try
            {
                string serverName = txtKaynakServerName.Text.ToString().Trim();
                string userName = txtKaynakUserName.Text.ToString().Trim();
                string password = txtKaynakPassword.Text.ToString().Trim();
                using (SqlConnection connection = new SqlConnection($"Data Source={serverName};Initial Catalog=master;User ID={userName};Password={password};"))
                {
                    connection.Open();
                    staticKaynakServerName = serverName;
                    staticKaynakUserName = userName;
                    staticKaynakPassword = password;
                    btnKaynakConnection.ForeColor = Color.Green;
                    if (btnKaynakConnection.Text == "Connection")
                    {
                        btnKaynakConnection.Text = "Disconnection";
                        txtKaynakUserName.ReadOnly = true;
                        txtKaynakServerName.ReadOnly = true;
                        txtKaynakPassword.ReadOnly = true;
                        btnAktar.Enabled = true;
                        btnKaynakTableExcelAl.Enabled = true;
                    }


                    SqlCommand command = new SqlCommand("SELECT name FROM sys.databases where name not in ('master','tempdb','model','msdb') order by name asc", connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dbNamesDataTable = new DataTable();
                        dbNamesDataTable.Load(reader);
                        kaynakDataBaseBindingSource.DataSource = dbNamesDataTable;
                    };
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Bağlantısı Kurulamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void gridViewDataBaseList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                var oRow = gridViewKaynakDataBaseList.GetFocusedDataRow();
                if (oRow != null)
                {
                    string dbName = Convert.ToString(oRow["name"]).Trim();
                    using (SqlConnection connection = new SqlConnection($"Data Source={staticKaynakServerName};Initial Catalog={dbName};User ID={staticKaynakUserName};Password={staticKaynakPassword};"))
                    {
                        connection.Open();



                        SqlCommand command = new SqlCommand(@"SELECT 
                                                                t.name AS table_name,
                                                                SUM(p.row_count) AS [Count] , 0 as TransferState 
                                                            FROM 
                                                                sys.tables t
                                                            INNER JOIN 
                                                                sys.schemas s ON t.schema_id = s.schema_id
                                                            LEFT JOIN 
                                                                sys.dm_db_partition_stats p ON t.object_id = p.object_id AND p.index_id IN (0, 1)
                                                            WHERE 
                                                                (p.index_id < 2 OR p.index_id IS NULL) 
                                                            AND is_filetable = 0
                                                            AND is_external = 0
                                                            AND is_ms_shipped = 0
                                                            AND temporal_type = 0
                                                            GROUP BY 
                                                                t.name
                                                            ORDER BY 
                                                                t.name;", connection);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable tableNamesDataTable = new DataTable();
                            tableNamesDataTable.Load(reader);
                            //SourcetableNamesDataTable = tableNamesDataTable;
                            kaynakTablesBindingSource.DataSource = tableNamesDataTable;
                        };
                        connection.Close();
                    }


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        private void gridViewTableList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }

        private void btnHedefConnection_Click(object sender, EventArgs e)
        {
            getHedefDataBase();
        }

        private void gridViewHedefDataBaseList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                var oRow = gridViewHedefDataBaseList.GetFocusedDataRow();
                if (oRow != null)
                {
                    string dbName = Convert.ToString(oRow["name"]).Trim();

                    getHedefTables(dbName);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        public void getHedefDataBase()
        {
            if (txtHedefServerName.Text.ToString().Trim() == "" || txtHedefServerName.Text.ToString().Trim() == "" || txtHedefServerName.Text.ToString().Trim() == "")
            {
                return;
            }
            if (btnHedefConnection.Text == "Disconnection")
            {
                btnHedefConnection.Text = "Connection";
                hedefDataBaseBindingSource.DataSource = null;
                hedefTablesBindingSource.DataSource = null;
                txtHedefUserName.ReadOnly = false;
                txtHedefServerName.ReadOnly = false;
                txtHedefPassword.ReadOnly = false;
                btnHedefTableExceleAl.Enabled = false;
                simpleButton2.Enabled = false;
                btbnHedefTableRowSil.Enabled = false;
                return;
            }
            try
            {
                string HedefserverName = txtHedefServerName.Text.ToString().Trim();
                string HedefuserName = txtHedefUserName.Text.ToString().Trim();
                string Hedefpassword = txtHedefPassword.Text.ToString().Trim();
                using (SqlConnection connection = new SqlConnection($"Data Source={HedefserverName};Initial Catalog=master;User ID={HedefuserName};Password={Hedefpassword};"))
                {
                    connection.Open();
                    staticHedefServerName = HedefserverName;
                    staticHedefUserName = HedefuserName;
                    staticHedefPassword = Hedefpassword;
                    btnHedefConnection.ForeColor = Color.Green;
                    if (btnHedefConnection.Text == "Connection")
                    {
                        btnHedefConnection.Text = "Disconnection";
                        txtHedefUserName.ReadOnly = true;
                        txtHedefServerName.ReadOnly = true;
                        txtHedefPassword.ReadOnly = true;
                        btnHedefTableExceleAl.Enabled = true;
                        btbnHedefTableRowSil.Enabled = true;
                        simpleButton2.Enabled = true;
                    }


                    SqlCommand command = new SqlCommand("SELECT name FROM sys.databases where name not in ('master','tempdb','model','msdb') order by name asc", connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dbNamesDataTable = new DataTable();
                        dbNamesDataTable.Load(reader);
                        hedefDataBaseBindingSource.DataSource = dbNamesDataTable;
                    };
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Bağlantısı Kurulamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void getHedefTables(string dbName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection($"Data Source={staticHedefServerName};Initial Catalog={dbName};User ID={staticHedefUserName};Password={staticHedefPassword};"))
                {
                    connection.Open();



                    SqlCommand command = new SqlCommand(@"SELECT 
                                                                t.name AS table_name,
                                                                SUM(p.row_count) AS [Count]
                                                            FROM 
                                                                sys.tables t
                                                            INNER JOIN 
                                                                sys.schemas s ON t.schema_id = s.schema_id
                                                            LEFT JOIN 
                                                                sys.dm_db_partition_stats p ON t.object_id = p.object_id AND p.index_id IN (0, 1)
                                                            WHERE 
                                                                (p.index_id < 2 OR p.index_id IS NULL)
                                                            GROUP BY 
                                                                t.name
                                                            ORDER BY 
                                                                t.name;", connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        tableNamesDataTable = new DataTable();
                        tableNamesDataTable.Load(reader);
                        hedefTablesBindingSource.DataSource = tableNamesDataTable;
                    };
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAktar_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                // Arka plandaki işlemi başlat
                backgroundWorker1.RunWorkerAsync();
            }


        }

        public void DoTransfer(string oRowTable)
        {
            try
            {
                string conn = "";
                var oRow = gridViewKaynakDataBaseList.GetFocusedDataRow();
                if (oRow != null)
                {
                    string dbName = Convert.ToString(oRow["name"]).Trim();
                    conn = $"Data Source={staticKaynakServerName};Initial Catalog={dbName};User ID={staticKaynakUserName};Password={staticKaynakPassword};";
                    string HedefdbName = "";
                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        connection.Open();


                        //var oRowTable = gridViewKaynakTableList.GetFocusedDataRow();
                        if (oRowTable != null)
                        {
                            string tblName = Convert.ToString(oRowTable).Trim();
                            string query = $@"
                                        SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
                                        FROM INFORMATION_SCHEMA.COLUMNS
                                        WHERE TABLE_NAME = '{tblName}' AND TABLE_SCHEMA = 'dbo'";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                string connHedef = "";
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    var oRowDb = gridViewHedefDataBaseList.GetFocusedDataRow();
                                    HedefdbName = Convert.ToString(oRowDb["name"]).Trim();
                                    connHedef = $"Data Source={staticHedefServerName};Initial Catalog={HedefdbName};User ID={staticHedefUserName};Password={staticHedefPassword};";
                                    var countRowControl = tableNamesDataTable.Select("table_name='" + oRowTable + "'");
                                    bool truncateState = true;
                                    if (Convert.ToInt32(countRowControl[0]["Count"]) > 0)
                                    {
                                        truncateState = DropTableIfExists(connHedef, tblName);
                                    }
                                    if (!truncateState)
                                    {
                                        isTransfer = truncateState;
                                        return;
                                    }
                                    //CreateTableInNewDatabase(reader, connHedef, tblName);
                                    reader.Close();
                                    DataColumn[] columnNames = GetColumnNamesAndTypes(connHedef, tblName);
                                    if (columnNames != null && columnNames.Length > 0)
                                    {
                                        using (SqlCommand selectCommand = new SqlCommand($"SELECT * FROM {tblName} With(nolock)", connection))
                                        {
                                            using (SqlDataReader readers = selectCommand.ExecuteReader())
                                            {
                                                using (SqlConnection newConnection = new SqlConnection(connHedef))
                                                {
                                                    newConnection.Open();
                                                    SqlTransaction transaction = newConnection.BeginTransaction();
                                                    try
                                                    {
                                                        DataTable dataTable = new DataTable();
                                                        foreach (DataColumn columnName in columnNames)
                                                        {
                                                            dataTable.Columns.Add(columnName.ColumnName, columnName.DataType);
                                                        }
                                                        int chunkSize = 50000;
                                                        List<DataTable> dataTableList = new List<DataTable>();
                                                        foreach (var columnName in columnNames)
                                                        {
                                                            while (readers.Read())
                                                            {
                                                                DataRow row = dataTable.NewRow();

                                                                for (int i = 0; i < columnNames.Length; i++)
                                                                {
                                                                    object columnValue = readers.GetValue(i);
                                                                    row[columnNames[i].ColumnName] = (columnValue == null) ? DBNull.Value : columnValue;
                                                                }

                                                                dataTable.Rows.Add(row);
                                                                if (dataTable.Rows.Count % chunkSize == 0 || readers.IsClosed)
                                                                {

                                                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(newConnection, SqlBulkCopyOptions.KeepIdentity, transaction))
                                                                    {

                                                                        bulkCopy.DestinationTableName = tblName;
                                                                        try
                                                                        {

                                                                            DataTable chunkDataTable = dataTable.Copy();

                                                                            bulkCopy.WriteToServer(chunkDataTable);

                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            isTransfer = false;
                                                                            transaction.Rollback();
                                                                            MessageBox.Show("Hata: " + ex.Message);
                                                                        }
                                                                    }
                                                                    GC.Collect();
                                                                    GC.WaitForPendingFinalizers();
                                                                    dataTable = new DataTable(); // Yeni bir DataTable oluştur
                                                                    foreach (DataColumn columnNamess in columnNames)
                                                                    {
                                                                        dataTable.Columns.Add(columnNamess.ColumnName, columnNamess.DataType);
                                                                    }
                                                                }
                                                                #region manuelInsert
                                                                //using (SqlCommand insertCommand = new SqlCommand(GenerateInsertQuery(tblName, columnNames), newConnection,transaction))
                                                                //{
                                                                //    // İkinci döngü: Her bir satırdaki sütun değerlerini oku
                                                                //    for (int i = 0; i < columnNames.Length; i++)
                                                                //    {
                                                                //        object columnValue = readers.GetValue(i);
                                                                //        if (columnValue == null)
                                                                //        {
                                                                //            // Eğer null ise uygun bir değer ekleyebilirsiniz, örneğin DBNull.Value
                                                                //            insertCommand.Parameters.Add(new SqlParameter($"@{columnNames[i]}", DBNull.Value));
                                                                //        }
                                                                //        else
                                                                //        {
                                                                //            if (readers.GetFieldType(i) == typeof(byte[]))
                                                                //            {
                                                                //                SqlParameter parameter = new SqlParameter($"@{columnNames[i]}", SqlDbType.Image) { Value = columnValue };
                                                                //                insertCommand.Parameters.Add(parameter);
                                                                //            }
                                                                //            else
                                                                //            {
                                                                //                insertCommand.Parameters.Add(new SqlParameter($"@{columnNames[i]}", columnValue));
                                                                //            }
                                                                //        }
                                                                //    }
                                                                //    insertCommand.ExecuteNonQuery();
                                                                //}
                                                                #endregion
                                                            }

                                                        }
                                                        if (dataTable.Rows.Count > 0)
                                                        {
                                                            dataTableList.Add(dataTable);
                                                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(newConnection, SqlBulkCopyOptions.KeepIdentity, transaction))
                                                            {

                                                                bulkCopy.DestinationTableName = tblName;
                                                                //bulkCopy.BatchSize = 100000;
                                                                try
                                                                {
                                                                    DataTable chunkDataTable = dataTable.Copy();
                                                                    bulkCopy.WriteToServer(chunkDataTable);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    isTransfer = false;
                                                                    transaction.Rollback();
                                                                    MessageBox.Show("Hata: " + ex.Message);
                                                                }
                                                            }
                                                            GC.Collect();
                                                            GC.WaitForPendingFinalizers();
                                                        }
                                                        transaction.Commit();
                                                        #region oldBulk
                                                        readers.Close();
                                                        //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(newConnection, SqlBulkCopyOptions.Default, transaction))
                                                        //{

                                                        //    bulkCopy.DestinationTableName = tblName;
                                                        //    //bulkCopy.BatchSize = 100000;
                                                        //    try
                                                        //    {
                                                        //        foreach (var item in dataTableList)
                                                        //        {
                                                        //            DataTable chunkDataTable = item.Copy();
                                                        //            bulkCopy.WriteToServer(chunkDataTable);
                                                        //            //GC.Collect();
                                                        //            //GC.WaitForPendingFinalizers();
                                                        //        }
                                                        //        transaction.Commit();

                                                        //    }
                                                        //    catch (Exception ex)
                                                        //    {
                                                        //        isTransfer = false;
                                                        //        transaction.Rollback();
                                                        //        MessageBox.Show("Hata: " + ex.Message);
                                                        //    }
                                                        //}
                                                        #endregion
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        try
                                                        {
                                                            isTransfer = false;
                                                            transaction.Rollback();
                                                            MessageBox.Show(ex.ToString());
                                                        }
                                                        catch (Exception rollbackEx)
                                                        {
                                                            MessageBox.Show("Rollback hatası: " + rollbackEx.Message);
                                                        }
                                                    }

                                                }
                                            }

                                        }



                                    }
                                    else
                                    {
                                        isTransfer = false;
                                        MessageBox.Show("Hedefte Sütun bilgileri alınamadı.");
                                    }
                                }

                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                isTransfer = false;
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public bool DropTableIfExists(string newConnectionString, string tableName)
        {
            using (SqlConnection newConnection = new SqlConnection(newConnectionString))
            {
                newConnection.Open();
                SqlTransaction transaction = newConnection.BeginTransaction();
                try
                {
                    string checkTableQuery = $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL TRUNCATE TABLE {tableName}";
                    int i = 0;
                    using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, newConnection, transaction))
                    {
                        i = checkTableCommand.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    return true;

                }
                catch (Exception ex)
                {
                    try
                    {

                        transaction.Rollback();
                        MessageBox.Show(ex.Message.ToString());
                        return false;
                    }
                    catch (Exception rollbackEx)
                    {
                        MessageBox.Show("Rollback hatası: " + rollbackEx.Message);
                        return false;
                    }
                }
                // Tablo var mı diye kontrol et

            }
        }
        static void CreateTableInNewDatabase(SqlDataReader reader, string conn, string tableName)
        {
            // Yeni veritabanı bağlantı bilgileri
            using (SqlConnection newConnection = new SqlConnection(conn))
            {
                newConnection.Open();

                // Tablo oluşturma sorgusu
                string createTableQuery = $"CREATE TABLE {tableName} (";

                while (reader.Read())
                {
                    string columnName = reader["COLUMN_NAME"].ToString();
                    string dataType = reader["DATA_TYPE"].ToString();
                    string maxLength = reader["CHARACTER_MAXIMUM_LENGTH"].ToString();
                    string isNullable = reader["IS_NULLABLE"].ToString();

                    // Sütun özelliklerini kullanarak CREATE TABLE sorgusunu oluştur
                    createTableQuery += $"[{columnName}]";
                    switch (dataType.ToLower())
                    {
                        case "varchar":
                        case "nvarchar":
                            createTableQuery += " " + $"{dataType}({(((string.IsNullOrEmpty(maxLength)) || (maxLength == "-1")) ? "max" : maxLength)})";
                            break;
                        case "varbinary":
                            createTableQuery += " " + $"{dataType}({(((string.IsNullOrEmpty(maxLength)) || (maxLength == "-1")) ? "max" : maxLength)})";
                            break;
                        //case "ntext":
                        //    createTableQuery += " " + $" {dataType}({(((string.IsNullOrEmpty(maxLength)) || (maxLength == "-1")) ? "max" : maxLength)})";
                        //    break;
                        case "bit":
                            createTableQuery += " " + $" {dataType}";
                            break;
                        case "text":
                            createTableQuery += " " + $" {dataType}";
                            break;
                        case "time":
                            createTableQuery += " " + $" {dataType}{(string.IsNullOrEmpty(maxLength) ? "(7)" : maxLength)}";
                            break;
                        case "float":
                        case "double":
                            createTableQuery += " " + $" {dataType}";
                            break;
                        case "image":
                            createTableQuery += " " + $" {dataType}";
                            break;
                        case "decimal":
                            createTableQuery += " " + $" {dataType}({(string.IsNullOrEmpty(maxLength) ? "18, 2" : maxLength)})";
                            break;
                        default:
                            createTableQuery += " " + $"{dataType}" + $" {(maxLength == "-1" ? "(max)" : (maxLength.Length > 0 ? ((Convert.ToInt32(maxLength) <= 8000) ? "(" + maxLength + ")" : "") : ""))} ";
                            break;
                    }
                    //if (!string.IsNullOrEmpty(maxLength))
                    //{
                    //    createTableQuery += $"({maxLength})";
                    //}

                    if (isNullable == "NO")
                    {
                        createTableQuery += " NOT NULL";
                    }

                    createTableQuery += ",";
                }

                // Son virgülü kaldır
                createTableQuery = createTableQuery.TrimEnd(',') + ")";

                // Yeni tabloyu oluştur
                using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, newConnection))
                {
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
        static DataColumn[] GetColumnNamesAndTypes(string connection, string tableName)
        {
            using (SqlConnection newConnection = new SqlConnection(connection))
            {
                newConnection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", newConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var columns = new List<DataColumn>();
                            while (reader.Read())
                            {
                                string columnName = reader.GetString(0);
                                string dataType = reader.GetString(1);

                                // Burada veri tipine göre uygun DataColumn türünü belirleyebilirsiniz
                                Type columnType = GetColumnTypeFromDataType(dataType);

                                DataColumn column = new DataColumn(columnName, columnType);
                                columns.Add(column);
                            }
                            return columns.ToArray();
                        }
                    }
                }
            }
            return null;
        }

        static Type GetColumnTypeFromDataType(string dataType)
        {
            // DataType'ı baz alarak uygun C# veri tipini döndür
            switch (dataType.ToLower())
            {
                case "int":
                    return typeof(int);
                case "nvarchar":
                case "varchar":
                    return typeof(string);
                case "varbinary":
                    return typeof(byte[]);
                // Diğer türleri ekleyebilirsiniz
                default:
                    return typeof(object); // Varsayılan olarak object tipini kullanabilirsiniz
            }
        }


        static string GenerateInsertQuery(string tableName, string[] columnNames)
        {
            //string[] columnList = columnNames;
            //List<string> guncellenmisListe = new List<string>();
            //foreach (string item in columnList)
            //{
            //    string guncellenmisString = item.Replace("[", "").Replace("]", "");
            //    guncellenmisListe.Add(guncellenmisString);
            //}
            string columns = string.Join(", ", columnNames.Select(column => "[" + column + "]"));
            string values = string.Join(", ", columnNames.Select(column => $"@{column}"));

            return $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (gridViewKaynakDataBaseList.RowCount == 0)
                {
                    isTransfer = false;
                    return;
                }
                if (gridViewKaynakTableList.RowCount == 0)
                {
                    isTransfer = false;
                    return;
                }
                if (gridViewHedefDataBaseList.RowCount == 0)
                {
                    isTransfer = false;
                    return;
                }
                isTransfer = true;
                var sourceRow = gridViewKaynakDataBaseList.GetFocusedDataRow();
                string dbKaynakName = Convert.ToString(sourceRow["name"]).Trim();
                var selectedTable = gridViewKaynakTableList.GetSelectedRows();
                //string selectedRowsString = string.Join(", ", selectedTable);
                var targetRow = gridViewHedefDataBaseList.GetFocusedDataRow();
                string dbHedefName = Convert.ToString(targetRow["name"]).Trim();
                List<string> selectedRowNames = new List<string>();

                if (selectedTable.Length >= 0 && selectedTable.Length <= gridViewKaynakTableList.RowCount)
                {
                    foreach (int item in selectedTable)
                    {
                        string rowName = gridViewKaynakTableList.GetRowCellValue(item, "table_name").ToString();
                        selectedRowNames.Add(rowName);
                    }
                    // Seçilen satırın adını alın

                }
                string selectedRowsString = string.Join(", ", selectedRowNames);
                DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show("Seçili satırlar aktarılacaktır onayliyor musunuz?\n" +
                    "Kaynak DB : " + dbKaynakName + "\n" +
                    "Seçili Tablolar :" + selectedRowsString + "\n" +
                    "Hedef DB : " + dbHedefName, "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (result == DialogResult.No)
                {
                    isTransfer = false;
                    return;
                }
                if (result == DialogResult.Yes)
                {
                    foreach (var item in selectedRowNames)
                    {
                        if (item != null && !string.IsNullOrEmpty(item))
                        {
                            DoTransfer(Convert.ToString(item));
                            //DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
                            //DevExpress.XtraEditors.ProgressBarControl progressBarControl = new DevExpress.XtraEditors.ProgressBarControl();
                            //repositoryItemProgressBar = repositoryItemProgressBar1;


                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    //var oRow = gridViewKaynakTableList.GetFocusedDataRow();
                                    //oRow["progressBar"] = 100;
                                    ////gridViewKaynakTableList.SetRowCellValue(gridViewKaynakTableList.FocusedRowHandle, "progressBar", 100);
                                    //gridViewKaynakTableList.RefreshRow(gridViewKaynakTableList.FocusedRowHandle);
                                    //repositoryItemProgressBar1.ShowTitle = true;
                                    //repositoryItemProgressBar1.CustomDisplayText += RepositoryItemProgressBar2_CustomDisplayText;
                                    ////this.gridViewKaynakTableList.DataSource = CreateDataTable();
                                    //gridViewKaynakTableList.Columns["progressBar"].FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;

                                    //gridViewKaynakTableList.SetRowCellValue(gridViewKaynakTableList.FocusedRowHandle, "progressBar", 50);
                                    //gridViewKaynakTableList.RefreshRow(gridViewKaynakTableList.FocusedRowHandle);
                                }));
                            }



                        }
                    }

                }
            }
            catch (Exception ex)
            {
                isTransfer = false;
                MessageBox.Show(ex.Message.ToString()); ;
            }

        }
        private void RepositoryItemProgressBar2_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.DisplayText == "100%")
            {
                e.DisplayText = "Finished";
                return;
            }
            if (e.DisplayText == "0%")
            {
                e.DisplayText = "Open";
                return;
            }
            e.DisplayText = "Started";
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // Hata kontrolü
                MessageBox.Show("Hata oluştu: " + e.Error.Message);
            }
            else
            {
                if (isTransfer == false)
                {
                    return;
                }
                // İşlem başarıyla tamamlandı
                MessageBox.Show("Veri başarıyla eklendi.");
                var oRowDb = gridViewHedefDataBaseList.GetFocusedDataRow();
                string HedefdbName = Convert.ToString(oRowDb["name"]).Trim();
                if (HedefdbName.Length > 0)
                {
                    getHedefTables(HedefdbName);
                }
            }
        }

        private void btnKaynakTableExcelAl_Click(object sender, EventArgs e)
        {
            if (gridViewKaynakTableList.RowCount > 0)
            {
                string lcFName = @DateTime.Now.Month.ToString().PadLeft(2, '0')
                + DateTime.Now.Day.ToString().PadLeft(2, '0')
                + DateTime.Now.Year.ToString()
                + DateTime.Now.Hour.ToString().PadLeft(2, '0')
                + DateTime.Now.Minute.ToString().PadLeft(2, '0')
                + DateTime.Now.Second.ToString().PadLeft(2, '0')
                + ".xlsx";
                string LcDir = System.IO.Path.GetTempPath();
                string LcFileName = LcDir + lcFName;
                gridControKaynakTableList.ExportToXlsx(LcFileName);
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = LcFileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();

                }
                catch
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Excel Dosyası Açılamadı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHedefTableExceleAl_Click(object sender, EventArgs e)
        {
            if (gridViewHedefTableList.RowCount > 0)
            {
                string lcFName = @DateTime.Now.Month.ToString().PadLeft(2, '0')
                + DateTime.Now.Day.ToString().PadLeft(2, '0')
                + DateTime.Now.Year.ToString()
                + DateTime.Now.Hour.ToString().PadLeft(2, '0')
                + DateTime.Now.Minute.ToString().PadLeft(2, '0')
                + DateTime.Now.Second.ToString().PadLeft(2, '0')
                + ".xlsx";
                string LcDir = System.IO.Path.GetTempPath();
                string LcFileName = LcDir + lcFName;
                gridControlHedefTableList.ExportToXlsx(LcFileName);
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = LcFileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();

                }
                catch
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Excel Dosyası Açılamadı.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Info oForm = new Info();
            oForm.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show("Hedefte seçili veri tabanına ait foreign keylerin kontrolü yapılıp ilgili tablelar delete edilecektir. Onaylıyor musunuz?", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (result == DialogResult.No)
            {
                return;
            }
            try
            {
                if (tableNamesDataTable.Rows.Count > 0)
                {
                    var oRow = gridViewHedefDataBaseList.GetFocusedDataRow();
                    if (oRow != null)
                    {
                        string HedefdbName = "";
                        //var oRowDb = gridViewHedefDataBaseList.GetFocusedDataRow();
                        HedefdbName = Convert.ToString(oRow["name"]).Trim();
                        string connHedef = "";
                        connHedef = $"Data Source={staticHedefServerName};Initial Catalog={HedefdbName};User ID={staticHedefUserName};Password={staticHedefPassword};";
                        List<string> tableList = new List<string>();
                        using (SqlConnection newConnection = new SqlConnection(connHedef))
                        {
                            newConnection.Open();
                            SqlTransaction transaction = newConnection.BeginTransaction();
                            try
                            {
                                string query = $@" select  tablename from
                                          (
                                          select
                                          object_name(f.parent_object_id) as TableName
                                          from sys.foreign_keys as f 
                                          inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id
                                          group by object_name(f.parent_object_id)
                                           
                                          union all
                                           
                                          select
                                          object_name (f.referenced_object_id) as TableName
                                          from sys.foreign_keys as f 
                                          inner join sys.foreign_key_columns as fc on f.object_id = fc.constraint_object_id
                                          group by object_name (f.referenced_object_id)
                                           
                                          ) x group by tablename";

                                using (SqlCommand command = new SqlCommand(query, newConnection))
                                {
                                    command.Transaction = transaction;
                                    using (SqlDataReader readers = command.ExecuteReader())
                                    {
                                        while (readers.Read())
                                        {
                                            string columnValue = Convert.ToString(readers.GetValue(0));
                                            tableList.Add(columnValue);
                                        }
                                        readers.Close();
                                    }
                                }
                                for (int i = 0; i < 5; i++)
                                {
                                    string deleteQuery = "";
                                    foreach (string item in tableList)
                                    {
                                        deleteQuery += $" delete from {item} DBCC CHECKIDENT ({item}, reseed, 0) \n ";


                                    }
                                    using (SqlCommand checkTableCommand = new SqlCommand(deleteQuery, newConnection, transaction))
                                    {
                                        checkTableCommand.CommandTimeout = 3000;
                                        try
                                        {
                                            checkTableCommand.ExecuteNonQuery();
                                        }
                                        catch
                                        {


                                        }


                                    }
                                }

                                transaction.Commit();
                                MessageBox.Show("İşlem başarılı.");
                            }
                            catch (Exception ex)
                            {
                                try
                                {

                                    transaction.Rollback();
                                    MessageBox.Show(ex.Message.ToString());
                                }
                                catch (Exception rollbackEx)
                                {
                                    MessageBox.Show("Rollback hatası: " + rollbackEx.Message);
                                }
                            }

                        }

                        var oRowDbs = gridViewHedefDataBaseList.GetFocusedDataRow();
                        string HedefdbNames = Convert.ToString(oRowDbs["name"]).Trim();
                        if (HedefdbNames.Length > 0)
                        {
                            getHedefTables(HedefdbNames);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }

        }

        private void gridViewKaynakTableList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    try
            //    {
            //        var transferState = View.GetRowCellValue(e.RowHandle, View.Columns["TransferState"]);
            //        if (transferState != null)
            //        {
            //            if ((int)transferState == 1 && !Convert.IsDBNull(transferState))
            //            {

            //                e.Appearance.BackColor = Color.Green;

            //            }
            //        }

            //    }
            //    catch
            //    {
            //    }
            //}
        }

        private void btbnHedefTableRowSil_Click(object sender, EventArgs e)
        {
            var selectedTable = gridViewHedefTableList.GetSelectedRows();
            List<string> selectedRowNames = new List<string>();
            if (selectedTable.Length > 0 && selectedTable.Length <= gridViewHedefTableList.RowCount)
            {
                foreach (int item in selectedTable)
                {
                    int countRow = Convert.ToInt32(gridViewHedefTableList.GetRowCellValue(item, "Count"));
                    if(countRow == 0)
                    {
                        continue;
                    }
                    string rowName = gridViewHedefTableList.GetRowCellValue(item, "table_name").ToString();
                    selectedRowNames.Add(rowName);
                }
                var oRow = gridViewHedefDataBaseList.GetFocusedDataRow();
                if (oRow != null)
                {
                    string HedefdbName = "";
                    //var oRowDb = gridViewHedefDataBaseList.GetFocusedDataRow();
                    HedefdbName = Convert.ToString(oRow["name"]).Trim();
                    string connHedef = "";
                    connHedef = $"Data Source={staticHedefServerName};Initial Catalog={HedefdbName};User ID={staticHedefUserName};Password={staticHedefPassword};";
                    List<string> tableList = new List<string>();
                    using (SqlConnection newConnection = new SqlConnection(connHedef))
                    {
                        newConnection.Open();
                        SqlTransaction transaction = newConnection.BeginTransaction();
                        try
                        {

                           
                                string deleteQuery = "";
                                foreach (string item in selectedRowNames)
                                {
                                    deleteQuery += $" delete from {item} DBCC CHECKIDENT ({item}, reseed, 0) \n ";
                                }
                                using (SqlCommand checkTableCommand = new SqlCommand(deleteQuery, newConnection, transaction))
                                {
                                    checkTableCommand.CommandTimeout = 3000;
                                    try
                                    {
                                        checkTableCommand.ExecuteNonQuery();
                                    }
                                    catch(Exception ex)
                                    {
                                        transaction.Rollback();
                                        MessageBox.Show("Hata: " + ex.Message);
                                    }


                                }
                            

                            transaction.Commit();
                            MessageBox.Show("İşlem başarılı.");
                        }
                        catch (Exception ex)
                        {
                            try
                            {

                                transaction.Rollback();
                                MessageBox.Show(ex.Message.ToString());
                            }
                            catch (Exception rollbackEx)
                            {
                                MessageBox.Show("Rollback hatası: " + rollbackEx.Message);
                            }
                        }

                    }

                    var oRowDbs = gridViewHedefDataBaseList.GetFocusedDataRow();
                    string HedefdbNames = Convert.ToString(oRowDbs["name"]).Trim();
                    if (HedefdbNames.Length > 0)
                    {
                        getHedefTables(HedefdbNames);
                    }

                }
                else
                {
                    return;
                }
            }
        }

    }
}
