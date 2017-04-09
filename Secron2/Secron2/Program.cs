using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secron2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Console.WriteLine("sdfsdfsd");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


          

        }

        public static GoogleCredential CreateCredentials(string path)
        {
            GoogleCredential credential;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var c = GoogleCredential.FromStream(stream);
                credential = c.CreateScoped(VisionService.Scope.CloudPlatform);
            }

            return credential;
        }

        public static VisionService CreateService(
    string applicationName,
    IConfigurableHttpClientInitializer credentials)
        {
            var service = new VisionService(new BaseClientService.Initializer()
            {
                ApplicationName = applicationName,
                HttpClientInitializer = credentials
            });

            return service;
        }

        private static AnnotateImageRequest CreateAnnotationImageRequest(
    string path,
    string[] featureTypes)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Not found.", path);
            }

            var request = new AnnotateImageRequest();
            request.Image = new Image();

            var bytes = File.ReadAllBytes(path);
            request.Image.Content = Convert.ToBase64String(bytes);

            request.Features = new List<Feature>();

            foreach (var featureType in featureTypes)
            {
                request.Features.Add(new Feature() { Type = featureType });
            }

            return request;
        }

        public static async Task<AnnotateImageResponse> AnnotateAsync(
    this VisionService service,
    FileInfo file,
    params string[] features)
        {
            var request = new BatchAnnotateImagesRequest();
            request.Requests = new List<AnnotateImageRequest>();
            request.Requests.Add(CreateAnnotationImageRequest(file.FullName, features));

            var result = await service.Images.Annotate(request).ExecuteAsync();

            if (result.Responses.Count > 0)
            {
                return result.Responses[0];
            }

            return null;
        }
    }
}
