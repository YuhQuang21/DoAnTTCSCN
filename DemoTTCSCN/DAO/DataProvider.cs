using DemoTTCSCN.Services;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DemoTTCSCN.DAO
{
    public class DataProvider
    {
        private static volatile DataProvider instance;
        private SendDataSocketService _sendDataSocketService;

        private DataProvider(SendDataSocketService sendDataSocketService)
        {
            _sendDataSocketService = sendDataSocketService;
        }

        private string strConnection = @"Data Source=QUANGHUY; Initial Catalog =QLDiemSV; User Id=sa; Password=Huytuyen2211@; Integrated Security=True";
        private static object key = new object();

        public static DataProvider Instance
        {
            get
            {
                lock (key)
                {
                    if (instance == null)
                    {
                        instance = new DataProvider(new SendDataSocketService());
                    }
                }
                return instance;
            }
        }

        // Thuc thi cau lenh va tra ve ket qua la danh sach du lieu (select.....)
        public async Task<DataTable> ExecuteQuery(string query, string[] parameter = null)
        {
            var result = await _sendDataSocketService.SendQuery(query, parameter);
            if (result != null)
            {
                ExceptionService.Instance.setException(result);
                return null;
            }
            DataTable data = new DataTable("data");
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                // trung gian qua mot cai SqlAdapter
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        // Thuc hien truy van insert, update, delete
        public async Task<int> ExecuteNonQuery(string query, string[] parameter = null)
        {
            var result = await _sendDataSocketService.SendQuery(query, parameter);
            if (result != null)
            {
                ExceptionService.Instance.setException(result);
                return 0;
            }
            int data = 0;
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                // trung gian qua mot cai SqlAdapter
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }

        public async Task<object> ExecuteScalar(string query, string[] parameter = null)
        {
            var result = await _sendDataSocketService.SendQuery(query, parameter);
            if (result != null)
            {
                ExceptionService.Instance.setException(result);
                return null;
            }
            object data = 0;
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                // trung gian qua mot cai SqlAdapter
                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }
    }
}