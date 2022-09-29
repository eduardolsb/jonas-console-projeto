using System.Data;
using System.Data.SqlClient;

namespace ConsoleAppJonas
{
    public class DataAccessLayer
    {
        #region PROPRIEDADES
        private bool IsUnitTest { get; set; }
        private SqlConnection Conn { get; set; }
        private SqlDataAdapter DataAdapter { get; set; }
        private SqlTransaction Trans { get; set; }

        //informação encapsulada na propriedade da classe
        public string ReceitaDaVovo { get => GetReceita(); }
        #endregion

        public DataAccessLayer(bool isUnitTest = false)
        {
            IsUnitTest = isUnitTest;
            Conn = new SqlConnection("data source=192.168.0.0,7500;initial catalog=MainSalmonela;persist security info=True;user id=sa;password=xxxxxxxx;");
            DataAdapter = new SqlDataAdapter();
        }

        #region ENCAPSULAMENTO
        private string GetReceita()
        {
            string temperoA = "";
            string temperoB = "O segredo de construção da comida da vovó";
            return temperoA + temperoB;
        }
        #endregion

        #region UTILS
        private void OpenConnection()
        {
            if(Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
        }
        private void CloseConnection()
        {
            Conn.Close();
        }
        private void Commit()
        {
            if (IsUnitTest)
            {
                Trans.Rollback();
            }
            else
            {
                Trans.Commit();
            }
        }
        #endregion

        #region FUNCTIONS
        public void ExecuteNonQuery(string query)
        {
            OpenConnection();
            Trans = Conn.BeginTransaction();
            try
            {
                SqlCommand command = new SqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Connection = Conn;
                command.ExecuteNonQuery();
                Commit();
                CloseConnection();
            }
            catch 
            {
                Trans.Rollback();
            }
        }
        public DataSet ExecuteQueryDataSet(string query, DataSet dataset, string nomedataset)
        {
            OpenConnection();
            Trans = Conn.BeginTransaction();
            try
            {
                SqlCommand command = new SqlCommand(query);
                command.CommandType = CommandType.Text;
                command.Connection = Conn;
                command.Transaction = Trans;

                DataAdapter.SelectCommand = command;
                DataAdapter.Fill(dataset, nomedataset);
                
                Commit();
                CloseConnection();

                return dataset;
            }
            catch
            {
                Trans.Rollback();
            }

            return new DataSet();
        }
        #endregion
    }
}
