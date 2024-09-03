using Microsoft.Data.SqlClient;
using Pautas.Models.Login;
using Pautas.Services.Conection;
using System.Data;

namespace Pautas.Services.Users
{
    public class LoginServices
    {
        ConnectionDb _connService = new ConnectionDb();

        #region ValidateAccess
        public User ValidateAccess(User model)
        {
            User resp = new User();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_GM_Acces_Portal", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                        cmd.Parameters.Add(new SqlParameter("@Clave", model.Clave));

                        SqlParameter spUserId = new SqlParameter("@USERID", SqlDbType.Int);
                        spUserId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spUserId);

                        SqlParameter spRolId = new SqlParameter("@ROL", SqlDbType.Int);
                        spRolId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spRolId);

                        SqlParameter spUserName = new SqlParameter("@USER_NAME", SqlDbType.NVarChar, 200);
                        spUserName.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spUserName);

                        SqlParameter sp_resp = new SqlParameter("@RESP", SqlDbType.Bit);
                        sp_resp.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(sp_resp);

                        SqlParameter sp_mess = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        sp_mess.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(sp_mess);

                        SqlParameter sp_curso = new SqlParameter("@Id_Curso", SqlDbType.Int);
                        sp_curso.Direction = ParameterDirection.Output; // Corrección aquí
                        cmd.Parameters.Add(sp_curso);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        resp.idUser = Convert.ToInt32(spUserId.Value);
                        resp.Name = spUserName.Value.ToString();
                        resp.Clave = model.Clave;
                        resp.IdRol = Convert.ToInt32(spRolId.Value);
                        resp.IdCurso = sp_curso.Value != DBNull.Value ? Convert.ToInt32(sp_curso.Value) : (int?)null; // Manejar el caso de NULL

                        resp.code = sp_resp.Value.ToString();
                        resp.message = sp_mess.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }
        #endregion
    }
}
