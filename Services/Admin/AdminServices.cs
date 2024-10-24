﻿
using Microsoft.Data.SqlClient;
using Pautas.Models.Login;
using Pautas.Models.Pautas;
using Pautas.Models.Admin;
using Pautas.Services.Conection;
using Pautas.Services.Extensions;

using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pautas.Models;
using Azure;

namespace Pautas.Services.Users
{
    public class AdminService
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
                                    Clave = dr.GetStringNull(dr.GetOrdinal("Clave")),
                                    Porcentaje = dr.GetInt32(dr.GetOrdinal("Porcentaje")),
                                    Pagar = dr.GetInt32(dr.GetOrdinal("Pagar"))

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
                        cmd.Parameters.AddWithValue("@Name", model.Name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Level", model.IdLevel ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IdCurso", model.IdCurso ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IdRol", model.IdRol ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Id_status", model.IdStatus ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Correo", model.Correo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Apellido", model.Apellido ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Clave", model.Clave ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Porcentaje", model.Porcentaje ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pagar", model.Pagar ?? (object)DBNull.Value);
                        // Output parameters
                        SqlParameter userIdParam = new SqlParameter("@Id_user", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        SqlParameter messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                        SqlParameter responseParam = new SqlParameter("@RESPONSE", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                        cmd.Parameters.Add(userIdParam);
                        cmd.Parameters.Add(messageParam);
                        cmd.Parameters.Add(responseParam);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieving output parameter values
                         UserId = (int)cmd.Parameters["@Id_user"].Value;

                        resp.message = messageParam.Value.ToString();
                        resp.code = Convert.ToBoolean(responseParam.Value) ? "success" : "error";
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
                        cmd.Parameters.AddWithValue("@Level", model.IdLevel);
                        cmd.Parameters.AddWithValue("@IdRol", model.IdRol);
                        //cmd.Parameters.Add(new SqlParameter("@Token", model.Token));
                        cmd.Parameters.Add(new SqlParameter("@Id_status", model.IdStatus));
                        cmd.Parameters.Add(new SqlParameter("@Correo", model.Correo));
                        cmd.Parameters.Add(new SqlParameter("@Apellido", model.Apellido));
                        cmd.Parameters.Add(new SqlParameter("@Clave", model.Clave));
                        cmd.Parameters.Add(new SqlParameter("@IdCurso", model.IdCurso));
                        cmd.Parameters.Add(new SqlParameter("@Porcentaje", model.Porcentaje));
                        cmd.Parameters.Add(new SqlParameter("@Pagar", model.Pagar));
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
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
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

        #region CursoSelect
        public List<Curso> SP_CURSO_SELECT()
        {
            List<Curso> olista = new List<Curso>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_CursoSelect", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new Curso()
                            {
                                Id = dr.GetInt32(dr.GetOrdinal("Id")),
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

        #region LevelDropdown
        public List<SelectListItem> LevelDropdown()
        {
            List<SelectListItem> oList = new List<SelectListItem>();

            var listRl = SP_LEVEL_SELECT();

            foreach (var Rl in listRl)
            {
                oList.Add(new SelectListItem
                {
                    Text = Rl.Name,
                    Value = Rl.Id_level.ToString()
                });
            }

            return oList;
        }
        #endregion

        #region LevelDropdown
        public List<SelectListItem> CursoDropdown()
        {
            List<SelectListItem> oList = new List<SelectListItem>();

            var listRl = SP_CURSO_SELECT();

            foreach (var Rl in listRl)
            {
                oList.Add(new SelectListItem
                {
                    Text = Rl.Name,
                    Value = Rl.Id.ToString()
                });
            }

            return oList;
        }
        #endregion
    }
}
