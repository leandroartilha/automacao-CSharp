using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace AutomacaoToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Digite o nome do arquivo:");

            string nomeArquivo = Console.ReadLine();

            Console.WriteLine("Arquivo: " + nomeArquivo);
            Console.WriteLine("Carregando...");

            string path = Directory.GetCurrentDirectory();
            string pathCerto = path.Replace("AutomacaoToCSV\\bin\\Debug\\net5.0", "");
            string[] lines = System.IO.File.ReadAllLines(pathCerto  + "\\"+ nomeArquivo);
            
            var contagem = 0;

            foreach (string line in lines)
            {
                string csvLine = string.Join(';', line);
                string date = csvLine.Substring(0, 10);
                string matricula = csvLine.Remove(0, 11);

                SqlCommand cmd = sqlCommandUI("");

                string sql = "INSERT INTO ACESSO_DIARIO(ID_SITE, ID_TIPO_COLABORADOR, DT_ACESSO, HR_ACESSO, QT_ACESSO, NU_RE) VALUES(222,1, CONVERT(DATETIME, '" + date + "', 103),9,1, '"+ matricula +"')";

                try
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    contagem++;
                    cmd.Connection.Close();
                }
                catch(Exception ex)
                {

                    cmd.Connection.Close();
                }

            }

            Console.WriteLine($"{contagem} linhas foram adicionadas com sucesso!");
            Console.ReadKey();
        }
        #region CONEXAO
        protected static string dataSourceUI = "";
        protected static string dataBaseUI = "";
        protected static string dbUserIDUI = "";
        protected static string dbUserPWDUI = "";

        protected static string connectionString()
        {
            return @"server=" + dataSourceUI + "; user ID=" + dbUserIDUI + ";password=" + dbUserPWDUI + ";Initial Catalog=" + dataBaseUI + "; App=AutomacaoToCSV " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        public static SqlConnection ConnUI()
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connectionString();
                conn.Open();
                return conn;
            }
            catch
            {
                return null;
            }
        }

        public static SqlCommand sqlCommandUI(string sql)
        {
            try
            {
                SqlConnection conn = ConnUI();
                SqlCommand command = new SqlCommand(sql, conn);
                command.CommandTimeout = 600;
                return command;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
