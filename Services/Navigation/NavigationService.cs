using Microsoft.Data.SqlClient;
using Pautas.Models.Pautas;
using Pautas.Services.Conection;
using System;
using System.Collections.Generic;
using System.Data;

namespace Pautas.Services.Navigation
{
    // Ejemplo de método para obtener la estructura de carpetas y archivos
    public class NavigationService
    {
        public List<FolderItem> GetFolderStructure(string rootPath)
        {
            List<FolderItem> folders = new List<FolderItem>();
            DirectoryInfo directoryInfo = new DirectoryInfo(rootPath);

            // Recorrer las carpetas en el directorio raíz
            foreach (var directory in directoryInfo.GetDirectories())
            {
                FolderItem folder = new FolderItem
                {
                    Name = directory.Name,
                    Subfolders = GetSubfolders(directory.FullName)
                };
                folders.Add(folder);
            }

            return folders;
        }

        private List<FolderItem> GetSubfolders(string path)
        {
            List<FolderItem> subfolders = new List<FolderItem>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            // Recorrer las subcarpetas
            foreach (var directory in directoryInfo.GetDirectories())
            {
                FolderItem folder = new FolderItem
                {
                    Name = directory.Name,
                    Subfolders = GetSubfolders(directory.FullName)
                };
                subfolders.Add(folder);
            }

            return subfolders;
        }
    }

    public class FolderItem
    {
        public string Name { get; set; }
        public List<FolderItem> Subfolders { get; set; }
    }

}
