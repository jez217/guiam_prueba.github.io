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

        public List<FoldersCurso> GetFoldersByCurso(string folderCurso)
        {
            List<FoldersCurso> folders = new List<FoldersCurso>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetFoldersByCurso", sql))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id_folders_curso", folderCurso);

                        sql.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                folders.Add(new FoldersCurso
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                      
                                    //FolderLevel = reader.GetInt32(reader.GetOrdinal("FolderLevel"))
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

        public List<FoldersCurso> GetFoldersByLevel(string folderLevel)
        {
            List<FoldersCurso> folders = new List<FoldersCurso>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetFoldersByLevel", sql))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id_folders_level", folderLevel);

                        sql.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                folders.Add(new FoldersCurso
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),

                                    //FolderLevel = reader.GetInt32(reader.GetOrdinal("FolderLevel"))
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

        public void DeleteFile(int fileId)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteFile", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", fileId);

                        sql.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el archivo: " + ex.Message);
            }
        }

        #region CreateFolderCurso
        public void CreateFolderCurso(FoldersCurso model)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateFolderCurso", sql))
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

        #region CreateFolderLevel
        public void CreateFolderLevel(FoldersLevel model)
        {

            //FoldersLevel model = new FoldersLevel();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateFolderLevel", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@Id_Folders_level", model.Id_Folders_level);

                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@Id_subfolders_curso", model.Id_subfolders_curso);
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

        public bool CreateFile(string fileName, string filePath,  int folderLevelId, DateTime uploadedAt)
        {
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                sql.Open();
                string query = "INSERT INTO Files (Name, FilePath, FolderId, UploadedAt, Id_Folders_level) VALUES (@Name, @FilePath, @FolderId, @UploadedAt, @FolderLevelId)";

                using (SqlCommand command = new SqlCommand(query, sql))
                {
                    command.Parameters.AddWithValue("@Name", fileName);
                    command.Parameters.AddWithValue("@FilePath", filePath);
                    command.Parameters.AddWithValue("@UploadedAt", uploadedAt);
                    command.Parameters.AddWithValue("@FolderLevelId", folderLevelId);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
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
                        cmd.Parameters.AddWithValue("@Id_Folders_level", model.Id_Folders_level.HasValue ? (object)model.Id_Folders_level.Value : DBNull.Value);  
                        cmd.Parameters.AddWithValue("@Id_level_reference", model.Id_level_reference.HasValue ? (object)model.Id_level_reference.Value : DBNull.Value);
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

        #region CreateFolder
        public void CreateSubFolder(Folder model)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateSubFolder", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@ParentFolderId", model.Id);
                        cmd.Parameters.AddWithValue("@Id_level_reference", model.Id_level_reference);
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
        public void UploadFile(string fileName, string filePath, int folderlevelId)
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
                        cmd.Parameters.AddWithValue("@FolderLevelId", folderlevelId);

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

        #region UploadFile
        public void UploadSubFile(string fileName, string filePath, int folderparentId, int idlevelreference)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UploadSubFile", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding input parameters
                        cmd.Parameters.AddWithValue("@Name", fileName);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);
                        cmd.Parameters.AddWithValue("@FolderParentId", folderparentId);
                        cmd.Parameters.AddWithValue("@Id_level_reference", idlevelreference);

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

        #region GetFolderLevelById
        public FoldersLevel GetLevelFolderById(int id)
        {
            FoldersLevel folder = new FoldersLevel();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetLevelFolderById", sql))
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
                                folder.Id_Folders_level = reader.GetInt32(2);
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

        #region GetSubFolderById
        public Folder GetSubFolderById(int id)
        {
            Folder folder = new Folder();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetSubFolderById", sql))
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
                                folder.Id_level_reference = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                                folder.ParentFolderId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);
                                folder.CreatedBy = reader.GetString(4);
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
                                    Id_level_reference = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                                    ParentFolderId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                    CreatedBy = reader.GetString(4)
                                });
                            }
                        }
                    }

                    // Obtener archivos
                    using (SqlCommand cmd = new SqlCommand("sp_GetFilesBySubFolderId", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FolderParentId", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                folder.Files.Add(new Models.Profesor.File // Usar Profesor.File para especificar el tipo del namespace Profesor
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    FolderParentId = reader.GetInt32(4)
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
                                folder.Id_level_reference = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                                folder.Id_Folders_level = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                                folder.CreatedBy = reader.GetString(4);
                            }
                        }
                    }

                    folder.SubFolders = new List<Folder>();
                    folder.Files = new List<Models.Profesor.File>(); // Usar Profesor.File para especificar el tipo del namespace Profesor

                    // Obtener subcarpetas
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Folders WHERE Id_Folders_level = @Id", sql))
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
                                    Id_level_reference = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    Id_Folders_level = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    CreatedBy = reader.GetString(4)
                                });
                            }
                        }
                    }

                    // Obtener archivos
                    using (SqlCommand cmd = new SqlCommand("sp_GetFilesByFolderId", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FolderLevelId", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                folder.Files.Add(new Models.Profesor.File // Usar Profesor.File para especificar el tipo del namespace Profesor
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    FilePath = reader.GetString(2),
                                    FolderLevelId = reader.GetInt32(3)
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

        //#region GetFolderById
        //public Folder GetsubFolderById(int id)
        //{
        //    Folder folder = new Folder();

        //    try
        //    {
        //        using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("sp_GetSubFolderById", sql))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@Id", id);
        //                sql.Open();

        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        folder.Id = reader.GetInt32(0);
        //                        folder.Name = reader.GetString(1);
        //                        folder.ParentFolderId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
        //                        folder.CreatedBy = reader.GetString(3);
        //                    }
        //                }
        //            }

        //            folder.SubFolders = new List<Folder>();
        //            folder.Files = new List<Models.Profesor.File>(); // Usar Profesor.File para especificar el tipo del namespace Profesor

        //            // Obtener subcarpetas
        //            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Folders WHERE ParentFolderId = @Id", sql))
        //            {
        //                cmd.Parameters.AddWithValue("@Id", id);

        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        folder.SubFolders.Add(new Folder
        //                        {
        //                            Id = reader.GetInt32(0),
        //                            Name = reader.GetString(1),
        //                            ParentFolderId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
        //                            CreatedBy = reader.GetString(3)
        //                        });
        //                    }
        //                }
        //            }

        //            // Obtener archivos
        //            using (SqlCommand cmd = new SqlCommand("sp_GetFilesBySubFolderId", sql))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@FolderParentId", id);

        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        folder.Files.Add(new Models.Profesor.File // Usar Profesor.File para especificar el tipo del namespace Profesor
        //                        {
        //                            Id = reader.GetInt32(0),
        //                            Name = reader.GetString(1),
        //                            FilePath = reader.GetString(2),
        //                            FolderParentId = reader.GetInt32(3)
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al obtener la carpeta por ID", ex);
        //    }

        //    return folder;
        //}
        //#endregion

        #region GetRootFoldersCurso
        public List<FoldersCurso> GetRootFoldersCurso()
        {
            List<FoldersCurso> rootFoldersCurso = new List<FoldersCurso>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetRootFoldersCurso", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sql.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rootFoldersCurso.Add(new FoldersCurso
                                {
                                    Id = reader.GetInt32(0),
                                    ParentFolderId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(2),

                                    Name = reader.GetString(2),
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

            return rootFoldersCurso;
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

        public FoldersLevel GetFolderLevelById(int id)
        {
            FoldersLevel folderLevel = null;

            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                sql.Open();

                using (SqlCommand command = new SqlCommand("sp_GetFoldersByLevelId", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id_Folders_level", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            folderLevel = new FoldersLevel
                            {
                                Name = reader.GetString(0),
                                Id_Folders_level = reader.GetInt32(1),
                                Id_level_reference = reader.GetInt32(3),
                                // otros campos si es necesario
                            };
                        }
                    }
                }
            }

            return folderLevel;
        }


        public async Task<List<Folder>> GetSubFoldersByParentId(int parentFolderId)
        {
            var subFolders = new List<Folder>();

            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                var command = new SqlCommand("SELECT Id, Name, ParentFolderId, CreatedBy, CreatedAt, FolderLevel FROM Folders WHERE ParentFolderId = @ParentFolderId", sql);
                command.Parameters.AddWithValue("@ParentFolderId", parentFolderId);

                await sql.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        subFolders.Add(new Folder
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ParentFolderId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            CreatedBy = reader.GetString(3),
                            CreatedAt = reader.GetDateTime(4),
                            FolderLevel = reader.GetInt32(5)
                        });
                    }
                }
            }

            return subFolders;
        }

        // Método para obtener el Id_subfolders_curso
        public int? GetFolderCursoId(int folderLevelId)
        {
            int? folderCursoId = null;

            using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
            {
                sql.Open();

                using (SqlCommand command = new SqlCommand("spGetFolderCursoId", sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FolderLevelId", folderLevelId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            folderCursoId = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
                        }
                    }
                }
            }

            return folderCursoId;
        }

        #region GetRootFoldersLevel
        public List<FoldersLevel> GetRootFoldersLevel()
        {
            List<FoldersLevel> rootFoldersCurso = new List<FoldersLevel>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetRootFoldersLevel", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        sql.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rootFoldersCurso.Add(new FoldersLevel
                                {
                                    Id = reader.GetInt32(0),
                                    Id_subfolders_curso = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(2),

                                    Name = reader.GetString(2),
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

            return rootFoldersCurso;
        }
        #endregion

        #region GetFolderByLevelStudent
        public List<FoldersLevel> GetFolderByLevelStudent(int id, string userLevel)
        {
            List<FoldersLevel> rootFoldersLevel = new List<FoldersLevel>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlUserDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetFoldersByLevelStudent", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id_subfolders_curso", id);
                        cmd.Parameters.AddWithValue("@Id_Folders_level", userLevel);

                        sql.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rootFoldersLevel.Add(new FoldersLevel
                                {
                                    Name = reader.GetString(0),
                                    Id_Folders_level = reader.GetInt32(1),
                                    Id_subfolders_curso = reader.GetInt32(2),

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
                                    Name = reader.GetString(0),
                                    Id_Folders_level = reader.GetInt32(1),
                                    Id_subfolders_curso = reader.GetInt32(2),
                                    Id_level_reference = reader.GetInt32(3),

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
