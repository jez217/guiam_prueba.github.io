using Microsoft.Data.SqlClient;
using Pautas.Models.Profesor;
using Pautas.Models.Pautas;
using Pautas.Services.Conection;
using System;
using System.Collections.Generic;
using System.Data;

namespace Pautas.Services.Profesor
{
    public class ProfesorServices
    {
        ConnectionDb _connService = new ConnectionDb();

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
