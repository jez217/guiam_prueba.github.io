
using Microsoft.Data.SqlClient;
using Pautas.Models.Login;
using Pautas.Models.Pautas;
using Pautas.Models.Admin;
using Pautas.Services.Conection;
using Pautas.Services.Extensions;

using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pautas.Models;

namespace Pautas.Services.Users
{
    public class AdminServices
    {
        ConnectionDb _connService = new ConnectionDb();


        #region UserSelect
        public User SP_USER_SELECT_BY_ONE(int idUser)
        {

            User user = new User();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_GM_User_Select_By_One", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id_user", idUser));

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new User()
                                {
                                    idUser = dr.GetInt32(dr.GetOrdinal("Id_user")),
                                    Name = dr.GetStringNull(dr.GetOrdinal("Name")),
                                    //IdRol = dr.GetInt32(dr.GetOrdinal("IdRol")),
                                    IdStatus = dr.GetInt32Null(dr.GetOrdinal("Id_status")),
                                    Correo = dr.GetStringNull(dr.GetOrdinal("Correo")),
                                    Apellido = dr.GetStringNull(dr.GetOrdinal("Apellido")),
                                    Clave = dr.GetStringNull(dr.GetOrdinal("Clave"))
                                };
                                user = obj;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar excepción (opcional)
            }
            return user;
        }
        #endregion

        #region UserSelect
        public List<User> SP_USER_SELECT()
        {
            List<User> olista = new List<User>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_GM_UserSelect", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new User()
                            {
                                id = dr.GetInt32Null(dr.GetOrdinal("id")),
                                idUser = dr.GetInt32(dr.GetOrdinal("Id_user")),
                                Name = dr.GetStringNull(dr.GetOrdinal("Name")),
                                IdRol = dr.GetInt32Null(dr.GetOrdinal("IdRol")),
                                IdStatus = dr.GetInt32Null(dr.GetOrdinal("Id_status")),
                                Correo = dr.GetStringNull(dr.GetOrdinal("Correo")),
                                Apellido = dr.GetStringNull(dr.GetOrdinal("Apellido")),
                                Clave = dr.GetStringNull(dr.GetOrdinal("Clave")),

                            };
                            olista.Add(obj);
                        }
                    }
                }
            }
            return olista;
        }
        #endregion

        #region CreateUser
        public GenericResponse CreateUser(User model)
        {
            GenericResponse resp = new GenericResponse();
            int UserId = 0;

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_GM_User_Create", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@IdRol", model.IdRol);
                        cmd.Parameters.AddWithValue("@Id_status", model.IdStatus);
                        cmd.Parameters.AddWithValue("@Correo", model.Correo);
                        cmd.Parameters.AddWithValue("@Apellido", model.Apellido);
                        cmd.Parameters.AddWithValue("@Clave", model.Clave);

                        // Adding output parameters
                        SqlParameter spUserId = new SqlParameter("@Id_user", SqlDbType.Int);
                        spUserId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spUserId);

                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        // Check if output parameters are DBNull and handle accordingly
                        if (spUserId.Value != DBNull.Value)
                        {
                            UserId = Convert.ToInt32(spUserId.Value);
                        }

                        resp.id = UserId;
                        resp.message = spMessage.Value != DBNull.Value ? spMessage.Value.ToString() : string.Empty;
                        resp.Resp = Convert.ToBoolean(spResponse.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }
        #endregion

        #region UpdateUser
        public GenericResponse UpdateUser(User model)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_GM_User_Update", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.Add(new SqlParameter("@Id_user", model.idUser));
                        cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                        //cmd.Parameters.Add(new SqlParameter("@IdRol", model.IdRol));
                        //cmd.Parameters.Add(new SqlParameter("@Token", model.Token));
                        cmd.Parameters.Add(new SqlParameter("@Id_status", model.IdStatus));
                        cmd.Parameters.Add(new SqlParameter("@Correo", model.Correo));
                        cmd.Parameters.Add(new SqlParameter("@Apellido", model.Apellido));
                        cmd.Parameters.Add(new SqlParameter("@Clave", model.Clave));

                        // Adding output parameters
                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieving output parameter values
                        resp.message = spMessage.Value.ToString();
                        resp.code = Convert.ToBoolean(spResponse.Value) ? "success" : "error";


                    }
                }
            }
            catch (Exception ex)
            {
                resp.code = "error";
                resp.message = ex.Message; // Capture exception message
            }
            return resp;
        }
        #endregion

        #region DeleteUser
        public User DeleteUser(int userId)
        {
            User resp = new User();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_GM_User_Delete", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.Add(new SqlParameter("@Id_user", userId));

                        // Adding output parameters
                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieving output parameter values
                        resp.message = spMessage.Value.ToString();
                        resp.code = Convert.ToBoolean(spResponse.Value) ? "success" : "error";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.code = "error";
                resp.message = ex.Message; // Capture exception message
            }
            return resp;
        }
        #endregion

        #region RolSelect
        public List<Rol> SP_ROL_SELECT()
        {
            List<Rol> olista = new List<Rol>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_RolSelect", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new Rol()
                            {
                                IdRol = dr.GetInt32(dr.GetOrdinal("IdRol")),
                                Descripcion = dr.GetStringNull(dr.GetOrdinal("Descripcion")),
                            };
                            olista.Add(obj);
                        }
                    }
                }
            }
            return olista;
        }
        #endregion

        #region LevelSelect
        public List<Level> SP_LEVEL_SELECT()
        {
            List<Level> olista = new List<Level>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_LevelSelect", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new Level()
                            {
                                Id_level = dr.GetInt32(dr.GetOrdinal("Id_level")),
                                Name = dr.GetString(dr.GetOrdinal("Name")),
                            };
                            olista.Add(obj);
                        }
                    }
                }
            }
            return olista;
        }
        #endregion

        #region LevelUser
        public Student SP_GM_LEVEL_INSERT(int USERID, int LEVELID)
        {
            Student resp = new Student();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand command = new SqlCommand("p_GM_User_Level_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@USERID", SqlDbType.Int).Value = USERID;
                        command.Parameters.Add("@LEVELID", SqlDbType.Int).Value = LEVELID;

                        SqlParameter messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParam);

                        SqlParameter responseParam = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        responseParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(responseParam);

                        connection.Open();
                        command.ExecuteNonQuery();
                        resp.id = USERID;
                        resp.message = messageParam.Value.ToString();
                        resp.Resp = Convert.ToBoolean(responseParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.id = USERID;
                resp.code = "error";
                resp.message = ex.Message;
            }

            return resp;
        }
        #endregion

        #region ProfitCenterDropdown
        public List<SelectListItem> RolDropdown()
        {
            List<SelectListItem> oList = new List<SelectListItem>();

            var listRl = SP_ROL_SELECT();

            foreach (var Rl in listRl)
            {
                oList.Add(new SelectListItem
                {
                    Text = Rl.Descripcion,
                    Value = Rl.IdRol.ToString()
                });
            }

            return oList;
        }

        #endregion
    }
}
