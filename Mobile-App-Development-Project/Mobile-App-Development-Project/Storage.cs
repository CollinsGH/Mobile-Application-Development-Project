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
        private const string FOLDER_NAME = "TravelCam";

        // Create a new file in the folder for this application in the pictures library
        public static async Task<StorageFile> CreateNewFile()
        {
            String imageName = DateTime.Now.ToString("yyyyMMddHHmmss");

            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync(FOLDER_NAME, CreationCollisionOption.OpenIfExists);
            StorageFile file = await folder.CreateFileAsync(imageName + ".jpg", CreationCollisionOption.GenerateUniqueName);

            return file;
        }

        // Retrieve and return all photos from this applications folder in pictures library 
        public static async Task<IReadOnlyList<StorageFile>> GetPhotos()
        {
            QueryOptions queryOption = new QueryOptions(CommonFileQuery.OrderByDate, new string[] { ".jpg" });

            queryOption.FolderDepth = FolderDepth.Deep;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync(FOLDER_NAME, CreationCollisionOption.OpenIfExists);

            return await folder.CreateFileQueryWithOptions(queryOption).GetFilesAsync();
        }

        // Save the AppSettings object to isolated storage as a JSON file
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

        // Read the settings file in isolated storage and convert it to an AppSettings object
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
