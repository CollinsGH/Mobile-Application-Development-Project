using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace Mobile_App_Development_Project
{
    public class Storage
    {
        private const string SETTINGS_FILENAME = "settings.json";

        // Retrieve and return all photos from the pictures library 
        public static async Task<IReadOnlyList<StorageFile>> GetPhotos()
        {
            QueryOptions queryOption = new QueryOptions(CommonFileQuery.OrderByDate, new string[] { ".jpg" });

            queryOption.FolderDepth = FolderDepth.Deep;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            return await KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(queryOption).GetFilesAsync();
        }

        public static void SaveSettingsInIsoStorage(AppSettings settings)
        {
            IsolatedStorageFile applicationStorageFileForUser = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream applicationStorageStreamForUser = new IsolatedStorageFileStream(SETTINGS_FILENAME, FileMode.Create, applicationStorageFileForUser);

            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(AppSettings));
            ser.WriteObject(ms, settings);
            byte[] json = ms.ToArray();
            string contents = Encoding.UTF8.GetString(json, 0, json.Length);

            using (StreamWriter sw = new StreamWriter(applicationStorageStreamForUser))
            {
                sw.WriteLine(contents);
            }
        }

        public static AppSettings ReadSettingsFromIsoStorage()
        {
            IsolatedStorageFile applicationStorageFileForUser = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream applicationStorageStreamForUser = new IsolatedStorageFileStream(SETTINGS_FILENAME, FileMode.Open, applicationStorageFileForUser);

            AppSettings settings = new AppSettings();
            
            using (StreamReader sr = new StreamReader(applicationStorageStreamForUser))
            {
                string json = sr.ReadToEnd();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(settings.GetType());
                settings = ser.ReadObject(ms) as AppSettings;
                Debug.WriteLine(settings);
                return settings;
            }
        }
    }
}
