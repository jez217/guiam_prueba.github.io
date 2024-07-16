using Microsoft.Data.SqlClient;
using Pautas.Models.Profesor;

using Pautas.Services.Conection;
using System;
using System.Collections.Generic;
using System.Data;
using Pautas.Models.Login;

namespace Pautas.Services.ProfesorService
{
    public class FolderAccessService
    {
        ConnectionDb _connService = new ConnectionDb();

        public List<Models.Profesor.ImageModel> GetFilesByFolderId(int folderId)
        {
            List<Models.Profesor.ImageModel> fileList = new List<Models.Profesor.ImageModel>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ImageDetailSelect", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FolderId", SqlDbType.Int).Value = folderId;

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var file = new ImageModel()
                                {
                                    Id = dr.GetInt32(dr.GetOrdinal("Id")),
                                    Name = dr.GetString(dr.GetOrdinal("Name")),
                                    FilePath = dr.GetString(dr.GetOrdinal("FilePath")),
                                    FolderId = dr.GetInt32(dr.GetOrdinal("FolderId")),
                                    UploadedAt = dr.GetDateTime(dr.GetOrdinal("UploadedAt"))
                                };
                                fileList.Add(file);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones aquí
                // Puedes registrar el error, lanzar una excepción o manejarlo de otra manera
                fileList.Add(new Models.Profesor.ImageModel { FilePath = "error", Name = ex.Message });
            }
            return fileList;
        }


        public bool HasAccessToFolder(string userId, int folderId)
        {
            bool hasAccess = false;

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand command = new SqlCommand("p_GM_User_Level_Insert", sql))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@USERID", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@LEVELID", SqlDbType.Int).Value = folderId;

                        SqlParameter messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParam);

                        SqlParameter responseParam = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        responseParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(responseParam);

                        sql.Open();
                        command.ExecuteNonQuery();

                        // Verificar si el procedimiento almacenado actualiza correctamente el acceso
                        hasAccess = Convert.ToBoolean(responseParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error en FolderAccessService: {ex.Message}");
                hasAccess = false;
            }

            return hasAccess;
        }

        public List<Folder> GetFoldersByLevel(string folderLevel)
        {
            List<Folder> folders = new List<Folder>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetFoldersByLevel", sql))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserLevel", folderLevel);

                        sql.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                folders.Add(new Folder
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    ParentFolderId = reader.IsDBNull(reader.GetOrdinal("ParentFolderId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ParentFolderId")),
                                    CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                                    FolderLevel = reader.GetInt32(reader.GetOrdinal("FolderLevel"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error en GetFoldersByLevel: {ex.Message}");
            }

            return folders;
        }

        public void RenameFolder(int id, string name)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_RENAME_FOLDER", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Name", name);

                        sql.Open();
                        cmd.ExecuteNonQuery();
                        sql.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrar el error
                throw new Exception("Error al renombrar la carpeta: " + ex.Message);
            }
        }

        public void DeleteFolder(int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DELETE_FOLDER", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);

                        sql.Open();
                        cmd.ExecuteNonQuery();
                        sql.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción, por ejemplo, registrar el error
                throw new Exception("Error al eliminar la carpeta: " + ex.Message);
            }
        }



        #region CreateFolder
        public void CreateFolder(Folder model)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateFolder", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@ParentFolderId", model.ParentFolderId.HasValue ? (object)model.ParentFolderId.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);

                        sql.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la carpeta", ex);
            }
        }
        #endregion

        #region UploadFile
        public void UploadFile(string fileName, string filePath, int folderId)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UploadFile", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@Name", fileName);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);
                        cmd.Parameters.AddWithValue("@FolderId", folderId);

                        sql.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al subir el archivo", ex);
            }
        }
        #endregion

        #region GetRootFolders
        public List<Folder> GetRootFolders()
        {
            List<Folder> rootFolders = new List<Folder>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetRootFolders", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sql.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rootFolders.Add(new Folder
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    ParentFolderId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                    CreatedBy = reader.GetString(3)
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

            return rootFolders;
        }
        #endregion


        #region GetFolderById
        public Folder GetFolderById(int id)
        {
            Folder folder = new Folder();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetFolderById", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        sql.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                folder.Id = reader.GetInt32(0);
                                folder.Name = reader.GetString(1);
                                folder.ParentFolderId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                                folder.CreatedBy = reader.GetString(3);
                            }
                        }
                    }

                    folder.SubFolders = new List<Folder>();
                    folder.Files = new List<Models.Profesor.File>(); // Usar Profesor.File para especificar el tipo del namespace Profesor

                    // Obtener subcarpetas
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Folders WHERE ParentFolderId = @Id", sql))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                folder.SubFolders.Add(new Folder
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    ParentFolderId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                    CreatedBy = reader.GetString(3)
                                });
                            }
                        }
                    }

                    // Obtener archivos
                    using (SqlCommand cmd = new SqlCommand("sp_GetFilesByFolderId", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FolderId", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                folder.Files.Add(new Models.Profesor.File // Usar Profesor.File para especificar el tipo del namespace Profesor
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    FilePath = reader.GetString(2),
                                    FolderId = reader.GetInt32(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la carpeta por ID", ex);
            }

            return folder;
        }
        #endregion


    }
}
