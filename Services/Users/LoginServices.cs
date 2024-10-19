using Microsoft.Data.SqlClient;
using Pautas.Models.Login;
using Pautas.Models.Profesor;
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
                        sp_curso.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(sp_curso);

                        SqlParameter sp_level = new SqlParameter("@Id_Level", SqlDbType.Int);
                        sp_level.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(sp_level);

                        SqlParameter sp_porc = new SqlParameter("@Porcentaje", SqlDbType.Int);
                        sp_porc.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(sp_porc);

                        SqlParameter sp_pagar = new SqlParameter("@Pagar", SqlDbType.Int);
                        sp_pagar.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(sp_pagar);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        // Solo asignar valores si RESP es true
                        bool isSuccess = Convert.ToBoolean(sp_resp.Value);
                        resp.code = isSuccess ? "success" : "error";
                        resp.message = sp_mess.Value != DBNull.Value ? sp_mess.Value.ToString() : "Error desconocido";

                        if (isSuccess)
                        {
                            // Asignar valores solo si RESP es exitoso
                            resp.idUser = spUserId.Value != DBNull.Value ? Convert.ToInt32(spUserId.Value) : (int?)null;
                            resp.Name = spUserName.Value != DBNull.Value ? spUserName.Value.ToString() : null;
                            resp.IdRol = spRolId.Value != DBNull.Value ? Convert.ToInt32(spRolId.Value) : (int?)null;
                            resp.IdCurso = sp_curso.Value != DBNull.Value ? Convert.ToInt32(sp_curso.Value) : (int?)null;
                            resp.IdLevel = sp_level.Value != DBNull.Value ? Convert.ToInt32(sp_level.Value) : (int?)null;
                            resp.Porcentaje = sp_porc.Value != DBNull.Value ? Convert.ToInt32(sp_porc.Value) : (int?)null;
                            resp.Pagar = sp_pagar.Value != DBNull.Value ? Convert.ToInt32(sp_pagar.Value) : (int?)null;
                        }
                        else
                        {
                            // Asignar valores nulos o vacíos en caso de fallo
                            resp.idUser = null;
                            resp.Name = null;
                            resp.IdRol = null;
                            resp.IdCurso = null;
                            resp.IdLevel = null;
                            resp.Porcentaje = null;
                            resp.Pagar = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resp.code = "error";
                resp.message = ex.Message; // Asignar mensaje de excepción en caso de error
            }
            return resp;
        }
        #endregion


        public FoldersCurso GetFolderCursoById(int id)
        {
            FoldersCurso folderCurso = null;

            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                sql.Open();

                using (SqlCommand command = new SqlCommand("spGetFolderCursoById", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            folderCurso = new FoldersCurso
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                // otros campos si es necesario
                            };
                        }
                    }
                }
            }

            return folderCurso;
        }

        #region GetFolderByLevel
        public List<FoldersLevel> GetFolderByLevel(int id)
        {
            List<FoldersLevel> rootFoldersLevel = new List<FoldersLevel>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetFoldersByLevel", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_subfolders_curso", id);

                        sql.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rootFoldersLevel.Add(new FoldersLevel
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Id_Folders_level = reader.GetInt32(2),
                                    Id_subfolders_curso = reader.GetInt32(3),

                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las carpetas raíz", ex);
            }

            return rootFoldersLevel;
        }
        #endregion

    }
}
