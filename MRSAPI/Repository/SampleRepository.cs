using MRSAPI.Gateway;
using MRSAPI.Models;
using MRSAPI.Repository.IRepository;
using System.Data;

namespace MRSAPI.Repository
{
    public class SampleRepository : ISampleRepository
    {
        private readonly DBHelper _dbHelper;
        public SampleRepository(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public ICollection<GenericModel> GetAllGenericInfo()
        {
            string Qry = "";
            Qry = " SELECT '0' GENERIC_CODE, 'All Generic' GENERIC_NAME FROM DUAL " +
                  "  UNION " +
                  "  SELECT 'S' GENERIC_CODE, 'Selected Generic' GENERIC_NAME FROM DUAL " +
                  "  UNION  " +
                  "  SELECT GENERIC_CODE, GENERIC_NAME FROM GENERIC ";

            Qry = Qry + "ORDER BY GENERIC_CODE ";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<GenericModel> items;
            items = (from DataRow row in dt.Rows
                     select new GenericModel
                     {
                         GENERIC_CODE = row["GENERIC_CODE"].ToString(),
                         GENERIC_NAME = row["GENERIC_NAME"].ToString()
                     }).ToList();

            return items;
        }
    }
}
