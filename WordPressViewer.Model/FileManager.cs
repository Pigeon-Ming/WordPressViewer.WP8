﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WordPressViewer.Model
{
    public class FileManager
    {
        public static async Task WriteFile(StorageFolder storageFolder,string fileName,string content)
        {
            IStorageItem item = (await storageFolder.GetItemsAsync()).ToList().Find(x => x.Name == fileName);
            StorageFile storageFile = null;
            if(item!=null)
                if (item is StorageFile)
                    storageFile = item as StorageFile;
            if (storageFile == null)
                storageFile = await storageFolder.CreateFileAsync(fileName);
            await Windows.Storage.FileIO.WriteTextAsync(storageFile,content);
        }

        public static async Task<string> ReadFile(StorageFolder storageFolder, string fileName)
        {
            IStorageItem item = (await storageFolder.GetItemsAsync()).ToList().Find(x => x.Name == fileName);
            StorageFile storageFile = null;
            if (item != null)
                if (item is StorageFile)
                    storageFile = item as StorageFile;
            if (storageFile == null)
                return "";
            return await Windows.Storage.FileIO.ReadTextAsync(storageFile);
        }

        public static async Task<StorageFile> GetFile(StorageFolder storageFolder, string fileName)
        {
            IStorageItem item = (await storageFolder.GetItemsAsync()).ToList().Find(x => x.Name == fileName);
            StorageFile storageFile = null;
            if (item != null)
                if (item is StorageFile)
                    storageFile = item as StorageFile;
            if (storageFile == null)
                return null;
            else
                return storageFile;
        }
    }
}
